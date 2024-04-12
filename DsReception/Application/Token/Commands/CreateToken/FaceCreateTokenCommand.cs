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
    public record FaceCreateTokenCommand : IRequest<IActionResult>
    {
        public Client User { get; set; }
    }



    public class FaceCreateTokenCommandHandler : IRequestHandler<FaceCreateTokenCommand, IActionResult>
    {
        private readonly UserManager<Client> _userManager;
        private readonly ApplicationDbContext _context;

        public FaceCreateTokenCommandHandler(UserManager<Client> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Handle(FaceCreateTokenCommand request, CancellationToken cancellationToken)
        {
            var client = await _userManager.FindByIdAsync(request.User.Id);
            if (client is not null)
            {
                return new ObjectResult(await GenerateTokenAsync(request.User.Email));
            }
            else
            {
                throw new("$The selected face is not registered.$");
            }
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
