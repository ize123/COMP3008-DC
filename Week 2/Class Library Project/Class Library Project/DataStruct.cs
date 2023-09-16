using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Class_Library_Project
{
    internal class DataStruct
    {
        public uint acctNo;
        public uint pin;
        public int balance;
        public string firstName;
        public string lastName;
        public Bitmap image;
        

        public DataStruct()
        {
            acctNo = 0;
            pin = 0;
            balance = 0;
            firstName = ""; 
            lastName = "";
            image = null;
        }

        public DataStruct(uint acctNo, uint pin, int balance, string firstName, string lastName, Bitmap image)
        {
            this.acctNo = acctNo;
            this.pin = pin;
            this.balance = balance;
            this.firstName = firstName;
            this.lastName = lastName;
            this.image = image;
        }
    }
}
