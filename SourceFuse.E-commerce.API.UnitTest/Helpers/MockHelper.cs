using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SourceFuse.E_commerce.API.Mappers;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SourceFuse.E_commerce.API.UnitTest.Helpers
{
    public class MockHelper
    {
        public static Mock<IGenericPersist<T>> GetGenericPersistMock<T>() where T : class
        {
            Mock<IGenericPersist<T>> genericPersistMock = new Mock<IGenericPersist<T>>();
            genericPersistMock
                .Setup(e => e.Add(It.IsAny<T>()))
                .Returns(true);
            genericPersistMock
                .Setup(e => e.SaveChangesAsync())
                .ReturnsAsync(true);
            genericPersistMock
                .Setup(e => e.Delete(It.IsAny<T>()))
                .Returns(true);

            return genericPersistMock;
        }

        public static IMapper GetMapper()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            return mapper;
        }

        public static Mock<IHttpContextAccessor> GetHttpContextAccessorMock()
        {
            var ipAddress = "192.168.1.123";
            var accessorMcok = new Mock<IHttpContextAccessor>();
            accessorMcok
                .Setup(a => a.HttpContext.Connection.RemoteIpAddress)
                .Returns(IPAddress.Parse(ipAddress));

            return accessorMcok;
        }

        public static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            ApplicationUser user = new ApplicationUser();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            userManagerMock
                .Setup(userManager => userManager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMock
                .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()));
            userManagerMock
                .Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            return userManagerMock;
        }

        public static Mock<T> GetPersistMock<T>() where T : class
        {
            Mock<T> persistMock = new Mock<T>();

            return persistMock;
        }
    }
}
