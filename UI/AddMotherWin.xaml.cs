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
using BE;

namespace UI
{
    /// <summary>
    /// Interaction logic for AddMotherWin.xaml
    /// </summary>
    public partial class MotherWin : Window
    {
        Mother tempMother = new Mother();

        public MotherWin()
        {
            InitializeComponent();
            //tempMother = MainWindow.bl.GetMothers()[0];  

            MotherGrid.DataContext = tempMother;
            foreach (Mother m in MainWindow.bl.GetMothers())
                iDTextBox.Items.Add(m.ID);
            UpdateBTN.IsEnabled = false;
            RemoveBTN.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource motherViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("motherViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // motherViewSource.Source = [generic data source]
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (iDTextBox.Text == null || iDTextBox.Text == "")
                    throw new Exception("Mother must have an ID");
                foreach (object txtbx in MotherGrid.Children)
                    if (txtbx is TextBox && ((TextBox)txtbx).Text == "" && (TextBox)txtbx != needNannyAddressTextBox)
                        throw new Exception("one or more essential fields are empty");
                bool[] days = new bool[7];
                days[6] = false;
                //DateTime[,] hours = h.GetMatrix();
                DateTime[,] hours = h.GetMatrix();
                for (int i = 0; i < 6; i++)
                {
                    tempMother.DaysNeeded[i] = (bool)h.Check[i].IsChecked;
                    tempMother.HoursNeeded[0, i] = hours[0, i];
                    tempMother.HoursNeeded[1, i] = hours[1, i];
                }
                foreach (string str in coms.GetListString())
                    tempMother.Comments.Add(str);
                Mother m = new Mother(tempMother);  // This is to prevent the binding from further altering the Nanny
                //BE.Nanny n = new BE.Nanny(iDTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, phoneTextBox.Text, addressTextBox.Text, birthDateDatePicker.SelectedDate.Value, (bool)elevatorCheckBox.IsChecked, (bool)isCostByHourCheckBox.IsChecked, (bool)vacationByMinisterOfEducationCheckBox.IsChecked, int.Parse(floorTextBox.Text), int.Parse(maxChildrenTextBox.Text), int.Parse(minAgeTextBox.Text), int.Parse(maxAgeTextBox.Text), int.Parse(hourSalaryTextBox.Text), int.Parse(monthSalaryTextBox.Text), int.Parse(expertiseTextBox.Text), days, h.GetMatrix(), null);
                MainWindow.bl.AddMother(m);
                iDTextBox.Items.Add(m.ID);
                iDTextBox.SelectedIndex = -1;
                iDTextBox.Text = "";
                if (MessageBox.Show("Nanny added succesfully, do you wish to stay on the Mother window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
                //((MainWindow)Application.Current.MainWindow).NannyLST.Items.Add(MainWindow.bl.ToShortString(n));
                //ListStringReader lsr = new ListStringReader(this, n.Recommendations);
                //lsr.ShowDialog();

            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to create mother:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //try
            //{
            //    bool[] days = new bool[7];
            //    days[6] = false;
            //    //DateTime[,] hours = h.GetMatrix();
            //    for (int i = 0; i < 6; i++)
            //    {
            //        days[i] = (bool)h.Check[i].IsChecked;
            //    }
            //    BE.Mother m = new BE.Mother(iDTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, phoneTextBox.Text, addressTextBox.Text, needNannyAddressTextBox.Text, days, h.GetMatrix(), null);
            //    MainWindow.bl.AddMother(m);
            //    ((MainWindow)Application.Current.MainWindow).MotherLST.Items.Add(MainWindow.bl.ToShortString(m));
            //    ListStringReader lsr = new ListStringReader(this, m.Comments);
            //    lsr.ShowDialog();
            //    Close();
            //}
            //catch (Exception excep)
            //{
            //    MessageBox.Show("ERROR: " + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        public void IDChanged(object sender, EventArgs e)
        {
            IDChanged();
        }

        public void IDChanged()
        {            
            Mother m = MainWindow.bl.FindMotherByID(iDTextBox.Text);
            if (m == null)
            {                
                tempMother = new Mother(iDTextBox.Text);
                //NannyGrid.DataContext = tempNanny;
                UpdateBTN.IsEnabled = false;
                RemoveBTN.IsEnabled = false;
                AddBTN.IsEnabled = true;
                //birthDateDatePicker.IsEnabled = true;
                h.Reset();
                coms.Clear();
            }
            else
            {
                tempMother = new Mother(m);
                
                UpdateBTN.IsEnabled = true;
                RemoveBTN.IsEnabled = true;
                AddBTN.IsEnabled = false;
                //birthDateDatePicker.IsEnabled = false;
                h.Reset();
                h.ManualSetting(tempMother.DaysNeeded, tempMother.HoursNeeded);
                coms.Clear();
                foreach (string str in m.Comments)
                    coms.AddString(str);
            }
            MotherGrid.DataContext = tempMother;
            // from here on ensuring that the user knows in advance if his the ID is taken
            if (MainWindow.bl.FindChildByID(iDTextBox.Text) != null || MainWindow.bl.FindNannyByID(iDTextBox.Text) != null)
            {
                AddBTN.Content = "ID already exists!!";
                AddBTN.FontSize = 13;
                AddBTN.IsEnabled = false;
            }
            else
            {
                AddBTN.Content = "Add";
                AddBTN.FontSize = 20;
                //AddBTN.IsEnabled = true;
            }
        }

        private void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("The mother will be completely removed\nThis action can't be undone.", "Verfication", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    MainWindow.bl.RemoveMother(MainWindow.bl.FindMotherByID(tempMother.ID));
                    Close();
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to remove mother:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (object txtbx in MotherGrid.Children)
                    if (txtbx is TextBox && ((TextBox)txtbx).Text == "" && (TextBox)txtbx != needNannyAddressTextBox)
                        throw new Exception("one or more essential fields are empty");
                Mother target = MainWindow.bl.FindMotherByID(tempMother.ID);
                MainWindow.bl.UpdateMother(target, Mother.Props.Address, tempMother.Address);
                MainWindow.bl.UpdateMother(target, Mother.Props.FirstName, tempMother.FirstName);
                MainWindow.bl.UpdateMother(target, Mother.Props.LastName, tempMother.LastName);
                MainWindow.bl.UpdateMother(target, Mother.Props.Phone, tempMother.Phone);
                MainWindow.bl.UpdateMother(target, Mother.Props.NeedNannyAddress, tempMother.NeedNannyAddress);
                bool[] days = new bool[7];
                for (int i = 0; i < 6; i++)
                    days[i] = (bool)h.Check[i].IsChecked;
                MainWindow.bl.UpdateMother(target, Mother.Props.DaysNeeded, days);
                MainWindow.bl.UpdateMother(target, Mother.Props.HoursNeeded, h.GetMatrix());
                target.Comments.Clear();
                foreach (string str in coms.GetListString())
                    MainWindow.bl.UpdateMother(target, Mother.Props.Comments, str);
                if (MessageBox.Show("Update was succesful, do you wish to stay on the Mother window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to update mother:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void iDTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mother m = MainWindow.bl.FindMotherByID(iDTextBox.Text);
            iDTextBox.ToolTip = (m == null ? "new mother" : m.ToString());
        }
    }
}
