using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechGPT.Application.Common.Mappings
{
    public interface IMappable<T>
    {
        public void Mapping(Profile profile);
    }
}
