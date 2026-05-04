using NUnit.Framework;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Wasla_Backend.Tests.Helpers;
using Wasla_Backend.Services.Implementation;
using Wasla_Backend.Models;
using Wasla_Backend.DTOs.Authentication;
using Wasla_Backend.Exceptions;
using MockFactory = Wasla_Backend.Tests.Helpers.MockFactory;

namespace Wasla_Backend.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private MockFactory _mocks;
        private UserService _sut;

        [SetUp]
        public void SetUp()
        {
            _mocks = new MockFactory();

            _sut = new UserService(
                _mocks.UserFactory.Object,
                _mocks.UserRepo.Object,
                _mocks.RoleRepo.Object,
                _mocks.EmailSender.Object,
                _mocks.CreateRealMapper(),
                _mocks.TokenHelper.Object,
                _mocks.UserManager.Object,
                _mocks.RefreshTokenRepo.Object,
                _mocks.HttpContextAccessor.Object,
                _mocks.DateTimeHelper.Object,
                _mocks.CacheManager.Object,
                _mocks.FileService.Object,
                _mocks.FileUrlBuilder.Object
            );
        }

        #region RegisterAsync

        [Test]
        public async Task Register_EmailAlreadyExists_ThrowsBadRequestException()
        {
            var dto = TestDataBuilder.BuildRegisterDto();
            var existingUser = TestDataBuilder.BuildUser(email: dto.Email);

            _mocks.UserRepo.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync(existingUser);

            var act = async () => await _sut.RegisterAsync(dto);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Test]
        public async Task Register_RoleNotFound_ThrowsNotFoundException()
        {
            var dto = TestDataBuilder.BuildRegisterDto();

            _mocks.UserRepo.Setup(r => r.GetUserByEmailAsync(dto.Email)).ReturnsAsync((ApplicationUser?)null);
            _mocks.RoleRepo.Setup(r => r.GetRoleByIdAsync(dto.roleId)).ReturnsAsync((ApplicationRole?)null);

            var act = async () => await _sut.RegisterAsync(dto);

            await act.Should().ThrowAsync<NotFoundException>();
        }

      

        #endregion
    }
}