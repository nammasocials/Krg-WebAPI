using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.ViewModels
{
    public class ApiErrorResponse
    {
        public string Message { get; set; }
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
