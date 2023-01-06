using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Hangfire.Storage;

namespace Lopes.Jobs.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HangfireController : ControllerBase
    {
        [HttpGet]
        [Route("ObterStatusJob")]
        public string ObterStatusJob(int idJob)
        {
            IStorageConnection connection = JobStorage.Current.GetConnection();
            JobData jobData = connection.GetJobData(idJob.ToString());
            string stateName = jobData.State;

            return stateName;
        }
    }
}