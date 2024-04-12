using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;
using System.Threading;

namespace DsReceptionAPI.Application.Token.Commands.CreateToken
{
    public record FormCreateTokenCommand : IRequest<IActionResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class FormCreateTokenCommandHandler : IRequestHandler<FormCreateTokenCommand, IActionResult>
    {
        private readonly UserManager<Client> _userManager;
        private readonly ApplicationDbContext _context;

        public FormCreateTokenCommandHandler(UserManager<Client> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Handle(FormCreateTokenCommand request, CancellationToken cancellationToken)
        {
            if (await IsValidUsernameandPasswordAsync(request.Username, request.Password))
            {
                return new ObjectResult(await GenerateTokenAsync(request.Username));
            }
            else
            {
                throw new("$Invalid Username or Password, please ensure you input the correct details.$");
            }
        }

        private async Task<bool> IsValidUsernameandPasswordAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> GenerateTokenAsync(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                 new JwtHeader(
                     new SigningCredentials(
                         new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretSoDoNotTell")),
                         SecurityAlgorithms.HmacSha256)),
                 new JwtPayload(claims));
            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };
            return output;
        }

    }
}
