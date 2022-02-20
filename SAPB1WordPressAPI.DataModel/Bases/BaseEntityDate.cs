using Newtonsoft.Json;
using SAPB1WordPressAPI.DataModel.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAPB1WordPressAPI.DataModel.Bases
{
    public class BaseEntityDate : BaseEntity
    {
        [Required]
        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Fecha Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public string StartTime
        {
            get
            {
                if (StartDate == null)
                    return "";
                return StartDate.ToString("HH:mm");
            }
        }

        [NotMapped]
        public string EndTime
        {
            get
            {
                if (EndDate == null)
                    return "";
                return EndDate.ToString("HH:mm");
            }
        }
    }
}
