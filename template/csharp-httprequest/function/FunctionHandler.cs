using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace function
{
    public class FunctionHandler
    {
        public async Task<(int, string)> Handle(HttpRequest request)
        {
            var reader = new StreamReader(request.Body);
            var input = await reader.ReadToEndAsync();

            return (200, $"Hello! Your input was {input}");
        }
    }
}