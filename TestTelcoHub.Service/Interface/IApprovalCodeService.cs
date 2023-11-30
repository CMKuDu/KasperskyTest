using TestTelcoHub.Constant;
using TestTelcoHub.Model.DTOs;

namespace TestTelcoHub.Service.Interface
{
    public interface IApprovalCodeService
    {
        public Task<ReponseBase> GenerateApprovalCode(ApprovalCodeDTO approvalCode);
    }
}
