using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.InputModels
{
    public class AMPMModel : ValidateNotifyModelBase
    {
        private string _ampm;

        [Required]
        public string AMPM
        {
            get { return _ampm; }
            set
            {
                _ampm = value;
                OnPropertyChanged("AMPM");
                OnErrorChanged("AMPM");
            }
        }
    }
}
