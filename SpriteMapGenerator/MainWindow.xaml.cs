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
        public double offx = 0,offy = 0;
        public int TileSizeX = 0,TileSizeY = 0;
        bool bPressed = false;
        Point MousePoint;
        Image CurrentImage = null;
        public class ListViewItem
        {
            public string sName     { get; set; }
            public string sPath     { get; set; }
            public Image imgImage   { get; set; }
            public ListViewItem(string s_Name,string s_Path, Image img_Image)
            {
                sName = s_Name;
                sPath = s_Path;
                imgImage = img_Image;
            }
        }   
        //Main
        public MainWindow()
        {
            InitializeComponent();
            CheckSize();
        }
        //Menu
        private void Menu_Load_Click(object sender, RoutedEventArgs e)
        {
            //Open Files
            OpenFileDialog ofFile = new OpenFileDialog();
            //Filter the filetypes, Turn on multiselect and Keep the final Directory
            ofFile.Filter = "All Image Files|*.jpg; *.bmp; *.png;";
            ofFile.Multiselect = true;
            ofFile.RestoreDirectory = true;
            //setup the temp int for looping and error checking bool
            int i = 0;
            Nullable<bool> ofFileResult = ofFile.ShowDialog();
            //Check we clicked ok.
            if (ofFileResult == true)
            {
                //Loop through imported files
                foreach (String file in ofFile.FileNames)
                {
                    //Generate the Bitmap Image
                    BitmapImage bmimg = new BitmapImage();
                    bmimg.BeginInit();
                    bmimg.UriSource = new Uri(file);
                    bmimg.EndInit();
                    //Generate the Image
                    Image img = new Image();
                    img.BeginInit();
                    img.Source = bmimg;
                    img.Width = bmimg.Width;
                    img.Height = bmimg.Height;
                    img.SetValue(Canvas.LeftProperty, offx);
                    img.SetValue(Canvas.TopProperty, offy);
                    img.EndInit();
                    //Update the OffX/OffY
                    offx += img.Width;
                    offy = 0;
                    CheckSize();
                    //Setup the ShortName
                    String imagename = ofFile.SafeFileNames[i].Substring(0, ofFile.SafeFileNames[i].Length - System.IO.Path.GetExtension(file).Length);
                    //Create the ListViewItem
                    ListViewItem item = new ListViewItem(imagename, ofFile.SafeFileNames[i], img);
                    //Add it to everything
                    LV_Layers.Items.Add(item);
                    LV_Sprites.Items.Add(item);
                    canvasSpriteSheet.Children.Add(img);
                    //Goto the next ID
                    i++;
                }
            }
        }
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Menu_XML_Click(object sender, RoutedEventArgs e)
        {
            //PngBitmapEncoder PNGEncode = new PngBitmapEncoder();
            foreach (Image Item in canvasSpriteSheet.Children)
            {
                //TODO - Exporting the canvas and XML file
            }
        }
        private void Menu_pList_Click(object sender, RoutedEventArgs e)
        {
            //PngBitmapEncoder PNGEncode = new PngBitmapEncoder();
            foreach (Image Item in canvasSpriteSheet.Children)
            {
                //TODO - Exporting the canvas and pList file
            }
        }
        //check what image we are pressing
        private void LV_Sprites_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem vClicked = null;
            if (LV_Sprites.Items.Count > 0)
            {
                vClicked = LV_Sprites.Items.GetItemAt(0) as ListViewItem;
            }
            //TODO
            if (vClicked != null)
            {
                Image img = new Image();
                img.BeginInit();
                img.Source = vClicked.imgImage.Source;
                img.Width = vClicked.imgImage.Width;
                img.Height = vClicked.imgImage.Height;
                img.SetValue(Canvas.LeftProperty, 0.0);
                img.SetValue(Canvas.TopProperty, img.Height);
                img.EndInit();
                canvasSpriteSheet.Children.Add(img);
                offy = img.Height;
                CheckSize();
                LV_Layers.Items.Add(new ListViewItem(vClicked.sName, vClicked.sPath, vClicked.imgImage));
            }
        }
        //moving the canvas around
        private void CanvasViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Space) && bPressed)
            {
                canvasSpriteSheet.SetValue(Canvas.LeftProperty, e.MouseDevice.GetPosition(this).X - MousePoint.X);
                canvasSpriteSheet.SetValue(Canvas.TopProperty, e.MouseDevice.GetPosition(this).Y - MousePoint.Y);

            }
        }
        private void CanvasViewport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MousePoint = e.MouseDevice.GetPosition(this);
            bPressed = true;
        }
        private void CanvasViewport_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            bPressed = false;
        }
        //Moving sprites on the canvas
        private void canvasSpriteSheet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point Mouse = e.MouseDevice.GetPosition(canvasSpriteSheet);
            foreach (Image Item in canvasSpriteSheet.Children)
            {
                if (CheckInside(Mouse, Item))
                {
                    CurrentImage = Item;
                }
            }
        }
        private void canvasSpriteSheet_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CurrentImage = null;
        }
        private void canvasSpriteSheet_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentImage != null)
            {
                int SnapVal = 0;
                Point Snap = new Point();
                Snap.X = (e.MouseDevice.GetPosition(canvasSpriteSheet).X - CurrentImage.Width * 0.5);
                Snap.Y = (e.MouseDevice.GetPosition(canvasSpriteSheet).Y - CurrentImage.Height * 0.5);
                CurrentImage.SetValue(Canvas.LeftProperty, Snap.X);
                CurrentImage.SetValue(Canvas.TopProperty, Snap.Y);
            }
        }
        //resizing
        private void CheckSize()
        {
            //Width
            if (offx > TileSizeX)
            {
                TileSizeX = Power(offx);
                canvasSpriteSheet.Width = TileSizeX;
            }
            Label_ImageWidth.Content = canvasSpriteSheet.Width;
            //Height
            if (offy > TileSizeY)
            {
                TileSizeY = Power(offy) ;
                canvasSpriteSheet.Height = TileSizeY;
            }
            Label_ImageHeight.Content = canvasSpriteSheet.Height;
        }
        private int Power(double value)
        {
            int power = 1;
            while (power < value)
            {
                power *= 2;
            }
            return power;
        }
        //BoxChecking
        private bool CheckInside(Point Value, Image img)
        {
            //Console.WriteLine(string.Format("X : {0} {1} {2}", (double)img.GetValue(Canvas.LeftProperty), Value.X, (double)img.GetValue(Canvas.LeftProperty) + img.Width));
            //Console.WriteLine(string.Format("Y : {0} {1} {2}", (double)img.GetValue(Canvas.TopProperty), Value.Y, (double)img.GetValue(Canvas.TopProperty) + img.Height));
            if (Value.X < (double)img.GetValue(Canvas.LeftProperty) + img.Width &&
                Value.X > (double)img.GetValue(Canvas.LeftProperty) &&
                Value.Y < (double)img.GetValue(Canvas.TopProperty) + img.Height &&
                Value.Y > (double)img.GetValue(Canvas.TopProperty)) 
            {
                return true;
            } else {
                return false;
            }
        }
    }
}
