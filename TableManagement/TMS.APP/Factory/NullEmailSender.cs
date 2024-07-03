using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TMS.APP.Factory
{
    public class NullEmailSender : IEmailSender<IdentityUser>
    {
        public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailAsync(IdentityUser user, string subject, string htmlMessage)
        {
            // Do nothing
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
