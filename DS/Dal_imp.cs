using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace DS
{
    /// <summary>
    /// an implementation of Idal
    /// </summary>
    public class Dal_imp : Idal
    {
        //static Random rnd = new Random();
        static int SerialNum = 10000000;
        static Dal_imp instance = null;
        public static Dal_imp Instance
        {
            get
            {
                if (instance == null)
                    instance = new Dal_imp();
                return instance;
            }
        }
        protected Dal_imp() { }
        /// <summary>
        /// adds a child
        /// </summary>
        /// <param name="c">child to add</param>
        public void AddChild(Child c)
        {
            foreach (Nanny ch in DataSource.Nannies)
                if (ch.ID == c.ID)
                    throw new Exception("ID already exists!");
            foreach (Child ch in DataSource.Children)            
                if (ch.ID == c.ID)
                    throw new Exception("ID already exists!");
            foreach (Mother ch in DataSource.Mothers)
                if (ch.ID == c.ID)
                    throw new Exception("ID already exists!");
            if (FindMotherByID(c.MotherID) == null)
                throw new Exception("mother doesn't exist!");
            DataSource.Children.Add(c);
            
        }
        /// <summary>
        /// adds a contract
        /// </summary>
        /// <param name="c">contract to add</param>
        public void AddContract(Contract c)
        {
            //bool NannyExists = false, momExists = false/*, serialexist = false*/;
            //foreach(Nanny n in DataSource.Nannies)
            //{
            //    if (n.ID == c.NannyID)
            //    {
            //        NannyExists = true;
            //        break;
            //    }
            //}
            //if (!NannyExists)
            //    throw new Exception("Nanny doesn't exist!");            
            //foreach (Child ch in DataSource.Children)
            //    if (ch.ID == c.ChildID)
            //    {
            //        momExists = true;
            //        break;
            //    }
            //if (!momExists)
            //    throw new Exception("Mother doesn't exist!"); // because a child always has a mother, lack of child is the only way to miss a mother
            Nanny NannyInContract = FindNannyByID(c.NannyID);
            Child ChildInContract = FindChildByID(c.ChildID);
            if (NannyInContract == null)
                throw new Exception("Nanny doesn't exist!");
            if (ChildInContract == null)
                throw new Exception("Mother doesn't exist!"); // because a child always has a mother, lack of child is the only way to miss a mother
            foreach (Contract con in GetContracts())
                if (con.ChildID == c.ChildID && con.NannyID == c.NannyID)
                    throw new Exception("Contarct between this nanny and child already exists");
            c.SerialNumber = (SerialNum++).ToString();
            //do
            //{
                
            //    //c.SerialNumber = rnd.Next(10).ToString() + rnd.Next(1000000, 9999999);
            //    foreach (Contract con in DataSource.Contracts)
            //        if (c.SerialNumber == con.SerialNumber)
            //        {
            //            serialexist = true;
            //            break;
            //        }
            //}
            //while (serialexist);
            DataSource.Contracts.Add(c);          

                
        }
        /// <summary>
        /// adds a mother
        /// </summary>
        /// <param name="m">mother to add</param>
        public void AddMother(Mother m)
        {
            foreach (Nanny ch in DataSource.Nannies)
                if (ch.ID == m.ID)
                    throw new Exception("ID already exists!");
            foreach (Child ch in DataSource.Children)
                if (ch.ID == m.ID)
                    throw new Exception("ID already exists!");
            foreach (Mother ch in DataSource.Mothers)
                if (ch.ID == m.ID)
                    throw new Exception("ID already exists!");
            DataSource.Mothers.Add(m);
        }
        /// <summary>
        /// adds a nanny
        /// </summary>
        /// <param name="n">nanny to add</param>
        public void AddNanny(Nanny n)
        {
            foreach (Nanny ch in DataSource.Nannies)
                if (ch.ID == n.ID)
                    throw new Exception("ID already exists!");
            foreach (Child ch in DataSource.Children)
                if (ch.ID == n.ID)
                    throw new Exception("ID already exists!");
            foreach (Mother ch in DataSource.Mothers)
                if (ch.ID == n.ID)
                    throw new Exception("ID already exists!");
            DataSource.Nannies.Add(n);
        }
        /// <summary>
        /// gets the list of children
        /// </summary>
        /// <returns></returns>
        public List<Child> GetChildren()
        {
            return DataSource.Children;
        }
        /// <summary>
        /// gets the list of contracts
        /// </summary>
        /// <returns></returns>
        public List<Contract> GetContracts()
        {
            return DataSource.Contracts;
        }
        /// <summary>
        /// gets the list of mothers
        /// </summary>
        /// <returns></returns>
        public List<Mother> GetMothers()
        {
            return DataSource.Mothers;
        }
        /// <summary>
        /// gets the list of nannies
        /// </summary>
        /// <returns></returns>
        public List<Nanny> GetNannies()
        {
            return DataSource.Nannies;
        }
        /// <summary>
        /// removes a child
        /// </summary>
        /// <param name="c">child to remove</param>
        public void RemoveChild(Child c)
        {
            string ContractNumber = "";
            IEnumerable<string> srNums = from Contract con in DataSource.Contracts
                                         where con.ChildID == c.ID
                                         select con.SerialNumber;
            //foreach (Contract con in DataSource.Contracts)
            //{
            //    if (con.ChildID == c.ID)
            //        ContractNumber += "\n" + con.SerialNumber;
            //}
            foreach (string srNum in srNums)
                ContractNumber += "\n" + srNum;
            if (ContractNumber == "")
                DataSource.Children.Remove(c);
            else
                throw new Exception("Child still involved in contracts. Serial numbers:" + ContractNumber);
        }
        /// <summary>
        /// removes a contract
        /// </summary>
        /// <param name="c">contract to remove</param>
        public void RemoveContract(Contract c)
        {
            DataSource.Contracts.Remove(c);
        }
        /// <summary>
        /// removes a mother
        /// </summary>
        /// <param name="m">mother to remove</param>
        public void RemoveMother(Mother m)
        {
            string childrenID = "";
            IEnumerable<string> IDs = from Child ch in DataSource.Children
                                         where ch.MotherID == m.ID
                                         select ch.ID;
            //foreach(Child ch in DataSource.Children)
            //{
            //    if (ch.MotherID == m.ID)
            //        childrenID += "\n" + ch.ID;
            //}
            foreach (string id in IDs)
                childrenID += "\n" + id;
            if (childrenID == "")
                DataSource.Mothers.Remove(m);
            else
                throw new Exception("Mother still has children. IDs:" + childrenID);
        }
        /// <summary>
        /// removes a nanny
        /// </summary>
        /// <param name="n">nanny to remove</param>
        public void RemoveNanny(Nanny n)
        {
            string ContractNumber = "";
            IEnumerable<string> serialInvolveNanny = from Contract con in DataSource.Contracts
                                                 where con.NannyID == n.ID
                                                 select con.SerialNumber;
            //foreach (Contract con in DataSource.Contracts)
            //{
            //    if (con.NannyID == n.ID)
            //        ContractNumber += "\n" + con.SerialNumber;
            //}
            foreach (string srNum in serialInvolveNanny)
                ContractNumber += "\n" + srNum;
            if (ContractNumber == "")
                DataSource.Nannies.Remove(n);
            else
                throw new Exception("Nanny still has contracts. Serial numbers:" + ContractNumber);
        }
        /// <summary>
        /// updates a property for a child
        /// </summary>
        /// <param name="c">the child to update</param>
        /// <param name="prop">the property to update (if it is SpecialNeeds, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        public void UpdateChild(Child c, Child.Props prop, object newVal)
        {
            switch (prop)
            {
                //case Child.Props.BirthDate:
                //    if (newVal is DateTime)
                //        c.BirthDate = (DateTime)newVal;
                //    else
                //        throw new Exception("wrong type for the 3rd parameter");
                //    break;
                case Child.Props.FirstName:
                    if (newVal is string)
                        c.FirstName = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Child.Props.HaveSpecialNeeds:
                    if (newVal is bool)
                    {
                        c.HaveSpecialNeeds = (bool)newVal;
                        if (!c.HaveSpecialNeeds)
                            c.SpecialNeeds.Clear(); // if the child has no special needs, that means the list of them makes no sense
                    }
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                //case Child.Props.ID:
                //    if (newVal is string)
                //    {
                //        foreach (Nanny ch in DataSource.Nannies)
                //            if (ch.ID == c.ID)
                //                throw new Exception("ID already exists!");
                //        foreach (Child ch in DataSource.Children)
                //            if (ch != c && ch.ID == c.ID)
                //                throw new Exception("ID already exists!");
                //        foreach (Mother ch in DataSource.Mothers)
                //            if (ch.ID == c.ID)
                //                throw new Exception("ID already exists!");
                //        c.ID = (string)newVal;
                //    }
                //    else
                //        throw new Exception("wrong type for the 3rd parameter");
                //    break;
                case Child.Props.LastName:
                    if (newVal is string)
                        c.LastName = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Child.Props.MotherID:
                    if (newVal is string)
                        foreach (Mother ch in DataSource.Mothers)
                            if (ch.ID == (string)newVal)
                            {
                                c.MotherID = (string)newVal;
                                break;
                            }
                            else
                                throw new Exception("mother doesn't exist");
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Child.Props.SpecialNeeds:
                    if (newVal is string)
                    {
                        c.SpecialNeeds.Add((string)newVal);
                        c.HaveSpecialNeeds = true;              // else the added speical need makes no sense. Note that this is actually a kind of "explicit binding" between the 2 fields
                    }
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
            }
            
        }
        /// <summary>
        /// returns the child who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        public Child FindChildByID(string id)
        {
            foreach (Child ch in DataSource.Children)
                if (ch.ID == id)
                    return ch;
            return null;
        }
        /// <summary>
        /// returns the mother who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        public Mother FindMotherByID(string id)
        {
            foreach (Mother m in DataSource.Mothers)
                if (m.ID == id)
                    return m;
            return null;
        }
        /// <summary>
        /// returns the nanny who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        public Nanny FindNannyByID(string id)
        {
            foreach (Nanny n in DataSource.Nannies)
                if (n.ID == id)
                    return n;
            return null;
        }
        /// <summary>
        /// updates a property for a nanny
        /// </summary>
        /// <param name="n">the nanny to update</param>
        /// <param name="prop">the property to update (if it is Recommendations, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        public void UpdateNanny(Nanny n, Nanny.Props prop, object newVal)
        {
            switch(prop)
            {
                case Nanny.Props.Address:
                    if (newVal is string)
                        n.Address = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                //case Nanny.Props.BirthDate:
                //    if (newVal is DateTime)
                //        n.BirthDate = (DateTime)newVal;
                //    else
                //        throw new Exception("wrong type for the 3rd parameter");
                //    break;
                case Nanny.Props.Elevator:
                    if (newVal is bool)
                        n.Elevator = (bool)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.Expertise:
                    if (newVal is int)
                        n.Expertise = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.FirstName:
                    if (newVal is string)
                        n.FirstName = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.Floor:
                    if (newVal is int)
                        n.Floor = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.HourSalary:
                    if (newVal is int)
                        n.HourSalary = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                //case Nanny.Props.ID:
                //    if (newVal is string)
                //    {
                //        foreach (Nanny ch in DataSource.Nannies)
                //            if (n != ch && ch.ID == n.ID)
                //                throw new Exception("ID already exists!");
                //        foreach (Child ch in DataSource.Children)
                //            if (ch.ID == n.ID)
                //                throw new Exception("ID already exists!");
                //        foreach (Mother ch in DataSource.Mothers)
                //            if (ch.ID == n.ID)
                //                throw new Exception("ID already exists!");
                //        n.ID = (string)newVal;
                //    }
                //    else
                //        throw new Exception("wrong type for the 3rd parameter");
                //    break;
                case Nanny.Props.IsCostByHour:
                    if (newVal is bool)
                        n.IsCostByHour = (bool)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.LastName:
                    if (newVal is string)
                        n.LastName = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.MaxAge:
                    if (newVal is int)
                        n.MaxAge = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.MaxChildren:
                    if (newVal is int)
                        n.MaxChildren = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.MinAge:
                    if (newVal is int)
                        n.MinAge = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.MonthSalary:
                    if (newVal is int)
                        n.MonthSalary = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.Phone:
                    if (newVal is string)
                        n.Phone = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.Recommendations:
                    if (newVal is string)
                        n.Recommendations.Add((string)newVal);
                    break;
                case Nanny.Props.VacationByMinisterOfEducation:
                    if (newVal is bool)
                        n.VacationByMinisterOfEducation = (bool)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.WorkDays:
                    if (newVal is bool[] && ((bool[])newVal).Length == 7)
                        for (int i = 0; i < 7; i++)
                            n.WorkDays[i] = ((bool[])newVal)[i];
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Nanny.Props.WorkHours:
                    DateTime[,] temp = newVal as DateTime[,];
                    if (temp != null && temp.GetLength(0) == 2 && temp.GetLength(1) == 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            n.WorkHours[0, i] = temp[0, i];
                            n.WorkHours[1, i] = temp[1, i];
                        }
                    }
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;

            }
        }
        /// <summary>
        /// updates a property for a mother
        /// </summary>
        /// <param name="m">the mother to update</param>
        /// <param name="prop">the property to update (if it is Comments, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        public void UpdateMother(Mother m, Mother.Props prop, object newVal)
        {
            switch(prop)
            {
                case Mother.Props.Address:
                    if (newVal is string)
                        m.Address = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Mother.Props.Comments:
                    if (newVal is string)
                       m.Comments.Add((string)newVal);
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Mother.Props.DaysNeeded:
                    if (newVal is bool[] && ((bool[])newVal).Length == 7)
                        for (int i = 0; i < 7; i++)
                            m.DaysNeeded[i] = ((bool[])newVal)[i];
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Mother.Props.FirstName:
                    if (newVal is string)
                        m.FirstName = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Mother.Props.HoursNeeded:
                    DateTime[,] temp = newVal as DateTime[,];
                    if (temp != null && temp.GetLength(0) == 2 && temp.GetLength(1) == 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            m.HoursNeeded[0, i] = temp[0, i];
                            m.HoursNeeded[1, i] = temp[1, i];
                        }
                    }
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                //case Mother.Props.ID:
                //    if (newVal is string)
                //    {
                //        foreach (Nanny ch in DataSource.Nannies)
                //            if (ch.ID == m.ID)
                //                throw new Exception("ID already exists!");
                //        foreach (Child ch in DataSource.Children)
                //            if (ch.ID == m.ID)
                //                throw new Exception("ID already exists!");
                //        foreach (Mother ch in DataSource.Mothers)
                //            if (ch != m && ch.ID == m.ID)
                //                throw new Exception("ID already exists!");
                //        m.ID = (string)newVal;
                //    }
                //    else
                //        throw new Exception("wrong type for the 3rd parameter");
                //    break;
                case Mother.Props.LastName:
                    if (newVal is string)
                        m.LastName = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Mother.Props.NeedNannyAddress:
                    if (newVal is string)
                        m.NeedNannyAddress = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Mother.Props.Phone:
                    if (newVal is string)
                        m.Phone = (string)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;

            }
        }
        /// <summary>
        /// updates a property for a contract
        /// </summary>
        /// <param name="m">the contract to update</param>
        /// <param name="prop">the property to update </param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        public void UpdateContract(Contract c, Contract.Props prop, object newVal)
        {
            switch (prop)
            {
                case Contract.Props.Beginning:
                    if (newVal is DateTime)
                        c.Beginning = (DateTime)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Contract.Props.ChildID:
                    if (newVal is string)
                        foreach (Child ch in DataSource.Children)
                        {
                            if (ch.ID == (string)newVal)
                            {
                                c.ChildID = (string)newVal;
                                break;
                            }
                            //else
                            //    throw new Exception("child doesn't exist");
                        }
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Contract.Props.End:
                    if (newVal is DateTime)
                        c.End = (DateTime)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Contract.Props.IsByHour:
                    if (newVal is bool)
                        c.IsByHour = (bool)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Contract.Props.IsMet:
                    if (newVal is bool)
                        c.IsMet = (bool)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                //case Contract.Props.IsSigned:
                //    if (newVal is bool)
                //        c.IsSigned = (bool)newVal;
                //    else
                //        throw new Exception("wrong type for the 3rd parameter");
                //    break;
                case Contract.Props.NannyID:
                    if (newVal is string)
                        foreach (Nanny ch in DataSource.Nannies)
                            if (ch.ID == (string)newVal)
                            {
                                c.NannyID = (string)newVal;
                                break;
                            }
                            else
                                throw new Exception("nanny doesn't exist");
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Contract.Props.PerHour:
                    if (newVal is int)
                        c.PerHour = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
                case Contract.Props.PerMonth:
                    if (newVal is int)
                        c.PerMonth = (int)newVal;
                    else
                        throw new Exception("wrong type for the 3rd parameter");
                    break;
            }

        }
        /// <summary>
        /// Initializes some variables into the data source
        /// </summary>
        public void InitSomeVars()
        {
            DateTime[,] hours1 = new DateTime[,] { { default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8) }, { default(DateTime).AddHours(15), default(DateTime).AddHours(15), default(DateTime).AddHours(19), default(DateTime).AddHours(19), default(DateTime).AddHours(19), default(DateTime).AddHours(19) } },
                hours2 = new DateTime[,] { { default(DateTime).AddHours(15), default(DateTime).AddHours(8), default(DateTime).AddHours(9), default(DateTime).AddHours(10), default(DateTime).AddHours(8), default(DateTime).AddHours(8) }, { default(DateTime).AddHours(23), default(DateTime).AddHours(15), default(DateTime).AddHours(19), default(DateTime).AddHours(18), default(DateTime).AddHours(16), default(DateTime).AddHours(19) } };
            List<string> recs1 = new List<string>(), coms1 = new List<string>();
            coms1.Add("a comment...");
            recs1.Add("very patient");
            recs1.Add("especially good with the youngest ones");
            Nanny n = new Nanny("123", "sara", "levi", "052-000-00-01", "Bialik, Ramat-Gan, Israel", new DateTime(1990, 1, 1), true, true, true, 5, 1, 4, 20, 20, 2000, 3, new bool[] { true, true, true, true, true, false, false }, hours1, recs1);
            Nanny n1 = new Nanny("1234", "rivka", "levi", "052-000-50-01", "Shaanan, Ramat-Gan, Israel", new DateTime(1990, 1, 1), true, false, true, 4, 6, 1, 12, 40, 3000, 3, new bool[] { false, true, true, true, true, true, true }, hours2, recs1);
            Mother m = new Mother("001", "leia", "organa", "058-012-34-56", "Hertzel, Tel-Aviv, Israel", "", new bool[] { true, true, true, true, true, false, false }, hours2, coms1);
            Child c1 = new Child("901", "001", "ruben", "organa", null, new DateTime(2017, 5, 1)),
                c2 = new Child("902", "001", "simon", "organa", null, new DateTime(2017, 5, 5));
            Contract con1 = new Contract(n.ID, c1.ID, false, new DateTime(2018, 1, 1), new DateTime(2018, 3, 1)),
                con2 = new Contract(n.ID, c2.ID, true, new DateTime(2018, 1, 5), new DateTime(2018, 3, 5));
            //SignContract(con2);
            AddNanny(n);
            AddNanny(n1);
            AddMother(m);
            AddChild(c1);
            AddChild(c2);
            AddContract(con1);
            AddContract(con2);
        }

        public void ClearListString(object target)
        {
            throw new NotImplementedException();
        }
    }
}
