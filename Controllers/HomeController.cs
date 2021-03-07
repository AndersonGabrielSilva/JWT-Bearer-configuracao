using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shop.Models;
using Shop.Services;
using Shop.Repositories;

namespace Shop.Controllers
{
    [Route("api/account")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";

            var retorno = new
            {
                user = user,
                token = token
            };

            return await Task.FromResult(retorno);
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous()=> "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated()=> String.Format("Autenticado - {0}", User.Identity.Name);
        /*Com User Identity ele olha para dentro do token e verifica o nome do usuario,
        Configuramos insto lá ao gerar o token*/


        [HttpGet]
        [Route("funcionario")]
        [Authorize(Roles = "gerente, funcionario")]
        public string Funcionario()=> "Funcionário";

        [HttpGet]
        [Route("gerente")]
        [Authorize(Roles = "gerente")]
        public string Gerente()=> "Gerente";
                   
    }
}
