using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        public IEnumerable<TodoItem> Get(Predicate<TodoItem> predicate)
        {
            return _context.TodoItems.ToList().FindAll(predicate);
        }

        public TodoItem GetById(long id)
        {
            return Get(x => x.Id == id).FirstOrDefault();
        }

        public void Create(TodoItem todo)
        {
            _context.TodoItems.Add(todo);
            _context.SaveChanges();
        }

        public void Update(long id, TodoItem todo)
        {
            _context.TodoItems.Update(todo);
            _context.SaveChanges();
        }

        public void Delete(TodoItem todo)
        {
            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
        }
    }
}
