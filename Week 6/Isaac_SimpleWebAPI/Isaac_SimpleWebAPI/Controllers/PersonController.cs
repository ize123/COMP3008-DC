using Class_Library;
using Isaac_SimpleWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using WPF_Client;

namespace Isaac_SimpleWebAPI.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // Can specify custom routes like this [Route("GetAllPersons")]
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

        // GET http://localhost:44671/person/getperson/{id}

        [HttpGet]
        public int GetNumEntries()
        {
            int numEntries = Database.GetNumRecords();
            return numEntries;
        }

        //GET http://localhost:44671/person/getnumentries

        [HttpPost]
        public IActionResult SearchPerson([FromBody] SearchData searchStr)
        {
            Person person = Database.Search(searchStr.searchStr);
            if (person == null)
                return NotFound();
            else
                return new ObjectResult(person) 
                { 
                    StatusCode = 200
                };
        }

        //GET http://localhost:44671/person/searchperson
    }
}
