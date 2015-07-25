namespace Bezysoftware.Navigation.BackButton.Sample
{
    using Windows.UI.Xaml.Controls;

    public sealed partial class Dialog : UserControl, IBackAwareObject
    {
        public Dialog()
        {
            this.InitializeComponent();
        }

        public bool AllowBackNavigation()
        {
            if (this.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return false;
            }

            return true;
        }
    }
}
