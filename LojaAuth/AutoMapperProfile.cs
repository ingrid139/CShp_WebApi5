using AutoMapper;
using LojaAuth.Api.Models;
using LojaAuth.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaAuth
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Produto, ProdutoJSONDTO>().ReverseMap();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            CreateMap<CompraDTO, Compra>().ReverseMap();
            CreateMap<PromocaoProduto, PromocaoProdutoDTO>().ReverseMap();
            CreateMap<Promocao, PromocaoDTO>()
                .ForMember(dest => dest.PromocaoProdutos, opt => opt.MapFrom(src => src.Produtos))
                .ReverseMap();
            CreateMap<ProdutoJSONDTO, Produto>().ReverseMap();
        }
    }
}
