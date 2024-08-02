using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XBox.Etc.UserControl
{
    public class ObservableErrorInfo : ObservableObject, IDataErrorInfo
    {
        private readonly Dictionary<string, string> _propertyErrors = new Dictionary<string, string>();
        private string _error;
        private bool _hasErrors;

        public string this[string columnName]
        {
            get
            {
                string result;
                if (!_propertyErrors.TryGetValue(columnName, out result))
                    return null;

                return result;
            }
        }

        public string Error
        {
            get { return _error; }
            protected set
            {
                if (SetProperty(ref _error, value))
                {
                    UpdateHasErrors();
                }
            }
        }

        public bool HasErrors
        {
            get { return _hasErrors; }
            set { SetProperty(ref _hasErrors, value); }
        }

        protected bool SetValidated(string propertyName, ref string textField, ref int field, string text, int? minValue, int? maxValue)
        {
            if (text != textField)
            {
                textField = text;

                int parsed;
                if (int.TryParse(text, out parsed))
                {
                    if (parsed >= (minValue ?? int.MinValue) && parsed <= (maxValue ?? int.MaxValue))
                    {
                        SetPropertyError(propertyName, null);
                        field = parsed;
                        RaisePropertyChanged(propertyName);
                        return true;
                    }
                }

                string message;
                if (minValue != null && maxValue != null)
                {
                    message = $"Value must be between {minValue.Value} and {maxValue.Value}";
                }
                else if (minValue != null)
                {
                    message = $"Value must be greater than or equal to {minValue.Value}";
                }
                else if (maxValue != null)
                {
                    message = $"Value must be less than or equal to {maxValue.Value}";
                }
                else
                {
                    message = "Value must be a valid integer";
                }
                SetPropertyError(propertyName, message);
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected bool SetValidated(ref string textField, ref int field, string text, [CallerMemberName] string propertyName = null)
        {
            if (text != textField)
            {
                textField = text;

                int parsed;
                if (int.TryParse(text, out parsed))
                {
                    SetPropertyError(propertyName, null);
                    field = parsed;
                    RaisePropertyChanged(propertyName);
                    return true;
                }

                string message = "Value must be an integer";
                SetPropertyError(propertyName, message);
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected bool SetValidated(ref string textField, ref double field, string text, double? minValue, double? maxValue, bool minExclusive = false, [CallerMemberName] string propertyName = null)
        {
            if (text != textField)
            {
                textField = text;

                double parsed;
                text = text.StartsWith(".") ? "0" + text : text;
                if (double.TryParse(text, out parsed))
                {
                    double min = (minValue ?? double.MinValue);
                    double max = (maxValue ?? double.MaxValue);
                    bool minOkay = minExclusive ? parsed > min : parsed >= min;
                    bool maxOkay = parsed <= max;
                    if (minOkay && maxOkay)
                    {
                        SetPropertyError(propertyName, null);
                        field = parsed;
                        RaisePropertyChanged(propertyName);
                        return true;
                    }
                }

                string message;
                if (minValue != null && maxValue != null)
                {
                    message = $"Value must be between {minValue.Value} and {maxValue.Value}";
                }
                else if (minValue != null)
                {
                    string exclusive = minExclusive ? "" : "or equal to ";
                    message = $"Value must be greater than {exclusive}{minValue.Value}";
                }
                else if (maxValue != null)
                {
                    message = $"Value must be less than or equal to {maxValue.Value}";
                }
                else
                {
                    message = "Value must be a valid decimal number";
                }

                SetPropertyError(propertyName, message);
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected bool SetValidated(ref string textField, ref decimal field, string text, decimal? minValue, decimal? maxValue, [CallerMemberName] string propertyName = null)
        {
            if (text != textField)
            {
                textField = text;

                decimal parsed;
                text = text.StartsWith(".") ? "0" + text : text;
                if (decimal.TryParse(text, NumberStyles.Any, null, out parsed))
                {
                    if (parsed >= (minValue ?? decimal.MinValue) && parsed <= (maxValue ?? decimal.MaxValue))
                    {
                        SetPropertyError(propertyName, null);
                        field = parsed;
                        RaisePropertyChanged(propertyName);
                        return true;
                    }
                }

                string message;
                if (minValue != null && maxValue != null)
                {
                    message = $"Value must be between {minValue.Value} and {maxValue.Value}";
                }
                else if (minValue != null)
                {
                    message = $"Value must be greater than or equal to {minValue.Value}";
                }
                else if (maxValue != null)
                {
                    message = $"Value must be less than or equal to {maxValue.Value}";
                }
                else
                {
                    message = "Value must be a valid decimal number";
                }

                SetPropertyError(propertyName, message);
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected bool SetValidated(ref string textField, ref double field, string text, [CallerMemberName] string propertyName = null)
        {
            if (text != textField)
            {
                textField = text;

                double parsed;
                if (double.TryParse(text, out parsed))
                {
                    SetPropertyError(propertyName, null);
                    field = parsed;
                    RaisePropertyChanged(propertyName);
                    return true;
                }

                string message = "Value must be a valid decimal number";
                SetPropertyError(propertyName, message);
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void SetPropertyError(string propertyName, string error)
        {
            VerifyPropertyName(propertyName);
            _propertyErrors[propertyName] = error;
            UpdateHasErrors();
        }

        private void UpdateHasErrors()
        {
            HasErrors = Error != null || _propertyErrors.Any(kvp => kvp.Value != null);
        }
    }
}
