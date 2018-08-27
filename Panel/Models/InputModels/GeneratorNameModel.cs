using System.ComponentModel.DataAnnotations;

namespace Panel.Models.InputModels
{
    public class GeneratorNameModel : ValidateNotifyModelBase
    {
        private string _generatorName;

        [Required]
        [StringLength(150)]
        public string GeneratorName
        {
            get { return _generatorName; }
            set
            {
                _generatorName = value;
                OnPropertyChanged(nameof(GeneratorName));
                OnErrorChanged(nameof(GeneratorName));
            }
        }       
    }
}
