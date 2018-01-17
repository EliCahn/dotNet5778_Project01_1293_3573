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
    /// Interaction logic for AddNannyWin.xaml
    /// </summary>
    public partial class NannyWin : Window 
    {
        Nanny tempNanny = new Nanny();

        public NannyWin()
        {
            InitializeComponent();
            //tempNanny = MainWindow.bl.GetNannies()[0];  
            
            NannyGrid.DataContext = tempNanny;
            foreach (Nanny n in MainWindow.bl.GetNannies())
                iDTextBox.Items.Add(n.ID);
            UpdateBTN.IsEnabled = false;
            RemoveBTN.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource nannyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("nannyViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // nannyViewSource.Source = [generic data source]
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
                    throw new Exception("Nanny must have an ID");
                foreach (object txtbx in NannyGrid.Children)
                    if (txtbx is TextBox && ((TextBox)txtbx).Text == "")
                        throw new Exception("one or more fields are empty");
                bool[] days = new bool[7];
                days[6] = false;
                //DateTime[,] hours = h.GetMatrix();
                DateTime[,] hours = h.GetMatrix();
                for (int i = 0; i < 6; i++)
                {
                    tempNanny.WorkDays[i] = (bool)h.Check[i].IsChecked;
                    tempNanny.WorkHours[0, i] = hours[0, i];
                    tempNanny.WorkHours[1, i] = hours[1, i];
                }
                foreach (string str in recs.GetListString())
                    tempNanny.Recommendations.Add(str);
                Nanny n = new Nanny(tempNanny);  // This is to prevent the binding from further altering the Nanny
                //BE.Nanny n = new BE.Nanny(iDTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, phoneTextBox.Text, addressTextBox.Text, birthDateDatePicker.SelectedDate.Value, (bool)elevatorCheckBox.IsChecked, (bool)isCostByHourCheckBox.IsChecked, (bool)vacationByMinisterOfEducationCheckBox.IsChecked, int.Parse(floorTextBox.Text), int.Parse(maxChildrenTextBox.Text), int.Parse(minAgeTextBox.Text), int.Parse(maxAgeTextBox.Text), int.Parse(hourSalaryTextBox.Text), int.Parse(monthSalaryTextBox.Text), int.Parse(expertiseTextBox.Text), days, h.GetMatrix(), null);
                MainWindow.bl.AddNanny(n);
                iDTextBox.Items.Add(n.ID);
                iDTextBox.SelectedIndex = -1;
                iDTextBox.Text = "";
                if (MessageBox.Show("Nanny added succesfully, do you wish to stay on the Nanny window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
                //((MainWindow)Application.Current.MainWindow).NannyLST.Items.Add(MainWindow.bl.ToShortString(n));
                //ListStringReader lsr = new ListStringReader(this, n.Recommendations);
                //lsr.ShowDialog();

            }
            catch(Exception excep)
            {
                MessageBox.Show("Failed to create nanny:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void iDTextBox_MouseEnter(object sender, MouseEventArgs e)
        { // this makes the tooltip show the nanny's "to string" details
            Nanny n = MainWindow.bl.FindNannyByID(iDTextBox.Text);
            iDTextBox.ToolTip = (n == null ? "new nanny" : n.ToString());
        }

        private void iDTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //Nanny n = MainWindow.bl.FindNannyByID(iDTextBox.Text);
            //if (n == null)
            //{
            //    tempNanny = new Nanny(iDTextBox.Text);
            //    //NannyGrid.DataContext = tempNanny;
            //    UpdateBTN.IsEnabled = false;
            //    RemoveBTN.IsEnabled = false;
            //    AddBTN.IsEnabled = true;
            //    birthDateDatePicker.IsEnabled = true;
            //}
            //else
            //{
            //    tempNanny = new Nanny(n);
            //    //tempNanny.ID = n.ID;
            //    //tempNanny.Floor = n.Floor;
            //    //tempNanny.Address = n.Address;
            //    //tempNanny.BirthDate = n.BirthDate;
            //    //tempNanny.Elevator = n.Elevator;
            //    //tempNanny.Expertise = n.Expertise;
            //    //tempNanny.FirstName = n.FirstName;
            //    //tempNanny.HourSalary = n.HourSalary;
            //    //tempNanny.IsCostByHour = n.IsCostByHour;
            //    //tempNanny.LastName = n.LastName;
            //    //tempNanny.MaxAge = n.MaxAge;
            //    //tempNanny.MaxChildren = n.MaxChildren;
            //    //tempNanny.MinAge = n.MinAge;
            //    //tempNanny.MonthSalary = n.MonthSalary;
            //    //tempNanny.Phone = n.Phone;
            //    //tempNanny.VacationByMinisterOfEducation = n.VacationByMinisterOfEducation;
            //    //for (int i = 0; i < 6; i++)
            //    //{
            //    //    tempNanny.WorkDays[i] = n.WorkDays[i];
            //    //    tempNanny.WorkHours[0, i] = n.WorkHours[0, i];
            //    //    tempNanny.WorkHours[1, i] = n.WorkHours[1, i];
            //    //}
            //    //tempNanny.WorkDays[6] = n.WorkDays[6];
            //    //foreach (string str in n.Recommendations)
            //    //    tempNanny.Recommendations.Add(str);
            //    UpdateBTN.IsEnabled = true;
            //    RemoveBTN.IsEnabled = true;
            //    AddBTN.IsEnabled = false;
            //    birthDateDatePicker.IsEnabled = false;
            //}
            //NannyGrid.DataContext = tempNanny;
            ////tempNanny.ID = "2222222222";
            ////tempNanny.Phone = "error message";
            ////NannyGrid.DataContext = tempNanny;
        }
        private void IDchanged()
        {
            Nanny n = MainWindow.bl.FindNannyByID(iDTextBox.Text);
            if (n == null)
            {
                tempNanny = new Nanny(iDTextBox.Text);
                //NannyGrid.DataContext = tempNanny;
                UpdateBTN.IsEnabled = false;
                RemoveBTN.IsEnabled = false;
                AddBTN.IsEnabled = true;
                birthDateDatePicker.IsEnabled = true;
                h.Reset();
                recs.Clear();
            }
            else
            {
                tempNanny = new Nanny(n);
                //tempNanny.ID = n.ID;
                //tempNanny.Floor = n.Floor;
                //tempNanny.Address = n.Address;
                //tempNanny.BirthDate = n.BirthDate;
                //tempNanny.Elevator = n.Elevator;
                //tempNanny.Expertise = n.Expertise;
                //tempNanny.FirstName = n.FirstName;
                //tempNanny.HourSalary = n.HourSalary;
                //tempNanny.IsCostByHour = n.IsCostByHour;
                //tempNanny.LastName = n.LastName;
                //tempNanny.MaxAge = n.MaxAge;
                //tempNanny.MaxChildren = n.MaxChildren;
                //tempNanny.MinAge = n.MinAge;
                //tempNanny.MonthSalary = n.MonthSalary;
                //tempNanny.Phone = n.Phone;
                //tempNanny.VacationByMinisterOfEducation = n.VacationByMinisterOfEducation;
                //for (int i = 0; i < 6; i++)
                //{
                //    tempNanny.WorkDays[i] = n.WorkDays[i];
                //    tempNanny.WorkHours[0, i] = n.WorkHours[0, i];
                //    tempNanny.WorkHours[1, i] = n.WorkHours[1, i];
                //}
                //tempNanny.WorkDays[6] = n.WorkDays[6];
                //foreach (string str in n.Recommendations)
                //    tempNanny.Recommendations.Add(str);
                UpdateBTN.IsEnabled = true;
                RemoveBTN.IsEnabled = true;
                AddBTN.IsEnabled = false;
                birthDateDatePicker.IsEnabled = false;
                h.Reset();
                h.ManualSetting(tempNanny.WorkDays, tempNanny.WorkHours);
                recs.Clear();
                foreach (string str in n.Recommendations)
                    recs.AddString(str);
            }
            NannyGrid.DataContext = tempNanny;
            // from here on ensuring that the user knows in advance if his the ID is taken
            if (MainWindow.bl.FindChildByID(iDTextBox.Text) != null || MainWindow.bl.FindMotherByID(iDTextBox.Text) != null)
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
            //tempNanny.ID = "2222222222";
            //tempNanny.Phone = "error message";
            //NannyGrid.DataContext = tempNanny;
        }

        private void iDTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Nanny n = MainWindow.bl.FindNannyByID(iDTextBox.Text);
            //if (n == null)
            //{
            //    //NannyGrid.DataContext = tempNanny;
            //    UpdateBTN.IsEnabled = false;
            //    RemoveBTN.IsEnabled = false;
            //    AddBTN.IsEnabled = true;
            //    birthDateDatePicker.IsEnabled = true;
            //}
            //else
            //{
            //    tempNanny.ID = n.ID;
            //    tempNanny.Floor = n.Floor;
            //    tempNanny.Address = n.Address;
            //    tempNanny.BirthDate = n.BirthDate;
            //    tempNanny.Elevator = n.Elevator;
            //    tempNanny.Expertise = n.Expertise;
            //    tempNanny.FirstName = n.FirstName;
            //    tempNanny.HourSalary = n.HourSalary;
            //    tempNanny.IsCostByHour = n.IsCostByHour;
            //    tempNanny.LastName = n.LastName;
            //    tempNanny.MaxAge = n.MaxAge;
            //    tempNanny.MaxChildren = n.MaxChildren;
            //    tempNanny.MinAge = n.MinAge;
            //    tempNanny.MonthSalary = n.MonthSalary;
            //    tempNanny.Phone = n.Phone;
            //    tempNanny.VacationByMinisterOfEducation = n.VacationByMinisterOfEducation;
            //    for (int i = 0; i < 6; i++)
            //    {
            //        tempNanny.WorkDays[i] = n.WorkDays[i];
            //        tempNanny.WorkHours[0, i] = n.WorkHours[0, i];
            //        tempNanny.WorkHours[1, i] = n.WorkHours[1, i];
            //    }
            //    tempNanny.WorkDays[6] = n.WorkDays[6];
            //    foreach (string str in n.Recommendations)
            //        tempNanny.Recommendations.Add(str);
            //    UpdateBTN.IsEnabled = true;
            //    RemoveBTN.IsEnabled = true;
            //    AddBTN.IsEnabled = false;
            //    birthDateDatePicker.IsEnabled = false;
            //}
            //NannyGrid.DataContext = tempNanny;
        }

        private void iDTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            //Nanny n = MainWindow.bl.FindNannyByID(iDTextBox.Text);
            //if (n == null)
            //{
            //    tempNanny = new Nanny(iDTextBox.Text);
            //    //NannyGrid.DataContext = tempNanny;
            //    UpdateBTN.IsEnabled = false;
            //    RemoveBTN.IsEnabled = false;
            //    AddBTN.IsEnabled = true;
            //    birthDateDatePicker.IsEnabled = true;
            //}
            //else
            //{
            //    tempNanny = new Nanny(n);
            //    //tempNanny.ID = n.ID;
            //    //tempNanny.Floor = n.Floor;
            //    //tempNanny.Address = n.Address;
            //    //tempNanny.BirthDate = n.BirthDate;
            //    //tempNanny.Elevator = n.Elevator;
            //    //tempNanny.Expertise = n.Expertise;
            //    //tempNanny.FirstName = n.FirstName;
            //    //tempNanny.HourSalary = n.HourSalary;
            //    //tempNanny.IsCostByHour = n.IsCostByHour;
            //    //tempNanny.LastName = n.LastName;
            //    //tempNanny.MaxAge = n.MaxAge;
            //    //tempNanny.MaxChildren = n.MaxChildren;
            //    //tempNanny.MinAge = n.MinAge;
            //    //tempNanny.MonthSalary = n.MonthSalary;
            //    //tempNanny.Phone = n.Phone;
            //    //tempNanny.VacationByMinisterOfEducation = n.VacationByMinisterOfEducation;
            //    //for (int i = 0; i < 6; i++)
            //    //{
            //    //    tempNanny.WorkDays[i] = n.WorkDays[i];
            //    //    tempNanny.WorkHours[0, i] = n.WorkHours[0, i];
            //    //    tempNanny.WorkHours[1, i] = n.WorkHours[1, i];
            //    //}
            //    //tempNanny.WorkDays[6] = n.WorkDays[6];
            //    //foreach (string str in n.Recommendations)
            //    //    tempNanny.Recommendations.Add(str);
            //    UpdateBTN.IsEnabled = true;
            //    RemoveBTN.IsEnabled = true;
            //    AddBTN.IsEnabled = false;
            //    birthDateDatePicker.IsEnabled = false;
            //}
            //NannyGrid.DataContext = tempNanny;
            ////tempNanny.ID = "2222222222";
            ////tempNanny.Phone = "error message";
            ////NannyGrid.DataContext = tempNanny;
        }

        private void IDchanged(object sender, MouseButtonEventArgs e)
        {
            IDchanged();
        }

        private void IDchanged(object sender, SelectionChangedEventArgs e)
        {
            IDchanged();
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (object txtbx in NannyGrid.Children)
                    if (txtbx is TextBox && ((TextBox)txtbx).Text == "")
                        throw new Exception("one or more fields are empty");
                Nanny target = MainWindow.bl.FindNannyByID(tempNanny.ID);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.Address, tempNanny.Address);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.Elevator, tempNanny.Elevator);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.Expertise, tempNanny.Expertise);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.FirstName, tempNanny.FirstName);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.Floor, tempNanny.Floor);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.HourSalary, tempNanny.HourSalary);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.IsCostByHour, tempNanny.IsCostByHour);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.LastName, tempNanny.LastName);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.MaxAge, tempNanny.MaxAge);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.MaxChildren, tempNanny.MaxChildren);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.MinAge, tempNanny.MinAge);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.MonthSalary, tempNanny.MonthSalary);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.Phone, tempNanny.Phone);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.VacationByMinisterOfEducation, tempNanny.VacationByMinisterOfEducation);
                bool[] days = new bool[7];
                for (int i = 0; i < 6; i++)
                {
                    days[i] = (bool)h.Check[i].IsChecked;
                    target.WorkDays[i] = (bool)h.Check[i].IsChecked;
                }
                MainWindow.bl.UpdateNanny(target, Nanny.Props.WorkDays, days);
                MainWindow.bl.UpdateNanny(target, Nanny.Props.WorkHours, h.GetMatrix());                
                target.Recommendations.Clear();
                foreach (string str in recs.GetListString())
                    MainWindow.bl.UpdateNanny(target, Nanny.Props.Recommendations, str);
                if (MessageBox.Show("Update was succesful, do you wish to stay on the Nanny window?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Close();
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to update nanny:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void IDchanged(object sender, TextChangedEventArgs e)
        {
            IDchanged();
        }

        private void RemoveBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("The nanny will be completely removed\nThis action can't be undone.", "Verfication", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    MainWindow.bl.RemoveNanny(MainWindow.bl.FindNannyByID(tempNanny.ID));
                    Close();
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Failed to remove nanny:\n" + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
