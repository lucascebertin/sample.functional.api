namespace Sample.Functional.API.Services
{
    using Extensions;
    using Infrastructure;
    using LanguageExt;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using static LanguageExt.Prelude;

    public class ServiçosDeUsuário
    {
        private readonly Repositório<Usuário> _repositórioDeUsuário;
        private readonly Repositório<Telefone> _repositórioDeTelefones;

        public ServiçosDeUsuário(Repositório<Usuário> repositórioDeUsuário,
            Repositório<Telefone> repositórioDeTelefones) =>
            (_repositórioDeUsuário = repositórioDeUsuário,
            _repositórioDeTelefones = repositórioDeTelefones)
            .Executar();

        public Either<string, Try<Usuário>> CadastrarUsuário(Option<Usuário> usuario) => 
            ValidarUsuário(usuario).Match(
                x => Right<string, Try<Usuário>>(TentarCadastrar(x)), 
                ex => Left<string, Try<Usuário>>(ex.Reduce((acc, next) => $"{acc} {next}")));

        public Try<Usuário> TentarCadastrar(Usuário usuário) =>
            from u in _repositórioDeUsuário.Adicionar(new Usuário(usuário.Id, usuário.Nome, usuário.Idade))
            from t in _repositórioDeTelefones.Adicionar(usuário.Telefones?.Select(
                x => new Telefone(x.Tipo, x.DDD, x.Numero, u.Id)))
            select u;

        public Validation<string, Usuário> ValidarUsuário(Option<Usuário> usuario) =>
            usuario.Match(
                x  => Success<string,Usuário>(new Usuário(x.Id, x.Nome, x.Idade)),
                () => Fail<string, Usuário>("Payload inválido")); 

        public Try<Option<IList<Usuário>>> ObterUsuários() =>
            _repositórioDeUsuário.Obter();

        public Try<Option<IList<Models.DTO.Usuário>>> ObterUsuáriosComTelefones() =>
            _repositórioDeUsuário.TentarObter(p => p.Telefones)
                .Map(Usuário.ConverterParaDTO);

        public Try<Option<IList<Models.DTO.Usuário>>> ObterUsuárioComTelefones(int id) =>
            _repositórioDeUsuário.TentarObter(
                u => u.Id == id,
                p => p.Telefones
            ).Map(Usuário.ConverterParaDTO);

        public Try<Option<IList<Usuário>>> ObterUsuários(
            Expression<Func<Usuário, object>>[] propriedades) =>
            _repositórioDeUsuário.TentarObter(propriedades);
    }
}
