namespace Application.Extensions.Mappers
{
    public static class SessionInfoMapper
    {
        public static Domain.DTO.SessionInfoSummaryDTO MapToSummaryDTO(this Domain.DTO.SessionInfoDTO sessionInfo)
        {
            return sessionInfo == null
                ? throw new ArgumentNullException(nameof(sessionInfo))
                : new Domain.DTO.SessionInfoSummaryDTO
                {
                    SessionId = sessionInfo.SessionId,
                    DeviceInfo = sessionInfo.DeviceInfo,
                    CreatedAt = sessionInfo.CreatedAt,
                    LastAccessedAt = sessionInfo.LastAccessedAt
                };
        }
    }
}
