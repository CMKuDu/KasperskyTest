using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.DTOs;

namespace TestTelcoHub.Command.ApprovalCode.Create
{
    public class CreateApprovalCodeCommand : ICommand<ReponseBase>
    {
        public ApprovalCodeDTO ApprovalCode { get; set; }
        public CreateApprovalCodeCommand (ApprovalCodeDTO approvalCodeDTO)
        {
            ApprovalCode = approvalCodeDTO;
        }
    }
}
