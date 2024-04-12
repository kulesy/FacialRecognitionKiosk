using AutoMapper;
using DsReceptionAPI.Application.Common.BaseQueries;
using DsReceptionAPI.Application.Common.Models;
using DsReceptionAPI.Infrastructure.Persistence;
using DsReceptionClassLibrary.Domain.Entities.Clients;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Clients.Queries.GetClients
{
    public record GetClientFacesQuery : IRequest<List<ImageEncoded>>
    {
        public string Id { get; set; }
    }

    public class GetClientFacesQueryHandler : IRequestHandler<GetClientFacesQuery, List<ImageEncoded>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<Client> _userManager;
        private readonly ApplicationDbContext _db;

        public GetClientFacesQueryHandler(IMapper mapper, UserManager<Client> userManager, ApplicationDbContext db)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<ImageEncoded>> Handle(GetClientFacesQuery request, CancellationToken cancellationToken)
        {
            var client = _userManager.Users.Include(e => e.Images).Where(e => e.Id == request.Id).FirstOrDefault();
            var images = client.Images;
            List<ImageEncoded> imagesEncoded = new();
            foreach (var image in images)
            {
                ImageEncoded imgEncoded = new();
                string imageBase64Data =
                Convert.ToBase64String(image.FileStream);
                string imageDataURL =
                string.Format("data:image/jpg;base64,{0}",
                imageBase64Data);
                imgEncoded.FileName = image.FileName;
                imgEncoded.FileStream = imageDataURL;
                imgEncoded.ClientId = image.ClientId;
                imagesEncoded.Add(imgEncoded);
            }
            return imagesEncoded;
        }
    }
}
