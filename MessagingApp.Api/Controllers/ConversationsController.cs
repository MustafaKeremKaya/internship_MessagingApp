using Microsoft.AspNetCore.Mvc;
using MessagingApp.Business.Abstract;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {

        private readonly IConversationService _conversationService;

        private readonly IMessageService _messageService;

        public ConversationsController(IConversationService conversationService, IMessageService messageService)
        {
            _conversationService = conversationService;
            _messageService = messageService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateConversationRequest request)
        {
            var result = _conversationService.Create(request);

            if (!result.Success)
            {

                return BadRequest(new { success = false, message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _conversationService.GetAll();

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _conversationService.GetById(id);

            if (!result.Success)
            {

                return NotFound(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet("{id}/messages")]
        public IActionResult GetMessages([FromRoute] int id)
        {
            var result = _messageService.GetMessagesByConversationId(id);

            if (!result.Success)
            {
                return NotFound(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPost("{id}/messages")]
        public IActionResult SendMessage([FromRoute] int id, [FromBody] SendMessageRequest request)
        {
            var result = _messageService.Send(id, request);

            if (!result.Success)
            {

                if (result.Message != null && result.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { success = false, message = result.Message });
                }
                return BadRequest(new { success = false, message = result.Message });
            }

            return CreatedAtAction(nameof(GetMessages), new { id }, new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }
    }
}
