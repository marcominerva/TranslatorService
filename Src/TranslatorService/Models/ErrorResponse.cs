using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorService.Models
{
    internal class ErrorResponse
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }
    }
}
