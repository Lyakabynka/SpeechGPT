﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application
{
    public enum ErrorCode
    {
        Unknown,

        UserAlreadyExists,
        UserNotFound,
        UserPasswordIncorrect,

        ChatNotFound,
    }
}
