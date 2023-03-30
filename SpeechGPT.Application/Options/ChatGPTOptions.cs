using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application.Options
{
    public class ChatGPTOptions
    {
        public static readonly string ChatGPTSection = "ChatGPT";

        public string ApiKey { get; set; }
    }
}
