using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Panel.Models.InputModels
{
    public class GeneratorStartedModel : ValidateNotifyModelBase
    {
        public HoursModel Hours { get; set; }
        public MinutesModel Minutes { get; set; }
        public SecondsModel Seconds { get; set; }
        public AMPMModel AMPM { get; set; }

        private TimeSpan _generatorStarted;
        private TimeSpan _resultStarted;
        public TimeSpan GeneratorStarted
        {
            get => _generatorStarted;
            set
            {
                if (TimeSpan.TryParse(Hours.Hours + ":" + Minutes.Minutes + ":" + Seconds.Seconds + AMPM.AMPM, out _resultStarted))
                    _generatorStarted = _resultStarted;
                OnPropertyChanged(nameof(GeneratorStarted));
                OnErrorChanged(nameof(GeneratorStarted));
            }
        }
    }
}
