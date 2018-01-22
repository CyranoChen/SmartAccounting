using AutoMapper;
using Sap.SmartAccounting.Mvc.Entities;

namespace Sap.SmartAccounting.Mvc.Models
{
    public class RoleDto
    {
        public static MapperConfiguration ConfigMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Role, RoleDto>()
                .ForMember(d => d.Company, opt => opt.MapFrom(s => Entities.Company.Cache.Load(s.CompanyId).CompanyDisplay))
                .ForMember(d => d.Account, opt => opt.MapFrom(s => Entities.Account.Cache.Load(s.AccountId).AccountDisplay))
            );

            return config;
        }

        #region Members and Properties

        public int ID { get; set; }

        public string Company { get; set; }

        public string Account { get; set; }

        public bool IsIncoming { get; set; }

        #endregion
    }
}