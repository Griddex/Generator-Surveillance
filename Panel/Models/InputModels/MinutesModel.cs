using System.ComponentModel.DataAnnotations;

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
