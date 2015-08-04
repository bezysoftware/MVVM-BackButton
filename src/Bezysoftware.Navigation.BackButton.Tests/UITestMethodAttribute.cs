namespace Bezysoftware.Navigation.BackButton.Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using System;
    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;

    public class UITestMethodAttribute : TestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            TestResult result = null;

            CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                result = testMethod.Invoke(new object[0]);
            }).AsTask().GetAwaiter().GetResult();

            return new TestResult[] { result };
        }
    }
}
