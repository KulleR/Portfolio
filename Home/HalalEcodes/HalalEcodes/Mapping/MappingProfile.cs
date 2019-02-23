using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HalalEcodes.Controllers.Showcases;
using HalalEcodes.Core.Extensions;
using HalalEcodes.Data;

namespace HalalEcodes.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ecode, EcodeShowcase>()
                .ForMember(destinationMember => destinationMember.Category,
                    expression => expression.MapFrom(m => m.Category.Desc))
            .ForMember(destinationMember => destinationMember.StatusName,
                expression => expression.MapFrom(m => m.Status.GetDescription()));
        }
    }
}
