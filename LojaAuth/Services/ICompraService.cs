using LojaAuth.Api.Models;
using System.Collections.Generic;

namespace LojaAuth.Services
{
    public interface ICompraService
    {
        IList<Compra> ProcurarPorClienteId(int clienteId);
        Compra ProcurarPorId(int compraId);
        IList<Compra> ProcurarPorProduto(int produtoId);
        Compra Salvar(Compra compra);
    }
}