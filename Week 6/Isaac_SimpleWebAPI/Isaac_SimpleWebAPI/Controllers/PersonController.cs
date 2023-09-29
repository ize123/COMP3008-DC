using Isaac_SimpleWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Isaac_SimpleWebAPI.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Person> GetAllPersons()
        {
            List<Person> persons = Database.AllPersons();
            return persons;
        }

        // GET http://localhost:44671/person/getallpersons

        [HttpGet]
        public IActionResult GetPerson(int id)
        {
            Person person = Database.GetPersonByIndex(id);
            if (person == null)
                return NotFound();
            else
                return new ObjectResult(person) { StatusCode = 200 };
        }

        // GET http://localhost:44671/person/getpersonbyindex/{id}

        [HttpGet]
        public int GetNumEntries()
        {
            int numEntries = Database.GetNumRecords();
            return numEntries;
        }

        //GET http://localhost:44671/person/getnumentries
    }
}
