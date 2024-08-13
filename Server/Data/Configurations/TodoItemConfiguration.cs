using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GrpcTodoCore.Proto;

namespace GrpcTodoServer.Data.Configurations;

internal class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItem").HasKey(t => t.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(30);
    }
}
