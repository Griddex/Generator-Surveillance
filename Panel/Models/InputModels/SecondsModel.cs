using System.ComponentModel.DataAnnotations;

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
