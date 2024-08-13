using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using GrpcTodoServer.Data;
using GrpcTodoCore.Proto;

namespace GrpcTodoServer.Services
{
    public class TodoItemService : Todos.TodosBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemService> _logger;

        public TodoItemService(TodoContext context, ILogger<TodoItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async override Task<GetTodoItemsResponse> GetTodoItems(GetTodoItemsRequest request, ServerCallContext context)
        {
            var ftodos = _context.Todos.AsQueryable();

            var filter = request.Filter;
            if (filter?.Title?.Length >= 2) ftodos = ftodos.Where(x => x.Title.Contains(filter.Title));
            if (filter?.IsComplete != null) ftodos = ftodos.Where(x => x.IsComplete == filter.IsComplete);

            var todos = await ftodos.Skip((request.Page - 1) * request.Size).Take(request.Size).ToListAsync();

            var response = new GetTodoItemsResponse();
            response.Count = await ftodos.CountAsync();
            response.TodoItems.AddRange(todos);

            return response;
        }

        public async override Task GetTodoItemsStream(GetTodoItemsRequest request, IServerStreamWriter<TodoItem> responseStream, ServerCallContext context)
        {
            var ftodos = _context.Todos.AsQueryable();

            var filter = request.Filter;
            if (filter?.Title?.Length >= 2) ftodos = ftodos.Where(x => x.Title.Contains(filter.Title));
            if (filter?.IsComplete != null) ftodos = ftodos.Where(x => x.IsComplete == filter.IsComplete);

            var todos = ftodos.Skip((request.Page - 1) * request.Size).Take(request.Size);
            foreach ( var todo in todos)
            {
                await responseStream.WriteAsync(todo);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        public async override Task<TodoItem> GetTodoItem(TodoItemRequest request, ServerCallContext context)
        {
            var todoItem = await _context.Todos.FindAsync(request.Id);

            if (todoItem == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

            return todoItem;
        }

        public async override Task<TodoItem> PostTodoItem(PostTodoItemRequest request, ServerCallContext context)
        {
            var todoItem = new TodoItem() {  Title = request.Title };
            _context.Todos.Add(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async override Task<TodoItem> PutTodoItem(TodoItem request, ServerCallContext context)
        {
            var todoItem = await _context.Todos.FindAsync(request.Id);

            if (todoItem == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

            todoItem.Title = request.Title;
            todoItem.IsComplete = request.IsComplete;
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Todos.Any(e => e.Id == request.Id) == false)
                    throw new RpcException(new Status(StatusCode.NotFound, "Not found"));
                throw;
            }

            return todoItem;
        }

        public async override Task<TodoItem> DeleteTodoItem(TodoItemRequest request, ServerCallContext context)
        {
            var todoItem = await _context.Todos.FindAsync(request.Id);

            if (todoItem == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

            _context.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }


        public async override Task Duplex(IAsyncStreamReader<Num> requestStream, IServerStreamWriter<Num> responseStream, ServerCallContext context)
        {
            //while(await requestStream.MoveNext())
            //{
            //    requestStream.Current.X *=2;
            //    await responseStream.WriteAsync(requestStream.Current);
            //}

            await foreach (var currect in requestStream.ReadAllAsync())
            {
                currect.X *= 2;
                await responseStream.WriteAsync(currect);
            }
        }
    }
}
