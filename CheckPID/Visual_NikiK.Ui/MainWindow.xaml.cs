using Xeam.Base.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Xeam.VisualInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(IBootstrapperMainWindow))]
    public partial class MainWindow : ThemedDialog, IBootstrapperMainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Theme theme)
            : base(theme)
        {
            InitializeComponent();
        }

        public Window Window
        {
            get { return this; }
        }

        public Theme Theme
        {
            set
            {
                SetTheme(value, ThemeBase, AccentColor, AccentContrastColor);//ThemeBase.BaseLightSmooth, Color.FromRgb(0xff, 0x7A, 0x00), Colors.Black);

            }
        }

        public ThemeBase ThemeBase
        {
            get;
            set;
        }

        public Color? AccentColor
        {
            get;
            set;
        }

        public Color? AccentContrastColor
        {
            get;
            set;
        }
    }
}
