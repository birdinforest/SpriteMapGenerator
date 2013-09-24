using System;
using System.IO;
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
using System.Windows.Markup;

namespace SpriteMapGenerator
{
    public class ListViewItem
    {
        public string sName { get; set; }
        public string sPath { get; set; }
        public Image imgImage { get; set; }
        public ListViewItem(string s_Name, string s_Path, Image img_Image)
        {
            sName = s_Name;
            sPath = s_Path;
            imgImage = img_Image;
        }
    } 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double dXOff = 0d, dYOff = 0d;
        private double dWidth = 0d, dHeight = 0d;
        private bool bPressed = false;

        private int SnapValX = 0;           public int SetSnapValX  { set { SnapValX = value; Label_SnapValX.Content = SnapValX; } }
        private int SnapValY = 0;           public int SetSnapValY  { set { SnapValY = value; Label_SnapValY.Content = SnapValY; } }
        private bool SnapX = false;         public  bool SetSnapX   { set { SnapX    = value; } }
        private bool SnapY = false;         public bool SetSnapY { set { SnapY = value; } }
        private double dMaxWidth = 1024.0;   public double SetMaxWidth { set { dMaxWidth = value; } }
        private double dMaxHeight = 1024.0; public double SetMaxHeight { set { dMaxHeight = value; } }

        public double SetWidth { set { canvasSpriteSheet.Width = value; Label_ImageWidth.Content = value; } }
        public double SetHeight { set { canvasSpriteSheet.Height = value; Label_ImageHeight.Content = value; } }

