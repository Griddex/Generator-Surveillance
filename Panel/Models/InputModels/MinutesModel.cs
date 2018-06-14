using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.InputModels
{
    public class MinutesModel : ValidateNotifyModelBase
    {
        private int _minutes;        

        [Required]
        public int Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged("Minutes");
                OnErrorChanged("Minutes");
            }
        }        
    }
}
