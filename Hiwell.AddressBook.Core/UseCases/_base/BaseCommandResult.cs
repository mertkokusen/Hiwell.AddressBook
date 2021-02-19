namespace Hiwell.AddressBook.Core.UseCases
{
    public abstract class BaseCommandResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public BaseCommandResult(string error = null)
        {
            this.Success = error is null;
            this.Error = error;
        }
    }
}
