using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SpiceHouse.Data.Models.Enums;

namespace SpiceHouse.Data.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CouponType { get; set; }

        public ECouponType ECouponType { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public decimal MinimumAmount { get; set; }

        public byte[] Picture { get; set; }

        public bool IsActive { get; set; }
    }
}
