using Microsoft.AspNetCore.Mvc;
using Tut_6.Models;

namespace Tut_6.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IEnumerable<Student> Details()
        {
            List<Student> students = ;
        }
    }
}
