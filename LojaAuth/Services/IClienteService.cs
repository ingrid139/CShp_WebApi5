using LojaAuth.Api.Models;
using System.Collections.Generic;

namespace LojaAuth.Services
{
    public interface IClienteService
    {
        Cliente ProcurarPorId(int clienteId);
        IList<Cliente> ProcurarPorNome(string nome);
        Cliente Salvar(Cliente cliente);
    }
}