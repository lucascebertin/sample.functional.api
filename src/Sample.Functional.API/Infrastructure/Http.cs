namespace Sample.Functional.API.Infrastructure
{
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public static class Http
    {
        public static IActionResult Ok<T>(T dado) =>
            new OkObjectResult(dado);

        public static IActionResult SemConteudo() =>
            new NoContentResult();

        public static IActionResult Erro() =>
            new StatusCodeResult(500);

        public static IActionResult ErroEMotivo(Exception ex) =>
            new ObjectResult(ex)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

        public static IActionResult ErroEMotivo(string erro) =>
            new ObjectResult(erro)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

        public static IActionResult ErroDeValidaçãoEMotivo(string erro) =>
            new ObjectResult(erro)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

        public static IActionResult Criado<T>(string local, T dado) =>
            new CreatedResult(local, dado);

        public static IActionResult ObtidoComSucesso<T>(IList<T> enumerado) =>
            enumerado.Any()
                ? Ok(enumerado)
                : SemConteudo();

        public static IActionResult ObtidoComSucesso<T>(Option<IList<T>> enumeradoOpcional) =>
            enumeradoOpcional.Match(
                ObtidoComSucesso,
                SemConteudo);

        public static Func<T, IActionResult> CriadoComSucesso<T>(string local) =>
            x => Criado(local, x);

        public static IActionResult ProcessarComoGet<T>(
            this Option<IEnumerable<T>> optional) =>
            optional.Match(
                Ok,
                Erro);

        public static IActionResult ProcessarComoGet<T>(
            this Try<Option<IList<T>>> enumeradoOpcional) =>
            ProcessarRequisicao(
                enumeradoOpcional,
                ObtidoComSucesso,
                ErroEMotivo);

        public static IActionResult ProcessarComoPost<T>(
            this Try<T> opcional,
            string local) =>
            opcional.Match(
                CriadoComSucesso<T>(local),
                ErroEMotivo);

        public static IActionResult ProcessarComoPost<T>(
            this Option<T> opcional,
            string local) =>
            opcional.Match(
                dado => Criado(local, dado),
                SemConteudo);

        public static IActionResult ProcessarComoPost<T>(
            this Either<string, Try<T>> opcional,
            string local) =>
            opcional.Match(
                x => x.ProcessarComoPost(local),
                ErroDeValidaçãoEMotivo);

        public static IActionResult ProcessarRequisicao<T>(
            Try<Option<T>> tentativaDeObterDadoOpcional,
            Func<Option<T>, IActionResult> sucesso,
            Func<Exception, IActionResult> falha) =>
                tentativaDeObterDadoOpcional
                    .Match(sucesso, falha);

        public static IActionResult ProcessarRequisicao<T>(
            Either<string, Try<T>> tentativaDeObterDadoOpcional,
            Func<Try<T>, IActionResult> sucesso,
            Func<string, IActionResult> falha) =>
            tentativaDeObterDadoOpcional
                .Match(sucesso, falha);

        public static IActionResult ProcessarRequisicaoPost<T>(
            Try<Option<T>> tentativaDeObterDadoOpcional,
            string local) =>
                ProcessarRequisicao(
                    tentativaDeObterDadoOpcional,
                    x => Criado(local, x),
                    ErroEMotivo);

        public static IActionResult ProcessarRequisicaoPost<T>(
            Either<string, Try<T>> tentativaDeObterDadoOpcional,
            string local) =>
            ProcessarRequisicao(
                tentativaDeObterDadoOpcional,
                x => Criado(local, x),
                ErroEMotivo);

        public static IActionResult ProcessarRequisicaoGet<T>(
            Try<Option<IList<T>>> tentativaDeObterListaOpcional) =>
            ProcessarRequisicao(
                tentativaDeObterListaOpcional,
                ObtidoComSucesso,
                ErroEMotivo);
    }
}
