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
        private static bool scanCurrentContent;
        private static bool showBackButtonWhenCanGoBack;

        public static readonly DependencyProperty IsBackButtonEnabledProperty = DependencyProperty.RegisterAttached("IsBackButtonEnabled", typeof(object), typeof(BackButtonManager), new PropertyMetadata(null, IsBackButtonEnabledChanged));

        public static object GetIsBackButtonEnabled(DependencyObject obj)
        {
            return (object)obj.GetValue(IsBackButtonEnabledProperty);
        }

        public static void SetIsBackButtonEnabled(DependencyObject obj, object value)
        {
            obj.SetValue(IsBackButtonEnabledProperty, value);
        }

        /// <summary>
        /// Register the frame.
        /// </summary>
        /// <param name="frame"> The frame. </param>
        /// <param name="showBackButtonWhenCanGoBack"> Automatically show back button when back navigation can happen. </param>
        /// <param name="scanCurrentContent"> Specifies whether the current window should be scanned for elements implementing <see cref="IBackAwareObject"/> which can prevent back navigation. </param>
        /// <param name="manuallyGoBack"> Specifies whether the BackButtonManager should manually go back when back key is pressed. If set to false, the default behavior of the platform is to deactivate the app instead of performing back navigation. </param>
        public static void RegisterFrame(Frame frame, bool showBackButtonWhenCanGoBack = true, bool scanCurrentContent = true, bool manuallyGoBack = true)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            BackButtonManager.frame = frame;
            BackButtonManager.showBackButtonWhenCanGoBack = showBackButtonWhenCanGoBack;
            BackButtonManager.scanCurrentContent = scanCurrentContent;
            BackButtonManager.manuallyGoBack = manuallyGoBack;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;

            frame.Navigated += FrameNavigated;
        }

        private static void FrameNavigated(object sender, NavigationEventArgs e)
        {
            var page = BackButtonManager.frame.Content as Page;

            SwitchBackButtonVisibility(GetIsBackButtonEnabled(page), page.GetType());
        }

        private static void SwitchBackButtonVisibility(object show, Type pageType)
        {
            bool s;
            if (show != null && bool.TryParse(show.ToString(), out s))
            {
                SwitchBackButtonVisibility(s, pageType);
            }
            else
            {
                SwitchBackButtonVisibility((bool?)null, pageType);
            }
        }

        private static void SwitchBackButtonVisibility(bool? show, Type pageType)
        {
            if (show.HasValue)
            {
                // visibility is set manually
                if (show.Value)
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
            else if (showBackButtonWhenCanGoBack && frame.CanGoBack)
            {
                // can go back, show back button
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // no manual visibility set, cannot go back
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private static void BackRequested(object sender, BackRequestedEventArgs e)
        {
            var content = frame;

            // start with the deepest objects. Unfortunatelly this cannot be cached, even for a single page, because custom dialogs 
            // might be injected dynamically into the View
            var items = scanCurrentContent ? content.FindVisualChildren<IBackAwareObject>().Distinct().Reverse().ToList() : Enumerable.Empty<IBackAwareObject>();

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

        private static void IsBackButtonEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }

            var page = obj as Page;

            if (page == null)
            {
                throw new ArgumentException("This attached property targets only Page");
            }

            SwitchBackButtonVisibility(e.NewValue.ToString(), page.GetType());
        }
    }
}
