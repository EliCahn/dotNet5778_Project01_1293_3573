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
    /// Interaction logic for SimpleGetTextWin.xaml
    /// This window lets the user choose an item from a collection, and then does something with the chosen item
    /// </summary>
    public partial class ChoicePromptWin : Window
    {
        //TextBox target;
        Action<object> FuncToDo;
        IEnumerable<object> Options;
        /// <summary>
        /// Creates the choice window
        /// </summary>
        /// <param name="OptionsOfChoice">The collection from which choices are made</param>
        /// <param name="promp">The prompt shown on the window (also used as breakdown for the different usages of the window)</param>
        /// <param name="FuncToDo">The function that will be done with the chosen choice</param>
        public ChoicePromptWin(/*TextBox target*/IEnumerable<object> OptionsOfChoice ,string promp, Action<object> FuncToDo)
        {
            InitializeComponent();
            PrompotLBL.Content = promp;
            this.FuncToDo = FuncToDo;
            Options = OptionsOfChoice;
            OKBtn.IsEnabled = false;
            switch (promp)
            {   // possible to add as many cases is here as needed
                case "Choose contract to sign":
                    foreach (Contract con in Options)
                        choiceComboBox.Items.Add(con.SerialNumber);
                    if (choiceComboBox.Items.Count == 0)
                    {
                        choiceComboBox.Items.Add("No unsigned contracts found!");
                        choiceComboBox.SelectedIndex = 0;
                        choiceComboBox.IsEnabled = false;
                    }
                    break;
                //case "What do you wish to find?":
                default:
                    foreach (string str in Options)
                        choiceComboBox.Items.Add(str);
                    break;
            }
            
            //this.target = target;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //target.Text = choiceComboBox.SelectedItem.ToString();
            try
            {
                switch ((string)PrompotLBL.Content)
                {   // possible to add as many cases is here as needed, respective to those in the constructor
                    case "Choose contract to sign":
                        foreach (Contract con in Options)
                            if (con.SerialNumber == choiceComboBox.SelectedItem.ToString())
                            {
                                FuncToDo(con);
                                Close();
                            }
                        break;
                    //case "What do you wish to find?":
                    default:
                        FuncToDo(choiceComboBox.SelectedItem);
                        break;
                }
                Close();
            }
            catch (Exception excep)
            {
                MessageBox.Show("ERROR: " + excep.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void choiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OKBtn.IsEnabled = choiceComboBox.SelectedIndex != -1;
        }

        private void choiceComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (choiceComboBox.SelectedIndex != -1)
            {
                foreach (Contract con in MainWindow.bl.ContractsByCondition(c => !c.IsSigned))
                    if (con.SerialNumber == choiceComboBox.SelectedItem.ToString())
                    {
                        choiceComboBox.ToolTip = con + "\nCurrently unsigned";
                        return;
                    }
            }
        }
    }

    //public partial class ChoicePromptWin : Window
    //{
    //    TextBox target;
    //    public ChoicePromptWin(TextBox target, string promp)
    //    {
    //        InitializeComponent();
    //        PrompotLBL.Content = promp;
    //        this.target = target;
    //    }

    //    private void Button_Click(object sender, RoutedEventArgs e)
    //    {
    //        target.Text = choiceComboBox.SelectedItem.ToString();
    //        Close();
    //    }
    //}
}
