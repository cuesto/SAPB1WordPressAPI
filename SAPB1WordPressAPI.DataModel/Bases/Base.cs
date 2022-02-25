using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SAPB1WordPressAPI.DataModel.Enums;
using System.ComponentModel;

namespace SAPB1WordPressAPI.DataModel.Bases
{
    public class Base
    {
        [JsonIgnore]
        [DefaultValue("0")]
        [HiddenInput(DisplayValue = false)]
        public virtual IsDeleted IsDeleted { get; set; }
    }
}
