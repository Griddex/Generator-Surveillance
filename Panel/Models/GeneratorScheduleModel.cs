using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models
{
    public class GeneratorScheduleModel : ValidateNotifyModelBase
    {
        private string _generatorName;

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(GeneratorScheduleModel), "ContainsNoSpecialCharactersValidation")]
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

        private string _everyHrs;

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(GeneratorScheduleModel), "ContainsNoSpecialCharactersValidation")]
        public string EveryHrs
        {
            get { return _everyHrs; }
            set
            {
                _everyHrs = value;
                OnPropertyChanged(nameof(EveryHrs));
                OnErrorChanged(nameof(EveryHrs));
            }
        }

        private string _hrsThreshold;

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(GeneratorScheduleModel), "ContainsNoSpecialCharactersValidation")]
        public string HrsThreshold
        {
            get { return _hrsThreshold; }
            set
            {
                _hrsThreshold = value;
                OnPropertyChanged(nameof(HrsThreshold));
                OnErrorChanged(nameof(HrsThreshold));
            }
        }

        private string _Authoriser;

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(GeneratorScheduleModel), "ContainsNoSpecialCharactersValidation")]
        public string Authoriser
        {
            get { return _Authoriser; }
            set
            {
                _Authoriser = value;
                OnPropertyChanged(nameof(Authoriser));
                OnErrorChanged(nameof(Authoriser));
            }
        }
    }
}
