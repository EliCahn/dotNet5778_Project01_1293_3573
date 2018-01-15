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
    /// Interaction logic for ListStringReader.xaml
    /// </summary>
    public partial class ListStringReader : Window
    {
        string defaulttxt;
        List<string> target;
        public ListStringReader(Window owner, List<string> target)
        {
            InitializeComponent();
            this.target = target;
            if (owner is NannyWin)
            {
                PromptLBL.Content = "Any recommendations?";
                defaulttxt = "Input recommendation";
            }
            else if (owner is MotherWin)
            {
                PromptLBL.Content = "Any comments?";
                defaulttxt = "Input comment";
            }
            else if (owner is ChildWindow)
            {
                PromptLBL.Content = "special needs?";
                defaulttxt = "Input need";
            }
            AddTXT.Text = defaulttxt;
        }

        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            target.Add(AddTXT.Text);
            AddTXT.Text = defaulttxt;
        }

        private void DoneBTN_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
