using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SoundBoard.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// Edit XML Comment Template for DialogService
    public static class DialogService
    {
        #region ErrorMessageDialog with OK button

        /// <summary>Shows the error message box.</summary>
        /// <param name="errorText">The error text.</param>
        /// Edit XML Comment Template for ShowErrorMessageBox
        public static void ShowErrorMessageBox(string errorText)
        {
            ShowErrorMessageBox(errorText, "");
        }

        /// <summary>Shows the error message box.</summary>
        /// <param name="errorText">The error text.</param>
        /// <param name="exceptionText">The exception text.</param>
        /// Edit XML Comment Template for ShowErrorMessageBox
        public static void ShowErrorMessageBox(string errorText, string exceptionText)
        {
            string sText = "Error:" + Environment.NewLine + errorText + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            if (string.IsNullOrEmpty(exceptionText) == false)
            {
                sText += "Exception:" + Environment.NewLine + exceptionText;
            }

            MessageBox.Show(sText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region WarningMessageDialog with yes no buttons

        /// <summary>Shows the warning message box.</summary>
        /// <param name="warningText">The warning text.</param>
        /// <returns>MessageBoxResult</returns>
        /// Edit XML Comment Template for ShowWarningMessageBox
        public static MessageBoxResult ShowWarningMessageBox(string warningText)
        {
            return ShowWarningMessageBox(warningText, "");
        }

        /// <summary>Shows the warning message box.</summary>
        /// <param name="warningText">The warning text.</param>
        /// <param name="exceptionText">The exception text.</param>
        /// <returns>MessageBoxResult</returns>
        /// Edit XML Comment Template for ShowWarningMessageBox
        public static MessageBoxResult ShowWarningMessageBox(string warningText, string exceptionText)
        {
            string sText = "Warning:" + Environment.NewLine + warningText + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            if (string.IsNullOrEmpty(exceptionText) == false)
            {
                sText += exceptionText;
            }

            return (MessageBox.Show(sText, "Information", MessageBoxButton.YesNo, MessageBoxImage.Question));
        }

        #endregion

        #region WarningMessageDialog with OK button
        /// <summary>
        /// Shows the warning message box with OK button
        /// </summary>
        /// <param name="warningText"></param>
        /// <returns>MessageBoxResult</returns>
        public static MessageBoxResult ShowWarningMessageBoxOk(string warningText)
        {
            return ShowWarningMessageBoxOk(warningText, "");
        }

        /// <summary>
        /// Shows the warning message box wih OK button
        /// </summary>
        /// <param name="warningText"></param>
        /// <param name="exceptionText"></param>
        /// <returns>MessageBoxResult</returns>
        public static MessageBoxResult ShowWarningMessageBoxOk(string warningText, string exceptionText)
        {
            string sText = "Warning:" + Environment.NewLine + warningText + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            if (string.IsNullOrEmpty(exceptionText) == false)
            {
                sText += exceptionText;
            }

            return (MessageBox.Show(sText, "Information", MessageBoxButton.OK, MessageBoxImage.Question));
        }
        #endregion

        #region ConfirmationMessageDialog with yes no buttons

        /// <summary>Shows the error message box.</summary>
        /// <param name="errorText">The error text.</param>
        /// Edit XML Comment Template for ShowErrorMessageBox
        public static MessageBoxResult ShowConfirmationMessagebox(string message)
        {
            return ShowConfirmationMessagebox(message, "");
        }

        /// <summary>Shows the warning message box.</summary>
        /// <param name="warningText">The warning text.</param>
        /// <param name="exceptionText">The exception text.</param>
        /// <returns>MessageBoxResult</returns>
        /// Edit XML Comment Template for ShowWarningMessageBox
        public static MessageBoxResult ShowConfirmationMessagebox(string message, string exceptionText)
        {
            string sText = "Confirm:" + Environment.NewLine + message + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            if (string.IsNullOrEmpty(exceptionText) == false)
            {
                sText += exceptionText;
            }

            return (MessageBox.Show(sText, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question));
        }
        #endregion
    }
}
