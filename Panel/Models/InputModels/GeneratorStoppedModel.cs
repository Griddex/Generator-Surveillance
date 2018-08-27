using System;

namespace Panel.Models.InputModels
{
    public class GeneratorStoppedModel : ValidateNotifyModelBase
    {
        public HoursModel Hours { get; set; }
        public MinutesModel Minutes { get; set; }
        public SecondsModel Seconds { get; set; }

        private TimeSpan _generatorStopped;
        private TimeSpan _resultStopped;
        public TimeSpan GeneratorStopped
        {
            get => _generatorStopped;
            set
            {
                if (TimeSpan.TryParse(Hours.Hours + ":" + Minutes.Minutes + ":" + Seconds.Seconds, out _resultStopped))
                    _generatorStopped = _resultStopped;
                OnPropertyChanged(nameof(GeneratorStopped));
                OnErrorChanged(nameof(GeneratorStopped));
            }
        }
    }
}
