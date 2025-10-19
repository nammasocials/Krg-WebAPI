using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.ViewModels
{
    public class VMAuthResponse
    {
        public Guid userCode {  get; set; }
        public string token { get; set; }
        public bool isAuthenticated { get; set; }
    }
    public class VMAuthReq
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
