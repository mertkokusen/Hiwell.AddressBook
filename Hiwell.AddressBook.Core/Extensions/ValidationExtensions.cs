using System;
using System.Net.Mail;
using FluentValidation;

namespace Hiwell.AddressBook.Core.Extensions
{
    static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must((value) => ValidateEmail(value)).WithMessage("Invalid email address.");
        }

        //TODO: We may change this with regex
        public static bool ValidateEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
