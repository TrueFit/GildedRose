using GildedRose.Store.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GildedRose.Store.Wrappers
{
    public class DapperGridReader : IGridReader
    {
        private GridReader reader;

        public DapperGridReader(GridReader reader)
        {
            this.reader = reader;
        }

        public IDbCommand Command { get => this.reader.Command; set => this.reader.Command = value; }

        public bool IsConsumed
        {
            get
            {
                return this.reader.IsConsumed;
            }
        }

        public void Dispose()
        {
            this.reader.Dispose();
        }

        public IEnumerable<TReturn> Read<TReturn>(Type[] types, Func<object[], TReturn> map, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read<TReturn>(types, map, splitOn, buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> func, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read(func, splitOn, buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> func, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read(func, splitOn, buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read(func, splitOn, buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read(func, splitOn, buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> func, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read(func, splitOn, buffered);
        }

        public IEnumerable<TReturn> Read<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> func, string splitOn = "id", bool buffered = true)
        {
            return this.reader.Read(func, splitOn, buffered);
        }

        public IEnumerable<object> Read(Type type, bool buffered = true)
        {
            return this.reader.Read(type, buffered);
        }

        public IEnumerable<T> Read<T>(bool buffered = true)
        {
            return this.reader.Read<T>(buffered);
        }

        public IEnumerable<dynamic> Read(bool buffered = true)
        {
            return this.reader.Read(buffered);
        }

        public Task<IEnumerable<dynamic>> ReadAsync(bool buffered = true)
        {
            return this.reader.ReadAsync(buffered);
        }

        public Task<IEnumerable<object>> ReadAsync(Type type, bool buffered = true)
        {
            return this.reader.ReadAsync(type, buffered);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(bool buffered = true)
        {
            return this.reader.ReadAsync<T>(buffered);
        }

        public T ReadFirst<T>()
        {
            return this.reader.ReadFirst<T>();
        }

        public object ReadFirst(Type type)
        {
            return this.reader.ReadFirst(type);
        }

        public dynamic ReadFirst()
        {
            return this.ReadFirst();
        }

        public Task<T> ReadFirstAsync<T>()
        {
            return this.reader.ReadFirstAsync<T>();
        }

        public Task<object> ReadFirstAsync(Type type)
        {
            return this.reader.ReadFirstAsync(type);
        }

        public Task<dynamic> ReadFirstAsync()
        {
            return this.reader.ReadFirstAsync();
        }

        public T ReadFirstOrDefault<T>()
        {
            return this.reader.ReadFirstOrDefault<T>();
        }

        public object ReadFirstOrDefault(Type type)
        {
            return this.reader.ReadFirstOrDefault(type);
        }

        public dynamic ReadFirstOrDefault()
        {
            return this.reader.ReadFirstOrDefault();
        }

        public Task<T> ReadFirstOrDefaultAsync<T>()
        {
            return this.reader.ReadFirstOrDefaultAsync<T>();
        }

        public Task<dynamic> ReadFirstOrDefaultAsync()
        {
            return this.reader.ReadFirstOrDefaultAsync();
        }

        public Task<object> ReadFirstOrDefaultAsync(Type type)
        {
            return this.reader.ReadFirstOrDefaultAsync(type);
        }

        public dynamic ReadSingle()
        {
            return this.reader.ReadSingle();
        }

        public object ReadSingle(Type type)
        {
            return this.reader.ReadSingle(type);
        }

        public T ReadSingle<T>()
        {
            return this.reader.ReadSingle<T>();
        }

        public Task<T> ReadSingleAsync<T>()
        {
            return this.reader.ReadSingleAsync<T>();
        }

        public Task<dynamic> ReadSingleAsync()
        {
            return this.reader.ReadSingleAsync();
        }

        public Task<object> ReadSingleAsync(Type type)
        {
            return this.reader.ReadSingleAsync(type);
        }

        public dynamic ReadSingleOrDefault()
        {
            return this.reader.ReadSingleOrDefault();
        }

        public object ReadSingleOrDefault(Type type)
        {
            return this.reader.ReadSingleOrDefault(type);
        }

        public T ReadSingleOrDefault<T>()
        {
            return this.reader.ReadSingleOrDefault<T>();
        }

        public Task<object> ReadSingleOrDefaultAsync(Type type)
        {
            return this.reader.ReadSingleOrDefaultAsync(type);
        }

        public Task<dynamic> ReadSingleOrDefaultAsync()
        {
            return this.reader.ReadSingleOrDefaultAsync();
        }

        public Task<T> ReadSingleOrDefaultAsync<T>()
        {
            return this.reader.ReadSingleOrDefaultAsync<T>();
        }
    }
}
