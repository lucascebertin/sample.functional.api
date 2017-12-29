namespace Sample.Functional.API.Models
{
    using Infrastructure;

    public class Telefone
    {
        public Telefone()
        {
            
        }

        public Telefone(TipoDeTelefone tipo, short ddd, int numero, int idUsuário)
        {
            Tipo = tipo;
            DDD = ddd;
            Numero = numero;
            IdUsuario = idUsuário;
        }

        public Telefone(int id, TipoDeTelefone tipo, short ddd, int numero, int idUsuário)
        {
            Tipo = tipo;
            DDD = ddd;
            Numero = numero;
            Id = id;
            IdUsuario = idUsuário;
        }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
        // Hack para entity framework funcionar
        public int Id { get; set; }
        public int? IdUsuario { get; set; }
        public Usuário Usuário { get; set; }
        public TipoDeTelefone Tipo { get; set; }
        public short DDD { get; set; }
        public int Numero { get; set; }

        public override int GetHashCode() =>
            Encryption.FnvHasherDefault(
                () => Id,
                () => IdUsuario,
                () => DDD,
                () => Numero,
                () => Tipo);

        public override bool Equals(object obj) =>
            !(obj is null)
                && obj is Telefone tel
                && Equals(Id, tel.Id)
                && Equals(DDD, tel.DDD)
                && Equals(IdUsuario, tel.IdUsuario)
                && Equals(Numero, tel.Numero)
                && Equals(Tipo, tel.Tipo);

        public override string ToString() =>
            $"{Id}: [{Tipo}] ({DDD}) {Numero}";
    }
}
