using System;

namespace Procent.DependencyInjection.app
{
    public class UsersController
    {
        private readonly IActivationLinkGenerator _activationLinkGenerator;
        private readonly IEmailService _emailService;
        private readonly IUsersDatabase _usersDatabase;

        public UsersController(IActivationLinkGenerator activationLinkGenerator, IEmailService emailService, IUsersDatabase usersDatabase)
        {
            _activationLinkGenerator = activationLinkGenerator;
            _emailService = emailService;
            _usersDatabase = usersDatabase;
        }

        public void RegisterUser(string email)
        {
            // check if email is valid
            if (EmailValidator.Validate(email) == false)
            {
                throw new ArgumentException("Invalid email address");
            }

            // check if email is not taken
            if (_usersDatabase.IsEmailTaken(email))
            {
                throw new InvalidOperationException("Email already taken");
            }

            // create new user
            var newUser = new User
            {
                Email = email,
                RegistrationToken = Guid.NewGuid().ToString(),
            };

            // insert user
            _usersDatabase.InsertUser(newUser);

            // generate activation link
            string registrationLink = _activationLinkGenerator.GenerateLink(newUser.RegistrationToken, newUser.Email);

            _emailService.RegistrationEmail(newUser.Email, registrationLink);
        }
    }
}