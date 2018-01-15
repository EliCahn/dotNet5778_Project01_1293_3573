using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// represents a mother
    /// </summary>
    public class Mother
    {/// <summary>
     /// represents an editable property of the mother
     /// </summary>
        public enum Props { LastName, FirstName, Phone, Address, NeedNannyAddress, DaysNeeded, HoursNeeded, Comments }
        string id, lstName, name, phone, address, needNannyAddress = "";
        bool[] daysNeeded = new bool[7];
        List<string> comments = new List<string>();
        DateTime[,] hoursNeeded = new DateTime[2, 6];
        /// <summary>
        /// initializes a mother
        /// </summary>
        public Mother() { }
        /// <summary>
        /// initiates a mother with the parameter ID
        /// </summary>
        /// <param name="id"></param>
        public Mother(string id)
        { ID = id; }
        public Mother(Mother m)
        {
            ID = m.ID;
            LastName = m.LastName;
            FirstName = m.FirstName;
            Phone = m.Phone;
            Address = m.Address;
            NeedNannyAddress = m.NeedNannyAddress;
            for (int i = 0; i < 6; i++)
            {
                DaysNeeded[i] = m.DaysNeeded[i];
                HoursNeeded[0, i] = m.hoursNeeded[0, i];
                HoursNeeded[1, i] = m.hoursNeeded[1, i];
            }
            foreach (string str in m.Comments)
                Comments.Add(str);
        }
        /// <summary>
        /// initializes a mother with the given parameters
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="fname">first name</param>
        /// <param name="lname">last name</param>
        /// <param name="phone">phone number</param>
        /// <param name="address">address</param>
        /// <param name="nanaddress">address in which the nanny is needed (if empty it is the home address)</param>
        /// <param name="days">days in which the nanny is needed</param>
        /// <param name="hours">hours in which the nanny is needed (first one is starting our, second is ending hour) per day</param>
        /// <param name="com">list of comments</param>
        public Mother(string id, string fname, string lname, string phone, string address, string nanaddress, bool[] days, DateTime[,] hours, List<string> com)
        {
            ID = id;
            FirstName = fname;
            LastName = lname;
            Phone = phone;
            Address = address;
            if (nanaddress != null)
                NeedNannyAddress = nanaddress;
            else
                needNannyAddress = "";
            //daysNeeded = days;
            //hoursNeeded = hours;
            if (days.Length == 7)
                for (int i = 0; i < DaysNeeded.Length; i++)
                    DaysNeeded[i] = days[i];
            else
                throw new Exception("days array must have the length of 7");
            //workHours = hours;
            if (hours.GetLength(0) == 2 && hours.GetLength(1) == 6)
                for (int i = 0; i < 6; i++)
                {
                    if (DaysNeeded[i])
                    {
                        HoursNeeded[0, i] = hours[0, i];
                        HoursNeeded[1, i] = hours[1, i];
                    }
                }
            else
                throw new Exception("hours matrix must be of size [2,6]");
            for (int i = 0; com != null && i < com.Count; i++)
                comments.Add(com[i]);
        }
        /// <summary>
        /// gets or sets the ID
        /// </summary>
        public string ID { get => id; set => id = value; }
        /// <summary>
        /// gets or sets the last name
        /// </summary>
        public string LastName { get => lstName; set => lstName = value; }
        /// <summary>
        /// gets or sets the first name
        /// </summary>
        public string FirstName { get => name; set => name = value; }
        /// <summary>
        /// gets or sets the phone number
        /// </summary>
        public string Phone { get => phone; set => phone = value; }
        /// <summary>
        /// gets or sets the address
        /// </summary>
        public string Address { get => address; set => address = value; }
        /// <summary>
        /// gets or sets the address where the mother requires a nanny
        /// </summary>
        public string NeedNannyAddress { get => needNannyAddress; set => needNannyAddress = value; }
        /// <summary>
        /// gets an array of the days in which the mother requires a nanny
        /// </summary>
        public bool[] DaysNeeded { get => daysNeeded; }
        /// <summary>
        /// gets a matrix of the hours the mother requires a nanny per day.
        /// the 1st index indicates- 0: starting hour, 1: ending hour
        /// the 2nd index indicates which day in the week it is.
        /// </summary>
        public DateTime[,] HoursNeeded { get => hoursNeeded; }
        /// <summary>
        /// gets a list of comments
        /// </summary>
        public List<string> Comments { get => comments; }
        /// <summary>
        /// returns this instance as string
        /// </summary>
        public override string ToString()
        {
            string retStr = "ID: " + ID + "\nName: " + FirstName + " " + LastName + "\nPhone number: " + Phone + "\nAddress: " + Address + "\nRequired Address: " + (NeedNannyAddress == "" ? "Home (Same address)" : needNannyAddress) + "\nRequired work time:";
            for (int i = 0; i < 6; i++)
            {
                if (DaysNeeded[i])
                {
                    retStr += "\n" + (Days)i + ": " + HoursNeeded[0, i].ToShortTimeString() + " - " + HoursNeeded[1, i].ToShortTimeString();
                }
            }
            if (comments.Count == 0)
                return retStr;
            retStr += "\nComments:";
            foreach (string com in Comments)
            {
                retStr += "\n" + com;
            }
            return retStr;
        }
    }

}
