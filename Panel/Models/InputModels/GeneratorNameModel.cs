using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Models.InputModels
{
    public class GeneratorNameModel : ValidateNotifyModelBase
    {
        private string _generatorName;

        [Required]
        [StringLength(50)]
        [CustomValidation(typeof(GeneratorNameModel), "ContainsNoSpecialCharactersValidation")]
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
