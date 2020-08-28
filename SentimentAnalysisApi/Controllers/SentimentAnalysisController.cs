using MediatR;
using Microsoft.AspNetCore.Mvc;
using SentimentMediator;
using System;
using System.Threading.Tasks;

namespace SentimentAnalysisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentimentAnalysisController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SentimentAnalysisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/sentimentanalysis?message=this+is+awesome
        [HttpGet]
        public async Task<IActionResult> Get(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new Exception("Bad Request");

            return Ok(await _mediator.Send(new SentimentRequest { Message = message }));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SentimentRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request?.Message))
                throw new Exception("Bad Request");

            return Ok(await _mediator.Send(request));
        }
    }
}