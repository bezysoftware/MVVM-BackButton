﻿namespace Bezysoftware.Navigation.BackButton
{
    using System.Collections.Generic;
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// The visual tree extensions.
    /// </summary>
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// Finds visual children of given type
        /// </summary>
        /// <param name="parent"> The parent </param>
        /// <typeparam name="T"> Type of visual child to look for </typeparam>
        /// <returns> The <see cref="IEnumerable"/>. </returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) 
            where T : class
        {
            return FindVisualChildrenInternal<T>(parent).Concat(FindVisualChildrenInPopups<T>());
        }

        private static IEnumerable<T> FindVisualChildrenInternal<T>(DependencyObject parent) where T : class
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                    var dataContext = ((child as FrameworkElement)?.DataContext as T);
                    if (dataContext != null)
                    {
                        yield return dataContext;
                    }

                    if (child != null && child is T)
                    {
                        yield return child as T;
                    }

                    foreach (T childOfChild in FindVisualChildrenInternal<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private static IEnumerable<T> FindVisualChildrenInPopups<T>() where T : class
        {
            foreach (var popup in VisualTreeHelper.GetOpenPopups(Window.Current))  
            {
                foreach (T childOfChild in FindVisualChildrenInternal<T>(popup.Child))
                {
                    yield return childOfChild;
                }
            }
        }
    }
}
