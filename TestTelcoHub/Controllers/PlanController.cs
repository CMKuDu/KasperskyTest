using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTelcoHub.Command.Product.Create;
using TestTelcoHub.Command.Product.Update.HardCancle;
using TestTelcoHub.Command.Product.Update.Renew;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Queries.Plans.GetPlans;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IBillingPlanService _billingPlanService;
        private readonly IMediator _mediator;
        //private readonly IMapper _mapper; 
        public PlanController(IBillingPlanService billingPlanService, IMediator mediator)
        {
            _billingPlanService = billingPlanService;
            _mediator = mediator;
            //_mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plan = await _mediator.Send(new GetPlanQuery());
            if (plan == null)
            {
                return BadRequest("Null");
            }
            return Ok(plan);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] PlanDTO planDTO)
        {
            var command = new CreateProductsCommand(planDTO);
            var result = await _mediator.Send(command);

            return result.Status == ResponseContants.Success
                ? Ok(new { Success = true, Message = result.Message, ExpirationInfo = result.ExpirationInfo, })
                : BadRequest(new { Success = false, Message = result.Message, result.Errors });
        }
        [HttpPost("HardCancle")]
        public async Task<IActionResult> HardCancle(HardCancleRq subsriptionId)
        {
            var command = new HardCancleCommand(subsriptionId);
            var result = await _mediator.Send(command);
            return result.Status == ResponseContants.Success
                ? Ok(new { Success = true, Message = result.Message, })
                : BadRequest(new { Success = false, Message = result.Message, result.Errors });
        }
        [HttpPost("Renew")]
        public async Task<IActionResult> Renew(HardCancleRq hardCancleRq)
        {
            var command = new RenewCommand(hardCancleRq);
            var response = await _mediator.Send(command);
            return response.Status == ResponseContants.Success
                 ? Ok(new { Success = true, Message = response.Message })
                 : BadRequest(new { Success = false, Message = response.Message,response.Errors });
        }

    }
}
