using Application.Common.ResultsErrors;

namespace Application.Interfaces
{
    public interface ICompanyService
    {
        Task<Result<string>> InviteMember(int memberID);
        Task<Result> AcceptInvite(string token);
    }
}
