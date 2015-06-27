using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

using Moq;

namespace GeneX.Security.TestExtensions
{
	public static class ContextExtensions
	{
		public static Mock<T2> CreateMockContext<T, T2>(Mock<DbSet<T>> mockSet)
			where T : class
			where T2 : DbContext
		{
			Mock<T2> mockContext = new Mock<T2>();
			mockContext.Setup(c => c.Set<T>()).Returns(mockSet.Object);

			return mockContext;
		}

		public static Mock<T> AddSetToContext<T, T2>(Mock<T> mockContext, Mock<DbSet<T2>> mockSet)
			where T : DbContext
			where T2 : class
		{
			mockContext.Setup(c => c.Set<T2>()).Returns(mockSet.Object);
			return mockContext;
		}

		public static Mock<DbSet<T>> Mock<T>(this List<T> list, bool async) where T : class
		{
			IQueryable<T> data = list.AsQueryable();
			Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();


			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

			if (async)
			{
				mockSet.As<IDbAsyncEnumerable<T>>()
					.Setup(m => m.GetAsyncEnumerator())
					.Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));

				mockSet.As<IQueryable<T>>()
					.Setup(m => m.Provider)
					.Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
			}
			else
			{
				mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
			}

			return mockSet;
		}
	}

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    } 
}