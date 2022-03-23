using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace BingHousingMVC.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password1")]
        public string OldPassword1 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password2")]
        public string OldPassword2 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password1")]
        public string NewPassword1 { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password1")]
        [Compare("NewPassword1", ErrorMessage = "The new password1 and confirmation password1 do not match.")]
        public string ConfirmPassword1 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password2")]
        public string NewPassword2 { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password2")]
        [Compare("NewPassword2", ErrorMessage = "The new password2 and confirmation password2 do not match.")]
        public string ConfirmPassword2 { get; set; }
    }


    public class ResetPasswordModel
    {
        [Required]
        public string ResetPasswordToken { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password1")]
        public string NewPassword1 { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password1")]
        [Compare("NewPassword1", ErrorMessage = "The new password1 and confirmation password1 do not match.")]
        public string ConfirmPassword1 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password2")]
        public string NewPassword2 { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password2")]
        [Compare("NewPassword2", ErrorMessage = "The new password2 and confirmation password2 do not match.")]
        public string ConfirmPassword2 { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password1")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password2")]
        public string Password2 { get; set; }

        [Required]
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        [StringLength(20, MinimumLength = 4)]
        [Remote("IsUserNameAvailable", "UserNameValidation")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [Editable(true)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password1")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password1")]
        [Compare("Password", ErrorMessage = "The password1 and confirmation password1 do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password2")]
        public string Password2 { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password2")]
        [Compare("Password2", ErrorMessage = "The password2 and confirmation password2 do not match.")]
        public string ConfirmPassword2 { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.+-]+\.(?:[a-zA-Z]{2}|COM|com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|edu|academy|biz|college|education|int)$", ErrorMessage = "Invalid Email Address")]
        [StringLength(50, ErrorMessage = "The Maximum Length must be 50 characters long.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

   
}
