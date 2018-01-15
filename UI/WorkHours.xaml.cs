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
using BE;

namespace UI
{
    /// <summary>
    /// Interaction logic for WorkHours.xaml
    /// </summary>
    public partial class WorkHours : UserControl
    {
        enum Days { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
        CheckBox[] chk = new CheckBox[6];
        public CheckBox[] Check { get => chk; }
        ComboBox[,] hours = new ComboBox[2, 6];
        public WorkHours()
        {
            InitializeComponent();            
            for (int i = 0; i < 6; i++)
            {
                hours[0, i] = new ComboBox();
                hours[1, i] = new ComboBox();
                hours[0, i].Items.Add("--Select Hour--");
                hours[1, i].Items.Add("--Select Hour--");
                hours[0, i].IsEnabled = false;
                hours[1, i].IsEnabled = false;
                for (int j = 0; j < 24; j++)
                {
                    hours[0, i].Items.Add(j.ToString("00") + ":00");
                    hours[1, i].Items.Add(j.ToString("00") + ":00");
                    hours[0, i].SelectedIndex = 0;
                    hours[1, i].SelectedIndex = 0;
                }
                //hours[0, i].TextAlignment = TextAlignment.Center;
                //hours[1, i].TextAlignment = TextAlignment.Center;
                //hours[0, i].Text = "0";
                //hours[1, i].Text = "0";
                Grid.SetColumn(hours[0, i], 1);
                Grid.SetColumn(hours[1, i], 2);
                Grid.SetRow(hours[0, i], i);
                Grid.SetRow(hours[1, i], i);
                HourMatrixGRD.Children.Add(hours[0, i]);
                HourMatrixGRD.Children.Add(hours[1, i]);
            }
            for (int i = 0; i < 6; i++)
            {
                chk[i] = new CheckBox();
                chk[i].Content = (Days)i;
                chk[i].IsChecked = false;
                chk[i].Click += CheckBoxChangeFunc;/*(s, e) => { hours[0, i].IsEnabled = (bool)chk[i].IsChecked; hours[1, i].IsEnabled = (bool)chk[i].IsChecked; }*/;
                Grid.SetRow(chk[i], i);
                HourMatrixGRD.Children.Add(chk[i]);
            }

                     
        }
        void CheckBoxChangeFunc(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
                if ((CheckBox)sender == chk[i])
                {
                    hours[0, i].IsEnabled = (bool)chk[i].IsChecked;
                    hours[1, i].IsEnabled = (bool)chk[i].IsChecked;
                }
        }
        /// <summary>
        /// gets a matrix of the hours marked in the UC
        /// </summary>
        /// <returns></returns>
        public DateTime[,] GetMatrix()
        {
            DateTime[,] ret = new DateTime[2, 6];
            for (int i = 0; i < 6; i++)
            {
                if ((bool)chk[i].IsChecked)
                {
                    if (hours[0, i].SelectedItem.ToString() == "--Select Hour--" || hours[1, i].SelectedItem.ToString() == "--Select Hour--")
                        throw new Exception("Please choose an hour or uncheck the day");
                    ret[0, i] = default(DateTime).AddHours(int.Parse(hours[0, i].SelectedItem.ToString()[0] + "" + hours[0, i].SelectedItem.ToString()[1]));
                    ret[1, i] = default(DateTime).AddHours(int.Parse(hours[1, i].SelectedItem.ToString()[0] + "" + hours[1, i].SelectedItem.ToString()[1]));
                }
            }
            return ret;
        }
        /// <summary>
        /// resets the whole interface to default settings
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < 6; i++)
            {
                chk[i].IsChecked = false;
                hours[0, i].SelectedIndex = 0;
                hours[1, i].SelectedIndex = 0;
                hours[0, i].IsEnabled = false;
                hours[1, i].IsEnabled = false;
            }
        }
        /// <summary>
        /// manually sets the interface to match the parameters
        /// </summary>
        /// <param name="days">indicates in which days is there work</param>
        /// <param name="hours">indicates between which hours is there work</param>
        public void ManualSetting(bool[] days, DateTime[,] hours)
        {
            for (int i = 0; i < 6; i++)
            {
                chk[i].IsChecked = days[i];
                if ((bool)chk[i].IsChecked)
                {
                    this.hours[0, i].IsEnabled = (bool)chk[i].IsChecked;
                    this.hours[1, i].IsEnabled = (bool)chk[i].IsChecked;
                    this.hours[0, i].SelectedIndex = Math.Abs((hours[0, i] - default(DateTime)).Hours) + 1;
                    this.hours[1, i].SelectedIndex = Math.Abs((hours[1, i] - default(DateTime)).Hours) + 1;
                }
            }
        }
    }
}
