# MVVM BackButton

## What?
Project to simplify your life with the back button in UWP.

## Why?
In UWP, working with the back button is not very convenient. There are three areas where you need to manually write rendundant code to work with it:

#### Show / hide back button on desktop
On desktop the app can display a back button in the top chrome. Pressing it has the same effect the back button has on a phone. Showing and hiding it needs to be done manually:

```csharp 
SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
```

This will show the button for every page in your app (including the main page). However, when you want to fine tune the visibility of the button (have it visible base on some condition in you ViewModel, or just have it hidden on some pages but visible on others), is when this library comes in handy. 

```xaml
<Page
    x:Class="Bezysoftware.Navigation.BackButton.Sample.SecondPage"
    ...
    xmlns:back="using:Bezysoftware.Navigation.BackButton"
    back:BackButtonManager.IsBackButtonEnabled="True">
    ...
</Page>
```

You can of couse use binding as well as static value. The default behavior is to only show the back button if navigation allows to go back and hide it when you cannot go back (typically on your main page).

#### Navigate to previous page on back button click
The default behavior of the back button is to deactivate the app on a phone and do nothing on desktop. You have to manually override this behavior to get the back navigation you'd expect. If you use this library, you get this for free.

#### Prevent back navigation
Last useful feature is to prevent navigation completely based on some condition. Typical scenario would be when you have a dialog open and instead of going back, you just want the dialog closed. Assuming the dialog is a UserControl, this gets very easy:
```csharp
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
```

The control just needs to implement `IBackAwareObject`. When back button is pressed, the `BackButtonManager` scan the current content of the Window, walks the Visual Tree and tries to find all instances of `IBackAwareObject`. It starts calling the `AllowBackNavigation` method (starting with the object deepest down the Visual Tree) and only allows back navigation if all objects in current View allow it.

The library also scans the DataContext of every UI element, so you can have your backing ViewModels implement the `IBackAwareObject` as well.

## How?
Initialization is a one-liner

```csharp
BackButtonManager.RegisterFrame(rootFrame, true, true, true);
```

Put this into your `App.xaml.cs`. The other three parameters specify whether back button should be automatically displayed when back navigation can happen (`frame.CanGoBack`), visual tree should be scanned for `IBackAwareObject`s (this can be fairly expensive if you have a complex layout) and whether back navigation should be performed when back button is pressed. `True` is default for all three.

## Where?

```
Install-Package Bezysoftware.Navigation.BackButton -Pre
```
