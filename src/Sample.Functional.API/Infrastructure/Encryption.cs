namespace Sample.Functional.API.Infrastructure
{
    using System;

    public static class Encryption
    {
        private const long HASH = -2128831035;
        private const int PRIME = 16777619;

        public static int FnvHasher(int hash, int prime, params Func<object>[] propriedades) =>
            propriedades.Map(f => f().GetHashCode())
                .Fold(hash, (acumulado, propriedade) => (acumulado * prime) ^ propriedade);

        public static int FnvHasherDefault(params Func<object>[] propriedades) =>
            FnvHasher(unchecked((int)HASH), PRIME, propriedades);
    }
}
