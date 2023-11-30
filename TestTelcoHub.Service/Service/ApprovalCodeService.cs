using AutoMapper;
using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Service.Service
{
    public class ApprovalCodeService : IApprovalCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ApprovalCodeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReponseBase> GenerateApprovalCode(ApprovalCodeDTO approvalCode)
        {
            if (approvalCode != null && !string.IsNullOrEmpty(approvalCode.Code))
            {
                var approvalEntity = _mapper.Map<ApprovalCode>(approvalCode);

                await _unitOfWork.Approval.Add(approvalEntity);
                _unitOfWork.Compele();

                return new ReponseBase
                {
                    Status = ResponseContants.Success,
                    Message = "ApprovalCode được khởi tạo",
                };
            }

            return new ReponseBase
            {
                Status = ResponseContants.Error,
                Message = "ApprovalCode không được khởi tạo do dữ liệu không hợp lệ",
            };
        }

    }
}
