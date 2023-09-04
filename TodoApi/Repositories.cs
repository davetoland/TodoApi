using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace TodoApiV7
{
    internal class TodoRepo : DbContext
    {
        public TodoRepo(DbContextOptions<TodoRepo> options)
        : base(options) { }

        internal DbSet<TodoItem> Todos => Set<TodoItem>();

        // return the Task, allow callers further upstream to await it
        internal Task<List<TodoItem>> GetAll(CancellationToken token)
            => Todos.ToListAsync(token);

        // Apply the predicate on the IQueryable, before calling ToList
        internal Task<List<TodoItem>> Get(Expression<Func<TodoItem, bool>> predicate, CancellationToken token)
            => Todos.Where(predicate).ToListAsync(token);

        // ValueTask, as this item may already be in EF context,
        // and we don't want to put a Task on the heap if it is
        internal ValueTask<TodoItem?> GetById(long id, CancellationToken token)
            => Todos.FindAsync(new object?[] { id }, cancellationToken: token);

        internal async Task<TodoItem?> Create(TodoItem todo, CancellationToken token)
        {
            EntityEntry<TodoItem> item;
            try
            {
                item = await Todos.AddAsync(todo, token);
                item.Entity.CreatedOn = DateTime.UtcNow;
                item.Entity.LastUpdated = DateTime.UtcNow;
                await SaveChangesAsync(token);
            }
            catch (Exception)
            {
                return null;
            }
            return item.Entity;
        }

        internal async Task<TodoItem?> Update(long id, Action<TodoItem> updater, CancellationToken token)
        {
            var item = await GetById(id, token);
            if (item == null)
                return null;

            try
            {
                updater(item);
                item.LastUpdated = DateTime.UtcNow;
                await SaveChangesAsync(token);
            }
            catch (Exception)
            {
                return null;
            }
            return item;
        }

        internal async Task<bool> Delete(long id, CancellationToken token)
        {
            var todo = await GetById(id, token);
            if (todo == null)
                return false;

            try
            {
                Todos.Remove(todo);
                await SaveChangesAsync(token);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
