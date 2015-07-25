namespace Bezysoftware.Navigation.BackButton
{
    using System.Linq;
    using Windows.UI.Core;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Helper class which can monitor back key presses and delegate corresponding events to active Views and their DataContexts which then have the option to handle it.
    /// </summary>
    public static class BackKeyHelper
    {
        private static Frame frame;
        private static bool manuallyGoBack;

        /// <summary>
        /// Register the frame.
        /// </summary>
        /// <param name="frame"> The frame. </param>
        /// <param name="manuallyGoBack"> Specifies whether the BackKeyHelper should manually go back when back key is pressed. If set to false, the default behavior of the platform is to deactivate the app instead of performing back navigation. </param>
        public static void RegisterFrame(Frame frame, bool manuallyGoBack)
        {
            BackKeyHelper.frame = frame;
            BackKeyHelper.manuallyGoBack = manuallyGoBack;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackKeyPressed;
        }

        private static void BackKeyPressed(object sender, BackRequestedEventArgs e)
        {
            var content = frame;

            // start with the deepest objects. Unfortunatelly this cannot be cached, even for a single page, because custom dialogs 
            // might be injected dynamically into the View
            var items = content.FindVisualChildren<IBackAwareObject>().Reverse().ToList();

            if (items.Any(view => view.AllowBackKeyNavigation()))
            {
                e.Handled = true;
            }
            else if (manuallyGoBack && frame.CanGoBack)
            {
                // the default behaviour is to close (suspend) the app
                e.Handled = true;
                frame.GoBack();
            }
        }
    }
}
