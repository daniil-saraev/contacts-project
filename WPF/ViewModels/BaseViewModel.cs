using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Desktop.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region DataErrorInfo

        private Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
                return new List<string>();

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };

            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                _validationErrors.Add(propertyName, new List<string>());
                foreach (ValidationResult validationResult in validationResults)
                {
                    _validationErrors[propertyName].Add(validationResult.ErrorMessage);
                }
            }
            RaiseErrorsChanged(propertyName);
        }

        public void ValidateModel()
        {
            _validationErrors.Clear();
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    string property = validationResult.MemberNames.ElementAt(0);
                    if (_validationErrors.ContainsKey(property))
                    {
                        _validationErrors[property].Add(validationResult.ErrorMessage);
                    }
                    else
                    {
                        _validationErrors.Add(property, new List<string> { validationResult.ErrorMessage });
                    }
                    RaiseErrorsChanged(property);
                }
            }
        }

        public void AddModelError(string property, string errorMessage)
        {
            if (_validationErrors.ContainsKey(property))
            {
                _validationErrors[property].Add(errorMessage);
            }
            else
            {
                _validationErrors.Add(property, new List<string> { errorMessage });
            }
            RaiseErrorsChanged(property);
        }

        #endregion

        #region Navigation

        public event Action<NavigationEventArgs>? NavigateCommandExecuted;

        protected void OnNavigateCommandExecuted<T>() where T : BaseViewModel
        {
            NavigateCommandExecuted?.Invoke(new NavigationEventArgs(nameof(T)));
        }

        #endregion

        public virtual void Dispose() { }
    }

    public class NavigationEventArgs
    {
        public string ViewModelClassName { get; }

        public NavigationEventArgs(string viewModelClassName)
        {
            ViewModelClassName = viewModelClassName;
        }
    }
}
