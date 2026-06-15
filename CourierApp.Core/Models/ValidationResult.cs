namespace CourierApp.Core.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; } = true;
        public List<string> Errors { get; private set; } = [];

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }
    }
}
