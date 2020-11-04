using System.ComponentModel.DataAnnotations;

namespace SpBot.Core
{
    //Этот код принадлежит Алексею Уланову a.k.a. Ultra_Rabbit'у
    public sealed class BotConfiguration
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Prefix { get; set; }
    }
}
