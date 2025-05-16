using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageDto dto)
        {
            // Use the correct conversation ID
            var conversation = await _unitOfWork.ConversationRepository.GetOrCreateAsync(
                dto.SenderId == "f66f0073-5b5c-4e98-b2b8-29d9b1374c76" ? dto.ReceiverId : dto.SenderId);

            if (conversation == null)
            {
                return BadRequest("Failed to create or find conversation.");
            }

            var message = new Message
            {
                ConversationId = conversation.Id,
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.MessageRepository.AddMessageAsync(message);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }



        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversation(int conversationId)
        {
            var messages = await _unitOfWork.MessageRepository.GetMessagesByConversationIdAsync(conversationId);
            return Ok(messages);
        }

        [HttpGet("unreadcount/{userId}")]
        public async Task<IActionResult> GetUnreadCount(string userId)
        {
            var count = await _unitOfWork.MessageRepository.GetUnreadCountAsync(userId);
            return Ok(count);
        }

        [HttpGet("admin/conversations")]
        public async Task<IActionResult> GetAdminConversations()
        {
            var conversations = await _unitOfWork.ConversationRepository.GetAdminConversationsAsync();

            var result = new List<ConversationDto>();

            foreach (var c in conversations)
            {
                // Fetch the user name from your user database
                var fullName = await _unitOfWork.UserRepository.GetUserFullNameAsync(c.CustomerId);

                result.Add(new ConversationDto
                {
                    ConversationId = c.Id,
                    UserId = c.CustomerId,
                    UserName = fullName ?? c.CustomerId, // Use FullName if available
                    LastMessage = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Content,
                    LastTimestamp = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Timestamp,
                    UnreadCount = c.Messages.Count(m => m.ReceiverId == c.AdminId && !m.IsRead)
                });
            }

            return Ok(result);
        }

        [HttpGet("user/{customerId}")]
        public async Task<IActionResult> GetCustomerConversations(string customerId)
        {
            var conversations = await _unitOfWork.ConversationRepository.GetCustomerConversationsAsync(customerId);

            var result = new List<ConversationDto>();

            foreach (var c in conversations)
            {
                // Fetch the user name from your user database
                var fullName = await _unitOfWork.UserRepository.GetUserFullNameAsync(c.CustomerId);

                result.Add(new ConversationDto
                {
                    ConversationId = c.Id,
                    UserId = c.CustomerId,
                    UserName = fullName ?? c.CustomerId, // Use FullName if available
                    LastMessage = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Content,
                    LastTimestamp = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Timestamp,
                    UnreadCount = c.Messages.Count(m => m.ReceiverId == customerId && !m.IsRead)
                });
            }

            return Ok(result);
        }

        [HttpGet("getorcreate")]
        public async Task<IActionResult> GetOrCreateConversation([FromQuery] string customerId, [FromQuery] string adminId)
        {
            var conversation = await _unitOfWork.ConversationRepository.GetOrCreateAsync(customerId, adminId);

            if (conversation == null)
                return NotFound();

            var lastMsg = conversation.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault();

            var convoDto = new ConversationDto
            {
                ConversationId = conversation.Id,
                UserId = conversation.CustomerId,
                UserName = conversation.CustomerId,  // Replace with actual customer name if available
                LastMessage = lastMsg?.Content,
                LastTimestamp = lastMsg?.Timestamp,
                UnreadCount = conversation.Messages.Count(m => m.ReceiverId == customerId && !m.IsRead)
            };

            return Ok(convoDto);
        }

    }


}


