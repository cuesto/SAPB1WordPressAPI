using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAPB1WordPressAPI.DataModel.Bases
{
    public class BaseEntity : Base
    {
        [HiddenInput(DisplayValue = false)]
        public DateTime? Created { get; set; }
        [MaxLength(50)]
        [HiddenInput(DisplayValue = false)]
        public string CreatedBy { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? Modified { get; set; }
        [HiddenInput(DisplayValue = false)]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }
}
