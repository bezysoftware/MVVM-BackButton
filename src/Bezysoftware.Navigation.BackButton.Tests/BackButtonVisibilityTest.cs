namespace Bezysoftware.Navigation.BackButton.Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Bezysoftware.Navigation.BackButton.Tests.Views;
    using Windows.UI.Core;
    using FluentAssertions;
    using Windows.UI.Xaml.Controls;
    using System.Linq;
    using Windows.UI.Xaml;

    [TestClass]
    public class BackButtonVisibilityTest
    {
        [UITestMethod]
        public void BackButtonCollapsedOnFirstPage()
        {
            var frame = App.Frame;

            BackButtonManager.RegisterFrame(frame);
            frame.Navigate(typeof(UnsetButtonPage));
            
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Collapsed);
        }

        [UITestMethod]
        public void BackButtonVisibleOnSecondPage()
        {
            var frame = new Frame();

            BackButtonManager.RegisterFrame(frame);
            frame.Navigate(typeof(UnsetButtonPage));
            frame.Navigate(typeof(UnsetButtonPage));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Visible);
        }

        [UITestMethod]
        public void BackButtonVisibleOnFirstPage()
        {
            var frame = App.Frame;

            BackButtonManager.RegisterFrame(frame);
            frame.Navigate(typeof(VisibleButtonPage));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Visible);
        }

        [UITestMethod]
        public void BackButtonCollapsedOnSecondPage()
        {
            var frame = App.Frame;

            BackButtonManager.RegisterFrame(frame);
            frame.Navigate(typeof(UnsetButtonPage));
            frame.Navigate(typeof(CollapsedButtonPage));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Collapsed);
        }

        [UITestMethod]
        public void BackButtonCollapsedThenVisibleOnFirstPage()
        {
            var frame = App.Frame;

            BackButtonManager.RegisterFrame(frame);
            frame.Navigate(typeof(BindingButtonPage));

            (frame.Content as BindingButtonPage).Vm.IsBackButtonEnabled = false;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Collapsed);

            (frame.Content as BindingButtonPage).Vm.IsBackButtonEnabled = true;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Visible);
        }

        [UITestMethod]
        public void BackButtonCollapsedThenVisibleOnSecondPage()
        {
            var frame = App.Frame;

            BackButtonManager.RegisterFrame(frame);
            frame.Navigate(typeof(UnsetButtonPage));
            frame.Navigate(typeof(BindingButtonPage));

            (frame.Content as BindingButtonPage).Vm.IsBackButtonEnabled = false;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Collapsed);

            (frame.Content as BindingButtonPage).Vm.IsBackButtonEnabled = true;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.ShouldBeEquivalentTo(AppViewBackButtonVisibility.Visible);
        }
    }
}
