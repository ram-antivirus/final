using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using Xeam.Base.MVVM;
using Xeam.VisualInstaller;
using Xeam.VisualInstaller.Utilities;

namespace Visual_NikiK.Ui
{
    public class InstalledAntivirusViewModel : PageViewModelBase
    {
        #region Bounded properties to the xaml controls, implementing INotifyPropertyChanged

        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }

        public string SubTitle
        {
            get { return GetValue(() => SubTitle); }
            set { SetValue(() => SubTitle, value); }
        }

        public string ButtonCancelText
        {
            get { return GetValue(() => ButtonCancelText); }
            set { SetValue(() => ButtonCancelText, value); }
        }

        public string ButtonPreviousText
        {
            get { return GetValue(() => ButtonPreviousText); }
            set { SetValue(() => ButtonPreviousText, value); }
        }

        public string CustomText
        {
            get { return GetValue(() => CustomText); }
            set { SetValue(() => CustomText, value); }
        }

        public string CustomLabelText
        {
            get { return GetValue(() => CustomLabelText); }
            set { SetValue(() => CustomLabelText, value); }
        }

        public bool NextButtonEnabled
        {
            get { return GetValue(() => NextButtonEnabled); }
            set { SetValue(() => NextButtonEnabled, value); }
        }

        #endregion

        #region Next Button Command bounded to the xaml-Button

        // This command will be executed, when the user presses the button "Enable the Next Button"
        public ICommand EnableNextButtonCommand
        {
            get
            {
                return new RelayCommand(
                      param =>
                      {
                          // The "IsEnabled"-property of the Next-button is bound to this property.
                          NextButtonEnabled = true;
                      });

            }
        }

        #endregion

        public InstalledAntivirusViewModel(MainWindowViewModel mainViewModel)
            : base(mainViewModel)
        {

            // By defaut, disable the Next button.
            NextButtonEnabled = false;

            // Set the strings of the page.
            Title = "CustomPage_Title";
            SubTitle = "CustomPage_SubTitle";
            CustomText = "CustomPage_CustomText";
            CustomLabelText = "CustomPage_CustomLabelText";

            // Read the text for the buttons from the localization files. These strings are predefined.
            ButtonCancelText = LocalizationReader.GetString("Button_Cancel");
            ButtonPreviousText = LocalizationReader.GetString("Button_Previous");

            // Note: You can localize your stings by defining the items you need in the localization files en-US/de-DE,...etc.
            // Example:
            //  Title = LocalizationReader.GetString("CustomPage_Title");
            //  Add in the localization file en-US the item CustomPage_Title with the value "This is my page title" and rebuild your project.

        }

        // design time constructor only
        public InstalledAntivirusViewModel()
            : this(null)
        {
        }

        #region protected overrides

        public override bool OnPassiveMode()
        {
            // ToDo: define here the behaviour of the page in the passive mode.
            return true;
        }

        public override bool OnSilentMode()
        {
            // ToDo: define here the behaviour of the page in the silent mode.
            return true;
        }

        #endregion

    }
}
