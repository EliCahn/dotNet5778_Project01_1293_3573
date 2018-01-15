using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// represents a child
    /// </summary>
    public class Child
    {/// <summary>
     /// represents an editable property of the child
     /// </summary>
        public enum Props { /*BirthDate,*/ MotherID, FirstName, LastName, HaveSpecialNeeds, SpecialNeeds }
        string id, momId, name, lstName; //note that if there wasn't a specifi requirement to have a field for the mother's ID, which implies a Mother type pointer is not wanted, I would have used a pointer instead to save much trouble later...
        bool haveSpecNeeds;
        DateTime birthDate;
        List<string> specNeeds = new List<string>();
        /// <summary>
        /// initiates a child
        /// </summary>
        public Child() { BirthDate = BirthDate = DateTime.Now.AddDays(-1); }
        /// <summary>
        /// initiates a child with the parameter ID
        /// </summary>
        /// <param name="id"></param>
        public Child(string id) { ID = id; BirthDate = DateTime.Now.AddDays(-1); }
        public Child(Child c)
        {
            ID = c.ID;
            MotherID = c.MotherID;
            FirstName = c.FirstName;
            LastName = c.LastName;
            haveSpecNeeds = c.haveSpecNeeds;
            BirthDate = c.BirthDate;
            foreach (string str in c.SpecialNeeds)
                SpecialNeeds.Add(str);
        }
        /// <summary>
        /// initializes a child with the given parameters
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="momid">ID of the mother</param>
        /// <param name="fname">first name</param>
        /// <param name="lname">last name</param>
        /// <param name="spec">special needs</param>
        /// <param name="birth">birth date</param>
        public Child(string id, string momid, string fname, string lname, /*bool havespec,*/ List<string> spec, DateTime birth)
        {
            ID = id;
            MotherID = momid;
            FirstName = fname;
            LastName = lname;
            HaveSpecialNeeds = !(spec == null || spec.Count == 0);
            //specNeeds = spec;
            if (HaveSpecialNeeds)
                for (int i = 0; spec != null && i < spec.Count; i++)
                    SpecialNeeds.Add(spec[i]);
            BirthDate = birth;
        }
        /// <summary>
        /// gets or sets the ID
        /// </summary>
        public string ID { get => id; set => id = value; }
        /// <summary>
        /// gets or sets the mother's ID
        /// </summary>
        public string MotherID { get => momId; set => momId = value; }
        /// <summary>
        /// gets or sets the first name
        /// </summary>
        public string FirstName { get => name; set => name = value; }
        /// <summary>
        /// gets or sets the last name
        /// </summary>
        public string LastName { get => lstName; set => lstName = value; }
        /// <summary>
        /// gets or sets if the child has special needs
        /// </summary>
        public bool HaveSpecialNeeds { get => haveSpecNeeds; set => haveSpecNeeds = value; }
        /// <summary>
        /// gets a list of the special needs
        /// </summary>
        public List<string> SpecialNeeds { get => specNeeds; }
        /// <summary>
        /// gets or sets the birth date
        /// </summary>
        public DateTime BirthDate { get => birthDate; set => birthDate = value; }
        /// <summary>
        /// returns this instance as string
        /// </summary>
        public override string ToString()
        {
            string retStr = "ID: " + ID + "\nName: " + FirstName + " " + LastName + "\nMother ID: " + MotherID + "\nBirth Date: " + BirthDate.ToLongDateString();
            if (HaveSpecialNeeds)
            {
                retStr += "\nSpecial Needs:";
                foreach (string need in SpecialNeeds)
                    retStr += "\n" + need;
            }
            return retStr;
        }
    }
}
