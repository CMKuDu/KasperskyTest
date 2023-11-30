using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Command.ApprovalCode.Create
{
    public class CreateApprovalCodeCommandHandler : ICommandHandler<CreateApprovalCodeCommand, ReponseBase>
    {
        private readonly IApprovalCodeService _approvalCodeService;
        public CreateApprovalCodeCommandHandler (IApprovalCodeService approvalCodeService)
        {
            _approvalCodeService = approvalCodeService;
        }
        public async Task<ReponseBase> Handle(CreateApprovalCodeCommand request, CancellationToken cancellationToken)
        {
            return await _approvalCodeService.GenerateApprovalCode(request.ApprovalCode);
        }
    }
}
