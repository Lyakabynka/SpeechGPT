using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using SpeechGPT.Application.Interfaces;
using SpeechGPT.Application.Options;
using SpeechGPT.Domain;

namespace SpeechGPT.Persistence.Services
{
    public class ChatGPT : IChatGPT
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
        
        public async Task<string> GetResponse(string requestBody)
        {
            var chatMessages = new List<ChatMessage>()
            {
                
            };
            var chatCompletionCreateRequest = new ChatCompletionCreateRequest()
            {
                
            };
            var completionResult = await _gpt3_5.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>()
                {
                    ChatMessage.FromSystem("You are a helpful assistant who provides user with precise," +
                    " brief and complete answers on questions."),
                    ChatMessage.FromUser(requestBody)
                },
                Temperature = 0.1f,
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
    }
}
