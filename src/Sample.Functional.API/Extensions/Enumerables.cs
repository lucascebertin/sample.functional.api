namespace Sample.Functional.API.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Enumerables
    {
        public static IList<TSource> ParaIList<TSource>(this IEnumerable<TSource> source) =>
            source.ToList();

    }
}
