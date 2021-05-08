using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MovieProject.Models
{
    public class ResetPasswordModel
    {
        [Display(Name = "Mật khẩu mới: ")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu",AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu: ")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Mật khẩu xác nhận không trùng khớp")]
        public string ComfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }
    }
}