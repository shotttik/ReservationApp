namespace Domain.DTO
{
    public sealed record BookingFieldChange(
        string Field,
        string? OldValue,
        string? NewValue);

}
