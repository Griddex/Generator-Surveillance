namespace Panel.Interfaces
{
    public interface IReminderSettingsRepository
    {
        void SetRepeatReminder(string GeneratorName);
        void DeactivateRepeatReminder(string GeneratorName);
        void DeleteReminder(string GeneratorName);
    }
}
