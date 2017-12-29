namespace Sample.Functional.API.Models.DTO
{
    using System.Collections.Generic;
    using System.Linq;

    public class Usuário
    {
        public Usuário(string nome, short idade, IList<Telefone> telefones)
        {
            Nome = nome;
            Idade = idade;
            Telefones = telefones;
        }

        public string Nome { get; set; }
        public short Idade { get; set; }
        public IList<Telefone> Telefones { get; set; }
        public Models.Usuário ConverterParaEntidade() =>
            new Models.Usuário(
                0, 
                Nome, 
                Idade, 
                Telefones.Select(p => new Models.Telefone(
                    p.TipoDeTelefone, p.DDD, p.Número, 0)).ToList()
            );
    }
}
