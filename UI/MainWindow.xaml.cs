using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;
using BL;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// an static instance of the implememntation of IBL
        /// </summary>
        public static IBL bl = BL_Factory.Instance;
        public MainWindow()
        {
            InitializeComponent();
            bl.InitSomeVars();
            //foreach(Nanny n in bl.GetNannies())            
            //    NannyLST.Items.Add(bl.ToShortString(n));           
            //foreach (Mother m in bl.GetMothers())
            //    MotherLST.Items.Add(bl.ToShortString(m));
            //foreach (Child c in bl.GetChildren())
            //    ChildLST.Items.Add(bl.ToShortString(c));
            //foreach (Contract c in bl.GetContracts())
            //    ContractLST.Items.Add(bl.ToShortString(c));
        }

        private void AddNannyBTN_Click(object sender, RoutedEventArgs e)
        {
            new NannyWin().ShowDialog();
        }

        private void AddMotherBTN_Click(object sender, RoutedEventArgs e)
        {
            new MotherWin().ShowDialog();
        }

        private void AddChildBTN_Click(object sender, RoutedEventArgs e)
        {
            new ChildWindow().ShowDialog();
        }

        private void AddContractBTN_Click(object sender, RoutedEventArgs e)
        {
            new ContractWin().ShowDialog();
        }

        private void ShowNannyBTN_Click(object sender, RoutedEventArgs e)
        {
            //if (NannyLST.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a nanny first");
            //    return;
            //}
            //string id = "";
            //for (int i = 0; i < NannyLST.SelectedItem.ToString().Length && NannyLST.SelectedItem.ToString()[i] != ' '; i++)
            //    id += NannyLST.SelectedItem.ToString()[i];
            //Nanny n = bl.FindNannyByID(id);
            //ShowObjectWin showObjectWin = new ShowObjectWin(n);
            //showObjectWin.ShowDialog();
            ListDisplay listDisplay = new ListDisplay("List of all nannies", "Go to selected nanny's window", bl.GetNannies());
            listDisplay.ShowDialog();
        }

        private void ShowMotherBTN_Click(object sender, RoutedEventArgs e)
        {
            //if (MotherLST.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a mother first");
            //    return;
            //}
            //string id = "";
            //for (int i = 0; i < MotherLST.SelectedItem.ToString().Length && MotherLST.SelectedItem.ToString()[i] != ' '; i++)
            //    id += MotherLST.SelectedItem.ToString()[i];
            //Mother m = bl.FindMotherByID(id);
            //ShowObjectWin showObjectWin = new ShowObjectWin(m);
            //showObjectWin.ShowDialog();
            ListDisplay listDisplay = new ListDisplay("List of all mothers", "Go to selected mother's window", bl.GetMothers());
            listDisplay.ShowDialog();
        }

        private void ShowChildBTN_Click(object sender, RoutedEventArgs e)
        {
            //if (ChildLST.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a child first");
            //    return;
            //}
            //string id = "";
            //for (int i = 0; i < ChildLST.SelectedItem.ToString().Length && ChildLST.SelectedItem.ToString()[i] != ' '; i++)
            //    id += ChildLST.SelectedItem.ToString()[i];
            //Child c = bl.FindChildByID(id);
            //ShowObjectWin showObjectWin = new ShowObjectWin(c);
            //showObjectWin.ShowDialog();
            ListDisplay listDisplay = new ListDisplay("List of all children", "Go to selected child's window", bl.GetChildren());
            listDisplay.ShowDialog();
        }

        private void ShowContractBTN_Click(object sender, RoutedEventArgs e)
        {
            //if (ContractLST.SelectedItem == null)
            //{
            //    MessageBox.Show("Please select a contract first");
            //    return;
            //}
            //string id = "";
            //for (int i = 0; i < ContractLST.SelectedItem.ToString().Length && ContractLST.SelectedItem.ToString()[i] != ' '; i++)
            //    id += ContractLST.SelectedItem.ToString()[i];
            //Contract c = bl.FindContractBySerialNum(id);
            //ShowObjectWin showObjectWin = new ShowObjectWin(c);
            //showObjectWin.ShowDialog();
            ListDisplay listDisplay = new ListDisplay("List of all contracts", "Go to selected contract's window", bl.GetContracts());
            listDisplay.ShowDialog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TextBox temp = new TextBox();
            try
            {
                ChoicePromptWin choicePromptWin = new ChoicePromptWin(bl.ContractsByCondition(c => !c.IsSigned), "Choose contract to sign", c => { bl.SignContract((Contract)c); MessageBox.Show("successfully signed"); });
            
                choicePromptWin.ShowDialog();
                //Contract c = bl.FindContractBySerialNum(temp.Text);
                //if (c == null)
                //{
                //    MessageBox.Show("Contract not found!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
                //bl.SignContract(c);
                
            }
            catch (Exception excep)
            {

                MessageBox.Show("ERROR: " + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<string> Queries = new List<string>();
            // for each of the following queries there should be a case in the switch below            
            Queries.Add("Children who don't have a single contract");
            Queries.Add("Nannies who get their vacation from the minister of economy");
            Queries.Add("Contracts in which the children met the nanny");
            Queries.Add("Contracts that are signed");
            Queries.Add("Mothers for which there is no nanny that can fit 100% with their schedule");
            ListDisplay listDisplay;
            // note that the constructor below has a quite long parameter
            ChoicePromptWin choicePromptWin = new ChoicePromptWin(Queries, "What do you wish to find?", (s) =>
            {
                switch ((string)s)
                { // a case for each of the queries above
                    case "Children who don't have a single contract":
                        listDisplay = new ListDisplay((string)s, "Go to selected child's window", bl.UnattendedChildren());
                        listDisplay.ShowDialog();
                        break;

                    case "Nannies who get their vacation from the minister of economy":
                        listDisplay = new ListDisplay((string)s, "Go to selected nanny's window", bl.VacationByEconomy());
                        listDisplay.ShowDialog();
                        break;

                    case "Contracts in which the children met the nanny":
                        listDisplay = new ListDisplay((string)s, "Go to selected contract's window", bl.ContractsByCondition(c => c.IsMet));
                        listDisplay.ShowDialog();
                        break;

                    case "Contracts that are signed":
                        listDisplay = new ListDisplay((string)s, "Go to selected contract's window", bl.ContractsByCondition(c => c.IsSigned));
                        listDisplay.ShowDialog();
                        break;

                    case "Mothers for which there is no nanny that can fit 100% with their schedule":
                        listDisplay = new ListDisplay((string)s, "Go to selected mother's window", from Mother m in bl.GetMothers() where bl.SuitableNannies(m).Count() == 0 select m);
                        listDisplay.ShowDialog();
                        break;
                }
            }); // This is the end of the constructor above
            choicePromptWin.ShowDialog();
            ///////////////////////////////////////////////////////////////////////// OLD***********************OLD
            //string ret = "Nannies with vaction from the minister of economy:\n";
            //foreach (Nanny n in bl.VacationByEconomy())
            //    ret += bl.ToShortString(n) + "\n";
            //ret += "**************\nUnattended Children:\n";
            //foreach (Child c in bl.UnattendedChildren())
            //    ret += bl.ToShortString(c) +"\n";
            //ret += "***************\nContracts where children met the nanny:\n";
            //foreach (Contract c in bl.ContractsByCondition(c => c.IsMet))
            //    ret += bl.ToShortString(c) + "\n";
            //ret += "***************\nNumber of signed contracts:" + bl.NumOfContractsByCondition(c => c.IsSigned);
            //MessageBox.Show(ret);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<string> Queries = new List<string>();
            GroupedListDisplay groupedlistDisplay;
            // for each of the following queries there should be a case in the switch below            
            Queries.Add("Nannies grouped according to the minimal acceptable age of their children");
            Queries.Add("Nannies grouped according to the maximal acceptable age of their children");
            Queries.Add("Contracts grouped by the distance between the nanny and her location");
            // note that the constructor below has a quite long parameter
            ChoicePromptWin choicePromptWin = new ChoicePromptWin(Queries, "Which group are you looking for?", (s) =>
            {   
                switch ((string)s)
                // a case for each of the queries above
                {
                    case "Nannies grouped according to the minimal acceptable age of their children":                   
                        groupedlistDisplay = new GroupedListDisplay((string)s, "Go to selected Nanny's window", bl.NannyByMinimalAge(true));
                        groupedlistDisplay.ShowDialog();
                        break;
                    case "Nannies grouped according to the maximal acceptable age of their children":
                        groupedlistDisplay = new GroupedListDisplay((string)s, "Go to selected Nanny's window", bl.NannyByMinimalAge(false));
                        groupedlistDisplay.ShowDialog();
                        break;
                    case "Contracts grouped by the distance between the nanny and her location":
                        new Thread(() =>
                        {
                            List<IGrouping<int, Contract>> ListToDisplay = bl.ContractsByDistance(true);
                            Dispatcher.Invoke(() =>
                            {
                                groupedlistDisplay = new GroupedListDisplay((string)s, "Go to selected Contract's window", ListToDisplay);
                                groupedlistDisplay.ShowDialog();
                            });
                        }).Start();
                        break;

                }
            });
            choicePromptWin.ShowDialog();
            ///////////////////////////////////////////////////////////////////////// OLD***********************OLD
            //string param= "";
            //if (MotherLST.SelectedItem != null)
            //    param = MotherLST.SelectedItem.ToString();
            //else
            //{
            //    MessageBox.Show("To test this function, please select a mother first");
            //    return;
            //}
            //Thread t = new Thread(forThread1);
            //t.Start(param);
        }

        //void forThread1(object s)
        //{
        //    string str = (string)s;
        //    string id = "", ret = "Nannies by proximity to chosen mother:\n";
        //    for (int i = 0; i < str.Length && str[i] != ' '; i++)
        //        id += str[i];
        //    Mother m = bl.FindMotherByID(id);
        //    foreach (Nanny n in bl.ProximityNannies(m))
        //        ret += bl.ToShortString(n) + "\n";
        //    ret += "*****************\nPerfectly Suitable Nannies for the mother (hours-wise):\n";
        //    foreach (Nanny n in bl.SuitableNannies(m))
        //        ret += bl.ToShortString(n) + "\n";
        //    ret += "******************\nMost Suitable Nannies (even if imperfect), top 5:\n";
        //    foreach (Nanny n in bl.Top5Nannies(m))
        //        if (n != null)
        //            ret += bl.ToShortString(n) + "\n";
        //    Dispatcher.Invoke(() => MessageBox.Show(ret));
        //    //MessageBox.Show(ret);

        //}

    }    
}
