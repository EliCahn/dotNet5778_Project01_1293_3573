using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using GoogleMapsAPI;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

namespace BL
{
    /// <summary>
    /// an implementation of IBL
    /// </summary>
    public class BL_imp : IBL
    {
        static Idal dal = DS.Dal_Factory.Instance;
        protected BL_imp() { }
        static BL_imp instance = null;
        /// <summary>
        /// returns a singleton instance of BL_imp
        /// </summary>
        public static BL_imp Instance
        {
            get
            {
                if (instance == null)
                    instance = new BL_imp();
                return instance;
            }
        }
        /// <summary>
        /// adds a child
        /// </summary>
        /// <param name="c">child to add</param>
        public void AddChild(Child c)
        {
            if (c.BirthDate.CompareTo(DateTime.Today) > 0)
                throw new Exception("Age cannot be below 0");            
            dal.AddChild(c);
        }
        /// <summary>
        /// adds a contract
        /// </summary>
        /// <param name="c">contract to add</param>
        public void AddContract(Contract c)
        {
            if (c.Beginning.CompareTo(c.End) > 0)
                throw new Exception("beginning must come before the end");            
            Nanny NannyInContract = FindNannyByID(c.NannyID);
            Child ChildInContract = FindChildByID(c.ChildID);
            if (NannyInContract == null)
                throw new Exception("nanny doesn't exist");
            if (ChildInContract == null)
                throw new Exception("mother doesn't exist");
            if (Math.Abs((ChildInContract.BirthDate - DateTime.Today).Days) / 30 < NannyInContract.MinAge || Math.Abs((ChildInContract.BirthDate - DateTime.Today).Days) / 30 > NannyInContract.MaxAge)           
                throw new Exception("child's age out of nannies boundries");            
            dal.AddContract(c);
            CalculateSalary(c);
            //Child ChildInContract = FindChildByID(c.ChildID);
            ////if (ChildInContract == null)
            ////    throw new Exception("child not found");            
            ////if (ChildInContract.BirthDate.AddMonths(3).CompareTo(DateTime.Today) < 0)
            ////    throw new Exception("child too young");
            //IEnumerable<Contract> ContractsOfSameMother = from Contract con in dal.GetContracts() // gets all contracts which involve same mother and same nanny excluding this one
            //                                              where con.SerialNumber != c.SerialNumber && con.NannyID == c.NannyID && FindChildByID(con.ChildID).MotherID == ChildInContract.MotherID
            //                                              select con;
            //Nanny NannyInContract = FindNannyByID(c.NannyID);
            //double rate = 1 - 0.02 * ContractsOfSameMother.Count<Contract>();
            //double salary = 0;
            //if (NannyInContract.IsCostByHour)  //calculates salary
            //{
            //    Mother MotherInContract = FindMotherByID(ChildInContract.MotherID);
            //    DateTime[,] ContractHours = IntersectHours(NannyInContract, MotherInContract);
            //    for (int i = 0; i < 6; i++)
            //    {
            //        if (ContractHours[0, i] != default(DateTime))
            //            salary += (ContractHours[1, i] - ContractHours[0, i]).Hours * 4 * NannyInContract.HourSalary;
            //    }
            //}
            //else
            //    salary = NannyInContract.MonthSalary;
            //salary *= rate;
            //c.PerMonth = (int)salary;
            //c.IsByHour = NannyInContract.IsCostByHour;
            //c.PerHour = NannyInContract.HourSalary;
            ////IEnumerable<Contract> ContractsWithSameNanny = from Contract con in dal.GetContracts()
            ////                                               where con.NannyID == c.NannyID && con.SerialNumber != c.SerialNumber
            ////                                               select con;
            ////if ()

        }
        /// <summary>
        /// adds a mother
        /// </summary>
        /// <param name="m">mother to add</param>
        public void AddMother(Mother m)
        {
            for (int i = 0; i < 6; i++)
            {
                if (m.DaysNeeded[i] && m.HoursNeeded[0, i].CompareTo(m.HoursNeeded[1, i]) >= 0)
                    throw new Exception("starting time must be before ending time");
                else if (!m.DaysNeeded[i]) // To make things simple: DaysNeeded is the "key" to HoursNeeded. That means that there will be no hours in a day in which the mother doens't need
                {
                    m.HoursNeeded[0, i] = default(DateTime);
                    m.HoursNeeded[1, i] = default(DateTime);
                }
            }
            dal.AddMother(m);
        }
        /// <summary>
        /// adds a nanny
        /// </summary>
        /// <param name="n">nanny to add</param>
        public void AddNanny(Nanny n)
        {
            if (n.MaxAge <= 0 || n.MinAge <= 0 || n.MaxChildren <= 0 || n.Expertise <= 0 || n.HourSalary <= 0 || n.MonthSalary <= 0)
                throw new Exception("one or more interger values are not positive, even though they have to be");
            if (n.BirthDate.AddYears(18).CompareTo(DateTime.Today) > 0)              
                 throw new Exception("Nanny too young");
            if (n.MaxAge < n.MinAge)
                throw new Exception("maximal age cannot be below minimal age");
            for (int i = 0; i < 6; i++)
            {
                if (n.WorkDays[i] && n.WorkHours[0, i].CompareTo(n.WorkHours[1, i]) >= 0)
                    throw new Exception("starting time must be before ending time");
                else if (!n.WorkDays[i]) // To make things simple: WorkDays is the "key" to WorkHours. That means that there will be no hours in a day in which the nanny doens't work
                {
                    n.WorkHours[0, i] = default(DateTime);
                    n.WorkHours[1, i] = default(DateTime);
                }
            }
            dal.AddNanny(n);
        }
        /// <summary>
        /// returns all contracts that match a given condition
        /// </summary>
        /// <param name="check">the condition for the contracts</param>
        /// <returns></returns>
        public IEnumerable<Contract> ContractsByCondition(Func<Contract, bool> check)
        {
            return from Contract con in dal.GetContracts()
                   where check(con)
                   select con;
        }
        /// <summary>
        /// returns the contracts grouped by distances between the nanny and the work place
        /// </summary>
        /// <param name="sort">"true" will result in the groups being sorted by distance as well</param>
        /// <returns></returns>
        public List<IGrouping<int, Contract>> ContractsByDistance(bool sort = false)
        {
            if (sort)
                return (from Contract c in GetContracts()
                       orderby Distance(FindNannyByID(c.NannyID).Address, (FindMotherByID(FindChildByID(c.ChildID).MotherID).NeedNannyAddress == "" ? FindMotherByID(FindChildByID(c.ChildID).MotherID).Address : FindMotherByID(FindChildByID(c.ChildID).MotherID).NeedNannyAddress))
                       group c by Distance(FindNannyByID(c.NannyID).Address, (FindMotherByID(FindChildByID(c.ChildID).MotherID).NeedNannyAddress == "" ? FindMotherByID(FindChildByID(c.ChildID).MotherID).Address : FindMotherByID(FindChildByID(c.ChildID).MotherID).NeedNannyAddress)) /1000).ToList();
            else
                return (from Contract c in GetContracts()
                       group c by Distance(FindNannyByID(c.NannyID).Address, (FindMotherByID(FindChildByID(c.ChildID).MotherID).NeedNannyAddress == "" ? FindMotherByID(FindChildByID(c.ChildID).MotherID).Address : FindMotherByID(FindChildByID(c.ChildID).MotherID).NeedNannyAddress)) / 1000).ToList();
        }
        /// <summary>
        /// calculates and returns distance between 2 addresses
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <returns></returns>
        public int Distance(string address1, string address2)
        {
            var drivingDirectionRequest = new DirectionsRequest
            {
                TravelMode = TravelMode.Walking,
                Origin = address1,
                Destination = address2,
            };
            DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
            Route route = drivingDirections.Routes.First();
            Leg leg = route.Legs.First();
            return leg.Distance.Value;

        }
        /// <summary>
        /// returns the child who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        public Child FindChildByID(string id)
        {
            return dal.FindChildByID(id);
        }
        /// <summary>
        /// returns the mother who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        public Mother FindMotherByID(string id)
        {
            return dal.FindMotherByID(id);
        }
        /// <summary>
        /// returns the nanny who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        public Nanny FindNannyByID(string id)
        {
            return dal.FindNannyByID(id);
        }
        /// <summary>
        /// gets the list of children
        /// </summary>
        /// <returns></returns>
        public List<Child> GetChildren()
        {
            return dal.GetChildren();
        }
        /// <summary>
        /// gets the list of contracts
        /// </summary>
        /// <returns></returns>
        public List<Contract> GetContracts()
        {
            return dal.GetContracts();
        }
        /// <summary>
        /// gets the list of mothers
        /// </summary>
        /// <returns></returns>
        public List<Mother> GetMothers()
        {
            return dal.GetMothers();
        }
        /// <summary>
        /// gets the list of nannies
        /// </summary>
        /// <returns></returns>
        public List<Nanny> GetNannies()
        {
            return dal.GetNannies();
        }
        /// <summary>
        /// returns the nannies grouped according to the min/max age they accept
        /// </summary>
        /// <param name="sort">true = sort groups by minimal age, false = sort groups by maximal age</param>
        /// <returns></returns>
        public IEnumerable<IGrouping<int, Nanny>> NannyByMinimalAge(bool sort = false)
        {
            if (sort)
                return from Nanny n in GetNannies()
                       orderby n.MinAge
                       group n by n.MinAge / 3;
            else
                return from Nanny n in GetNannies()
                       orderby n.MaxAge
                       group n by n.MaxAge / 3;
        }
        /// <summary>
        /// returns how many contracts match a given condition
        /// </summary>
        /// <param name="check">condition to match</param>
        /// <returns></returns>
        public int NumOfContractsByCondition(Func<Contract, bool> check)
        {
            return ContractsByCondition(check).Count<Contract>();
        }
        /// <summary>
        /// returns all nannies that are in close proximity (30km) to the mother's required location
        /// </summary>
        /// <param name="m">the mother in question</param>
        /// <returns></returns>
        public List<Nanny> ProximityNannies(Mother m)
        {// assuming an "acceptable distance" is 30km
            return (from Nanny n in GetNannies() where Distance(n.Address, (m.NeedNannyAddress == "" ? m.Address : m.NeedNannyAddress)) <= 30000 select n).ToList();
        }
        /// <summary>
        /// removes a child
        /// </summary>
        /// <param name="c">child to remove</param>
        public void RemoveChild(Child c)
        {
            dal.RemoveChild(c);
        }
        /// <summary>
        /// removes a contract
        /// </summary>
        /// <param name="c">contract to remove</param>
        public void RemoveContract(Contract c)
        {
            dal.RemoveContract(c);
        }
        /// <summary>
        /// removes a mother
        /// </summary>
        /// <param name="m">mother to remove</param>
        public void RemoveMother(Mother m)
        {
            dal.RemoveMother(m);
        }
        /// <summary>
        /// removes a nanny
        /// </summary>
        /// <param name="n">nanny to remove</param>
        public void RemoveNanny(Nanny n)
        {
            dal.RemoveNanny(n);
        }
        /// <summary>
        /// returns an IEnumerable of all children who have no nanny
        /// </summary>
        /// <returns>list of all children who have no nanny</returns>
        public IEnumerable<Child> UnattendedChildren()
        {
            return from Child c in GetChildren()
                   where (from Contract con in GetContracts() where con.ChildID == c.ID select con).Count<Contract>() == 0
                   select c;
            //List<Child> retList = GetChildren().ToList();
            //foreach(Child c in retList)
            //{
            //    if ((from Contract con in GetContracts() where con.ChildID == c.ID select con).Count<Contract>() > 0)
            //        retList.Remove(c);
            //}
            //return retList;
        }
        /// <summary>
        /// updates a property for a child
        /// </summary>
        /// <param name="c">the child to update</param>
        /// <param name="prop">the property to update (if it is SpecialNeeds, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        public void UpdateChild(Child c, Child.Props prop, object newVal)
        {//****
            //if (prop == Child.Props.BirthDate)
            //{
            //    if (!(newVal)
            //}
            //    throw new Exception("Age cannot be below 0");
            dal.UpdateChild(c, prop, newVal);
        }
        /// <summary>
        /// returns all Nannies that get vacations according to the minister of economy
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Nanny> VacationByEconomy()
        {
            return from Nanny n in GetNannies() where !n.VacationByMinisterOfEducation select n;
        }
        /// <summary>
        /// returns an intersection of the hours in which the nanny can work for the mother
        /// </summary>
        /// <param name="n">the nanny</param>
        /// <param name="m">the mother</param>
        /// <returns>a matrix of the possible hours</returns>
        public DateTime[,] IntersectHours(Nanny n, Mother m)
        {
            DateTime[,] retMat = new DateTime[2, 6];
            for (int i = 0; i < 6; i++)
                if (n.WorkDays[i] && m.DaysNeeded[i] && n.WorkHours[0, i].CompareTo(m.HoursNeeded[1, i]) < 0)
                {
                    retMat[0, i] = (n.WorkHours[0, i].CompareTo(m.HoursNeeded[0, i]) > 0 ? n.WorkHours[0, i] : m.HoursNeeded[0, i]);
                    retMat[1, i] = (n.WorkHours[1, i].CompareTo(m.HoursNeeded[1, i]) < 0 ? n.WorkHours[1, i] : m.HoursNeeded[1, i]);
                    //if (retMat[0, i].CompareTo(retMat[1,i]) > 0)
                    //{
                    //    retMat[1, i] = retMat[0, i];
                    //}
                }
            return retMat;
        }
        /// <summary>
        /// signs a contract if possible
        /// </summary>
        /// <param name="c">contract to sign</param>
        public void SignContract(Contract c)
        {
            IEnumerable<Contract> ContractsWithSameNanny = from Contract con in dal.GetContracts()
                                                           where con.NannyID == c.NannyID && con.IsSigned && con.SerialNumber != c.SerialNumber
                                                           select con;
            if (ContractsWithSameNanny.Count() < FindNannyByID(c.NannyID).MaxChildren && FindChildByID(c.ChildID).BirthDate.AddMonths(3).CompareTo(DateTime.Today) < 0)
                //c.IsSigned = true;
                UpdateContract(c, Contract.Props.IsSigned, true);
            else
                throw new Exception("Contract can't be signed!");
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
            if ((prop == Nanny.Props.MaxChildren || prop == Nanny.Props.MaxAge || prop == Nanny.Props.MinAge || prop == Nanny.Props.HourSalary || prop == Nanny.Props.MonthSalary || prop == Nanny.Props.Expertise) && (!(newVal is int) || (int)newVal <= 0))
                throw new Exception(prop + "'s value must be a positive integer");
            if (prop == Nanny.Props.MaxAge)
            {
                if (!(newVal is int))
                    throw new Exception("wrong type for the 3rd parameter");
                if ((int)newVal < n.MinAge)
                    throw new Exception("maximal age cannot be below minimal age or wrong parameter type");
                string errMsg = "Contracts exist which contradict the new age limit:";
                IEnumerable<Contract> NannysContracts = from Contract c in GetContracts()
                                                        where c.NannyID == n.ID
                                                        select c;
                foreach (Contract c in NannysContracts)
                {
                    if (Math.Abs((FindChildByID(c.ChildID).BirthDate - DateTime.Today).Days) / 30 > (int)newVal)
                        errMsg += "\n" + c.SerialNumber;
                }
                if (errMsg != "Contracts exist which contradict the new age limit:")
                    throw new Exception(errMsg);
            }
            if (prop == Nanny.Props.MinAge)
            {
                if (!(newVal is int))
                    throw new Exception("wrong type for the 3rd parameter");
                if ((int)newVal > n.MaxAge)
                    throw new Exception("maximal age cannot be below minimal age or wrong parameter type");
                string errMsg = "Contracts exist which contradict the new age limit:";
                IEnumerable<Contract> NannysContracts = from Contract c in GetContracts()
                                                        where c.NannyID == n.ID
                                                        select c;
                foreach (Contract c in NannysContracts)
                {
                    if (Math.Abs((FindChildByID(c.ChildID).BirthDate - DateTime.Today).Days) / 30 < (int)newVal)
                        errMsg += "\n" + c.SerialNumber;
                }
                if (errMsg != "Contracts exist which contradict the new age limit:")
                    throw new Exception(errMsg);
            }
            //&& (!(newVal is int) || (int)newVal < n.MinAge))
            //throw new Exception("maximal age cannot be below minimal age or wrong parameter type");
            //if (prop == Nanny.Props.MinAge && (!(newVal is int) || (int)newVal > n.MaxAge))
            //    throw new Exception("minimal age cannot be above maximal age or wrong parameter type");
           
            if (prop == Nanny.Props.WorkHours)
            {
                DateTime[,] temp = newVal as DateTime[,];
                if (temp == null)
                    throw new Exception("wrong type for the 3rd parameter");
                for (int i = 0; i < 6; i++)
                {
                    if (n.WorkDays[i] && temp[0, i].CompareTo(temp[1, i]) >= 0)
                    {
                        n.WorkDays[i] = false;
                        UpdateNanny(n, Nanny.Props.WorkDays, n.WorkDays);
                        throw new Exception("starting time must be before ending time");                        
                    }
                    else if (!n.WorkDays[i]) // To make things simple: WorkDays is the "key" to WorkHours. That means that there will be no hours in a day in which the nanny doens't work
                    {
                        temp[0, i] = default(DateTime);
                        temp[1, i] = default(DateTime);
                    }
                }
            }
            dal.UpdateNanny(n, prop, newVal);
            //if (prop == Nanny.Props.WorkDays)
            //{   // Since we decided the WorkHours is depended on WorkDays, after a succesful change to the key we need to change the hours too
            //    DateTime[,] temp = (DateTime[,])(n.WorkHours.Clone());
            //    for (int i = 0; i < 6; i++)
            //        if (!n.WorkDays[i])
            //        {
            //            temp[0, i] = default(DateTime);
            //            temp[1, i] = default(DateTime);
            //        }
            //    UpdateNanny(n, Nanny.Props.WorkHours, temp);
            //}
            IEnumerable<Contract> HerContracts = from Contract con in GetContracts() where con.NannyID == n.ID select con;
            foreach (Contract con in HerContracts)
                CalculateSalary(con);
            
           
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
            if (prop == Mother.Props.HoursNeeded)
            {
                DateTime[,] temp = newVal as DateTime[,];
                if (temp == null)
                    throw new Exception("wrong type for the 3rd parameter");
                for (int i = 0; i < 6; i++)
                {
                    if (m.DaysNeeded[i] && temp[0, i].CompareTo(temp[1, i]) >= 0)
                    {
                        m.DaysNeeded[i] = false;
                        UpdateMother(m, Mother.Props.DaysNeeded, m.DaysNeeded);
                        throw new Exception("starting time must be before ending time");
                    }
                    else if (!m.DaysNeeded[i]) // To make things simple: WorkDays is the "key" to WorkHours. That means that there will be no hours in a day in which the nanny doens't work
                    {
                        temp[0, i] = default(DateTime);
                        temp[1, i] = default(DateTime);
                    }
                }
            }
            dal.UpdateMother(m, prop, newVal);
            //if (prop == Mother.Props.DaysNeeded)
            //{   // Since we decided the HoursNeeded is depended on DaysNeeded, after a succesful change to the key we need to change the hours too
            //    DateTime[,] temp = (DateTime[,])(m.HoursNeeded.Clone());
            //    for (int i = 0; i < 6; i++)
            //        if (!m.DaysNeeded[i])
            //        {
            //            temp[0, i] = default(DateTime);
            //            temp[1, i] = default(DateTime);
            //        }
            //    UpdateMother(m, Mother.Props.HoursNeeded, temp);
            //}
            IEnumerable<Contract> HerContracts = from Contract con in GetContracts() where FindChildByID(con.ChildID).MotherID == m.ID select con;
            foreach (Contract con in HerContracts)
                CalculateSalary(con);
        }
        /// <summary>
        /// updates a property for a contract
        /// </summary>
        /// <param name="m">the contract to update</param>
        /// <param name="prop">the property to update </param>
        /// <param name="newVal">the new value for the property</param>
        /// <param name="TerminateLoop">if set on true, it will not use calculate salary in the end, used to prevent recurisve calls</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        public void UpdateContract(Contract c, Contract.Props prop, object newVal, bool TerminateLoop = false)
        {
            Nanny NannyInContract;
            Child ChildInContract;
            switch (prop)
            {                
                case Contract.Props.ChildID:
                    if (!(newVal is string))
                        throw new Exception("wrong type for the 3rd parameter");
                    ChildInContract = FindChildByID((string)newVal);
                    if (ChildInContract == null)
                        throw new Exception("Child doesn't exist");
                    NannyInContract = FindNannyByID(c.NannyID);
                    if (Math.Abs((ChildInContract.BirthDate - DateTime.Today).Days) / 30 < NannyInContract.MinAge || (ChildInContract.BirthDate - DateTime.Today).Days / 30 > NannyInContract.MaxAge)
                        throw new Exception("child's age out of nannies boundries");
                    break;

                case Contract.Props.NannyID:
                    if (!(newVal is string))
                        throw new Exception("wrong type for the 3rd parameter");
                    NannyInContract = FindNannyByID((string)newVal);
                    if (NannyInContract == null)
                        throw new Exception("Nanny doesn't exist");
                    break;

                case Contract.Props.Beginning:
                    if (newVal is DateTime)
                    {
                        if (((DateTime)newVal).CompareTo(c.End) > 0)
                            throw new Exception("beginning must come before the end");
                    }
                    break;
                case Contract.Props.End:
                    if (newVal is DateTime)
                    {
                        if (((DateTime)newVal).CompareTo(c.Beginning) < 0)
                            throw new Exception("end must come after the beginning");
                    }
                    break;                    

                case Contract.Props.PerHour:
                    if (newVal is int)
                    {
                        if ((int)newVal <= 0)
                            throw new Exception("salary must be a positive integer");
                    }
                    break;
            }
            //if(prop == Contract.Props.ChildID)
            //{
            //    if (!(newVal is string))
            //        throw new Exception("wrong type for the 3rd parameter");
            //    Child ChildInContract = FindChildByID((string)newVal);
            //    if (ChildInContract == null)
            //        throw new Exception("Child doesn't exist");                
            //}
            //if (prop == Contract.Props.NannyID)
            //{
            //    if (!(newVal is string))
            //        throw new Exception("wrong type for the 3rd parameter");
            //    Nanny NannyInContract = FindNannyByID((string)newVal);
            //    if (NannyInContract == null)
            //        throw new Exception("Nanny doesn't exist");                
            //}
            //if (prop == Contract.Props.Beginning && (!(newVal is DateTime) || ((DateTime)newVal).CompareTo(c.End) > 0))
            //    throw new Exception("beginning must come before the end or wrong parameter type");
            //if (prop == Contract.Props.End && (!(newVal is DateTime) || ((DateTime)newVal).CompareTo(c.Beginning) < 0))
            //    throw new Exception("end must come after the beginning or wrong parameter type");
            //if (prop == Contract.Props.PerHour && (!(newVal is int) || (int)newVal <= 0))
            //    throw new Exception("salary must be a positive integer");
            dal.UpdateContract(c, prop, newVal);
            if (!TerminateLoop && prop != Contract.Props.IsMet && prop != Contract.Props.Beginning && prop != Contract.Props.End && prop != Contract.Props.IsSigned && prop != Contract.Props.PerMonth)
                CalculateSalary(c);

            
        }
        /// <summary>
        /// returns all nannies that can work all the required hours
        /// </summary>
        /// <param name="m">the mother which needs the nanny</param>
        /// <returns></returns>
        public IEnumerable<Nanny> SuitableNannies(Mother m)
        {
            return from Nanny n in GetNannies() where TotalHours(m.HoursNeeded) == TotalHours(IntersectHours(n, m)) select n;
        }
        /// <summary>
        /// returns the total number of hours in the matrix
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public int TotalHours (DateTime[,] hours)
        {
            int ret = 0;
            for (int i = 0; i < 6; i++)
            {
                if (hours[0, i] != default(DateTime))
                    ret += (hours[1, i] - hours[0, i]).Hours;
            }
            return ret;
        }
        /// <summary>
        /// returns the top 5 closest to suitable nannies for the parameter mother
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Nanny[] Top5Nannies(Mother m)
        {
            Nanny[] ret = new Nanny[5];
            IEnumerable<Nanny> suitables = SuitableNannies(m);
            int i = 0;
            foreach (Nanny n in suitables)
            {
                ret[i++] = n;
                if (i == 5)
                    return ret;
            }
            Nanny[] temp = new Nanny[5 - i]; // the remaining not suitable nannies will be here
            foreach (Nanny n in GetNannies())
            {
                if (!(TotalHours(IntersectHours(n, m)) >= TotalHours(m.HoursNeeded))) // if the nanny isn't suitable
                    for (int j = 0; j < 5- i; j++) // finding a position for the nanny n in "temp"
                        if (TotalHours(IntersectHours(n, m)) > 0 && (temp[j] == null || TotalHours(IntersectHours(n, m)) > TotalHours(IntersectHours(temp[j], m)))) // if n is better than temp[j]
                        {
                            for (int k = j; k < 4 - i; k++) // push the whole array forward (last nanny is removed)
                                temp[k + 1] = temp[k];
                            temp[j] = n; // and insert the n nanny
                            break;
                        }
            }
            for (int j = 0; i < 5; i++) // merges ret with temp
                ret[i] = temp[j++];
            //for (int j = 4; j > 0; j--) // the lazy but working way to remove duplicates
            //    if (ret[j] == ret[j - 1])
            //        ret[j] = null;
            return ret;
        }        
        /// <summary>
        /// calculates the salary that the nanny will get for the parameter contract
        /// </summary>
        /// <param name="c">contract</param>
        void CalculateSalary(Contract c)
        {
            Child ChildInContract = FindChildByID(c.ChildID);
            IEnumerable<Contract> ContractsOfSameMother = from Contract con in dal.GetContracts() // gets all contracts which involve same mother and same nanny excluding this one
                                                          where con.SerialNumber != c.SerialNumber && con.NannyID == c.NannyID && FindChildByID(con.ChildID).MotherID == ChildInContract.MotherID
                                                          select con;
            Nanny NannyInContract = FindNannyByID(c.NannyID);
            double rate = 1 - 0.02 * ContractsOfSameMother.Count<Contract>();
            double salary = 0;
            if (NannyInContract.IsCostByHour)  //calculates salary
            {
                Mother MotherInContract = FindMotherByID(ChildInContract.MotherID);
                DateTime[,] ContractHours = IntersectHours(NannyInContract, MotherInContract);
                for (int i = 0; i < 6; i++)
                {
                    if (ContractHours[0, i] != default(DateTime))
                        salary += (ContractHours[1, i] - ContractHours[0, i]).Hours * 4 * NannyInContract.HourSalary;
                }
            }
            else
                salary = NannyInContract.MonthSalary;
            salary *= rate;
            UpdateContract(c, Contract.Props.PerMonth, (int)salary, true);
            UpdateContract(c, Contract.Props.IsByHour, NannyInContract.IsCostByHour, true);
            UpdateContract(c, Contract.Props.PerHour, NannyInContract.HourSalary, true);
            //c.PerMonth = (int)salary;
            //c.IsByHour = NannyInContract.IsCostByHour;
            //c.PerHour = NannyInContract.HourSalary;
        }
        /// <summary>
        /// Initializes some variables into the data source
        /// </summary>
        public void InitSomeVars()
        {
            dal.InitSomeVars();
            foreach (Contract c in GetContracts())
                CalculateSalary(c);
            //DateTime[,] hours1 = new DateTime[,] { { default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8) }, { default(DateTime).AddHours(15), default(DateTime).AddHours(15), default(DateTime).AddHours(19), default(DateTime).AddHours(19), default(DateTime).AddHours(19), default(DateTime).AddHours(19) } },
            //    hours2 = hours1 = new DateTime[,] { { default(DateTime).AddHours(15), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8), default(DateTime).AddHours(8) }, { default(DateTime).AddHours(23), default(DateTime).AddHours(15), default(DateTime).AddHours(19), default(DateTime).AddHours(19), default(DateTime).AddHours(19), default(DateTime).AddHours(19) } };
            //List<string> recs1 = new List<string>();
            //recs1.Add("very patient");
            //recs1.Add("especially good with the youngest ones");           
            //Nanny n = new Nanny("123", "sara", "levi", "052-000-00-01", "Bialik, Ramat-Gan, Israel", new DateTime(1990, 1, 1), true, true, true, 5, 1, 4, 7, 20, 2000, 3, new bool[] { true, true, true, true, true, false, false }, hours1, recs1);
            //Nanny n1 = new Nanny("1234", "rivka", "levi", "052-000-50-01", "Shaanan, Ramat-Gan, Israel", new DateTime(1990, 1, 1), true, false, true, 4, 6, 1, 12, 40, 3000, 3, new bool[] { false, true, true, true, true, false, false }, hours2, recs1);
            //Mother m = new Mother("001", "leia", "organa", "058-012-34-56", "Hertzel, Tel-Aviv, Israel", "", new bool[] { true, true, true, true, true, false, false }, hours2, null);
            //Child c1 = new Child("901", "001", "ruben", "organa", null, new DateTime(2017, 5, 1)),
            //    c2 = new Child("902", "001", "simon", "organa", null, new DateTime(2017, 5, 5));
            //Contract con1 = new Contract(n.ID, c1.ID, false, new DateTime(2018, 1, 1), new DateTime(2018, 3, 1)),
            //    con2 = new Contract(n.ID, c2.ID, true, new DateTime(2018, 1, 5), new DateTime(2018, 3, 5));
            ////SignContract(con2);
            //AddNanny(n);
            //AddNanny(n1);
            //AddMother(m);
            //AddChild(c1);
            //AddChild(c2);
            //AddContract(con1);
            //AddContract(con2);
        }
        /// <summary>
        /// returns a short version of the string of the nanny
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string ToShortString(Nanny n)
        {
            return n.ID + " | " + n.FirstName + " " + n.LastName;
        }
        /// <summary>
        /// returns a short version of the string of the mother
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string ToShortString(Mother m)
        {
            return m.ID + " | " + m.FirstName + " " + m.LastName;
        }
        /// <summary>
        /// returns a short version of the string of the child
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string ToShortString(Child c)
        {
            return c.ID + " | " + c.FirstName + " " + c.LastName;
        }
        /// <summary>
        /// returns a short version of the string of the contract
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string ToShortString(Contract c)
        {
            Nanny n = FindNannyByID(c.NannyID);
            Child ch = FindChildByID(c.ChildID);
            return c.SerialNumber + " | " + c.NannyID + " with " + c.ChildID;
        }
        /// <summary>
        /// returns the contract who has the parameter number (returns null if not found)
        /// </summary>
        /// <param name="id">number to search</param>
        /// <returns></returns>
        public Contract FindContractBySerialNum(string serNum)
        {
            //foreach (Contract n in GetContracts())
            //    if (n.SerialNumber == serNum)
            //        return n;
            //return null;
            return ContractsByCondition(c => c.SerialNumber == serNum).First();
        }

        public void ClearListString(object target)
        {
            dal.ClearListString(target);
            
        }
    }
}
