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
using Xeam.VisualInstaller.Utilities;


namespace Visual_NikiK.Ui
{
    /// <summary>
    /// Interaction logic for SqlServerConnection.xaml
    /// </summary>
    [Export(typeof(IBootstrapperPage))]
    public partial class SqlServerConnectionPage : UserControl, IBootstrapperPage
    {
        private SqlServerConnectionPageViewModel _model;
        private bool alreadyLoaded;

        public SqlServerConnectionPage()
        {
            InitializeComponent();
        }

        private void DatabaseNameEntries_DropDownOpened(object sender, EventArgs e)
        {
            _model.UpdateDatabaseEntries();
        }

        private void ServerNameEntries_DropDownOpened(object sender, EventArgs e)
        {
            if (!alreadyLoaded)
            {
                _model.FindSqlServers();
                alreadyLoaded = true;
            }
        }

        public string PageName
        {
            get { return "SqlServerConnection"; }
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
            _model = new SqlServerConnectionPageViewModel(mainViewModel);
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
