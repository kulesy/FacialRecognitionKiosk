using DsReceptionClassLibrary.Domain.Entities.Validation;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DsReceptionAPI.Application.Face.Services
{
    public class FaceService
    {
        private readonly IConfiguration config;

        public FaceService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<IFaceClient> GetClient()
        {
            var client = new FaceClient(
                            new ApiKeyServiceClientCredentials(config.GetValue<string>("FaceApi:Key"))) 
                                { Endpoint = config.GetValue<string>("FaceApi:Url") };
            try
            {
                PersonGroup personGroup = await client.PersonGroup.GetAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"));
            }
            catch (Exception e)
            {
                await client.PersonGroup.CreateAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"), config.GetValue<string>("FaceApi:PersonGroup:Name"));
            }
            return client;
        }

        /// <summary>
        /// This adds a person to the group there name is the primary key we hold against the client.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Person> CreatePersonAsync(string name)
        {
            var client = await GetClient();
            return await client.PersonGroupPerson.CreateAsync(personGroupId: config.GetValue<string>("FaceApi:PersonGroup:Id"), name: name);
        }

        // We can train the group after more images or added or if the 
        public async Task<bool> Train()
        {
            var client = await GetClient();
            await client.PersonGroup.TrainAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"));
            for(var i = 0; i < 5 ; i++)
            {
                await Task.Delay(1000);
                var trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(config.GetValue<string>("FaceApi:PersonGroup:Id"));

                Debug.WriteLine($"Training status: {trainingStatus.Status}.");

                if (trainingStatus.Status == TrainingStatusType.Succeeded) 
                {
                    return true;
                }
            }
            return false;
        }
    }
}
