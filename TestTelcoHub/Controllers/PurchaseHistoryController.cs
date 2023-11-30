using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTelcoHub.Command.PurchaseHistorise.Update.ModifyExpiration;
using TestTelcoHub.Command.PurchaseHistorise.Update.ModifyQuantity;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Queries.Purs.GetPurById;
using TestTelcoHub.Queries.Purs.GetPurs;
using TestTelcoHub.Queries.Purs.GetPursByUser;

namespace TestTelcoHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PurchaseHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var pur = await _mediator.Send(new GetPurQuery());
            if (pur == null)
            {
                return NotFound("Danh sach rong");
            }
            return Ok(pur);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string subscriptionId)
        {
            var pur = await _mediator.Send(new GetPursByIdQuery(subscriptionId));
            if (pur == null)
            {
                return NotFound("Không có thông tin");
            }
            return Ok(pur);
        }
        [HttpGet("GetByUser")]
        [Authorize]
        public async Task<IActionResult> GetPurchaseHistoryByUser()
        {
            var pur = await _mediator.Send(new GetPursByUserQuery());
            if (pur == null)
            {
                return NotFound("Danh sach rong");
            }
            return Ok(pur);
        }
        [HttpPut("ModifyExpiration")]
        public async Task<IActionResult> ModifyExpiration(ModifyExpirationViewModel model)
        {
            var command = new ModifyExpirationCommand(model);
            var result = await _mediator.Send(command);

            return result.Status == ResponseContants.Success
                 ? Ok(new { Success = true, Message = result.Message })
                 : BadRequest(new { Success = false, Message = result.Message,result.Errors});
        }
        [HttpPut("ModifyQuantity")]
        public async Task<IActionResult> ModifyQuantity(ModifyQuantityViewModel model)
        {
            var command = new ModifyQuantityCommand(model);
            var response = await _mediator.Send(command);
            return response.Status == ResponseContants.Success
                ? Ok(new { Success = true, Message = response.Message })
                : BadRequest(new { Success = false, Message = response.Message,response.Errors });
        }
        
    }
}
