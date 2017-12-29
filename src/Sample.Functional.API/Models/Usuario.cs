namespace Sample.Functional.API.Models
{
    using Infrastructure;
    using LanguageExt;
    using System.Collections.Generic;
    using System.Linq;

    public class Usuário
    {
        // ReSharper disable once UnusedMember.Local
        // Hack para entity framework funcionar
        private Usuário() { }
        public Usuário(int id, string nome, short idade)
        {
            Id = id;
            Nome = nome;
            Idade = idade;
        }

        public Usuário(int id, string nome, short idade, ICollection<Telefone> telefones)
        {
            Id = id;
            Nome = nome;
            Idade = idade;
            Telefones = telefones;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        // Hack para entity framework funcionar
        public int Id { get; protected set; }
        public string Nome { get; protected set; }
        public short Idade { get; protected set; }
        public virtual ICollection<Telefone> Telefones { get; protected set; }

        public override int GetHashCode() =>
            Encryption.FnvHasherDefault(
                () => Id,
                () => Nome,
                () => Idade) +
                (Telefones.Any()
                    ? Telefones.Sum(x => x.GetHashCode())
                    : 0);

        public static DTO.Usuário ConverterParaDTO(Usuário objeto) =>
            new DTO.Usuário(
                objeto.Nome,
                objeto.Idade,
                objeto.Telefones
                    .Select(x => new DTO.Telefone(x.DDD, x.Numero, x.Tipo))
                    .ToList());

        public static IList<DTO.Usuário> ConverterParaDTO(IList<Usuário> objetos) =>
            objetos.Select(ConverterParaDTO).ToList();

        public static Option<IList<DTO.Usuário>> ConverterParaDTO(Option<IList<Usuário>> usuários) =>
            usuários.Map(ConverterParaDTO).ToOption();
    }
}
