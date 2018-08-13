using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/todo
        /// <summary>
        /// Gets all the TODO items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        // GET api/todo/5
        /// <summary>
        /// Gets an existing TODO item
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
                return NotFound();

            return item;
        }

        // POST api/todo
        /// <summary>
        /// Adds a new TODO item
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        // PUT api/todo/5
        /// <summary>
        /// Updates an existing TODO item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPut("{id}")]
        public IActionResult Update(long id, TodoItem item)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
                return NotFound();

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE api/todo/5
        /// <summary>
        /// Deletes an existing TODO item
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
                return NotFound();

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
