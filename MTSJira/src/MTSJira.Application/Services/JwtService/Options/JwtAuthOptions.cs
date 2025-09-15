using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MTSJira.Application.Services.JwtService.Options
{
    public class JwtAuthOptions
    {
        const string KEY = "mysupersecret_secretkey$98745_5B5455B5DD4E23CF165C7F8BA0BB1CD1F2F5D1BAF5BCD8C67B728BF2182BF341";   // ключ для шифрования

        /// <summary>
        /// Издатель токена
        /// </summary>
        public const string ISSUER = "$urvey_Service";
        /// <summary>
        /// Потребитель токена
        /// </summary>
        public const string AUDIENCE = "$ur@Service";
        /// <summary>
        /// Tocken lifetime
        /// </summary>
        public const int LIFETIME = 300; // 
        /// <summary>
        /// Returns key
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }

        /// <summary>
        /// Returns default token validation parameters
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters GetSystemTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                // укзывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = ISSUER,
                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = AUDIENCE,
                // будет ли валидироваться время существования
                ValidateLifetime = true,
                // установка ключа безопасности
                IssuerSigningKey = GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true,

                RequireExpirationTime = true,

                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
