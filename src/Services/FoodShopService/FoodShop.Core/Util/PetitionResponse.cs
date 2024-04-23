using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Core.Util
{
    public class PetitionResponse<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public string? Module { get; set; }
        public string? URL { get; set; }
        public T? Result { get; set; }

        public PetitionResponse() {}

        public PetitionResponse(bool success, string message, string module, string url, T result)
        {
            Success = success;
            Message = message;
            Module = module;
            URL = url;
            Result = result;
        }
    }
}
