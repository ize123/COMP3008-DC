using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Tut_6.Models
{
    public class Database
    {
        List<Student> studentsList;
        public Database()
        {
            studentsList = new List<Student>();
            InitStudents();
        }

        private void InitStudents()
        {
            DatabaseGenerator generator = new DatabaseGenerator();
            for (int i = 0; i < 100_000; i++)
            {
                generator.GetNextAccount(out uint pin, out uint AcctNo, out string Firstname, out string LastName, out int balance, out Bitmap img);
                Student person = new Student(pin, AcctNo, balance, Firstname, LastName, img);
                studentsList.Add(person);               
            }
        }
        public uint GetAcctNoByIndex(int index)
        {
            return GetAllStudents()[index].acctNo;
        }

        public uint GetPINByIndex(int index)
        {
            return GetAllStudents()[index].pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            return GetAllStudents()[index].firstName;
        }

        public string GetLastNameByIndex(int index)
        {
            return GetAllStudents()[index].lastName;
        }

        public int GetBalanceByIndex(int index)
        {
            return GetAllStudents()[index].balance;
        }

        public Bitmap GetImageByIndex(int index)
        {
            return GetAllStudents()[index].image;
        }

        public int GetNumRecords()
        {
            return GetAllStudents().Count;
        } 

        public Student GetStudentByIndex(int index)
        {
            return GetAllStudents()[index];
        }

        public List<Student> GetAllStudents()
        {
            return studentsList;
        }
    }
}
