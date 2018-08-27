using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Models
{
    public class ValidateNotifyModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public void OnErrorChanged(string propertyName) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        private ConcurrentDictionary<string, List<string>> errors = new ConcurrentDictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        public bool HasErrors
        {
            get { return errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        private object _lock = new object();
        public void Validate()
        {
            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var kv in errors.ToList())
                {
                    if (validationResults.All(r => r.MemberNames.All(m => m != kv.Key)))
                    {
                        List<string> outList;
                        errors.TryRemove(kv.Key, out outList);
                        OnErrorChanged(kv.Key);
                    }
                }

                var q = from r in validationResults
                        from m in r.MemberNames
                        group r by m into g
                        select g;

                foreach (var prop in q)
                {
                    var messages = prop.Select(r => r.ErrorMessage).ToList();
                    if (errors.ContainsKey(prop.Key))
                    {
                        List<string> outList;
                        errors.TryRemove(prop.Key, out outList);
                    }
                    errors.TryAdd(prop.Key, messages);
                    OnErrorChanged(prop.Key);
                }
            }
        }
    }
}
