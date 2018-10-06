// Type: System.Linq.EnumerableEx
// Assembly: System.Interactive, Version=1.0.2856.104, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// Assembly location: C:\Users\mlukic\Desktop\Dropbox\8tracksApp\8tracks Windows App\Libs\ReactiveUI\System.Interactive.dll

using System;
using System.Collections.Generic;
using System.Concurrency;

namespace System.Linq
{
    public static class EnumerableEx
    {
        public static IEnumerable<TSource> Synchronize<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<TSource> ObserveOn<TSource>(this IEnumerable<TSource> source, IScheduler scheduler);
        public static IEnumerable<TSource> StartWith<TSource>(this IEnumerable<TSource> source, TSource first);
        public static IEnumerable<TSource> StartWith<TSource>(this IEnumerable<TSource> source, params TSource[] first);
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext);

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext,
                                                       Action onCompleted);

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext,
                                                       Action<Exception> onError);

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer);

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext,
                                                       Action<Exception> onError, Action onCompleted);

        public static IEnumerable<TSource> Finally<TSource>(this IEnumerable<TSource> source, Action finallyAction);

        public static IEnumerable<IList<TSource>> BufferWithCount<TSource>(this IEnumerable<TSource> source, int count,
                                                                           int skip);

        public static IEnumerable<IList<TSource>> BufferWithCount<TSource>(this IEnumerable<TSource> source, int count);
        public static IEnumerable<TSource> Defer<TSource>(Func<IEnumerable<TSource>> enumerableFactory);
        public static IEnumerable<Notification<TSource>> Materialize<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<TSource> Dematerialize<TSource>(this IEnumerable<Notification<TSource>> source);

        public static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IEnumerable<TSource> source,
                                                                          TAccumulate seed,
                                                                          Func<TAccumulate, TSource, TAccumulate>
                                                                              accumulator);

        public static IEnumerable<TSource> Scan<TSource>(this IEnumerable<TSource> source,
                                                         Func<TSource, TSource, TSource> accumulator);

        public static IEnumerable<TAccumulate> Scan0<TSource, TAccumulate>(this IEnumerable<TSource> source,
                                                                           TAccumulate seed,
                                                                           Func<TAccumulate, TSource, TAccumulate>
                                                                               accumulator);

        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source,
                                                                               Func<TSource, TKey> keySelector,
                                                                               IEqualityComparer<TKey> comparer);

        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source,
                                                                         IEqualityComparer<TSource> comparer);

        public static IEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IEnumerable<TSource> source,
                                                                               Func<TSource, TKey> keySelector);

        public static IEnumerable<TSource> DistinctUntilChanged<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<TSource> While<TSource>(Func<bool> condition, IEnumerable<TSource> source);

        public static IEnumerable<TResult> If<TResult>(Func<bool> condition, IEnumerable<TResult> thenSource,
                                                       IEnumerable<TResult> elseSource);

        public static IEnumerable<TSource> DoWhile<TSource>(this IEnumerable<TSource> source, Func<bool> condition);

        public static IEnumerable<TResult> Case<TValue, TResult>(Func<TValue> selector,
                                                                 IDictionary<TValue, IEnumerable<TResult>> sources,
                                                                 IEnumerable<TResult> defaultSource);

        public static IEnumerable<TResult> Case<TValue, TResult>(Func<TValue> selector,
                                                                 IDictionary<TValue, IEnumerable<TResult>> sources);

        public static IEnumerable<TResult> For<TSource, TResult>(IEnumerable<TSource> source,
                                                                 Func<TSource, IEnumerable<TResult>> resultSelector);

        public static IEnumerable<TResult> Let<TValue, TResult>(TValue value,
                                                                Func<TValue, IEnumerable<TResult>> selector);

        public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count);
        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count);
        public static IEnumerable<TSource> Remotable<TSource>(this IEnumerable<TSource> source);

        public static IEnumerable<TOther> SelectMany<TSource, TOther>(this IEnumerable<TSource> source,
                                                                      IEnumerable<TOther> other);

        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<TSource> first,
                                                                      IEnumerable<TSource> second);

        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(params IEnumerable<TSource>[] sources);
        public static IEnumerable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IEnumerable<TSource>> sources);
        public static IEnumerable<TSource> Concat<TSource>(params IEnumerable<TSource>[] sources);
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<IEnumerable<TSource>> sources);
        public static IEnumerable<TSource> Catch<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second);
        public static IEnumerable<TSource> Catch<TSource>(params IEnumerable<TSource>[] sources);
        public static IEnumerable<TSource> Catch<TSource>(IEnumerable<IEnumerable<TSource>> sources);

        public static IEnumerable<TSource> Catch<TSource, TException>(this IEnumerable<TSource> source,
                                                                      Func<TException, IEnumerable<TSource>> handler)
            where TException : Exception;

        public static IEnumerable<T[]> ForkJoin<T>(IEnumerable<IEnumerable<T>> sources);
        public static IEnumerable<T[]> ForkJoin<T>(params IEnumerable<T>[] sources);
        public static IEnumerable<TSource> Return<TSource>(TSource value);
        public static IEnumerable<TSource> Throw<TSource>(Exception exception);

        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition,
                                                                     Func<TState, TState> iterate,
                                                                     Func<TState, TResult> resultSelector);

        public static IEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceSelector,
                                                                     Func<TResource, IEnumerable<TSource>> resourceUsage)
            where TResource : IDisposable;

        public static IEnumerable<TSource> Repeat<TSource>(TSource value);
        public static IEnumerable<TSource> Repeat<TSource>(TSource value, int repeatCount);
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int repeatCount);
        public static IEnumerable<TValue> Retry<TValue>(this IEnumerable<TValue> source);
        public static IEnumerable<TValue> Retry<TValue>(this IEnumerable<TValue> source, int retryCount);
        public static TSource Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer);
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
                                                   IComparer<TKey> comparer);

        public static TSource Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer);
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector);

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
                                                   IComparer<TKey> comparer);

        public static void Run<TSource>(this IEnumerable<TSource> source);
        public static void Run<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext);

        public static void Run<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext,
                                        Action<Exception> onError);

        public static void Run<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted);

        public static void Run<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext,
                                        Action<Exception> onError, Action onCompleted);

        public static void Run<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer);
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> source);

        public static IEnumerable<TResult> Let<TSource, TResult>(this IEnumerable<TSource> source,
                                                                 Func<IEnumerable<TSource>, IEnumerable<TResult>>
                                                                     function);

        public static IEnumerable<TSource> MemoizeAll<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<TSource> Memoize<TSource>(this IEnumerable<TSource> source);

        public static IEnumerable<TResult> Publish<TSource, TResult>(this IEnumerable<TSource> source,
                                                                     Func<IEnumerable<TSource>, IEnumerable<TResult>>
                                                                         function);

        public static IEnumerable<TResult> Publish<TSource, TResult>(this IEnumerable<TSource> source,
                                                                     Func<IEnumerable<TSource>, IEnumerable<TResult>>
                                                                         function, TSource initialValue);

        public static IEnumerable<TResult> Replay<TSource, TResult>(this IEnumerable<TSource> source,
                                                                    Func<IEnumerable<TSource>, IEnumerable<TResult>>
                                                                        function);

        public static IEnumerable<TResult> Replay<TSource, TResult>(this IEnumerable<TSource> source,
                                                                    Func<IEnumerable<TSource>, IEnumerable<TResult>>
                                                                        function, int bufferSize);

        public static IEnumerable<TResult> Prune<TSource, TResult>(this IEnumerable<TSource> source,
                                                                   Func<IEnumerable<TSource>, IEnumerable<TResult>>
                                                                       function);

        public static IEnumerable<TSource> Share<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<TSource> Memoize<TSource>(this IEnumerable<TSource> source, int bufferSize);

        public static IEnumerable<TAccumulate> AggregateEnumerable<TSource, TAccumulate>(
            this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator);

        public static IEnumerable<TSource> AggregateEnumerable<TSource>(this IEnumerable<TSource> source,
                                                                        Func<TSource, TSource, TSource> accumulator);

        public static IEnumerable<bool> IsEmptyEnumerable<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<bool> AnyEnumerable<TSource>(this IEnumerable<TSource> source);

        public static IEnumerable<bool> AnyEnumerable<TSource>(this IEnumerable<TSource> source,
                                                               Func<TSource, bool> predicate);

        public static IEnumerable<bool> AllEnumerable<TSource>(this IEnumerable<TSource> source,
                                                               Func<TSource, bool> predicate);

        public static IEnumerable<bool> ContainsEnumerable<TSource>(this IEnumerable<TSource> source, TSource value,
                                                                    IEqualityComparer<TSource> comparer);

        public static IEnumerable<bool> ContainsEnumerable<TSource>(this IEnumerable<TSource> source, TSource value);
        public static IEnumerable<int> CountEnumerable<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<long> LongCountEnumerable<TSource>(this IEnumerable<TSource> source);
        public static IEnumerable<double> SumEnumerable(this IEnumerable<double> source);
        public static IEnumerable<float> SumEnumerable(this IEnumerable<float> source);
        public static IEnumerable<decimal> SumEnumerable(this IEnumerable<decimal> source);
        public static IEnumerable<int> SumEnumerable(this IEnumerable<int> source);
        public static IEnumerable<long> SumEnumerable(this IEnumerable<long> source);
        public static IEnumerable<double?> SumEnumerable(this IEnumerable<double?> source);
        public static IEnumerable<float?> SumEnumerable(this IEnumerable<float?> source);
        public static IEnumerable<decimal?> SumEnumerable(this IEnumerable<decimal?> source);
        public static IEnumerable<int?> SumEnumerable(this IEnumerable<int?> source);
        public static IEnumerable<long?> SumEnumerable(this IEnumerable<long?> source);

        public static IEnumerable<IList<TSource>> MinByEnumerable<TSource, TKey>(this IEnumerable<TSource> source,
                                                                                 Func<TSource, TKey> keySelector);

        public static IEnumerable<IList<TSource>> MinByEnumerable<TSource, TKey>(this IEnumerable<TSource> source,
                                                                                 Func<TSource, TKey> keySelector,
                                                                                 IComparer<TKey> comparer);

        public static IEnumerable<TSource> MinEnumerable<TSource>(this IEnumerable<TSource> source);

        public static IEnumerable<TSource> MinEnumerable<TSource>(this IEnumerable<TSource> source,
                                                                  IComparer<TSource> comparer);

        public static IEnumerable<double> MinEnumerable(this IEnumerable<double> source);
        public static IEnumerable<float> MinEnumerable(this IEnumerable<float> source);
        public static IEnumerable<decimal> MinEnumerable(this IEnumerable<decimal> source);
        public static IEnumerable<int> MinEnumerable(this IEnumerable<int> source);
        public static IEnumerable<long> MinEnumerable(this IEnumerable<long> source);
        public static IEnumerable<double?> MinEnumerable(this IEnumerable<double?> source);
        public static IEnumerable<float?> MinEnumerable(this IEnumerable<float?> source);
        public static IEnumerable<decimal?> MinEnumerable(this IEnumerable<decimal?> source);
        public static IEnumerable<int?> MinEnumerable(this IEnumerable<int?> source);
        public static IEnumerable<long?> MinEnumerable(this IEnumerable<long?> source);

        public static IEnumerable<IList<TSource>> MaxByEnumerable<TSource, TKey>(this IEnumerable<TSource> source,
                                                                                 Func<TSource, TKey> keySelector);

        public static IEnumerable<IList<TSource>> MaxByEnumerable<TSource, TKey>(this IEnumerable<TSource> source,
                                                                                 Func<TSource, TKey> keySelector,
                                                                                 IComparer<TKey> comparer);

        public static IEnumerable<TSource> MaxEnumerable<TSource>(this IEnumerable<TSource> source);

        public static IEnumerable<TSource> MaxEnumerable<TSource>(this IEnumerable<TSource> source,
                                                                  IComparer<TSource> comparer);

        public static IEnumerable<double> MaxEnumerable(this IEnumerable<double> source);
        public static IEnumerable<float> MaxEnumerable(this IEnumerable<float> source);
        public static IEnumerable<decimal> MaxEnumerable(this IEnumerable<decimal> source);
        public static IEnumerable<int> MaxEnumerable(this IEnumerable<int> source);
        public static IEnumerable<long> MaxEnumerable(this IEnumerable<long> source);
        public static IEnumerable<double?> MaxEnumerable(this IEnumerable<double?> source);
        public static IEnumerable<float?> MaxEnumerable(this IEnumerable<float?> source);
        public static IEnumerable<decimal?> MaxEnumerable(this IEnumerable<decimal?> source);
        public static IEnumerable<int?> MaxEnumerable(this IEnumerable<int?> source);
        public static IEnumerable<long?> MaxEnumerable(this IEnumerable<long?> source);
        public static IEnumerable<double> AverageEnumerable(this IEnumerable<double> source);
        public static IEnumerable<float> AverageEnumerable(this IEnumerable<float> source);
        public static IEnumerable<decimal> AverageEnumerable(this IEnumerable<decimal> source);
        public static IEnumerable<double> AverageEnumerable(this IEnumerable<int> source);
        public static IEnumerable<double> AverageEnumerable(this IEnumerable<long> source);
        public static IEnumerable<double?> AverageEnumerable(this IEnumerable<double?> source);
        public static IEnumerable<float?> AverageEnumerable(this IEnumerable<float?> source);
        public static IEnumerable<decimal?> AverageEnumerable(this IEnumerable<decimal?> source);
        public static IEnumerable<double?> AverageEnumerable(this IEnumerable<int?> source);
        public static IEnumerable<double?> AverageEnumerable(this IEnumerable<long?> source);
    }
}
