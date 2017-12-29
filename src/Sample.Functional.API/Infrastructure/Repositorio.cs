namespace Sample.Functional.API.Infrastructure
{
    using Extensions;
    using LanguageExt;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using static LanguageExt.Prelude;

    public class Repositório<T> where T : class
    {
        private readonly DbContext _contexto;

        public Repositório(DbContext contexto) =>
            _contexto = contexto;

        public Try<T> Adicionar(T objeto) =>
            Try((_contexto.Add(objeto), _contexto.SaveChanges())
                .Apply(_ => objeto));

        public Unit AdicionarAoDbSet(Try<DbSet<T>> dbSet, IEnumerable<T> objetos) =>
            dbSet.Match(
                x  => fun(() => x.AddRange(objetos))(), 
                ex => Unit.Default);

        public Unit CriarDbSetEAdicionar(DbContext contexto, IEnumerable<T> objetos) =>
            fun(() => AdicionarAoDbSet(CriarDbSet(contexto), objetos))();

        public Try<DbSet<T>> CriarDbSet(DbContext contexto) =>
            Try(contexto.Set<T>());

        public Try<Unit> Adicionar(IEnumerable<T> objetos) =>
            objetos != null 
            ? Try((CriarDbSetEAdicionar(_contexto, objetos),
                  _contexto.SaveChanges()
                ).Executar())
            : Try(Unit.Default);

        public Try<Option<IList<T>>> Obter() =>
            Try(() => Some(_contexto.Set<T>().ParaIList()));

        public Try<IQueryable<T>> PrepararQuery(DbSet<T> dbSet,
            params Expression<Func<T, object>>[] propriedades) =>
            Try(propriedades.Map(x => x).Fold(
                dbSet.AsQueryable(), (acc, next) => acc.Include(next)));

        public Try<IQueryable<T>> PrepararQuery(DbSet<T> dbSet,
            Expression<Func<T,bool>> condição,
            params Expression<Func<T, object>>[] propriedades) =>
            Try(propriedades.Map(x => x).Fold(
                dbSet.AsQueryable(), (acc, next) => acc.Include(next))
                .Where(condição));

        public Try<Option<IList<T>>> TentarObter(
            params Expression<Func<T, object>>[] propriedadesDeNavegação) =>
            from dbSet in CriarDbSet(_contexto)
            from query in PrepararQuery(dbSet, propriedadesDeNavegação)
            from resultado in Try(query.ToList())
            select resultado.Count > 0 
                ? Some(resultado.ParaIList()) 
                : None;

        public Try<Option<IList<T>>> TentarObter(
            Expression<Func<T,bool>> condição, 
            params Expression<Func<T, object>>[] propriedadesDeNavegação) =>
            from dbSet in CriarDbSet(_contexto)
            from query in PrepararQuery(dbSet, condição, propriedadesDeNavegação)
            from resultado in Try(query.ToList())
            select resultado.Count > 0
                ? Some(resultado.ParaIList())
                : None;
    }
}