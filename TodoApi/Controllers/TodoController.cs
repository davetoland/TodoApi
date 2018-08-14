using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoRepository _repo;

        public TodoController(TodoRepository repo)
        {
            _repo = repo;
        }

        // GET: api/todo
        /// <summary>
        /// Gets all the TODO items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _repo.Get(x => true);
        }

        // GET api/todo/5
        /// <summary>
        /// Gets an existing TODO item
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null)
                return NotFound();

            return item;
        }

        // GET api/todo/5
        /// <summary>
        /// Gets an existing TODO item
        /// </summary>
        [HttpGet("match")]
        public ActionResult<IEnumerable<TodoItem>> GetByName([FromQuery] string name)
        {
            var items = _repo.Get(x => x.Name == name);
            if (items == null)
                return NotFound();

            return new ActionResult<IEnumerable<TodoItem>>(items);
        }

        // POST api/todo
        /// <summary>
        /// Adds a new TODO item
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _repo.Create(item);
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
            var todo = _repo.GetById(id);
            if (todo == null)
                return NotFound();

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _repo.Update(id, item);
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
            var todo = _repo.GetById(id);
            if (todo == null)
                return NotFound();

            _repo.Delete(todo);
            return NoContent();
        }
    }
}
