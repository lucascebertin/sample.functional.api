namespace Sample.Functional.API.Extensions
{
    using LanguageExt;
    using System;
    using static LanguageExt.Prelude;

    public static class Actions 
    {
        public static Try<Unit> RetornarUnit(this Action action) =>
            Try(fun(action)().Executar());

        public static Try<Unit> RetornarUnit<T>(this Action<T> action, T dado) =>
            Try(fun(action)(dado).Executar());

        public static Try<TOut> RetornarTipado<TIn, TOut>(this Action<TIn> action, 
            TIn dado, TOut saida) =>
            Try(fun(action)(dado).Apply(_ => saida));

        public static Try<TOut> RetornarTipado<TIn, TOut>(this Action action, 
            TIn dado, TOut saida) =>
            Try(fun(action)().Apply(_ => saida));

        public static Unit Executar<TA>(this TA a) =>
            Unit.Default;
    }
}
