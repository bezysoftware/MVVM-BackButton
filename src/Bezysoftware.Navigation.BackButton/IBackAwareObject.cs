namespace Bezysoftware.Navigation.BackButton
{
    /// <summary>
    /// Allows the implementing object to prevent back navigation triggered by back button, voice or gesture commands. It needs to be either a View (Page or its element) or it's backing DataContext.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Usually you will want to use implement this interface on your Pages, Dialogs and other View related objects. 
    /// </para>
    /// <para>
    /// Since the <see cref="BackButtonManager"/> scans also DataContext's of visual elements, you can also use it your ViewModels. 
    /// However the recommendation is to use it only on ViewModels which are backing already visible dialogs (for example your PageViewModel
    /// displays a dialog which has a PageViewModel as its DataContext).
    /// </para>
    /// <para>
    /// If you want to be able to handle back button on your typical page ViewModels, use the MVVM Navigation framework instead. 
    /// https://github.com/bezysoftware/MVVM-Navigation
    /// </para>
    /// </remarks>
    public interface IBackAwareObject
    {
        /// <summary>
        /// Called when back navigation is requested using back button, voice or gesture command and the implementing object is in active view.
        /// </summary>
        /// <returns> True if navigation back should be allowed. </returns>
        bool AllowBackNavigation();
    }
}
