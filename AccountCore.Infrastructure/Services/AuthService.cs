using AccountCore.Application.Common.Errors;
using AccountCore.Application.Common.Results;
using AccountCore.Application.Contracts.Authentication;
using AccountCore.Application.Interfaces;
using AccountCore.Domain.Entities;
using AccountCore.Infrastructure.Helpers;
using Hangfire;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AccountCore.Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> usermanager,
    ILogger<AuthService> logger,
    IHttpContextAccessor httpContextAccessor,
     IEmailSender emailSender) : IAuthService
{
    private readonly UserManager<ApplicationUser> _usermanager = usermanager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IHttpContextAccessor _httpcontextaccessor = httpContextAccessor;
    private readonly IEmailSender _emailsender = emailSender;

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _usermanager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();

        var result = await _usermanager.CreateAsync(user, request.Password);


        if (result.Succeeded)
        {
            // Generate Verification Code
            var code = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code: {code}", code);

            // send email
            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _httpcontextaccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
            }
        );

        BackgroundJob.Enqueue(() => _emailsender.SendEmailAsync(user.Email!, "✅  Voice Pulse: Email Confirmation", emailBody));
    }
}
