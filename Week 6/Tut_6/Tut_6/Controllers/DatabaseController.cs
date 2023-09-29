using Microsoft.AspNetCore.Mvc;
using Tut_6.Models;

namespace Tut_6.Controllers
{
    public class DatabaseController : Controller
    {
        [HttpGet]
        public IEnumerable<Student> Details()
        {
            List<Student> students = Database.GetAllStudents();
            return students;
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Student student = Database.GetStudentByIndex(id);
            if(student == null) { 
                return NotFound();
            }
            else
            {
                return new ObjectResult(student) { StatusCode = 200};
            }
        }
    }
}
