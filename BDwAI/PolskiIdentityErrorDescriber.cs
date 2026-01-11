using Microsoft.AspNetCore.Identity;

namespace BDwAI
{
    public class PolskiIdentityErrorDescriber : IdentityErrorDescriber
    {
        // Hasło za krótkie
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Hasło musi mieć co najmniej {length} znaków."
            };
        }

        // Brak znaku specjalnego (!@#)
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Hasło musi zawierać co najmniej jeden znak specjalny (np. ! @ # $ %)."
            };
        }

        // Brak dużej litery
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Hasło musi zawierać co najmniej jedną wielką literę ('A'-'Z')."
            };
        }

        // Brak cyfry
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Hasło musi zawierać co najmniej jedną cyfrę ('0'-'9')."
            };
        }

        // Zduplikowany email
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"Adres email '{email}' jest już zajęty."
            };
        }

        // Zduplikowana nazwa użytkownika (jeśli używasz)
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"Nazwa użytkownika '{userName}' jest już zajęta."
            };
        }
    }
}