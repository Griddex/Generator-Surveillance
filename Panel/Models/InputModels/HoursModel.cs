using System.ComponentModel.DataAnnotations;

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
