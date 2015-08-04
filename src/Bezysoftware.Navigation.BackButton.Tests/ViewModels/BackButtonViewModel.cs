namespace Bezysoftware.Navigation.BackButton.Tests.ViewModels
{
    using GalaSoft.MvvmLight;

    public class BackButtonViewModel : ViewModelBase
    {
        private bool isBackButtonEnabled;

        public bool IsBackButtonEnabled
        {
            get { return this.isBackButtonEnabled; }
            set { this.Set(() => this.IsBackButtonEnabled, ref this.isBackButtonEnabled, value); }
        }
    }
}
