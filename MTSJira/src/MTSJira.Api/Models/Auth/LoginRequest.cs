using System.ComponentModel.DataAnnotations;

namespace MTSJira.Api.Models.Auth
{
    /// <summary>
    /// Модель запроса дял авторизации пользователя
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [Required]
        [StringLength(1, MinimumLength = 50)]
        public string Login { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        [StringLength(1, MinimumLength = 50)]
        public string Password { get; set; } = string.Empty;
    }
}
