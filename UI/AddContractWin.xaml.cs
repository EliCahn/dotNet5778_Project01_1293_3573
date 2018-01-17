using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using BE;

namespace UI
{
    /// <summary>
    /// Interaction logic for AddContractWin.xaml
    /// </summary>
    public partial class ContractWin : Window
    {
        Contract tempContract = new Contract();

        public ContractWin()
        {
            InitializeComponent();
            ContGrid.DataContext = tempContract;
            SerNumComboBox.Items.Add("New Contract");
            foreach (Contract con in MainWindow.bl.GetContracts())
                SerNumComboBox.Items.Add(con.SerialNumber);
            SerNumComboBox.SelectedIndex = 0;
            foreach (Child c in MainWindow.bl.GetChildren())
                childIDTextBox.Items.Add(c.ID);
            nannyIDTextBox.IsEnabled = false;
            UpdateBTN.IsEnabled = false;
            RemoveBTN.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource contractViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("contractViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // contractViewSource.Source = [generic data source]
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    BE.Contract c = new BE.Contract(nannyIDTextBox.Text, childIDTextBox.Text, (bool)isMetCheckBox.IsChecked, beginningDatePicker.SelectedDate.Value, endDatePicker.SelectedDate.Value);
            //    MainWindow.bl.AddContract(c);
            //    ((MainWindow)Application.Current.MainWindow).ContractLST.Items.Add(MainWindow.bl.ToShortString(c));                
            //    Close();
            //}
            //catch (Exception excep)
            //{
            //    MessageBox.Show("ERROR: " + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            try
            {
                if (childIDTextBox.SelectedIndex == -1 || nannyIDTextBox.SelectedIndex == -1 || (string)nannyIDTextBox.SelectedItem == "No match found!")
                    throw new Exception("One or more essential fields are empty");
                Contract c = new Contract(tempContract);
                c.NannyID = nannyIDTextBox.SelectedItem.ToString();
                c.ChildID = childIDTextBox.SelectedItem.ToString();
                MainWindow.bl.AddContract(c);
                SerNumComboBox.Items.Add(c.SerialNumber);
                SerNumComboBox.SelectedIndex = 0;                
                if (MessageBox.Show("Contract added succesfully, do you wish to stay on the Contract window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to create contract:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("The contract will be completely removed\nThis action can't be undone.", "Verfication", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    MainWindow.bl.RemoveContract(MainWindow.bl.FindContractBySerialNum(tempContract.SerialNumber));
                    Close();
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to remove contract:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contract target = MainWindow.bl.FindContractBySerialNum(SerNumComboBox.SelectedItem.ToString());
                MainWindow.bl.UpdateContract(target, Contract.Props.Beginning, tempContract.Beginning);
                MainWindow.bl.UpdateContract(target, Contract.Props.End, tempContract.End);
                MainWindow.bl.UpdateContract(target, Contract.Props.IsMet, tempContract.IsMet);
                if (MessageBox.Show("Update was succesful, do you wish to stay on the Contract window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to update contract:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void childIDTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)(SerNumComboBox.SelectedItem) != "New Contract" || childIDTextBox.SelectedIndex == -1)
                return;
            //object[] ThreadArgs = new object[2]; // will be used as arguments for the thread
            nannyIDTextBox.Items.Clear();
            //nannyIDTextBox.Items.Add("No match found!");
            //nannyIDTextBox.SelectedIndex = 0;
            //nannyIDTextBox.IsEnabled = false;
            //if (childIDTextBox.SelectedIndex == -1)
            //{
            //    nannyIDTextBox.IsEnabled = false;
            //    return;
            //}
            Mother MotherOfContract = MainWindow.bl.FindMotherByID(MainWindow.bl.FindChildByID(childIDTextBox.SelectedItem.ToString()).MotherID);
            //ThreadArgs[1] = MotherOfContract;
            //ThreadArgs[0] = MainWindow.bl.SuitableNannies(MotherOfContract);
            //(new Thread(ThreadNanniesToComboBOX)).Start(ThreadArgs); // in this thread the nannies found are intersected with the nannies in proximity to find nannies who work at all needed hours and also are in proximity
            //if (nannyIDTextBox.Items.Count > 0)
            //    return;
            //ThreadArgs[0] = MainWindow.bl.Top5Nannies(MotherOfContract);
            //(new Thread(ThreadNanniesToComboBOX)).Start(ThreadArgs); // in this thread the nannies found are intersected with the nannies in proximity to find the best 5 nannies (according to their schedule) that also are in proximity
            //if (nannyIDTextBox.Items.Count > 0)
            //    return;
            //IEnumerable<Nanny> NanniesWithHours = from Nanny n in MainWindow.bl.GetNannies() where MainWindow.bl.TotalHours(MainWindow.bl.IntersectHours(n, MotherOfContract)) > 0 select n;            
            //ThreadArgs[0] = NanniesWithHours;
            Thread t = new Thread(ThreadNanniesToComboBOX);
            t.Start(MotherOfContract);
            //(new Thread(ThreadNanniesToComboBOX)).Start(ThreadArgs); // in this thread the nannies found are intersected with the nannies in proximity to find nannies who work at least 1 needed hour and also are in proximity           
            //IEnumerable<Nanny> NanniesWithHoursAndWithingProximity = NanniesWithHours.Intersect(MainWindow.bl.ProximityNannies(MotherOfContract));
            //foreach (Nanny n in NanniesWithHoursAndWithingProximity)
            //    nannyIDTextBox.Items.Add(n.ID);

            //if (nannyIDTextBox.Items.Count == 0)
            //{
            //    nannyIDTextBox.Items.Add("No match found!");
            //    nannyIDTextBox.SelectedIndex = 0;
            //    nannyIDTextBox.IsEnabled = false;
            //    MessageBox.Show("There is no nanny in 30km proximity of the required destination who works in part of the required hours!", "No results found!", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

        private void ThreadNanniesToComboBOX(object args)
        {
            try
            {
                Dispatcher.Invoke(() => { childIDTextBox.IsEnabled = false; nannyIDTextBox.IsEnabled = false; }); //Temporary locking changes to this property to prevent disturbances to the thread's work
                Mother MotherOfContract = (Mother)args;
                List<Nanny> proxNannies = MainWindow.bl.ProximityNannies(MotherOfContract).ToList();
                List<Nanny> NanniesWithHoursAndWithingProximity = (from Nanny n in MainWindow.bl.SuitableNannies(MotherOfContract) where proxNannies.Find(nan => nan.ID == n.ID) != null select n).ToList(); /*MainWindow.bl.SuitableNannies(MotherOfContract).Intersect(proxNannies).ToList()*/
                foreach (Nanny n in NanniesWithHoursAndWithingProximity)
                    Dispatcher.Invoke(() => { nannyIDTextBox.Items.Add(n.ID); /*nannyIDTextBox.Items.Remove("No match found!")*/; nannyIDTextBox.SelectedIndex = -1; nannyIDTextBox.IsEnabled = true; });
                // List<Nanny> NanniesWithHours = (from Nanny n in MainWindow.bl.GetNannies() where MainWindow.bl.TotalHours(MainWindow.bl.IntersectHours(n, MotherOfContract)) > 0 select n).ToList();

                //if (NanniesWithHoursAndWithingProximity.Count() > 0)
                //    return;
                NanniesWithHoursAndWithingProximity = (from Nanny n in MainWindow.bl.Top5Nannies(MotherOfContract) where n != null && proxNannies.Find(nan => nan.ID == n.ID) != null select n).ToList(); /*MainWindow.bl.Top5Nannies(MotherOfContract).Intersect(proxNannies).ToList();*/
                //IEnumerable<Nanny> NanniesWithHours = (IEnumerable<Nanny>)(((object[])args)[0]);
                //Mother MotherOfContract = (Mother)(((object[])args)[1]);
                //NanniesWithHoursAndWithingProximity = NanniesWithHours.Intersect(MainWindow.bl.ProximityNannies(MotherOfContract));
                foreach (Nanny n in NanniesWithHoursAndWithingProximity)
                    Dispatcher.Invoke(() => { if (!nannyIDTextBox.Items.Contains(n.ID)) nannyIDTextBox.Items.Add(n.ID); /*nannyIDTextBox.Items.Remove("No match found!")*/; nannyIDTextBox.SelectedIndex = -1; nannyIDTextBox.IsEnabled = true; });
                Dispatcher.Invoke(() =>
                {
                    if (nannyIDTextBox.Items.Count == 0)
                    {
                        nannyIDTextBox.Items.Add("No match found!");
                        nannyIDTextBox.SelectedIndex = 0;
                        nannyIDTextBox.IsEnabled = false;
                    }
                });
                Dispatcher.Invoke(() => childIDTextBox.IsEnabled = true); // Releasing the lock from the first line to enbable switching of the child again
            }
            catch (Exception excep) //Google maps tends to fail from time to time. For these cases the exception must be handles within the thread
            {
                Dispatcher.Invoke(() => { MessageBox.Show("An error has occured while trying to match nannies to the selected child!\nError message: " + excep.Message + "\nTry to restart the Contract Window, or to wait a little before trying to choose the child again", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); nannyIDTextBox.IsEnabled = false; Dispatcher.Invoke(() => childIDTextBox.IsEnabled = true); });
            }
        }

        private void SerNumComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)(SerNumComboBox.SelectedItem) == "New Contract")
            {
                tempContract = new Contract();
                childIDTextBox.IsEnabled = true;
                nannyIDTextBox.IsEnabled = false;
                childIDTextBox.SelectedIndex = -1;
                nannyIDTextBox.Items.Clear();
                AddBTN.IsEnabled = true;
                UpdateBTN.IsEnabled = false;
                RemoveBTN.IsEnabled = false;
                ContGrid.DataContext = tempContract;
            }
            else
            {
                Contract c = MainWindow.bl.FindContractBySerialNum(SerNumComboBox.SelectedItem.ToString());
                tempContract = new Contract(c);
                childIDTextBox.IsEnabled = false;
                nannyIDTextBox.IsEnabled = false;
                childIDTextBox.SelectedItem = tempContract.ChildID;
                nannyIDTextBox.Items.Add(tempContract.NannyID);
                nannyIDTextBox.SelectedItem = tempContract.NannyID;
                AddBTN.IsEnabled = false;
                UpdateBTN.IsEnabled = true;
                RemoveBTN.IsEnabled = true;
                ContGrid.DataContext = tempContract;
            }
        }

        private void SerNumComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (SerNumComboBox.SelectedIndex == 0)
                SerNumComboBox.ToolTip = "You are creating a new contract";
            else
            {
                Contract c = MainWindow.bl.FindContractBySerialNum(SerNumComboBox.SelectedItem.ToString());
                SerNumComboBox.ToolTip = c.ToString();
            }
        }

        private void childIDTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (childIDTextBox.SelectedIndex == -1)
                childIDTextBox.ToolTip = "Choose a child";
            else
            {
                Child c = MainWindow.bl.FindChildByID(childIDTextBox.SelectedItem.ToString());
                childIDTextBox.ToolTip = c.ToString();
            }
        }

        private void nannyIDTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (nannyIDTextBox.SelectedIndex == -1)
                nannyIDTextBox.ToolTip = "Choose a nanny (sorted by number of relevant hours available)";
            else
            {
                Nanny n = MainWindow.bl.FindNannyByID(nannyIDTextBox.SelectedItem.ToString());
                nannyIDTextBox.ToolTip = n.ToString();
            }
        }
    }
}
