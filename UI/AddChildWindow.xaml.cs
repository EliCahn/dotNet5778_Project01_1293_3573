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
    /// Interaction logic for AddChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        Child tempChild = new Child();

        public ChildWindow()
        {
            InitializeComponent();

            ChildGrid.DataContext = tempChild;
            foreach (Child c in MainWindow.bl.GetChildren())
                iDTextBox.Items.Add(c.ID);
            foreach (Mother m in MainWindow.bl.GetMothers())
                motherIDTextBox.Items.Add(m.ID);
            UpdateBTN.IsEnabled = false;
            RemoveBTN.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource childViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("childViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // childViewSource.Source = [generic data source]
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (iDTextBox.Text == null || iDTextBox.Text == "")
                    throw new Exception("Child must have an ID");
                if (firstNameTextBox.Text == "" || lastNameTextBox.Text == "" || motherIDTextBox.SelectedIndex == -1)
                    throw new Exception("one or more essential fields are empty");
                if ((bool)haveSpecialNeedsCheckBox.IsChecked && needs.GetListString().Count == 0)
                    throw new Exception("Either add a special need or uncheck the checkbox");
                if ((!(bool)haveSpecialNeedsCheckBox.IsChecked) && needs.GetListString().Count != 0)
                {
                    if (MessageBox.Show("Speical needs are unchecked. All special needs in the list will be disregarded.\nDo you wish to proceed?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;
                    needs.Clear();
                }
                tempChild.SpecialNeeds.Clear();
                if (tempChild.HaveSpecialNeeds)
                    foreach (string str in needs.GetListString())
                        tempChild.SpecialNeeds.Add(str);
                tempChild.MotherID = motherIDTextBox.SelectedItem.ToString();
                Child c = new Child(tempChild);  // This is to prevent the binding from further altering the Nanny
                //BE.Nanny n = new BE.Nanny(iDTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, phoneTextBox.Text, addressTextBox.Text, birthDateDatePicker.SelectedDate.Value, (bool)elevatorCheckBox.IsChecked, (bool)isCostByHourCheckBox.IsChecked, (bool)vacationByMinisterOfEducationCheckBox.IsChecked, int.Parse(floorTextBox.Text), int.Parse(maxChildrenTextBox.Text), int.Parse(minAgeTextBox.Text), int.Parse(maxAgeTextBox.Text), int.Parse(hourSalaryTextBox.Text), int.Parse(monthSalaryTextBox.Text), int.Parse(expertiseTextBox.Text), days, h.GetMatrix(), null);
                MainWindow.bl.AddChild(c);
                iDTextBox.Items.Add(c.ID);
                iDTextBox.SelectedIndex = -1;
                iDTextBox.Text = "";
                if (MessageBox.Show("Child added succesfully, do you wish to stay on the Children window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
                //((MainWindow)Application.Current.MainWindow).NannyLST.Items.Add(MainWindow.bl.ToShortString(n));
                //ListStringReader lsr = new ListStringReader(this, n.Recommendations);
                //lsr.ShowDialog();

            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to create child:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //try
            //{
            //    BE.Child c = new BE.Child(iDTextBox.Text, motherIDTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, null, birthDateDatePicker.SelectedDate.Value);
            //    MainWindow.bl.AddChild(c);
            //    ((MainWindow)Application.Current.MainWindow).ChildLST.Items.Add(MainWindow.bl.ToShortString(c));
            //    if ((bool)haveSpecialNeedsCheckBox.IsChecked)
            //    {
            //        ListStringReader lsr = new ListStringReader(this, c.SpecialNeeds);
            //        lsr.ShowDialog();
            //    }
            //    Close();
            //}
            //catch (Exception excep)
            //{
            //    MessageBox.Show("ERROR: " + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void IDChanged(object sender, EventArgs e)
        {
            IDChanged();
            //Child c = MainWindow.bl.FindChildByID(iDTextBox.Text);
            //if (c == null)
            //{
            //    tempChild = new Child(iDTextBox.Text);
            //    //NannyGrid.DataContext = tempNanny;
            //    UpdateBTN.IsEnabled = false;
            //    RemoveBTN.IsEnabled = false;
            //    birthDateDatePicker.IsEnabled = true;
            //    motherIDTextBox.IsEnabled = true;
            //    motherIDTextBox.SelectedIndex = -1;
            //    AddBTN.IsEnabled = true;                                
            //    needs.Clear();
            //    motherIDTextBox.SelectedIndex = -1;
            //}
            //else
            //{
            //    tempChild = new Child(c);
            //    UpdateBTN.IsEnabled = true;
            //    RemoveBTN.IsEnabled = true;
            //    AddBTN.IsEnabled = false;
            //    birthDateDatePicker.IsEnabled = false;
            //    motherIDTextBox.IsEnabled = false;
            //    motherIDTextBox.SelectedItem = tempChild.MotherID;
            //    needs.Clear();
            //    foreach (string str in c.SpecialNeeds)
            //        needs.AddString(str);
            //}
            //ChildGrid.DataContext = tempChild;
            //// from here on ensuring that the user knows in advance if his the ID is taken
            //if (MainWindow.bl.FindMotherByID(iDTextBox.Text) != null || MainWindow.bl.FindNannyByID(iDTextBox.Text) != null)
            //{
            //    AddBTN.Content = "ID already exists!!";
            //    AddBTN.FontSize = 13;
            //    AddBTN.IsEnabled = false;
            //}
            //else
            //{
            //    AddBTN.Content = "Add";
            //    AddBTN.FontSize = 20;
            //    //AddBTN.IsEnabled = true;
            //}
        }

        private void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("The child will be completely removed\nThis action can't be undone.", "Verfication", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    MainWindow.bl.RemoveChild(MainWindow.bl.FindChildByID(tempChild.ID));
                    Close();
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to remove child:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (firstNameTextBox.Text == "" || lastNameTextBox.Text == "")
                    throw new Exception("one or more essential fields are empty");
                if ((bool)haveSpecialNeedsCheckBox.IsChecked && needs.GetListString().Count == 0)
                    throw new Exception("Either add a special need or uncheck the checkbox");
                if ((!(bool)haveSpecialNeedsCheckBox.IsChecked) && needs.GetListString().Count != 0)
                {
                    if (MessageBox.Show("Speical needs are unchecked. All special needs in the list will be disregarded.\nDo you wish to proceed?", "Verification", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;
                    needs.Clear();
                }
                Child target = MainWindow.bl.FindChildByID(tempChild.ID);
                MainWindow.bl.UpdateChild(target, Child.Props.FirstName, tempChild.FirstName);
                MainWindow.bl.UpdateChild(target, Child.Props.LastName, tempChild.LastName);
                MainWindow.bl.UpdateChild(target, Child.Props.HaveSpecialNeeds, tempChild.HaveSpecialNeeds);
                MainWindow.bl.ClearListString(target);
                if (tempChild.HaveSpecialNeeds)
                    foreach (string str in needs.GetListString())
                        MainWindow.bl.UpdateChild(target, Child.Props.SpecialNeeds, str);
                if (MessageBox.Show("Update was succesful, do you wish to stay on the Children window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();

            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to update child:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void motherIDTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Mother m = null;
            if (motherIDTextBox.SelectedIndex != -1)
                m = MainWindow.bl.FindMotherByID(motherIDTextBox.SelectedItem.ToString());
            motherIDTextBox.ToolTip = (m == null ? "choose a mother" : m.ToString());
        }

        private void iDTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Child c = MainWindow.bl.FindChildByID(iDTextBox.Text);
            iDTextBox.ToolTip = (c == null ? "new child" : c.ToString());
        }

        private void IDChanged()
        {
            Child c = MainWindow.bl.FindChildByID(iDTextBox.Text);
            if (c == null)
            {
                tempChild = new Child(iDTextBox.Text);
                //NannyGrid.DataContext = tempNanny;
                UpdateBTN.IsEnabled = false;
                RemoveBTN.IsEnabled = false;
                birthDateDatePicker.IsEnabled = true;
                motherIDTextBox.IsEnabled = true;
                motherIDTextBox.SelectedIndex = -1;
                AddBTN.IsEnabled = true;
                needs.Clear();
                motherIDTextBox.SelectedIndex = -1;
            }
            else
            {
                tempChild = new Child(c);
                UpdateBTN.IsEnabled = true;
                RemoveBTN.IsEnabled = true;
                AddBTN.IsEnabled = false;
                birthDateDatePicker.IsEnabled = false;
                motherIDTextBox.IsEnabled = false;
                motherIDTextBox.SelectedItem = tempChild.MotherID;
                needs.Clear();
                foreach (string str in c.SpecialNeeds)
                    needs.AddString(str);
            }
            ChildGrid.DataContext = tempChild;
            // from here on ensuring that the user knows in advance if his the ID is taken
            if (MainWindow.bl.FindMotherByID(iDTextBox.Text) != null || MainWindow.bl.FindNannyByID(iDTextBox.Text) != null)
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
    }
}
