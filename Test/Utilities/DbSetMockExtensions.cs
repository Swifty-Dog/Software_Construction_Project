// using Microsoft.EntityFrameworkCore;
// using Moq;
// using System.Collections.Generic;
// using System.Linq;

// namespace Tests.Utilities
// {
//     public static class DbSetMockExtensions
//     {
//         public static Mock<DbSet<T>> ReturnsDbSet<T>(this Mock<DbSet<T>> mockDbSet, IList<T> sourceList) where T : class
//         {
//             var queryable = sourceList.AsQueryable();
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
//             mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
//             mockDbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
//             mockDbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(c => sourceList.Remove(c));
//             return mockDbSet;
//         }
//     }
// }


using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Moq.Language.Flow;

namespace Tests.Utilities
{
    public static class DbSetMockExtensions
    {
        public static Mock<DbSet<T>> ReturnsDbSet<T>(this Mock<DbSet<T>> dbSetMock, IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            dbSetMock.As<IAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestAsyncEnumerator<T>(queryableData.GetEnumerator()));

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryableData.Provider));

            dbSetMock.Setup(m => m.FindAsync(It.IsAny<object[]>())) // Mocks FindAsync
                .ReturnsAsync((object[] ids) => queryableData.FirstOrDefault(d => (int)d.GetType().GetProperty("Id").GetValue(d, null) == (int)ids[0]));

            return dbSetMock;
        }
    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression) => _inner.CreateQuery(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new TestAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression) => _inner.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var resultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                .GetMethod(
                    name: nameof(IQueryProvider.Execute),
                    genericParameterCount: 1,
                    types: new[] { typeof(Expression) })
                ?.MakeGenericMethod(resultType)
                .Invoke(_inner, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                ?.MakeGenericMethod(resultType)
                .Invoke(null, new[] { executionResult });
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public TestAsyncEnumerable(Expression expression) : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return default;
        }

        public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());

        public T Current => _inner.Current;
    }
}
