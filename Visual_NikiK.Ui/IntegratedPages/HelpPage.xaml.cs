using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using Xeam.VisualInstaller;

namespace Visual_NikiK.Ui
{
    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class HelpPage : UserControl, IBootstrapperPage
    {
        HelpPageViewModel _model;

        public HelpPage()
        {
            InitializeComponent();
        }

        public string PageName
        {
            get { return "Help"; }
        }

        public FrameworkElement Page
        {
            get { return this; }
        }

        public string LocalizedPageName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string LocalizedPageNameId => throw new NotImplementedException();

        public bool AddToProcessStepBar { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Initialize(MainWindowViewModel mainViewModel)
        {
            _model = new HelpPageViewModel(mainViewModel);
            this.DataContext = _model;
        }

        public bool OnSilentInstall()
        {
            return _model.OnSilentMode();
        }

        public bool OnPassiveInstall()
        {
            return _model.OnPassiveMode();
        }

        /// <summary>
        /// Called before a page is shown
        /// </summary>
        /// <returns>true - the page will be shown
        ///          false - the page will not be shown and the sequence will go to the next page</returns>
        public bool OnBeforeShowPage()
        {
            return true;
        }
    }
}
