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
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public String MyText
        {
            get { return String.Format("This tool was created by Luke Monaghan.\n \nFull source is avaliable at :\nwww.github.com/lukemonaghan/SpriteMapGenerator \n \nCreated for Educational Porpose at the Academy Of Interactive Entertainment Canberra."); }
        }

        public About()
        {
            InitializeComponent();
            textBlock1.Text = MyText;
        }
    }
}
