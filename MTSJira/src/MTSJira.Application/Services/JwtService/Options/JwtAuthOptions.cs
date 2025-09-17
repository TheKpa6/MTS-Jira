using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MTSJira.Application.Services.JwtService.Options
{
    public class JwtAuthOptions
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int Lifetime { get; set; }
    }
}
