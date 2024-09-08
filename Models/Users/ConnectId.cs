using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Users;

[Table("connect_ids")]
public class ConnectId(string userTelegramTag, string userTelegramId)
{
    [Key]
    [Required]
    [Column("user_telegram_tag")]
    [StringLength(33, MinimumLength = 2,
        ErrorMessage = "Длина имени пользователя telegram должна быть от 2 до 33 символов")]
    [RegularExpression(@"@[A-Za-z0-9]+",
        ErrorMessage = "Имя пользователя telegram должно начинаться с @ и содержать только" +
                       "буквы и цифры латинского алфавита")]
    //[Remote(action: "CheckEmail", controller: "Home", ErrorMessage ="Email уже используется")]
    public string UserTelegramTag { get; set; } = userTelegramTag;

    [Required]
    [MinLength(1, ErrorMessage = "Телеграмм id не может быть пустым")]
    [MaxLength(255)]
    [Column("user_telegram_id")]
    public string UserTelegramId { get; set; } = userTelegramId;
}