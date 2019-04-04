using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Panel.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        protected ConcurrentDictionary<string, List<string>> _errors = new ConcurrentDictionary<string, List<string>>();
        public bool HasErrors
        {
            get
            {
                { return _errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> errorsForName;
            _errors.TryGetValue(propertyName, out errorsForName);
            return errorsForName;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public void OnErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }                 
    }
}
