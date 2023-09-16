using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Class_Library_Project
{
    public class Database
    {
        List<DataStruct> dataStructs;
        private int MAX = 50;
        DatabaseGenerator generator = new DatabaseGenerator();

        public Database()
        {
            dataStructs = new List<Class_Library_Project.DataStruct>();
            for(int i = 0; i < MAX; i++)
            {
                generator.GetNextAccount(out uint pin, out uint AcctNo, out string Firstname, out string LastName, out int balance, out Bitmap img);
                DataStruct person = new DataStruct(pin, AcctNo, balance, Firstname, LastName, img);
                dataStructs.Add(person);
            }
        }
        public uint GetAcctNoByIndex(int index)
        {
            return dataStructs[index].acctNo;
        }

        public uint GetPINByIndex(int index)
        {
            return dataStructs[index].pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            return dataStructs[index].firstName;
        }

        public string GetLastNameByIndex(int index)
        {
            return dataStructs[index].lastName;
        }

        public int GetBalanceByIndex(int index)
        {
            return dataStructs[index].balance;
        }

        public Bitmap GetImageByIndex(int index)
        {
            return dataStructs[index].image;
        }

       public int GetNumRecords()
        {
            return dataStructs.Count;
        }
    }
}
