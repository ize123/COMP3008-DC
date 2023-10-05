using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BusinessTier.Controllers
{
    public class GetValuesBizController : Controller
    {
        [Route("/getvalues")]
        [HttpGet] 
        public ActionResult GetValues() 
        {
            RestClient client = new RestClient("http://localhost:23509");
            RestRequest request = new RestRequest("/getvalues");
            RestResponse response = client.Execute(request);
            int value = JsonConvert.DeserializeObject<int>(response.Content);
            return Ok(value);
        }
    }
}
