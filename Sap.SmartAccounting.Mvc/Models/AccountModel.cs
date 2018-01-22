using System.ComponentModel.DataAnnotations;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class AccountDto
    {
        #region Members and Properties

        public int ID { get; set; }

        [Display(Name = "No. in B1")]
        public string B1Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Incoming / Outcoming")]
        public bool IsIncoming { get; set; }

        #endregion
    }
}