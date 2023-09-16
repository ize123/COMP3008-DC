using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Principal;
using Class_Library_Project;
using System.Runtime.Serialization.Formatters;
using server;
using System.Drawing;
using System.IO;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : ServerInterface
    {
        private Database dbl;
        public DataServer() 
        {
            dbl = new Database();
        }
        public int GetNumEntries()
        {
            //implement
            return dbl.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out MemoryStream image)
        {
            //implement
            acctNo = 0;
            pin = 0;
            bal = 0;
            fName = null;
            lName = null;
            image = null;
            
            try
            {
                acctNo = dbl.GetAcctNoByIndex(index - 1);
                pin = dbl.GetPINByIndex(index - 1);
                bal = dbl.GetBalanceByIndex(index - 1);
                fName = dbl.GetFirstNameByIndex(index - 1);
                lName = dbl.GetLastNameByIndex(index - 1);
                image = ConvertToStream(dbl.GetImageByIndex(index - 1));
            }
            catch(ArgumentOutOfRangeException e)
            {
                ServerError se = new ServerError();
                se.ProblemType = "ERROR: " + e.Message + "\nMIN = 1, MAX = " + GetNumEntries();
                Console.WriteLine(se.ProblemType);
                throw new FaultException<ServerError>(se);
            } 
        } 

        private static MemoryStream ConvertToStream(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms;
        }
    }
}
