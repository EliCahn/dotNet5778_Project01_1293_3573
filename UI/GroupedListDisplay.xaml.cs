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

namespace UI
{
    /// <summary>
    /// Interaction logic for GroupedListDisplay.xaml
    /// </summary>
    public partial class GroupedListDisplay : Window
    {
        IEnumerable<IGrouping<int, object>> ContentForList; // This time, for clarity, we cannot put the objects directly in the items of the listbox due to IGrouping's default ToString() not being coherent, therefore we have to save them here instead
        /// <summary>
        /// Initiates a list displayer window
        /// </summary>
        /// <param name="DescText">The text for te description</param>
        /// <param name="ButtonText">The text for the button which opens the detailed window for individuals</param>
        /// <param name="ContentForList">The groups for the comboBox to choose from to display</param>
        public GroupedListDisplay(string DescText, string ButtonText, IEnumerable<IGrouping<int, object>> ContentForList)
        {
            InitializeComponent();
            this.ContentForList = ContentForList;
            OpenSelectionBTN.Content = ButtonText;
            DescriptionLBL.Content = DescText;
            ResultLST.Items.Add("Choose a group from the list above");
            ResultLST.IsEnabled = false;
            switch(DescText)
            { // this one gotta be specific, for easier context
                case "Nannies grouped according to the minimal acceptable age of their children":
                case "Nannies grouped according to the maximal acceptable age of their children":
                    foreach (IGrouping<int, object> group in ContentForList)
                        ChooseGroup.Items.Add(group.Key * 3 + " - " + (group.Key * 3 + 2) + " months [" + group.Count() + " nanny(s)]");
                    break;
                case "Contracts grouped by the distance between the nanny and her location":
                    new Thread((Content) => { foreach (IGrouping<int, object> group in (IEnumerable<IGrouping<int, object>>)Content) Dispatcher.Invoke(() => 
                    {
                        ChooseGroup.Items.Add(group.Key + " - " + (group.Key + 1) + " km(s) [" + group.Count() + " contract(s)]");
                        ChooseGroup.Items.Remove("No results found who fit your search. The database might be empty");
                        ChooseGroup.IsEnabled = true;
                    }); }).Start(ContentForList);                    
                    break;
            }
            //foreach (IGrouping<int, object> group in ContentForList)
            //    ChooseGroup.Items.Add(group.Key);
            if (ChooseGroup.Items.Count == 0)
            {
                ChooseGroup.Items.Add("No results found who fit your search. The database might be empty");
                ChooseGroup.IsEnabled = false;
                ChooseGroup.SelectedIndex = 0;
                CountLBL.Content = "No groups found";
            }
            else
                CountLBL.Content = ChooseGroup.Items.Count + " group(s) found";
        }

        private void ResultLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenSelectionBTN.IsEnabled = ResultLST.SelectedIndex != -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenSelectionBTN_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will enable editing and even removal of the selected item, and therefore, will additionally result in the closing of this window, due to the search results being outdated.\nAre you sure you wish to continue?", "You are about to leave this winddow", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            if (ResultLST.SelectedItem is BE.Nanny)
            {
                NannyWin nannyWin = new NannyWin();
                nannyWin.iDTextBox.Text = (ResultLST.SelectedItem as BE.Nanny).ID;
                nannyWin.iDTextBox.IsEnabled = false;
                nannyWin.ShowDialog();
            }
            else if (ResultLST.SelectedItem is BE.Contract)
            {
                ContractWin contractWin = new ContractWin();
                contractWin.SerNumComboBox.SelectedItem = (ResultLST.SelectedItem as BE.Contract).SerialNumber;
                contractWin.SerNumComboBox.IsEnabled = false;
                contractWin.ShowDialog();
            }
            else if (ResultLST.SelectedItem is BE.Child)
            {
                ChildWindow childWindow = new ChildWindow();
                childWindow.iDTextBox.Text = (ResultLST.SelectedItem as BE.Child).ID;
                childWindow.ChildGrid.DataContext = MainWindow.bl.FindChildByID((ResultLST.SelectedItem as BE.Child).ID);
                childWindow.iDTextBox.IsEnabled = false;
                childWindow.ShowDialog();
            }
            else if (ResultLST.SelectedItem is BE.Mother)
            {
                MotherWin motherWin = new MotherWin();
                motherWin.iDTextBox.Text = (ResultLST.SelectedItem as BE.Mother).ID;
                motherWin.MotherGrid.DataContext = MainWindow.bl.FindMotherByID((ResultLST.SelectedItem as BE.Mother).ID);
                motherWin.iDTextBox.IsEnabled = false;
                motherWin.ShowDialog();
            }
            Close();
        }

        private void ChooseGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResultLST.Items.Clear();
            ResultLST.IsEnabled = true;
            if (ChooseGroup.SelectedIndex != -1 && ChooseGroup.SelectedItem.ToString() != "No results found who fit your search. The database might be empty")
            {
                switch ((string)DescriptionLBL.Content)
                {
                    case "Nannies grouped according to the minimal acceptable age of their children":
                    case "Nannies grouped according to the maximal acceptable age of their children":
                        foreach (IGrouping<int, object> group in ContentForList)
                            if (group.Key == int.Parse(ChooseGroup.SelectedItem.ToString().Split(' ')[0]) / 3)
                            {
                                foreach (object obj in group)
                                    ResultLST.Items.Add(obj);                                
                                return;
                            }
                        break;
                    case "Contracts grouped by the distance between the nanny and her location":
                        new Thread((Content) => {
                            foreach (IGrouping<int, object> group in (IEnumerable<IGrouping<int, object>>)Content) Dispatcher.Invoke(() =>
                            {
                                if (group.Key == int.Parse(ChooseGroup.SelectedItem.ToString().Split(' ')[0]))
                                {
                                    foreach (object obj in group)
                                        ResultLST.Items.Add(obj);
                                    return;
                                }
                            });
                        }).Start(ContentForList);
                        //foreach (IGrouping<int, object> group in ContentForList)
                        //if (group.Key == int.Parse(ChooseGroup.SelectedItem.ToString().Split(' ')[0]))
                        //{
                        //    foreach (object obj in group)
                        //        ResultLST.Items.Add(obj);
                        //    return;
                        //}
                        break;
                }
                
                    
            }
        }
    }
}
