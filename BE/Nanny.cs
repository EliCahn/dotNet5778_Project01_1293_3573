using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Nanny
    {/// <summary>
    /// represents an editable property of the nanny
    /// </summary>
        public enum Props { FirstName, LastName, Phone, /*BirthDate,*/ Address, Elevator, IsCostByHour,Floor, VacationByMinisterOfEducation, MaxChildren, MinAge, MaxAge, HourSalary, MonthSalary, WorkDays, WorkHours, Recommendations, Expertise }
        string name, id, lstName, phone, address;
        DateTime birthdate;
        bool elevator, byHour, vacByEdu;
        int floor, maxChildren, minAge, maxAge, Hsalary, Msalary, exp;
        bool[] workDays = new bool[7];
        DateTime[,] workHours = new DateTime[2, 6];
        List<string> recommendations = new List<string>();
        /// <summary>
        /// initiates a nanny
        /// </summary>
        public Nanny() { BirthDate = DateTime.Now.AddYears(-18).AddDays(-1); }
        /// <summary>
        /// initiates a nanny with the parameter ID
        /// </summary>
        public Nanny(string id) { ID = id; BirthDate = DateTime.Now.AddYears(-18).AddDays(-1); }
        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="n"></param>
        public Nanny(Nanny n)
        {
            ID = n.id;
            Floor = n.Floor;
            Address = n.Address;
            BirthDate = n.BirthDate;
            Elevator = n.Elevator;
            Expertise = n.Expertise;
            FirstName = n.FirstName;
            HourSalary = n.HourSalary;
            IsCostByHour = n.IsCostByHour;
            LastName = n.LastName;
            MaxAge = n.MaxAge;
            MaxChildren = n.MaxChildren;
            MinAge = n.MinAge;
            MonthSalary = n.MonthSalary;
            Phone = n.Phone;
            VacationByMinisterOfEducation = n.VacationByMinisterOfEducation;
            for (int i = 0; i < 6; i++)
            {
                WorkDays[i] = n.WorkDays[i];
                WorkHours[0, i] = n.WorkHours[0, i];
                WorkHours[1, i] = n.WorkHours[1, i];
            }
            WorkDays[6] = n.WorkDays[6];
            foreach (string str in n.Recommendations)
                Recommendations.Add(str);
        }
        /// <summary>         
        /// initializes a nanny with the given parameters
        /// </summary>        
        /// <param name="id">ID</param>
        /// <param name="fname">first name</param>
        /// <param name="lname">last name</param>
        /// <param name="phone">phone number</param>
        /// <param name="address">address</param>
        /// <param name="birth">birth date</param>
        /// <param name="elevator">does the nanny have elevator?</param>
        /// <param name="byHour">does the nanny get paid by hour?</param>
        /// <param name="byedu">does the nanny get vacations according to the minister of education?</param>
        /// <param name="floor">the floor the nanny lives in</param>
        /// <param name="maxchildren">the maximum number of contracts the nanny can have signed at once</param>
        /// <param name="minage">minimal acceptable age for a child (by months)</param>
        /// <param name="maxage">maximal acceptable age for a child (by months)</param>
        /// <param name="hsal">salary per hour</param>
        /// <param name="msal">salary per month</param>
        /// <param name="exp">years of expertise</param>
        /// <param name="days">days in which the nanny works</param>
        /// <param name="hours">hours in which the nanny works (first one is starting our, second is ending hour) per day</param>
        /// <param name="recs">reccomendations</param>
        public Nanny(string id, string fname, string lname, string phone, string address, DateTime birth, bool elevator, bool byHour, bool byedu, int floor, int maxchildren, int minage, int maxage, int hsal, int msal, int exp, bool[] days, DateTime[,] hours, List<string> recs)
        {
            ID = id;
            FirstName = fname;
            LastName = lname;
            Phone = phone;
            Address = address;
            BirthDate = birth;
            Elevator = elevator;
            IsCostByHour = byHour;
            VacationByMinisterOfEducation = byedu;
            Floor = floor;
            MaxChildren = maxchildren;
            MaxAge = maxage;
            MinAge = minage;
            HourSalary = hsal;
            MonthSalary = msal;
            Expertise = exp;
            //workDays = days;
            if (days.Length == 7)
                for (int i = 0; i < workDays.Length; i++)
                    WorkDays[i] = days[i];
            else
                throw new Exception("days array must have the length of 7");
            //workHours = hours;
            if (hours.GetLength(0) == 2 && hours.GetLength(1) == 6)
            {
                for (int i = 0; i < 6; i++)
                    if (WorkDays[i])
                    {
                        WorkHours[0, i] = hours[0, i];
                        WorkHours[1, i] = hours[1, i];
                    }
            }
            else
                throw new Exception("hours matrix must be of size [2,6]");
            for (int i = 0; recs!= null && i < recs.Count; i++)
                Recommendations.Add(recs[i]);
        }
        /// <summary>
        /// gets or sets the first name
        /// </summary>
        public string FirstName
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// gets or sets the ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// gets or sets the last name
        /// </summary>
        public string LastName
        {
            get { return lstName; }
            set { lstName = value; }
        }
        /// <summary>
        /// gets or sets the phone number
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        /// <summary>
        /// gets or sets the birth date
        /// </summary>
        public DateTime BirthDate
        {
            get { return birthdate; }
            set { birthdate = value; }
        }
        /// <summary>
        /// gets or sets the address
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        /// shows if is there an elevator (setable)
        /// </summary>
        public bool Elevator
        {
            get { return elevator; }
            set { elevator = value; }
        }
        /// <summary>
        /// shows if the payment is by hour (if false it is by month) (setable)
        /// </summary>
        public bool IsCostByHour
        {
            get { return byHour; }
            set { byHour = value; }
        }
        /// <summary>
        /// shows if the vacation is based on the Minister of Education's rules (if not, it is the Minister of Economy) (setable)
        /// </summary>
        public bool VacationByMinisterOfEducation
        {
            get { return vacByEdu; }
            set { vacByEdu = value; }
        }
        /// <summary>
        /// gets or sets the floor
        /// </summary>
        public int Floor { get => floor; set => floor = value; }
        /// <summary>
        /// gets or sets the maximal number of children that the nanny can have at once
        /// </summary>
        public int MaxChildren { get => maxChildren; set => maxChildren = value; }
        /// <summary>
        /// gets or sets the minimal acceptable age of a child
        /// </summary>
        public int MinAge { get => minAge; set => minAge = value; }
        /// <summary>
        /// gets or sets the maximal acceptable age of a child
        /// </summary>
        public int MaxAge { get => maxAge; set => maxAge = value; }
        /// <summary>
        /// gets or set the Salary per hour
        /// </summary>
        public int HourSalary { get => Hsalary; set => Hsalary = value; }
        /// <summary>
        /// gets or set the Salary per month
        /// </summary>
        public int MonthSalary { get => Msalary; set => Msalary = value; }
        /// <summary>
        /// gets an array of the days in which the nanny works
        /// </summary>
        public bool[] WorkDays { get => workDays;}
        /// <summary>
        /// gets a matrix of the hours the nanny works per day.
        /// the 1st index indicates- 0: starting hour, 1: ending hour
        /// the 2nd index indicates which day in the week it is.
        /// </summary>
        public DateTime[,] WorkHours { get => workHours; }
        /// <summary>
        /// gets a list of the recommendations
        /// </summary>
        public List<string> Recommendations { get => recommendations;}
        /// <summary>
        /// gets or sets the years of expertise
        /// </summary>
        public int Expertise { get => exp; set => exp = value; }
        /// <summary>
        /// returns this instance as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string retStr = "ID: " + ID + "\nName: " + FirstName + " " + LastName + "\nBirth date: " + BirthDate.ToLongDateString() + "\nPhone number: " + Phone + "\nAddress: " + Address + "\nExpertise: " + Expertise + " years\nSalary: " + (byHour ? Hsalary + " per hour" : Msalary + " per month") + "\nWork Time:";
            for (int i = 0; i < 6; i++)
            {
                if (WorkDays[i])
                {
                    retStr += "\n" + (Days)i + ": " + WorkHours[0, i].ToShortTimeString() + " - " + WorkHours[1, i].ToShortTimeString();
                }
            }
            if (Recommendations.Count == 0)
                return retStr;
            retStr += "\nRecommendations:";
            foreach (string rec in Recommendations)
            {
                retStr += "\n" + rec;
            }
            return retStr;
        }
    }
}
