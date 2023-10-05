using DataTier.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataTier.Controllers
{
    public class GetValuesController : Controller
    {
        [Route("/getvalues")]
        [HttpGet]
        public int GetNumEntries()
        {
            int numEntries = Database.GetNumRecords();
            return numEntries;
        }

        //GET http://localhost:23509//getvalues
    }
}
