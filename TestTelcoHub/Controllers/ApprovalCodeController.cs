using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTelcoHub.Command.ApprovalCode.Create;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Model.Prototype;

namespace TestTelcoHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalCodeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApprovalCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("GenerateApprovalCode")]
        public async Task<IActionResult> GenerateApprovalCode(ApprovalCodeDTO approvalCodeDTO)
        {
            var command = new CreateApprovalCodeCommand(approvalCodeDTO);
            var response = await _mediator.Send(command);
            return response.Status == ResponseContants.Success
                 ? Ok(new { Success = true, Message = response.Message })
                 : BadRequest(new { Success = false, Message = response.Message, response.Errors });
        }
    }
}
