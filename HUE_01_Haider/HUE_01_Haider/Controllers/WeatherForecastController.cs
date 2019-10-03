using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HUE_01_Haider.Controllers
{
    [ApiController]
    [Route("contactList")]
    // RS: Class name does not fit to file name
    public partial class ToDoController : ControllerBase
    {

        private static readonly List<Contact> contactList = new List<Contact> { new Contact(0, "Thomas", "Haider", "test@test.test") };

        [HttpGet]
        [Route("{name}", Name = "GetSpecificItem")]
        public IActionResult GetItem(string name)
        {
            // Search by ID is required, not by name
            IEnumerable<Contact> searchByNameQuery =
                from contact in contactList
                where contact.firstName == name || contact.lastName == name
                select contact;
            // Consider .Any() instead of .Count() > 0
            if (searchByNameQuery.Count() > 0)
            {
                return Ok(searchByNameQuery);
            }
            else
            {
                return BadRequest("Invalid or missing name");
            }
        }

        [HttpGet]
        public IActionResult GetAllItems()
        {
            return Ok(contactList);
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] Contact newItem)
        {
            contactList.Add(newItem);
            // Controller name inconsistency (called "GetSpecificItem" above)
            return CreatedAtRoute("GetSpecificTodoItem", new { index = contactList.IndexOf(newItem) }, newItem);
        }

        [HttpDelete]
        [Route("{index}")]
        public IActionResult DeleteItem(int index)
        {
            if (index >= 0 && index < contactList.Count)
            {
                contactList.RemoveAt(index);
                return Ok("Successful operation");
            }

            return BadRequest("Invalid ID supplied");
        }
    }
}
