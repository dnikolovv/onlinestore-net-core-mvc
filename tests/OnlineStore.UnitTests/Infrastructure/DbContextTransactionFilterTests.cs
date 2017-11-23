namespace OnlineStore.UnitTests.Infrastructure
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Mvc.Filters;
    using OnlineStore.Data;
    using OnlineStore.Infrastructure;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class DbContextTransactionFilterTests
    {
        [Fact]
        public async Task CommitsTransaction()
        {
            // Arrange
            var db = A.Fake<ApplicationDbContext>(opts => opts.WithArgumentsForConstructor(() =>
                new ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext>())));
            
            A.CallTo(() => db.BeginTransaction()).DoesNothing();
            A.CallTo(() => db.CommitTransactionAsync()).Returns(Task.FromResult(0));

            var filter = new DbContextTransactionFilter(db);

            var filterContext = ActionExecutingContextProvider.GetActionExecutingContext("POST");

            var actionExecutionDelegate = A.Fake<ActionExecutionDelegate>();

            A.CallTo(() => actionExecutionDelegate.Invoke()).Returns(Task.FromResult<ActionExecutedContext>(null));

            // Act
            await filter.OnActionExecutionAsync(filterContext, actionExecutionDelegate);

            // Assert
            A.CallTo(() => db.BeginTransaction()).MustHaveHappened();
            A.CallTo(() => db.CommitTransactionAsync()).MustHaveHappened();
        }

        [Fact]
        public async Task RollbacksTransactionOnError()
        {
            // Arrange
            var db = A.Fake<ApplicationDbContext>(opts => opts.WithArgumentsForConstructor(() =>
                new ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext>())));

            A.CallTo(() => db.BeginTransaction()).DoesNothing();

            var filter = new DbContextTransactionFilter(db);

            var filterContext = ActionExecutingContextProvider.GetActionExecutingContext("POST");
            var actionExecutionDelegate = A.Fake<ActionExecutionDelegate>();

            A.CallTo(() => actionExecutionDelegate.Invoke()).Throws<InvalidOperationException>();

            // Act
            try { await filter.OnActionExecutionAsync(filterContext, actionExecutionDelegate); } catch { }

            // Assert
            A.CallTo(() => db.BeginTransaction()).MustHaveHappened();
            A.CallTo(() => db.CommitTransactionAsync()).MustNotHaveHappened();
            A.CallTo(() => db.RollbackTransaction()).MustHaveHappened();
        }
    }
}
