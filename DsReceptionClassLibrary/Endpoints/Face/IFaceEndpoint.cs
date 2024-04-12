using DsReceptionClassLibrary.Domain.Entities.Clients;
using DsReceptionClassLibrary.Domain.Entities.Validation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsReceptionClassLibrary.EndPoints.Face
{
    public interface IFaceEndpoint
    {
        Task<List<IdentifyResult>> DetectAllClientFaces(List<DetectedFace> detectedFaces);
        Task<List<Client>> DetectAllClients(List<IdentifyResult> identityResults);
        Task<List<DetectedFace>> DetectAllFaces(List<IBrowserFile> files);
        List<string> DetectionMessage(List<Client> clientsInImage, int unknownCount = 0);
        Task<CQRSResponse> TrainFaces();
    }
}