using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace GildedRose.Stores.Base
{
    public interface IGridReader : IDisposable
    {
        IDbCommand Command { get; set; }

        bool IsConsumed { get; }

        new void Dispose(); //changed from void Dispose() to new void Dispose()

        IEnumerable<TReturn> Read<TReturn>(Type[] types, Func<object[], TReturn> map, string splitOn = "id", bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> func, string splitOn = "id", bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> func, string splitOn = "id", bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn = "id", bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn = "id", bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> func, string splitOn = "id", bool buffered = true);

        IEnumerable<TReturn> Read<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> func, string splitOn = "id", bool buffered = true);

        IEnumerable<object> Read(Type type, bool buffered = true);

        IEnumerable<T> Read<T>(bool buffered = true);

        IEnumerable<dynamic> Read(bool buffered = true);

        Task<IEnumerable<dynamic>> ReadAsync(bool buffered = true);

        Task<IEnumerable<object>> ReadAsync(Type type, bool buffered = true);

        Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true);

        T ReadFirst<T>();

        object ReadFirst(Type type);

        dynamic ReadFirst();

        Task<T> ReadFirstAsync<T>();

        Task<object> ReadFirstAsync(Type type);

        Task<dynamic> ReadFirstAsync();

        T ReadFirstOrDefault<T>();

        object ReadFirstOrDefault(Type type);

        dynamic ReadFirstOrDefault();

        Task<T> ReadFirstOrDefaultAsync<T>();

        Task<dynamic> ReadFirstOrDefaultAsync();

        Task<object> ReadFirstOrDefaultAsync(Type type);

        dynamic ReadSingle();

        object ReadSingle(Type type);

        T ReadSingle<T>();

        Task<T> ReadSingleAsync<T>();

        Task<dynamic> ReadSingleAsync();

        Task<object> ReadSingleAsync(Type type);

        dynamic ReadSingleOrDefault();

        object ReadSingleOrDefault(Type type);

        T ReadSingleOrDefault<T>();

        Task<object> ReadSingleOrDefaultAsync(Type type);

        Task<dynamic> ReadSingleOrDefaultAsync();

        Task<T> ReadSingleOrDefaultAsync<T>();
    }
}