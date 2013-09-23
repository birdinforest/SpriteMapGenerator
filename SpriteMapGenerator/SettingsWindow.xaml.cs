using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace SpriteMapGenerator
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public int ixVal, iyVal;
        public bool bSnapX, bSnapY;
        public double dMaxWidth, dMaxHeight;
        public double dXOff, dYOff;
        MainWindow MainWin;

        public SettingsWindow()
        {
            InitializeComponent();
            MainWin = Application.Current.Windows[0] as MainWindow;
        }
        public SettingsWindow(int x, int y, bool xx, bool yy,double MaxW, double MaxH,double W,double H)
        {
            InitializeComponent();

            ixVal   = x;    tb_XVal.Text      = x.ToString();
            iyVal   = y;    tb_YVal.Text      = y.ToString();

            bSnapX = xx;    cb_XVal.IsChecked = xx;
            bSnapY = yy;    cb_YVal.IsChecked = yy;

            dMaxWidth = MaxW; tb_MaxWidth.Text = MaxW.ToString();
            dMaxHeight = MaxH; tb_MaxHeight.Text = MaxH.ToString();

            dXOff = W; tb_MaxHeight.Text = W.ToString();
            dYOff = H; tb_MaxHeight.Text = H.ToString();

            MainWin = Application.Current.Windows[0] as MainWindow;
        }

        private void cb_XVal_Click(object sender, RoutedEventArgs e)
        {
            bSnapX = (bool)cb_XVal.IsChecked;
            if (MainWin != null)
            {
                MainWin.SetSnapX = bSnapX;
            }
        }
        private void cb_YVal_Click(object sender, RoutedEventArgs e)
        {
            bSnapY = (bool)cb_YVal.IsChecked;
            if (MainWin != null)
            {
                MainWin.SetSnapY = bSnapY;
            }
        }
        private void tb_XVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Str = tb_XVal.Text.ToString();
            if (Str.Length > 0)
            {
                if (IsTextAllowed(Str))
                {
                    ixVal = int.Parse(Str);
                    if (MainWin != null)
                    {
                        MainWin.SetSnapValX = ixVal;
                    }
                }
            }
        }
        private void tb_YVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Str = tb_YVal.Text.ToString();
            if (Str.Length > 0)
            {
                if (IsTextAllowed(Str))
                {
                    iyVal = int.Parse(Str);
                    if (MainWin != null)
                    {
                        MainWin.SetSnapValY = iyVal;
                    }
                }
            }
        }

        private void tb_MaxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Str = tb_MaxHeight.Text.ToString();
            if (Str.Length > 0)
            {
                if (IsTextAllowed(Str))
                {
                    dMaxHeight = double.Parse(Str);
                    if (MainWin != null)
                    {
                        MainWin.SetMaxHeight = dMaxHeight;
                    }
                }
            }
        }

        private void tb_MaxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Str = tb_MaxWidth.Text.ToString();
            if (Str.Length > 0)
            {
                if (IsTextAllowed(Str))
                {
                    dMaxWidth = double.Parse(Str);
                    if (MainWin != null)
                    {
                        MainWin.SetMaxWidth = dMaxWidth;
                    }
                }
            }
        }
        private void tb_Height_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Str = tb_Height.Text.ToString();
            if (Str.Length > 0)
            {
                if (IsTextAllowed(Str))
                {
                    dYOff = double.Parse(Str);
                    if (MainWin != null)
                    {
                        MainWin.SetHeight = dYOff;
                    }
                }
            }
        }

        private void tb_Width_TextChanged(object sender, TextChangedEventArgs e)
        {
            string Str = tb_Width.Text.ToString();
            if (Str.Length > 0)
            {
                if (IsTextAllowed(Str))
                {
                    dXOff = double.Parse(Str);
                    if (MainWin != null)
                    {
                        MainWin.SetWidth = dXOff;
                    }
                }
            }
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }
    }
}
