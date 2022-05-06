using System.ComponentModel.DataAnnotations;

namespace SpBot.Core
{
    public sealed class BotConfiguration
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Prefix { get; set; }
    }
}
