using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace XBox.Etc.UserControl
{
    /// <summary>
    /// Services to allow display of various windows/dialogs in the application
    /// </summary>
    public interface IWindowServices
    {
        /// <summary>
        /// Show a common dialog, block until it is closed,
        /// then return its result; this method is used
        /// rather than showing the dialog directly to support
        /// unit testing.
        /// </summary>
        /// <param name="dialog">Dialog to show</param>
        /// <returns></returns>
        bool? ShowDialog(CommonDialog dialog);

        /// <summary>
        /// Request confirmation with a dialog, with Yes/No (and possibly Cancel) as valid responses.
        /// Will block until the dialog is closed and then return the result.
        /// </summary>
        /// <param name="message">Message for the confirmation</param>
        /// <param name="caption">Caption (window title) of the dialog</param>
        /// <param name="allowCancel">Whether to allow Cancel as an option</param>
        /// <returns>True if 'yes', false if 'no', null if 'cancel'</returns>
        bool? RequestConfirmation(string message, string caption, bool allowCancel = false);

        /// <summary>
        /// Show an error message dialog and block until it is closed.
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="caption">Caption (window title) of the dialog</param>
        void ShowError(string message, string caption);

        /// <summary>
        /// Show a warning message dialog and block until it is closed.
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="caption">Caption (window title) of the dialog</param>
        void ShowWarning(string message, string caption);

        /// <summary>
        /// Show an info message dialog and block until it is closed.
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="caption">Caption (window title) of the dialog</param>
        void ShowInfo(string message, string caption);

        /// <summary>
        /// Show a dialog corresponding to the specified view model and block until it is closed;
        /// this automatically looks up the appropriate view and will return the dialog result.
        /// </summary>
        /// <param name="viewModel">View model for dialog to display</param>
        /// <returns>Dialog result from the view</returns>
        bool? ShowDialog(Object viewModel);

        /// <summary>
        /// Show a popup window for the corresponding view model, looking up the view automatically;
        /// this method does not block until the popup is closed.
        /// </summary>
        /// <param name="viewModel">View model to show as popup</param>
        void ShowPopup(Object viewModel);

        /// <summary>
        /// Show a window for the corresponding view model, looking up the view automatically;
        /// this method does not block until the window is closed.
        /// </summary>
        /// <param name="viewModel">View model to show in a window</param>
        /// <param name="context">Context passed to Caliburn ShowWindow method</param>
        /// <param name="settings">Settings passed to Caliburn ShowWindow metohd</param>
        void ShowWindow(Object viewModel, object context = null, IDictionary<string, object> settings = null);

        /// <summary>
        /// Shows a modal window and returns immediately without closing it. The return from this method is a handle to the System.Windows.Window.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>The handle to the System.Windows.Window</returns>
        Window ShowModalWindow(ObservableObject viewModel);

        /// <summary>
        /// Gets the handle to the active window.
        /// </summary>
        /// <returns></returns>
        Window GetActiveWindow();
    }


}