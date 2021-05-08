using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MovieProject.Models
{
    public class ChangePasswordModel
    {
        [Key]
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Hãy nhập tên tài khoản")]

        public string UserName { set; get; }

        [Display(Name = "Mật khẩu hiện tại")]
        [Required(ErrorMessage = "Hãy nhập mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        public string OldPasswword { set; get; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Hãy nhập mật khẩu mới")]
        [DataType(DataType.Password)]
        public string NewPassword { set; get; }


        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Hãy xác nhận mật khẩu")]
        //[Compare(otherProperty:"PasswordNew", ErrorMessage = "Mật khẩu xác nhận không chính xác")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { set; get; }

    }
}