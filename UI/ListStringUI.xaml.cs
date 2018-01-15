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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for ListStringUI.xaml
    /// </summary>
    public partial class ListStringUI : UserControl
    {
        public ListStringUI()
        {
            InitializeComponent();
            addBTN.IsEnabled = false;
            removeBTN.IsEnabled = false;
        }

        private void addTXT_TextChanged(object sender, TextChangedEventArgs e)
        {
            addBTN.IsEnabled = addTXT.Text != "";
        }

        private void strLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeBTN.IsEnabled = strLST.SelectedIndex != -1;
        }

        private void addBTN_Click(object sender, RoutedEventArgs e)
        {
            strLST.Items.Add(addTXT.Text);
            addTXT.Text = "";
        }

        private void removeBTN_Click(object sender, RoutedEventArgs e)
        {
            strLST.Items.Remove(strLST.SelectedItem);
        }
        /// <summary>
        /// returns the list of strings currently in the listbox
        /// </summary>
        /// <returns></returns>
        public List<string> GetListString()
        {
            List<string> retList = new List<string>();
            foreach (string str in strLST.Items)
                retList.Add(str);
            return retList;
        }
        /// <summary>
        /// adds a parameter string to the listbox
        /// </summary>
        /// <param name="str"></param>
        public void AddString(string str)
        {
            strLST.Items.Add(str);
        }
        /// <summary>
        /// clears all strings from the listbox
        /// </summary>
        public void Clear()
        {
            strLST.Items.Clear();
        }
    }
}
