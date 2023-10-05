using ClassLibraryDLL;
using DataTier.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataTier.Controllers
{
    public class GetPersonController : Controller
    {
        [Route("/getperson/{id}")]
        [HttpGet]
        public IActionResult GetPerson(int id)
        {
            try
            {
                Person person = Database.GetPersonByIndex(id);

                if (person == null)
                    return NotFound();
                else
                {
                    DataIntermed dataIntermed = new DataIntermed();
                    dataIntermed.balance = person.balance;
                    dataIntermed.acctNo = person.acctNo;
                    dataIntermed.pin = person.pin;
                    dataIntermed.firstName = person.firstName;
                    dataIntermed.lastName = person.lastName;
                    dataIntermed.image = person.image;

                    return new ObjectResult(dataIntermed) { StatusCode = 200 };
                }
            }
            catch (ApiException ex)
            {
                // Rethrow the ApiException using a new exception
                string serializedException = JsonConvert.SerializeObject(ex);
                return new ContentResult
                {
                    Content = serializedException,
                    StatusCode = ex.StatusCode
                };
            }
        }
    }
}
