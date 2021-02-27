using System;
using Xunit;
using APICore.Common.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using APICore.Services.Exceptions;
using APICore.Test.Mocks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;

namespace APICore.Test
{
    public class AccountTesting
    {
        //private readonly LoginAction login;
        //private readonly LogoutAction logout;
        //private readonly RegisterAction register;
        //public AccountTesting()
        //{
        //    login = new LoginAction();
        //    logout = new LogoutAction();
        //    register = new RegisterAction();
        //}
        [Fact(DisplayName = "Successfully Register Should Return Created Status Code (201)")]
        public void SuccessfullyRegisterShouldReturnCreated()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "Carlos@itguy.com",
                FullName = "Carlos Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountController = RegisterAction.RegisterEndpoint;

            // ACT
            var taskResult = (ObjectResult)accountController.Register(fakeUserRequest).Result;

            // ASSERT
            Assert.Equal(201, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Empty Email Should Return Bad Request Exception")]
        public void EmptyEmailShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountController = RegisterAction.RegisterEndpoint;

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Email In Use Should Return Bad Request Exception")]
        public void EmailInUseShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe@itguy.com"
            };

            var accountController = RegisterAction.RegisterEndpoint;

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Empty Password Should Return Bad Request Exception")]
        public void EmptyPasswordShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountController = RegisterAction.RegisterEndpoint;

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Small Password Should Return Bad Request Exception")]
        public void SmallPasswordShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "S3cr",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountController = RegisterAction.RegisterEndpoint;

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Passwords Doesn't Match Should Return Bad Request Exception")]
        public void PasswordDoesntMatchShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeUserRequest = new SignUpRequest
            {
                Email = "pepe2@itguy.com",
                FullName = "Pepe Perez",
                Gender = 0,
                Phone = "+53 12345678",
                Birthday = DateTime.Now,
                Password = "Z3cretP@$$",
                ConfirmationPassword = "S3cretP@$$"
            };

            var accountController = RegisterAction.RegisterEndpoint;

            // ACT
            var taskResult = (BaseBadRequestException)accountController.Register(fakeUserRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Login Should Return Ok Status Code (200)")]
        public void SuccessfullyLoginShouldReturnOk()
        {
            // ARRANGE
            var fakeLoginRequest = new LoginRequest
            {
                Email = "carlos@itguy.com",
                Password = "S3cretP@$$"
            };

            var accountController = LoginAction.LoginEndpoint;

            // ACT
            var taskResult = (ObjectResult)accountController.Login(fakeLoginRequest).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }
        [Fact(DisplayName = "Empty Email On Login Should Return Not Found Exception")]
        public void EmptyEmailOnLoginShouldReturnBadRequestException()
        {
            // ARRANGE
            var fakeLoginRequest = new LoginRequest
            {
                Email = "",
                Password = "S3cretP@$$"
            };

            var accountController = LoginAction.LoginEndpoint;

            // ACT
            var taskResult = (BaseNotFoundException)accountController.Login(fakeLoginRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(404, taskResult.HttpCode);
        }
        [Fact(DisplayName = "Wrong Password Should Return Unauthorized Exception")]
        public void WrongPasswordShouldReturnUnauthorizedException()
        {
            // ARRANGE
            var fakeLoginRequest = new LoginRequest
            {
                Email = "carlos@itguy.com",
                Password = "Z3cretP@$$"
            };

            var accountController = LoginAction.LoginEndpoint;

            // ACT
            var taskResult = (BaseUnauthorizedException)accountController.Login(fakeLoginRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(401, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Logout Should Return Ok Status Code (200)")]
        public void SuccessfullyLogoutShouldReturnOk()
        {
            // ARRANGE
            var fakehttpContext = new DefaultHttpContext();

            fakehttpContext.Request.Headers.Add("Authorization", "Bearer s0m34cc$3$$T0k3n");

            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.UserData, "1")
            };

            var accountController = LogoutAction.LogoutEndpoint(fakehttpContext, fakeClaims);

            // ACT
            var taskResult = (OkResult)accountController.Logout().Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }
        [Fact(DisplayName = "Wrong User Logout Should Return Not Found Exception(404)")]
        public void WrongUserLogoutShouldReturnNotFoundException()
        {
            // ARRANGE
            var fakehttpContext = new DefaultHttpContext();

            fakehttpContext.Request.Headers.Add("Authorization", "Bearer s0m34cc$3$$T0k3n");

            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.UserData, "100")
            };

            var accountController = LogoutAction.LogoutEndpoint(fakehttpContext, fakeClaims);

            // ACT
            var taskResult = (BaseNotFoundException)accountController.GlobalLogout().Exception.InnerException;

            // ASSERT
            Assert.Equal(404, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Global Logout Should Return Ok Status Code (200)")]
        public void SuccessfullyGlobalLogoutShouldReturnOk()
        {
            // ARRANGE
            var fakehttpContext = new DefaultHttpContext();

            fakehttpContext.Request.Headers.Add("Authorization", "Bearer s0m34cc$3$$T0k3n");

            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.UserData, "1")
            };

            var accountController = LogoutAction.LogoutEndpoint(fakehttpContext, fakeClaims);

            // ACT
            var taskResult = (OkResult)accountController.GlobalLogout().Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }
        [Fact(DisplayName = "Wrong User Global Logout Should Return Not Found Exception(404)")]
        public void WrongUserGlobalLogoutShouldReturnNotFoundException()
        {
            // ARRANGE
            var fakehttpContext = new DefaultHttpContext();

            fakehttpContext.Request.Headers.Add("Authorization", "Bearer s0m34cc$3$$T0k3n");

            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.UserData, "100")
            };

