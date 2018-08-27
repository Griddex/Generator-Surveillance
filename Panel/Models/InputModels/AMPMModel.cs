using System.ComponentModel.DataAnnotations;

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
