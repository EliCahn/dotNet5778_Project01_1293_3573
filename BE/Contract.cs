using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// represents a contract between a nanny and a child
    /// </summary>
    public class Contract
    {/// <summary>
     /// represents an editable property of the contract
     /// </summary>
        public enum Props {  NannyID, ChildID, IsMet, IsSigned, IsByHour, PerHour, PerMonth, Beginning, End}
        string serNum, nannyId, childId; //note that if there wasn't a specifi requirement to have a field for the ID's, which implies a Nanny and Child type pointers are not wanted, I would have used pointers instead to save much trouble later...
        bool isMet, isSigned = false, isByHour;
        int perH, perM;
        DateTime b, e;
        /// <summary>
        /// initiates a contract
        /// </summary>
        /// 
        public Contract()
        {            
            Beginning = DateTime.Now.AddDays(-1);
            End = DateTime.Now.AddDays(1);
        }
        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="c"></param>
        public Contract(Contract c)
        {
            NannyID = c.NannyID;
            ChildID = c.ChildID;
            IsMet = c.IsMet;
            IsSigned = c.IsSigned;
            IsByHour = c.IsByHour;
            PerHour = c.PerHour;
            Beginning = c.Beginning;
            End = c.End;
            SerialNumber = c.SerialNumber;
            PerMonth = c.PerMonth;
        }
        /// <summary>
        /// initializes a contract with the given parameters
        /// </summary>
        /// <param name="nanid">ID of the nanny</param>
        /// <param name="chid">ID of the child</param>
        /// <param name="ismet">did the nanny meet the child?</param>
        /// <param name="b">beginning date for the contract</param>
        /// <param name="e">end date for the contract</param>
        public Contract(string nanid, string chid, bool ismet, /*int perh,*/ DateTime b, DateTime e)
        {
            NannyID = nanid;
            ChildID = chid;
            IsMet = ismet;
            //PerHour = PerHour;
            Beginning = b;
            End = e;
        }
        /// <summary>
        /// gets or sets the serial number
        /// </summary>
        public string SerialNumber { get => serNum; set => serNum = value; }
        /// <summary>
        /// gets or sets the nanny's ID
        /// </summary>
        public string NannyID { get => nannyId; set => nannyId = value; }
        /// <summary>
        /// gets or sets the child's ID
        /// </summary>
        public string ChildID { get => childId; set => childId = value; }
        /// <summary>
        /// gets or sets if the child met the nanny
        /// </summary>
        public bool IsMet { get => isMet; set => isMet = value; }
        /// <summary>
        /// gets or sets if the contract was signed
        /// </summary>
        public bool IsSigned { get => isSigned; set => isSigned = value; }
        /// <summary>
        /// gets or sets if the payment is per hours or months
        /// </summary>
        public bool IsByHour { get => isByHour; set => isByHour = value; }
        /// <summary>
        /// gets or sets the salary per hour
        /// </summary>
        public int PerHour { get => perH; set => perH = value; }
        /// <summary>
        /// gets or sets the salary per month
        /// </summary>
        public int PerMonth { get => perM; set => perM = value; }
        /// <summary>
        /// gets or sets the beginning date
        /// </summary>
        public DateTime Beginning { get => b; set => b = value; }
        /// <summary>
        /// gets or sets the ending date
        /// </summary>
        public DateTime End { get => e; set => e = value; }
        /// <summary>
        /// returns this instance as string
        /// </summary>
        public override string ToString()
        {
            return "Contract Number: " + SerialNumber + "\nNanny's ID: " + NannyID + "\nChild's ID: " + ChildID + "\nSalary: " + (IsByHour ? PerHour + "/h => " + PerMonth + " per month" : PerMonth + " per month") + "\nDuration: " + Beginning.ToLongDateString() + " - " + End.ToLongDateString() + (isSigned? "\nSigned" : "");
        }
    }
}
