using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyTasty.Infrastructure.Jwt
{
    public class JwtOptions
    {
        public string AccessSecret { get; set; }
        public string RefreshSecret { get; set; }
        public int ExpiresIn { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
