using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.IO;

namespace DAL
{
    class Dal_XML_imp : Idal
    {
        static string Address = (Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)).FullName + "\\Resources";
        public void AddChild(Child c)
        {
            throw new NotImplementedException();
        }

        public void AddContract(Contract c)
        {
            throw new NotImplementedException();
        }

        public void AddMother(Mother m)
        {
            throw new NotImplementedException();
        }

        public void AddNanny(Nanny n)
        {
            throw new NotImplementedException();
        }

        public Child FindChildByID(string id)
        {
            throw new NotImplementedException();
        }

        public Mother FindMotherByID(string id)
        {
            throw new NotImplementedException();
        }

        public Nanny FindNannyByID(string id)
        {
            throw new NotImplementedException();
        }

        public List<Child> GetChildren()
        {
            throw new NotImplementedException();
        }

        public List<Contract> GetContracts()
        {
            throw new NotImplementedException();
        }

        public List<Mother> GetMothers()
        {
            throw new NotImplementedException();
        }

        public List<Nanny> GetNannies()
        {
            throw new NotImplementedException();
        }

        public void InitSomeVars()
        {
            throw new NotImplementedException();
        }

        public void RemoveChild(Child c)
        {
            throw new NotImplementedException();
        }

        public void RemoveContract(Contract c)
        {
            throw new NotImplementedException();
        }

        public void RemoveMother(Mother m)
        {
            throw new NotImplementedException();
        }

        public void RemoveNanny(Nanny n)
        {
            throw new NotImplementedException();
        }

        public void UpdateChild(Child c, Child.Props prop, object newVal)
        {
            throw new NotImplementedException();
        }

        public void UpdateContract(Contract c, Contract.Props prop, object newVal)
        {
            throw new NotImplementedException();
        }

        public void UpdateMother(Mother m, Mother.Props prop, object newVal)
        {
            throw new NotImplementedException();
        }

        public void UpdateNanny(Nanny n, Nanny.Props prop, object newVal)
        {
            throw new NotImplementedException();
        }
    }
}
