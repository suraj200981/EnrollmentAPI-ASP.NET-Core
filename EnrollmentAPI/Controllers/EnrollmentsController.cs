using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using EnrollmentAPI.Models;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Net.Http;

namespace EnrollmentAPI
{
    public static class EnrollmentsController
    {

        //GET get all enrollments
        [FunctionName("Enrollments")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [CosmosDB("database-enrollment", "Enrollments", ConnectionStringSetting = "CosmosDB")] IEnumerable<enrollmentModel> enrollmentItems,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request to get entire list of enrollments");

            return new OkObjectResult(enrollmentItems);
        }

        //GET get all enrollments
        [FunctionName("GetEnrollment")]
        public static IQueryable<enrollmentModel> GetEnrollment(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
          [CosmosDB(ConnectionStringSetting = "CosmosDB")] DocumentClient _docmentClient,

          ILogger log

        )
        {

            string enrollmentID = req.Query["id"]; // read storeId to get driver for from querystring

            log.LogInformation($"C# HTTP trigger function processed a request to get specific enrollment with id of: {enrollmentID}");

            return _docmentClient.CreateDocumentQuery<enrollmentModel>(UriFactory.CreateDocumentCollectionUri("database-enrollment", "Enrollments"),
                new FeedOptions { MaxItemCount = 1 }).Where((i) => i.Id == enrollmentID);
        }




        //Create enrollments
        [FunctionName("CreateEnrollment")]
        public static async Task<HttpRequestMessage> CreateEnrollment(
          [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req,
          [CosmosDB(ConnectionStringSetting = "CosmosDB")]
           DocumentClient _docmentClient,
          ILogger log

        )
        {


            dynamic data = await req.Content.ReadAsAsync<enrollmentModel>(); 
            var response = await _docmentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("database-enrollment", "Enrollments"), data);

            return null;

        }

        //Delete enrollments
        [FunctionName("DeleteEnrollment")]
        public static async Task<IActionResult> DeleteEnrollmentAsync(
          [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
          [CosmosDB(ConnectionStringSetting = "CosmosDB")]
           DocumentClient _docmentClient,
          ILogger log, string Id

        )
        {

            var option = new FeedOptions { EnableCrossPartitionQuery = true };

            var response = _docmentClient.CreateDocumentQuery<enrollmentModel>(UriFactory.CreateDocumentCollectionUri("database-enrollment", "Enrollments"), option).Where((i) => i.Id == Id)
                 .AsEnumerable().FirstOrDefault();


            if (response == null)
            {
                return new NotFoundResult();
            }

           

            return new OkResult();

        }
    }
}
