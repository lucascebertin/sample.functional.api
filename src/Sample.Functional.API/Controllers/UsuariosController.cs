namespace Sample.Functional.API.Controllers
{
    using Infrastructure;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Models.DTO;
    using Services;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.Net;

    [Produces("application/json")]
    [Route("api/Usuarios")]
    public class UsuariosController : Controller
    {
        private readonly ServiçosDeUsuário _serviçosDeUsuário;

        public UsuariosController(ServiçosDeUsuário serviçosDeUsuário) =>
            _serviçosDeUsuário = serviçosDeUsuário;

        [HttpGet("{id?}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Usuário), "Obtido com sucesso")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(System.Collections.Generic.IList<Usuário>), "Obtido com sucesso")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, null, "Sem conteúdo")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, typeof(Exception), "Falha de infraestrutura")]
        public IActionResult Get([FromRoute]int? id) =>
            id.Match(
                x => _serviçosDeUsuário
                           .ObterUsuárioComTelefones(x)
                ,
                () => _serviçosDeUsuário
                          .ObterUsuáriosComTelefones()
            ).ProcessarComoGet();


        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(Unit), "Inserido com sucesso")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, typeof(Exception), "Falha de infraestrutura")]
        public IActionResult Post([FromBody] Usuário usuário) =>
            _serviçosDeUsuário
                .CadastrarUsuário(usuário?.ConverterParaEntidade())
                .ProcessarComoPost(u => $"api/Usuarios/{u.Id}");
    }
}