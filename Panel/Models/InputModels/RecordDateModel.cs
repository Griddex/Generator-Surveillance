using System;
using System.ComponentModel.DataAnnotations;

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