        private Point MousePoint;
        private Image CurrentImage = null;
        private Window SettingsWindowV = null;
        private Point Snap;
        //Main
        public MainWindow()
        {
            InitializeComponent();
            CheckSize(0d,0d);
            Snap = new Point();
            Label_SnapValX.Content = SnapValX;
            Label_SnapValY.Content = SnapValY;
        }
        private void SpriteMapGenerator_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SettingsWindowV != null)
            {
                SettingsWindowV.Close();
            }
        }
        //Menu
        private void Menu_Load_Click(object sender, RoutedEventArgs e)
        {
            //Open Files
            OpenFileDialog ofFile = new OpenFileDialog();
            //Filter the filetypes, Turn on multiselect and Keep the final Directory
            ofFile.Filter = "Image Files|*.jpg; *.jpeg; *.bmp; *.png; ";
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
                    img.Width = bmimg.PixelWidth;
                    img.Height = bmimg.PixelHeight;
                    img.SetValue(Canvas.LeftProperty, dXOff);
                    img.SetValue(Canvas.TopProperty, dYOff);
                    img.EndInit();

                    //Update the dXOff/dYOff
                    CheckSize(img.Width,img.Height);
                    //Setup the ShortName
                    String imagename = ofFile.SafeFileNames[i].Substring(0, ofFile.SafeFileNames[i].Length - System.IO.Path.GetExtension(file).Length);
                    //Create the ListViewItem
                    ListViewItem item = new ListViewItem(imagename, ofFile.SafeFileNames[i], img);
                    //Add it to everything
                    LV_Layers.Items.Insert(0, item);
                    LV_Sprites.Items.Add(item);
                    canvasSpriteSheet.Children.Add(img);
                    //Goto the next ID
                    i++;
                }
            }
        }
        private void Menu_New_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want wipe everything?", "Wipe Canvas", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ClearAll();
            }
        }
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?","Exit Sprite Map Generator", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindowV = new SettingsWindow(SnapValX, SnapValY, SnapX, SnapY, dMaxWidth, dMaxHeight, canvasSpriteSheet.Width, canvasSpriteSheet.Height);
            SettingsWindowV.Show();
        }
        private void Menu_XML_Click(object sender, RoutedEventArgs e)
        {
            //Export the Canvas to PNG
            canvasSpriteSheet.Background.Opacity = 0d;
            //Create RenderTarget for the image
            RenderTargetBitmap RTB_Image = new RenderTargetBitmap((int)canvasSpriteSheet.ActualWidth, (int)canvasSpriteSheet.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            canvasSpriteSheet.Measure(new Size((int)canvasSpriteSheet.ActualWidth, (int)canvasSpriteSheet.ActualHeight));
            canvasSpriteSheet.Arrange(new Rect(0,0,(int)canvasSpriteSheet.ActualWidth, (int)canvasSpriteSheet.ActualHeight));
            RTB_Image.Render(canvasSpriteSheet);
            //Create the PNG from the RenderTarget
            PngBitmapEncoder PNG_Image = new PngBitmapEncoder();
            PNG_Image.Frames.Add(BitmapFrame.Create(RTB_Image));
            //Exporting 
            SaveFileDialog SFD_Export = new SaveFileDialog();
            SFD_Export.Filter = "PNG Image | *.png";
            Nullable<bool> ofFileResult = SFD_Export.ShowDialog();
            //Check we clicked ok.
            if (ofFileResult == true){
                //PNG
                FileStream FS_File = File.Create(SFD_Export.FileName);
                PNG_Image.Save(FS_File);
                FS_File.Close();
                //XML
                String sXML = SFD_Export.FileName.Substring(0, SFD_Export.FileName.Length - System.IO.Path.GetExtension(SFD_Export.FileName).Length);
                sXML += ".xml";
                ExportXML(sXML);
            }
            canvasSpriteSheet.Background.Opacity = 1d;
        }
        //check what image we are pressing
        private void LV_Sprites_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem vClicked = null;
            if (LV_Sprites.Items.Count > 0)
            {
                vClicked = LV_Sprites.SelectedItem as ListViewItem;
            }
            //TODO
            if (vClicked != null)
            {
                Image img = new Image();

                img.BeginInit();
                img.Source = vClicked.imgImage.Source;
                img.Width = vClicked.imgImage.Width;
                img.Height = vClicked.imgImage.Height;
                img.SetValue(Canvas.LeftProperty, dXOff);
                img.SetValue(Canvas.TopProperty, dYOff);
                img.EndInit();

                canvasSpriteSheet.Children.Add(img);
                CheckSize(img.Width,img.Height);
                LV_Layers.Items.Add(new ListViewItem(vClicked.sName, vClicked.sPath, vClicked.imgImage));
            }
        }
        //moving the canvas around
        private void CanvasViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (bPressed)
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
            MousePoint = e.MouseDevice.GetPosition(canvasSpriteSheet);
            foreach (Image Item in canvasSpriteSheet.Children)
            {
                if (CheckInside(MousePoint, Item))
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
            if (CurrentImage != null) {
                Snap.X = (e.MouseDevice.GetPosition(canvasSpriteSheet).X - CurrentImage.Width * 0.5);
                Snap.Y = (e.MouseDevice.GetPosition(canvasSpriteSheet).Y - CurrentImage.Height * 0.5);
                if (SnapX)
                {
                    Snap.X = (int)Snap.X / SnapValX * SnapValX;
                } if (SnapY) 
                {
                    Snap.Y = (int)Snap.Y / SnapValY * SnapValY;
                }
                CurrentImage.SetValue(Canvas.LeftProperty   , Snap.X);
                CurrentImage.SetValue(Canvas.TopProperty    , Snap.Y);
            }
        }
        //Changing Layer Depths
        private void LV_Layers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem draggedItem = null;
            if (LV_Sprites.Items.Count > 0)
            {
                draggedItem = LV_Layers.SelectedItem as ListViewItem;
            }
            if (draggedItem != null)
            {
                MessageBox.Show(draggedItem.sName);
            }
        }
        private void LV_Layers_Drop(object sender, DragEventArgs e)
        {
            MessageBox.Show("Yeah Dropped");
        }
        //resizing
        private void CheckBounds()
        {
            //This will replace CheckSize
        }
        private void ClearAll()
        {
            canvasSpriteSheet.Children.Clear();
            LV_Layers.Items.Clear();
            LV_Sprites.Items.Clear();
            canvasSpriteSheet.Width = 0.0;
            Label_ImageWidth.Content = canvasSpriteSheet.Width;
            canvasSpriteSheet.Height = 0.0;
            Label_ImageHeight.Content = canvasSpriteSheet.Height;
            dXOff = 0.0;
            dYOff = 0.0;
        }
        private void CheckSize(double Width, double Height)
        {
            //Scale the image up towards the max Image Width
            if (dWidth + Width < dMaxWidth)
            {
                if (dXOff + Width > Power(dWidth))
                {
                    dWidth += Width;
                }
            }
            //Add the offset of the next image
            if (dXOff + Width < dMaxWidth)
            {
                dXOff += Width;
            }
            else
            {
                dXOff = 0d;
                dHeight += Height;
                dYOff += Height;
            }
            //Height
            if (dHeight < Height)
            {
                dHeight = Height;
            }
            //Replace this with AutoTiling
            if (dWidth != canvasSpriteSheet.Width && dWidth <= dMaxWidth)
            {
                canvasSpriteSheet.Width = Power(dWidth);
                dWidth = canvasSpriteSheet.Width;
                Label_ImageWidth.Content = canvasSpriteSheet.Width;
            }
            //Height
            if (dHeight != canvasSpriteSheet.Height && dHeight <= dMaxHeight)
            {
                canvasSpriteSheet.Height = Power(dHeight);
                dHeight = canvasSpriteSheet.Height;
                Label_ImageHeight.Content = canvasSpriteSheet.Height;
            }
        }
        private void CheckCanvas()
        {
            //check if there is an image outside the bounds
            //resize coresponding side
            //check the size of the canvas is still a Power2
            //if true continue
            //move opposites to keep the view in the same place
            //move other images to keep them in the same "place"
        }
        private int Power(double value)
        {
            //Check if its 0
            if (value == 0)
            {
                return 0;
            }
            //else
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
        private void ExportXML(String Path)
        {
            //This exports the canvas.. Needs to be fixed up 
            string sXML = XamlWriter.Save(canvasSpriteSheet);
            FileStream FS_Export = File.Create(Path);
            StreamWriter SW_Export = new StreamWriter(FS_Export);
            SW_Export.Write(sXML);
            SW_Export.Close();
            FS_Export.Close();
        }
    }
}
