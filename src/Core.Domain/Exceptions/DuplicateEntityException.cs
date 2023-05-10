namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions
{
    // TODO: Add docs comments

    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException() { }
        public DuplicateEntityException(string message) : base(message) { }
        public DuplicateEntityException(string message, Exception innerException) : base(message, innerException)
        { }

        public DuplicateEntityException(string name, object key)
            : base($"Entity \"{name}\" with key ({key}) already exists.")
        { }
    }
}
