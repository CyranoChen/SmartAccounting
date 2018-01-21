using System.ComponentModel.DataAnnotations;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class CompanyDto
    {
        #region Members and Properties

        public int ID { get; set; }

        [Display(Name = "No. in B1")]
        public string B1Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Company Code")]
        public string CompanyCode { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        #endregion

    }
}