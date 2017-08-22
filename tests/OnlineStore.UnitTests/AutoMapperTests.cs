namespace OnlineStore.UnitTests
{
    using AutoMapper;
    using FakeItEasy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;
    using Xunit;

    public class AutoMapperTests
    {
        [Fact]
        public void HasValidConfiguration()
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            Mapper.AssertConfigurationIsValid();
        }
    }
}
