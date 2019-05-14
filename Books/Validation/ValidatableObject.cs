using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Books.Models;

namespace Books.Validation
{
    public class ValidatableObject<T> : NotifyBase
    {
        private T _value;
        private List<string> _errors;
        private readonly List<IValidationRule<T>> _rules;
        private bool _isValid;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyPropertyChange(nameof(Value));
            }
        }

        public List<string> Errors
        {
            get => _errors;
            set
            {
                _errors = value;
                NotifyPropertyChange(nameof(Errors));
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                NotifyPropertyChange(nameof(IsValid));
            }
        }

        public List<IValidationRule<T>> Rules => _rules;

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new List<string>();
            _rules = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = _rules.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
    }
}
