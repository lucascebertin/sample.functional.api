namespace Sample.Functional.API.Models.DTO
{
    public class Telefone
    {
        public Telefone(short ddd, int número, TipoDeTelefone tipoDeTelefone)
        {
            DDD = ddd;
            Número = número;
            TipoDeTelefone = tipoDeTelefone;
        }

        public short DDD { get; set; }
        public int Número { get; set; }
        public TipoDeTelefone TipoDeTelefone { get; set; }
    }
}
