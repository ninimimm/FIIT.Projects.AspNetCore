using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Users;

[Table("admins")]
public class Admin(string adminTelegramTag, string adminLink)
{
    [Key]
    [Required]
    [Column("admin_telegram_tag")]
    [StringLength(33, MinimumLength = 2,
        ErrorMessage = "Длина имени пользователя telegram должна быть от 2 до 33 символов")]
    [RegularExpression(@"@[A-Za-z0-9]+",
        ErrorMessage = "Имя пользователя telegram должно начинаться с @ и содержать только" +
            "буквы и цифры латинского алфавита")]
    //[Remote(action: "CheckEmail", controller: "Home", ErrorMessage ="Email уже используется")]
    public string AdminTelegramTag { get; set; } = adminTelegramTag;

    [Required]
    [Column("admin_link")]
    [MaxLength(255)]
    [Url(ErrorMessage = "Некорректная ссылка")]
    public string AdminLink { get; set; } = adminLink;
}