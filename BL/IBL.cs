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
        void UpdateChild(Child c, Child.Props prop, object newVal);
        void UpdateNanny(Nanny n, Nanny.Props prop, object newVal);
        void UpdateMother(Mother m, Mother.Props prop, object newVal);
        void UpdateContract(Contract c, Contract.Props prop, object newVal);
        List<Nanny> GetNannies();
        List<Mother> GetMothers();
        List<Child> GetChildren();
        List<Contract> GetContracts();
        Child FindChildByID(string id);
        Mother FindMotherByID(string id);
        Nanny FindNannyByID(string id);
        Contract FindContractBySerialNum(string serNum);
        void SignContract(Contract c);
        int Distance(string address1, string address2);
        IEnumerable<Nanny> SuitableNannies(Mother m);
        Nanny[] Top5Nannies(Mother m);
        IEnumerable<Nanny> ProximityNannies(Mother m);
        IEnumerable<Child> UnattendedChildren();
        IEnumerable<Nanny> VacationByEconomy();
        IEnumerable<Contract> ContractsByCondition(Func<Contract,bool> check);
        int NumOfContractsByCondition(Func<Contract, bool> check);
        /// <summary>
        /// returns the nannies grouped according to the min/max age they accept
        /// </summary>
        /// <param name="sort">true = sort groups by minimal age, false = sort groups by maximal age</param>
        /// <returns></returns>
        IEnumerable<IGrouping<int, Nanny>> NannyByMinimalAge(bool sort = false);
        IEnumerable<IGrouping<int, Contract>> ContractsByDistance(bool sort = false);
        string ToShortString(Nanny n);
        string ToShortString(Mother m);
        string ToShortString(Child c);
        string ToShortString(Contract c);
        DateTime[,] IntersectHours(Nanny n, Mother m);
        int TotalHours(DateTime[,] hours);
        void InitSomeVars();
    }
}
