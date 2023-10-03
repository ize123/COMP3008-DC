using System.Drawing;

namespace Isaac_SimpleWebAPI.Models
{
    public class Database
    {
        static List<Person> personsList = new List<Person>();

        public static List<Person> AllPersons()
        {
            return personsList;
        }

        public static void Generate()
        {
            Generator generator = new Generator();
            for (int i = 0; i < 10; i++)
            {
                generator.GetNextAccount(out uint pin, out uint AcctNo, out string Firstname, out string LastName, out int balance, out string img);
                Person person = new Person(pin, AcctNo, balance, Firstname, LastName, img);
                personsList.Add(person);
            }
        }

        public uint GetAcctNoByIndex(int index)
        {
            return AllPersons()[index].acctNo;
        }

        public uint GetPINByIndex(int index)
        {
            return AllPersons()[index].pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            return AllPersons()[index].firstName;
        }

        public string GetLastNameByIndex(int index)
        {
            return AllPersons()[index].lastName;
        }

        public int GetBalanceByIndex(int index)
        {
            return AllPersons()[index].balance;
        }

        public string GetImageByIndex(int index)
        {
            return AllPersons()[index].image;
        }

        public static int GetNumRecords()
        {
            return AllPersons().Count;
        }

        public static Person? GetPersonByIndex(int index)
        {
            return AllPersons()[index];
        }

        public static Person? Search(string searchStr)
        {
            Person person = null;
            var match = AllPersons().FirstOrDefault(x => x.lastName.ToLower().Equals(searchStr.ToLower()));
            if(match != null)
            {
                int index = AllPersons().IndexOf(match);
                person = GetPersonByIndex(index);
            }
            return person;
        }
    }
}
