using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using Sap.SmartAccounting.Mvc.Entities;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class PaymentDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Payment, PaymentDto>()
                .ForMember(d => d.Company, opt => opt.MapFrom(s => s.CompanyName))
                .ForMember(d => d.Bank, opt => opt.MapFrom(s => s.BankName))
                .ForMember(d => d.Account, opt => opt.MapFrom(s => s.AccountName))
            );

            return config;
        }

        #region Members and Properties

        public int ID { get; set; }

        [Display(Name = "No in B1")]
        public string B1Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Company Information")]
        public string Company { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Bank Information")]
        public string Bank { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Receive Date")]
        public DateTime ReceiveDate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Display(Name = "Account")]
        public string Account { get; set; }

        public PaymentStatusEnum Status { get; set; }

        #endregion

    }
}