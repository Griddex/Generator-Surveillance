using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Panel.Models.InputModels
{
    public class RecordDateModel : ValidateNotifyModelBase
    {
        private DateTime _recordDate;
        
        [Required]
        public DateTime RecordDate
        {
            get { return _recordDate; }
            set
            {
                _recordDate = value;
                OnPropertyChanged("RecordDate");
                OnErrorChanged("RecordDate");
            }
        }        
    }
}
