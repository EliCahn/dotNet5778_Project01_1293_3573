using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.IO;
using System.Xml.Linq;

namespace DAL
{
    class Dal_XML_imp : Idal
    {
        static string Address = (Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)).FullName + "\\Resources";
        static XElement ChildrenRoot;
        static XElement MothersRoot;
        static XElement NanniesRoot;
        static XElement ContractsRoot;
        protected enum TypeToLoad { Nanny, Mother, Child, Contract}
        protected void LoadData(TypeToLoad t)
        {
            try
            {
                switch(t)
                {
                    case TypeToLoad.Child:
                        ChildrenRoot = XElement.Load(Address + "\\Children.xml");
                        break;
                    case TypeToLoad.Contract:
                        ContractsRoot = XElement.Load(Address + "\\Contracts.xml");
                        break;
                    case TypeToLoad.Mother:
                        MothersRoot = XElement.Load(Address + "\\Mothers.xml");
                        break;
                    case TypeToLoad.Nanny:
                        NanniesRoot = XElement.Load(Address + "\\Nannies.xml");
                        break;
                }    
            }
            catch (Exception)
            {
                throw new Exception("File upload failed");
            }
        }
        public void AddChild(Child c)
        {
            LoadData();
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
            LoadData(TypeToLoad.Child);
            List<Child> retList;
            try
            {
                retList = (from ch in ChildrenRoot.Elements() select new Child(ch.Element("ID").Value, ch.Element("MotherID").Value, ch.Element("FirstName").Value, ch.Element("LastName").Value, (from need in ch.Element("SpecialNeeds").Elements() select need.Value).ToList(), new DateTime(int.Parse(ch.Element("BirthDate").Element("Year").Value), int.Parse(ch.Element("BirthDate").Element("Month").Value), int.Parse(ch.Element("BirthDate").Element("Day").Value)))).ToList();
                foreach(Child c in retList)
                {
                    c.HaveSpecialNeeds = c.SpecialNeeds.Count != 0;
                }
            }
            catch
            {
                retList = null;
            }
            return retList;
        }

        public List<Contract> GetContracts()
        {
            LoadData(TypeToLoad.Contract);
            List<Contract> retList;
            try
            {
                retList = (from con in ContractsRoot.Elements() select new Contract() {
                    ChildID = con.Element("ChildID").Value,
                    NannyID = con.Element("NannyID").Value,
                    SerialNumber = con.Element("SerialNumber").Value,
                    PerHour = int.Parse(con.Element("PerHour").Value),
                    PerMonth = int.Parse(con.Element("PerMonth").Value),
                    IsByHour = bool.Parse(con.Element("IsByHour").Value),
                    IsMet = bool.Parse(con.Element("IsMet").Value),
                    IsSigned = bool.Parse(con.Element("IsSigned").Value),
                    Beginning = new DateTime(int.Parse(con.Element("Beginning").Element("Year").Value), int.Parse(con.Element("Beginning").Element("Month").Value), int.Parse(con.Element("Beginning").Element("Day").Value)),
                    End = new DateTime(int.Parse(con.Element("End").Element("Year").Value), int.Parse(con.Element("End").Element("Month").Value), int.Parse(con.Element("End").Element("Day").Value)),

                }).ToList();
            }
            catch
            {
                retList = null;
            }
            return retList;
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
