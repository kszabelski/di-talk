using System;
using NSubstitute;
using Procent.DependencyInjection.app;
using Xunit;

namespace Procent.DependencyInjection.Tests
{
    public class UserController_RegisterUser_Tests
    {
        readonly UsersController _controller;
        string _email;

        public UserController_RegisterUser_Tests()
        {
            var linkGenerator = Substitute.For<IActivationLinkGenerator>();
            var emailService = Substitute.For<IEmailService>();
            var usersDatabase = Substitute.For<IUsersDatabase>();

            _controller = new UsersController(linkGenerator, emailService, usersDatabase);

            _email = "email";
        }

        void execute()
        {
            _controller.RegisterUser(_email);
        }

        [Fact]
        public void throws_when_email_not_valid()
        {
            _email = "invalid_email";

            Assert.Throws<ArgumentException>(
                () => execute()
            );
        }
    }
}