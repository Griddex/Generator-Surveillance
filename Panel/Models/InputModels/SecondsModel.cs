using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.InputModels
{
    public class SecondsModel : ValidateNotifyModelBase
    {
        private int _seconds;
      
        [Required]
        public int Seconds
        {
            get { return _seconds; }
            set
            {
                _seconds = value;
                OnPropertyChanged("Seconds");
                OnErrorChanged("Seconds");
            }
        }
    }
}
