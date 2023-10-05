using ClassLibraryDLL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace BusinessTier.Controllers
{
    public class SearchBizController : Controller
    {
        [Route("/search")]
        [HttpPost]
        public IActionResult SearchPerson([FromBody] SearchData searchData)
        {
            RestClient client = new RestClient("http://localhost:23509");
            RestRequest request = new RestRequest("/search", Method.Post);
            request.AddBody(searchData);
            RestResponse response = client.Post(request);

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                return new ContentResult
                {
                    Content = response.Content,
                    StatusCode = 404
                };
            }
            else
            {
                DataIntermed value = JsonConvert.DeserializeObject<DataIntermed>(response.Content);
                return Ok(value);
            }   
        }
    }
}
