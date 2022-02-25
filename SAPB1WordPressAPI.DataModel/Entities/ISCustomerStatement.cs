using Newtonsoft.Json;
using SAPB1WordPressAPI.DataModel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAPB1WordPressAPI.DataModel.Entities
{
    public class ISCustomerStatement
    {
        [Display(Name = "Invoice Posting Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime InvoicePostingDate { get; set; }

        [Display(Name = "Inv Document Number")]
        public int InvDocumentNumber { get; set; }

        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "NumAtCard")]
        public string NumAtCard { get; set; }

        [Display(Name = "Doc Total")]
        public decimal? DocTotal { get; set; }

        [Display(Name = "Paid To Invoice")]
        public decimal? PaidToInvoice { get; set; }

        [Display(Name = "Payment Document Number")]
        public int PaymentDocumentNumber { get; set; }

        [Display(Name = "Payment Posting Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime PaymentPostingDate { get; set; }

        [Display(Name = "Remaining Payment")]
        public decimal? RemainingPayment { get; set; }
    }
}
