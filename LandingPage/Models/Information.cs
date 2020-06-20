using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LandingPage.Models
{
    public class Information
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui lòng cho chúng tôi biết tên của bạn.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng cho chúng tôi biết Email của bạn.")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng Email, ví dụ: abc@gmail.com")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Chưa đúng định dạng Email, ví dụ: abc@gmail.com")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng cho chúng tôi biết SĐT của bạn.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Chưa đúng định dạng SĐT.")]
        public string Phone { get; set; }
        public string Message { get; set; }
        public EnumStatus Status { get; set; }
        public enum EnumStatus
        {
            New = 1,
            Called = 0,
            Success = 2,
            Fail = -1
        }
        public DateTime? CreateAt { get; set; }
    }
}