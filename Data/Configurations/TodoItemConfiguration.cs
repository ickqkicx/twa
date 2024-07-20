using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestfulAPI.Data.Entities;

namespace RestfulAPI.Data.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItem").HasKey(t => t.Id);
        builder.Property(x => x.Title).IsRequired().HasMaxLength(30);
    }
}