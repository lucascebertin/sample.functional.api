namespace Sample.Functional.API.Testes.Unitarios
{
    using FluentAssertions;
    using Infrastructure;
    using Models;
    using System.Collections.Generic;
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Deve_criar_hash_das_propriedades_informadas()
        {
            const uint hashInitializer = 2166136261;
            const int primeNumber = 16777619;

            var usuario = new Usuário(1, "lucas", 28, new List<Telefone>());

            int hashTest;
            unchecked
            {
                hashTest = ((int)hashInitializer * primeNumber) ^ usuario.Id.GetHashCode();
                hashTest = (hashTest * primeNumber) ^ usuario.Nome.GetHashCode();
                hashTest = (hashTest * primeNumber) ^ usuario.Idade.GetHashCode();
            }

            var primeiroHash = Encryption.FnvHasherDefault(
                () => usuario.Id,
                () => usuario.Nome,
                () => usuario.Idade);

            var segundoHash = Encryption.FnvHasherDefault(
                () => usuario.Id,
                () => usuario.Nome,
                () => usuario.Idade);

            primeiroHash.Should().Be(segundoHash);
            primeiroHash.Should().Be(hashTest);
        }

        [Fact]
        public void Devem_gerar_o_mesmo_hash_dois_objetos_com_os_mesmos_valores_informados_via_constructor()
        {
            new Usuário(1, "teste", 20, new List<Telefone>()).GetHashCode()
              .ShouldBeEquivalentTo(new Usuário(1, "teste", 20, new List<Telefone>()).GetHashCode());
        }
    }
}
