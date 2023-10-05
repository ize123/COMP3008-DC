using ClassLibraryDLL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace BusinessTier.Controllers
{
    public class GetPersonBizController : Controller
    {
        [Route("/getperson/{id}")]
        [HttpGet]
        public IActionResult GetPerson(int id)
        {
            try
            {
                RestClient client = new RestClient("http://localhost:23509");
                RestRequest request = new RestRequest("/getperson/" + id);
                RestResponse response = client.Get(request);
                if (response.StatusCode == HttpStatusCode.NotFound) // Check the status code
                {
                    // Now you can access the properties of the ApiException
                    return new ContentResult
                    {
                        Content = response.Content,
                        StatusCode = 404
                    };
                }
                else
                {
                    // Handle a successful response here
                    DataIntermed dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(response.Content);
                    return Ok(dataIntermed);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
