namespace Bezysoftware.Navigation.BackButton.Tests.Views
{
    using ViewModels;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BindingButtonPage : Page
    {
        public BindingButtonPage()
        {
            this.InitializeComponent();
            this.DataContext = new BackButtonViewModel();
        }

        public BackButtonViewModel Vm
        {
            get { return this.DataContext as BackButtonViewModel; }
        }
    }
}
