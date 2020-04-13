using AutoMapper;
using IdentityModel.Client;
using LojaAuth.Api.Models;
using LojaAuth.DTO;
using LojaAuth.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LojaAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        // GET api/cliente/{id}
        [HttpGet("{id}")]
        public ActionResult<ClienteDTO> Get(int id)
        {
            var cliente = _clienteService.ProcurarPorId(id);

            if (cliente != null)
            {
                var retorno = _mapper.Map<ClienteDTO>(cliente);

                return Ok(retorno);
            }
            else
                return NotFound();
        }

        // POST api/cliente
        // binding argumento
        [HttpPost]
        public ActionResult<ClienteDTO> Post([FromBody]ClienteDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var endereco = new Endereco()
            {
                Logradouro = value.Endereco.Logradouro,
                Numero = value.Endereco.Numero,
                Complemento = value.Endereco.Complemento,
                Bairro = value.Endereco.Bairro,
                Cidade = value.Endereco.Cidade
            };

            var cliente = new Cliente()
            {
                Nome = value.Nome,
                EnderecoDeEntrega = endereco
            };

            var retornoCliente = _clienteService.Salvar(cliente);

            var retorno = _mapper.Map<ClienteDTO>(retornoCliente);

            return Ok(retorno);
        }

        // POST api/cliente
        [HttpPut]
        public ActionResult<ClienteDTO> Put([FromBody] ClienteDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var endereco = new Endereco()
            {
                Id = value.EnderecoId,
                Logradouro = value.Endereco.Logradouro,
                Numero = value.Endereco.Numero,
                Complemento = value.Endereco.Complemento,
                Bairro = value.Endereco.Bairro,
                Cidade = value.Endereco.Cidade
            };

            var cliente = new Cliente()
            {
                Id = value.Id,
                Nome = value.Nome,
                EnderecoId = value.EnderecoId,
                EnderecoDeEntrega = endereco
            };

            var retornoCliente = _clienteService.Salvar(cliente);
            var retorno = _mapper.Map<ClienteDTO>(retornoCliente);

            return Ok(retorno);
        }


        [HttpGet("getToken")]
        public async Task<ActionResult<TokenResponse>> GetToken([FromBody]TokenDTO value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // async request - await para aguardar retorno
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            // nesta parte, temos um exemplo de requisição com o tipo "password" 
            // esta é a forma mais comum
            var tokenClient = new TokenClient(disco.TokenEndpoint, "codenation.api_client", "codenation.api_secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(value.UserName, value.Password, "codenation");

            // Se não tiver tiver um erro retornar token
            if (!tokenResponse.IsError)
            {
                return Ok(tokenResponse);
            }

            //retorna não autorizado e descrição do erro
            return Unauthorized(tokenResponse.ErrorDescription);
        }

    }
}