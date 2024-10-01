using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Net.Sockets;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Sherad.Domain.Entitys;

namespace Hospital.Application.Common
{

    public class RabbitMqJwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly IBasicProperties _props;
        private readonly IRequestClient<ValidateTokenRequest> _requestClient;


        public RabbitMqJwtAuthenticationHandler(
                     IOptionsMonitor<AuthenticationSchemeOptions> options,
                     ILoggerFactory logger,
                     UrlEncoder encoder,
                     ISystemClock clock,
                     IRequestClient<ValidateTokenRequest> requestClient)
                     : base(options, logger, encoder, clock)
        {
            _requestClient = requestClient;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Не найден или не коректный заголовок авторизации.");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var response = await _requestClient.GetResponse<TokenValidationResponse>(new ValidateTokenRequest { Token = token });

            if (!response.Message.IsSuccess)
            {
                return AuthenticateResult.Fail("Неволидный токен.");
            }

            JsonDocument doc = JsonDocument.Parse(response.Message.Result.ToString());
            JsonElement root = doc.RootElement;

            List<string> roles = root.EnumerateArray()
                .Select(element => element.GetString())
                .ToList();

            var claims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();


            var identity = new ClaimsIdentity(claims, nameof(RabbitMqJwtAuthenticationHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
