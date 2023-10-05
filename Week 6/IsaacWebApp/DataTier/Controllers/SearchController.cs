using ClassLibraryDLL;
using DataTier.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataTier.Controllers
{
    public class SearchController : Controller
    {
        [Route("/search")]
        [HttpPost]
        public IActionResult SearchPerson([FromBody] SearchData searchData)
        {
            Person person = Database.Search(searchData.searchStr);
            if (person == null)
            {
                ApiException apiException = new ApiException("NOT FOUND", 404, "No result found in database for search: " + searchData.searchStr);
                string serializedException = JsonConvert.SerializeObject(apiException);
                return new ContentResult
                {
                    Content = serializedException,
                    StatusCode = apiException.StatusCode
                };
            }                
            else
            {
                DataIntermed dataIntermed = new DataIntermed();
                dataIntermed.balance = person.balance;
                dataIntermed.acctNo = person.acctNo;
                dataIntermed.pin = person.pin;
                dataIntermed.firstName = person.firstName;
                dataIntermed.lastName = person.lastName;
                dataIntermed.image = person.image;

                return new ObjectResult(dataIntermed)
                {
                    StatusCode = 200
                };
            }       
        }
    }
}
