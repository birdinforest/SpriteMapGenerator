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
        MainWindow MainWin;

        public SettingsWindow()
        {
            InitializeComponent();

            MainWin = Application.Current.Windows[0] as MainWindow;
        }
        public SettingsWindow(int x, int y, bool xx, bool yy,double MaxW, double MaxH)
        {
            InitializeComponent();

            ixVal   = x;    tb_XVal.Text      = x.ToString();
            iyVal   = y;    tb_YVal.Text      = y.ToString();

            bSnapX = xx;    cb_XVal.IsChecked = xx;
            bSnapY = yy;    cb_YVal.IsChecked = yy;

            dMaxWidth = MaxW; tb_MaxWidth.Text = MaxW.ToString();
            dMaxHeight = MaxH; tb_MaxHeight.Text = MaxH.ToString();

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
            ixVal = int.Parse(tb_XVal.Text.ToString());
            if (MainWin != null)
            {
                MainWin.SetSnapValX = ixVal;
            }
        }
        private void tb_YVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            iyVal = int.Parse(tb_YVal.Text.ToString());
            if (MainWin != null)
            {
                MainWin.SetSnapValY = iyVal;
            }
        }

        private void tb_MaxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            dMaxHeight = double.Parse(tb_MaxHeight.Text.ToString());
            if (MainWin != null)
            {
                MainWin.SetMaxHeight = dMaxHeight;
            }
        }

        private void tb_MaxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            dMaxWidth = double.Parse(tb_MaxWidth.Text.ToString());
            if (MainWin != null)
            {
                MainWin.SetMaxWidth = dMaxWidth;
            }
        }
    }
}
