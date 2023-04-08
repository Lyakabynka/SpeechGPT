using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using SpeechGPT.Application.CQRS.Queries.ViewModels;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Application.Options;
using SpeechGPT.Domain;
using SpeechGPT.Domain.Enums;

namespace SpeechGPT.Persistence.Services
{
    public class ChatGPT :  IChatGPT
    {
        private readonly ChatGPTOptions _options;
        private readonly OpenAIService _gpt3_5;
        public ChatGPT(IOptions<ChatGPTOptions> options)
        {
            _options = options.Value;

            _gpt3_5 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = _options.ApiKey,
                DefaultModelId = Models.ChatGpt3_5Turbo
            });
        }
        
        public async Task<string> Handle(string requestBody, ChatVm chatVm)
        {
            var previousMessages = GetSystemMessages();

            previousMessages.AddRange(
                chatVm.MessageVms.Select(
                            messageVm =>
                                messageVm.MessageType switch
                                {
                                    MessageType.System => ChatMessage.FromSystem(messageVm.Body),
                                    MessageType.Request => ChatMessage.FromUser(messageVm.Body),
                                    MessageType.Response => ChatMessage.FromAssistant(messageVm.Body)
                                }
            ));
            
            
            var completionResult = await _gpt3_5.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = previousMessages,
                Temperature = 0.8f,
                MaxTokens = 200
            });

            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }
            else
            {
                return "Unsuccessful operation.";
            }
        }

        private List<ChatMessage> GetSystemMessages()
        {
            return new List<ChatMessage>
            {
                ChatMessage.FromSystem("From now you are Jarvice. If user asks your name, your name is Jarvice. If user texts: QWERTY`123. Respond: I am human228!")
            };
        }
    }
}
