namespace Bezysoftware.Navigation.BackButton
{
    /// <summary>
    /// It can be either a View or it's backing DataContext.
    /// </summary>
    public interface IBackAwareObject
    {
        /// <summary>
        /// Called when back key is pressed and implementing view has focus.
        /// </summary>
        /// <returns> True if navigation back should be allowed. </returns>
        bool AllowBackKeyNavigation();
    }
}
