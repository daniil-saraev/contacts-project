using CommunityToolkit.Mvvm.Input;
using Desktop.Common.Commands.Async;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Desktop.Common.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// Encapsulates a process that needs to be represented with a progress bar.
        /// </summary>
        public IAsyncRelayCommand? LoadingTask { get; protected set; }

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

        /// <summary>
        /// Validates a value based on validation attributes the property has.
        /// Adds new validation error and raises <see cref="ErrorsChanged"/> if validation failed.
        /// </summary>
        protected virtual void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            if (_validationErrors.ContainsKey(propertyName))
                _validationErrors.Remove(propertyName);

            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };

            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                _validationErrors.Add(propertyName, new List<string>());
                foreach (var validationResult in validationResults)
                {
                    _validationErrors[propertyName].Add(validationResult.ErrorMessage);
                }
            }
            RaiseErrorsChanged(propertyName);
        }

        /// <summary>
        /// Validates all properties based on validation attributes they have.
        /// Adds new validation error for each property that failed validation and raises <see cref="ErrorsChanged"/>.
        /// </summary>
        public void ValidateModel()
        {
            _validationErrors.Clear();
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    var property = validationResult.MemberNames.ElementAt(0);
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


        /// <summary>
        /// Adds new validation error of a specified property and raises <see cref="ErrorsChanged"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="errorMessage"></param>
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
    }
}
