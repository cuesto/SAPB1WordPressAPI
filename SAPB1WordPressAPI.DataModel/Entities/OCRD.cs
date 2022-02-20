﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAPB1WordPressAPI.DataModel.Entities
{
    public class OCRD
    {
        [Key]
        [StringLength(15)]
        public string CardCode { get; set; }

        [StringLength(100)]
        public string CardName { get; set; }

        [JsonIgnore]
        public char CardType { get; set; }

        [JsonIgnore]
        public char frozenFor { get; set; }

        [NotMapped]
        public string DisplayAutoComplete
        {
            get
            {
                if (CardCode == null)
                    return "";
                return $"{CardCode} - {CardName}";
            }
        }
    }
}
