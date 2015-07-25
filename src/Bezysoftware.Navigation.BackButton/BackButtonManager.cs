namespace Bezysoftware.Navigation.BackButton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;


    /// <summary>
    /// Helper class which can monitor back key presses and delegate corresponding events to active Views and their DataContexts which then have the option to handle it.
    /// </summary>
    public class BackButtonManager : DependencyObject
    {
        private static Frame frame;
        private static bool manuallyGoBack;
        private static List<Type> pageTypes;

        public static readonly DependencyProperty IsBackButtonEnabledProperty = DependencyProperty.RegisterAttached("IsBackButtonEnabled", typeof(bool), typeof(BackButtonManager), new PropertyMetadata(0));

        static BackButtonManager()
        {
            pageTypes = new List<Type>();
        }

        public static bool GetIsBackButtonEnabled(DependencyObject obj)
        {
            var page = obj as Page;

            if (page == null)
            {
                throw new ArgumentException("This attached property targets only Page");
            }

            return (bool)obj.GetValue(IsBackButtonEnabledProperty);
        }

        public static void SetIsBackButtonEnabled(DependencyObject obj, bool value)
        {
            var page = obj as Page;

            if (page == null)
            {
                throw new ArgumentException("This attached property targets only Page");
            }

            if (value)
            {
                pageTypes.Add(page.GetType());
            }
            else
            {
                pageTypes.Remove(page.GetType());
            }

            obj.SetValue(IsBackButtonEnabledProperty, value);
        }

        /// <summary>
        /// Register the frame.
        /// </summary>
        /// <param name="frame"> The frame. </param>
        /// <param name="manuallyGoBack"> Specifies whether the BackKeyHelper should manually go back when back key is pressed. If set to false, the default behavior of the platform is to deactivate the app instead of performing back navigation. </param>
        public static void RegisterFrame(Frame frame, bool manuallyGoBack = true)
        {
            BackButtonManager.frame = frame;
            BackButtonManager.manuallyGoBack = manuallyGoBack;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;

            frame.Navigated += FrameNavigated;
        }

        private static void FrameNavigated(object sender, NavigationEventArgs e)
        {
            if (pageTypes.Contains(e.SourcePageType))
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private static void BackRequested(object sender, BackRequestedEventArgs e)
        {
            var content = frame;

            // start with the deepest objects. Unfortunatelly this cannot be cached, even for a single page, because custom dialogs 
            // might be injected dynamically into the View
            var items = content.FindVisualChildren<IBackAwareObject>().Distinct().Reverse().ToList();

            if (items.Any(view => !view.AllowBackNavigation()))
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
