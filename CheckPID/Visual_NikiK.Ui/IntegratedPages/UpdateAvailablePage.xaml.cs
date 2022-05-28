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

namespace Visual_NikiK.Ui.IntegratedPages
{
    /// <summary>
    /// Interaction logic for CheckForUpdatePage.xaml
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class UpdateAvailablePage : UserControl, IBootstrapperPage
    {
        UpdateAvailablePageViewModel _model;

        public string PageName { get { return "UpdateAvailable"; } }
        public FrameworkElement Page { get { return this; } }

        public UpdateAvailablePage()
        {
            InitializeComponent();
        }

        public void Initialize(MainWindowViewModel mainViewModel)
        {
            // create view model for the page
            _model = new UpdateAvailablePageViewModel(mainViewModel);
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
            return _model.OnBeforeShowPage();
        }

    }
}
