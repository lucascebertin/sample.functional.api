namespace Sample.Functional.API.Testes.Integracao
{
    using FluentAssertions;
    using Infrastructure.Contextos;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class UsuáriosControllerTests : IDisposable
    {
        private readonly TestServer _servidorDeTestes;

        public UsuáriosControllerTests()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>()
                .UseApplicationInsights();

            _servidorDeTestes = new TestServer(builder);
        }

        [Fact]
        public async Task Deve_retornar_status_ok_quando_via_GET_requisitar_usuários_com_ao_menos_um_registro_na_tabela()
        {
            var contexto = _servidorDeTestes.Host.Services.GetService<ContextoDeUsuários>();
            var clienteHttp = _servidorDeTestes.CreateClient();
            var usuárioDeTestes = new Usuário(0, "Teste 1", 20, null);

            var entry = contexto.Usuários.Add(usuárioDeTestes);
            entry.State = EntityState.Added;
            await contexto.SaveChangesAsync();

            var requisicaoGet = new HttpRequestMessage(HttpMethod.Get, "api/Usuarios");
            requisicaoGet.Headers.Add("Accept", "application/json");

            var resposta = await clienteHttp.SendAsync(requisicaoGet);

            var jsonObtido = JsonConvert.DeserializeObject<IList<Models.DTO.Usuário>>(
                await resposta.Content.ReadAsStringAsync());

            resposta.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonObtido.Should().NotBeNullOrEmpty();
            jsonObtido.First().ShouldBeEquivalentTo(
                new Usuário(0, usuárioDeTestes.Nome, usuárioDeTestes.Idade, new List<Telefone>()));
        }

        [Fact]
        public async Task Deve_retornar_status_sem_conteúdo_quando_via_GET_requisitar_usuários_semm_registros_na_tabela()
        {
            var clienteHttp = _servidorDeTestes.CreateClient();
            var requisicaoGet = new HttpRequestMessage(HttpMethod.Get, "api/Usuarios");
            requisicaoGet.Headers.Add("Accept", "application/json");

            var resposta = await clienteHttp.SendAsync(requisicaoGet);
            var jsonObtido = JsonConvert.DeserializeObject<IList<Models.DTO.Usuário>>(
                await resposta.Content.ReadAsStringAsync());

            resposta.StatusCode.Should().Be(HttpStatusCode.NoContent);
            jsonObtido.Should().BeNull();
        }

        [Fact]
        public async Task Deve_retornar_status_created_quanto_via_post_enviar_um_json_válido_representando_um_usuário()
        {
            var client = _servidorDeTestes.CreateClient();
            var usuario = new Usuário(0, "Teste", 20, new List<Telefone>());

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/Usuarios")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(usuario),
                    Encoding.UTF8,
                    "application/json")
            };

            postRequest.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(postRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Deve_retornar_status_badrequest_quanto_via_post_enviar_um_json_inválido()
        {
            var client = _servidorDeTestes.CreateClient();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "api/Usuarios")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(null),
                    Encoding.UTF8,
                    "application/json")
            };

            postRequest.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(postRequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var mensagemDeErro = await response.Content.ReadAsStringAsync();
            mensagemDeErro.Should().Be("\"Payload inválido\"");
        }
        public void Dispose() =>
            _servidorDeTestes?.Dispose();
    }
}