            var accountController = LogoutAction.LogoutEndpoint(fakehttpContext, fakeClaims);

            // ACT
            var taskResult = (BaseNotFoundException)accountController.GlobalLogout().Exception.InnerException;

            // ASSERT
            Assert.Equal(404, taskResult.HttpCode);
        }
        [Fact(DisplayName = "Successfully Change Account Status Should Return Ok(200)")]
        public void SuccessfullyChangeAccountStatusShouldReturnOk()
        {
            // ARRANGE
            var fakehttpContext = new DefaultHttpContext();

            fakehttpContext.Request.Headers.Add("Authorization", "Bearer s0m34cc$3$$T0k3n");

            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.UserData, "1")
            };
            ChangeAccountStatusRequest fakeChangeAccountStatus = new ChangeAccountStatusRequest
            {
                Identity = "someRandomIdentityString",
                Active = true
            };

            var accountController = UserStatusAction.UserStatusEndpoint(fakehttpContext, fakeClaims);

            // ACT
            var taskResult = (OkResult)accountController.ChangeAccountStatus(fakeChangeAccountStatus).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }
        [Fact(DisplayName = "Inactive User Change Account Status Himself Should Return Forbidden Exception (403)")]
        public void InactiveUserChangeAccountStatusHimselfShouldReturnForbiddenException()
        {
            // ARRANGE
            var fakehttpContext = new DefaultHttpContext();

            fakehttpContext.Request.Headers.Add("Authorization", "Bearer s0m34cc$3$$T0k3ntwo");

            var fakeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.UserData, "2")
            };
            ChangeAccountStatusRequest fakeChangeAccountStatus = new ChangeAccountStatusRequest
            {
                Identity = "someRandomIdentityString",
                Active = true
            };

            var accountController = UserStatusAction.UserStatusEndpoint(fakehttpContext, fakeClaims);

            // ACT
            var taskResult = (BaseForbiddenException)accountController.ChangeAccountStatus(fakeChangeAccountStatus).Exception.InnerException;

            // ASSERT
            Assert.Equal(403, taskResult.HttpCode);
        }
    }
}
