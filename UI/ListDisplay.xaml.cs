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

namespace UI
{
    /// <summary>
    /// Interaction logic for ListDisplay.xaml
    /// </summary>
    public partial class ListDisplay : Window
    {
        /// <summary>
        /// Initiates a list displayer window
        /// </summary>
        /// <param name="DescText">The text for te description</param>
        /// <param name="ButtonText">The text for the button which opens the detailed window for individuals</param>
        /// <param name="ContentForList">The items for the list to display</param>
        public ListDisplay(string DescText, string ButtonText, IEnumerable<object> ContentForList)
        {
            InitializeComponent();
            OpenSelectionBTN.Content = ButtonText;
            DescriptionLBL.Content = DescText;
            foreach (object obj in ContentForList)
                ResultLST.Items.Add(obj);
            if (ResultLST.Items.Count == 0)
            {
                ResultLST.Items.Add("No results found who fit your search category!");
                ResultLST.IsEnabled = false;
                CountLBL.Content = "No results found";
            }
            else
                CountLBL.Content = ResultLST.Items.Count + " result(s) found";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ResultLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenSelectionBTN.IsEnabled = ResultLST.SelectedIndex != -1;
        }

        private void OpenSelectionBTN_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will enable editing and even removal of the selected item, and therefore, will additionally result in the closing of this window, due to the search results being outdated.\nAre you sure you wish to continue?", "You are about to leave this winddow", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            // The point of all the below is similar: Open the window for the respective chosen item of the list, and lock it on that specific item.
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
    }
}
