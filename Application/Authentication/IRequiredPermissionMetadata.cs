namespace Application.Authentication
{
    public interface IRequiredPermissionMetadata
    {
        IReadOnlyList<string> RequiredPermissions { get; }
    }
}
