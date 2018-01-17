using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    /// <summary>
    /// all functions that deal with business logic
    /// </summary>
    public interface IBL
    {
        /// <summary>
        /// adds a nanny
        /// </summary>
        /// <param name="n">the nanny to add</param>
        void AddNanny(Nanny n);
        /// <summary>
        /// removes a nanny
        /// </summary>
        /// <param name="n">the nanny to remove</param>
        void RemoveNanny(Nanny n);
        //void UpdateNanny(Nanny n, Nanny updatedNanny);
        /// <summary>
        /// adds a mother
        /// </summary>
        /// <param name="m">the mother to add</param>
        void AddMother(Mother m);
        /// <summary>
        /// removes a mother
        /// </summary>
        /// <param name="m">the mother to remove</param>
        void RemoveMother(Mother m);
        /// <summary>
        /// adds a child
        /// </summary>
        /// <param name="c">the child to add</param>
        void AddChild(Child c);
        /// <summary>
        /// removes a child
        /// </summary>
        /// <param name="c">the child to remove</param>
        void RemoveChild(Child c);
        /// <summary>
        /// adds a contract
        /// </summary>
        /// <param name="c">the contract to add</param>
        void AddContract(Contract c);
        /// <summary>
        /// removes a contract
        /// </summary>
        /// <param name="c">the contract to remove</param>
        void RemoveContract(Contract c);
                /// <summary>
        /// updates a property for a child
        /// </summary>
        /// <param name="c">the child to update</param>
        /// <param name="prop">the property to update (if it is SpecialNeeds, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        void UpdateChild(Child c, Child.Props prop, object newVal);
         /// <summary>
        /// updates a property for a nanny
        /// </summary>
        /// <param name="n">the nanny to update</param>
        /// <param name="prop">the property to update (if it is Recommendations, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        void UpdateNanny(Nanny n, Nanny.Props prop, object newVal);
        /// <summary>
        /// updates a property for a mother
        /// </summary>
        /// <param name="m">the mother to update</param>
        /// <param name="prop">the property to update (if it is Comments, it will ADD the new value to the list)</param>
        /// <param name="newVal">the new value for the property</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        void UpdateMother(Mother m, Mother.Props prop, object newVal);
        /// <summary>
        /// updates a property for a contract
        /// </summary>
        /// <param name="m">the contract to update</param>
        /// <param name="prop">the property to update </param>
        /// <param name="newVal">the new value for the property</param>
        /// <param name="TerminateLoop">if set on true, it will not use calculate salary in the end, used to prevent recurisve calls</param>
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        void UpdateContract(Contract c, Contract.Props prop, object newVal, bool TerminateLoop = false);
         /// <summary>
         /// gets the list of nannies
         /// </summary>
         /// <returns></returns>
        List<Nanny> GetNannies();
        /// <summary>
        /// gets the list of mothers
        /// </summary>
        /// <returns></returns>
        List<Mother> GetMothers();
        /// <summary>
        /// gets the list of children
        /// </summary>
        /// <returns></returns>
        List<Child> GetChildren();
         /// <summary>
        /// gets the list of contracts
        /// </summary>
        /// <returns></returns>
        List<Contract> GetContracts();
         /// <summary>
        /// returns the child who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        Child FindChildByID(string id);
        /// <summary>
        /// returns the mother who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        Mother FindMotherByID(string id);
        /// <summary>
        /// returns the nanny who has the parameter ID (returns null if not found)
        /// </summary>
        /// <param name="id">ID to search</param>
        /// <returns></returns>
        Nanny FindNannyByID(string id);
         /// <summary>
        /// returns the contract who has the parameter number (returns null if not found)
        /// </summary>
        /// <param name="id">number to search</param>
        /// <returns></returns>
        Contract FindContractBySerialNum(string serNum);
           /// <summary>
        /// signs a contract if possible
        /// </summary>
        /// <param name="c">contract to sign</param>
        void SignContract(Contract c);
         /// <summary>
        /// calculates and returns distance between 2 addresses
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <returns></returns>
        int Distance(string address1, string address2);
          /// <summary>
        /// returns all nannies that can work all the required hours
        /// </summary>
        /// <param name="m">the mother which needs the nanny</param>
        /// <returns></returns>
        IEnumerable<Nanny> SuitableNannies(Mother m);
         /// <summary>
        /// returns the top 5 closest to suitable nannies for the parameter mother
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        Nanny[] Top5Nannies(Mother m);
           /// <summary>
        /// returns all nannies that are in close proximity (30km) to the mother's required location
        /// </summary>
        /// <param name="m">the mother in question</param>
        /// <returns></returns>
        List<Nanny> ProximityNannies(Mother m);
         /// <summary>
        /// returns an IEnumerable of all children who have no nanny
        /// </summary>
        /// <returns>list of all children who have no nanny</returns>
        IEnumerable<Child> UnattendedChildren();
        /// <summary>
        /// returns all Nannies that get vacations according to the minister of economy
        /// </summary>
        /// <returns></returns>
        IEnumerable<Nanny> VacationByEconomy();
          /// <summary>
        /// returns all contracts that match a given condition
        /// </summary>
        /// <param name="check">the condition for the contracts</param>
        /// <returns></returns>
        IEnumerable<Contract> ContractsByCondition(Func<Contract,bool> check);
           /// <summary>
        /// returns how many contracts match a given condition
        /// </summary>
        /// <param name="check">condition to match</param>
        /// <returns></returns>
        int NumOfContractsByCondition(Func<Contract, bool> check);
        /// <summary>
        /// returns the nannies grouped according to the min/max age they accept
        /// </summary>
        /// <param name="sort">true = sort groups by minimal age, false = sort groups by maximal age</param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, Nanny>> NannyByMinimalAge(bool sort = false);
        /// <summary>
        /// returns the contracts grouped by distances between the nanny and the work place
        /// </summary>
        /// <param name="sort">"true" will result in the groups being sorted by distance as well</param>
        /// <returns></returns>
        List<IGrouping<int, Contract>> ContractsByDistance(bool sort = false);
        /// <summary>
        /// returns a short version of the string of the nanny
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        string ToShortString(Nanny n);
        /// <summary>
        /// returns a short version of the string of the mother
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        string ToShortString(Mother m);
        /// <summary>
        /// returns a short version of the string of the child
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        string ToShortString(Child c);
         /// <summary>
        /// returns a short version of the string of the contract
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        string ToShortString(Contract c);
        /// <summary>
        /// returns an intersection of the hours in which the nanny can work for the mother
        /// </summary>
        /// <param name="n">the nanny</param>
        /// <param name="m">the mother</param>
        /// <returns>a matrix of the possible hours</returns>
        DateTime[,] IntersectHours(Nanny n, Mother m);
        /// <summary>
        /// returns the total number of hours in the matrix
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        int TotalHours(DateTime[,] hours);
         /// <summary>
        /// Initializes some variables into the xml data source if it is empty
        /// </summary>
        void InitSomeVars();
        void ClearListString(object target);
    }
}
