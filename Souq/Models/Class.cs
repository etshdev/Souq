using AutoMapper;
using Souq.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Souq.Models
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Product,ViewsModels.ProductViewModel>();
            CreateMap<ViewsModels.ProductViewModel, Product>();

           // CreateMap<ViewsModels.ProductViewModel, Product>().ForMember(x => x.quantity, x => x.MapFrom(x => x.quantity));
        }
    }
}
