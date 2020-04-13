using LojaAuth.Api.Models;
using System.Collections.Generic;

namespace LojaAuth.Services
{
    public interface IPromocoesService
    {
        Promocao ProcurarPorId(int promocaoId);
        IList<Promocao> ProdutosPorPromocaoId(int promocaoId);
        Promocao ProdutosPromocoes();
        IList<Promocao> ProdutosPromocoesLista();
        Promocao Salvar(Promocao produto);
    }
}