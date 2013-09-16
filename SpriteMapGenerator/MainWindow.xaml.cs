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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SpriteMapGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class ListViewItem
        {
            public string sName { get; set; }
            public string sPath { get; set; }
            public Image imgImage   { get; set; }
            public ListViewItem(string s_Name,string s_Path, Image img_Image)
            {
                sName = s_Name;
                sPath = s_Path;
                imgImage = img_Image;
            }
        }   
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Menu_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofFile = new OpenFileDialog();
            ofFile.Filter = "All Image Files|*.jpg; *.bmp; *.png;";
            ofFile.Multiselect = true;
            ofFile.RestoreDirectory = true;
            int i = 0;
            Nullable<bool> ofFileResult = ofFile.ShowDialog();
            if (ofFileResult == true)
            {
                foreach (String file in ofFile.FileNames)
                {
                    BitmapImage bmimg = new BitmapImage();
                    bmimg.BeginInit();
                    bmimg.UriSource = new Uri(file);
                    bmimg.EndInit();

                    Image img = new Image();
                    img.BeginInit();
                    img.Source = bmimg;
                    img.Width = bmimg.Width;
                    img.Height = bmimg.Height;
                    img.EndInit();

                    String imagename = ofFile.SafeFileNames[i].Substring(0, ofFile.SafeFileNames[i].Length - System.IO.Path.GetExtension(file).Length);

                    ListViewItem item = new ListViewItem(imagename, ofFile.SafeFileNames[i], img);
                    LV_Sprites.Items.Add(item);

                    canvasSpriteSheet.Children.Add(img);
                    i++;
                }
            }

        }
        private void Menu_Save_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Menu_XML_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Menu_pList_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
