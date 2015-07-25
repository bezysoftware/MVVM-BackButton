namespace Bezysoftware.Navigation.BackButton.Sample.ViewModels
{
    using System;
    using GalaSoft.MvvmLight;

    public class MainViewModel : ViewModelBase, IBackAwareObject
    {
        private bool allowBack = false;
        private string backNavigationWarning;

        public bool AllowBackKeyNavigation()
        {
            if (allowBack)
            {
                this.BackNavigationWarning = string.Empty;
            }
            else
            {
                this.BackNavigationWarning = "MainViewModel: Navigation prevented this time. Next back key press will be allowed.";
            }

            allowBack = !allowBack;

            return !allowBack;
        }

        public string BackNavigationWarning
        {
            get { return this.backNavigationWarning; }
            private set { this.Set(() => this.BackNavigationWarning, ref this.backNavigationWarning, value); }
        }
    }
}