using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.InputModels
{
    public class HoursModel : ValidateNotifyModelBase
    {
        private int _hours;

        [Required]        
        public int Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged("Hours");
                OnErrorChanged("Hours");
            }
        }        
    }
}
