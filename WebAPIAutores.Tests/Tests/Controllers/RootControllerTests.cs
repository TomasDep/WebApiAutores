using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Moq;
using WebAPIAutores.Core.Controllers.V1;
using WebAPIAutores.Tests.Mocks;

namespace WebAPIAutores.Tests.UnitTests
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task RootController_IsAdmin_GetFourLines()
        {
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Resultado = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();
            var result = await rootController.Get();
            Assert.AreEqual(4, result.Value.Count());
        }

        [TestMethod]
        public async Task RootController_NotAdmin_GetTwoLines()
        {
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.Resultado = AuthorizationResult.Failed();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();
            var result = await rootController.Get();
            Assert.AreEqual(2, result.Value.Count());
        }

        [TestMethod]
        public async Task RootController_NotAdmin_GetTwoLines_UsingMoq()
        {
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(m => m.Link(
                It.IsAny<string>(),
                It.IsAny<object>()
            )).Returns(string.Empty);

            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = mockUrlHelper.Object;

            var result = await rootController.Get();
            Assert.AreEqual(2, result.Value.Count());
        }
    }
}