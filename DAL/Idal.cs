using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    /// <summary>
    /// has functions to deal with the data source
    /// </summary>
    public interface Idal
    {
        //Idal Instance { get; }
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
        /// <exception cref="System.Exception">thrown if the new value is not of a proper type for the property</exception>
        void UpdateContract(Contract c, Contract.Props prop, object newVal);
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
        /// Initializes some variables into the data source
        /// </summary>
        void InitSomeVars();
        
    }
}
