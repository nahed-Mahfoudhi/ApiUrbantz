using ApiUrbantz.UTILS;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiUrbantz.Models;


namespace ApiUrbantz.Models
{
    [BasicAuthentication]
    [System.Web.Mvc.RequireHttps]
    public class TaskStatusChangedController: ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]dynamic jsonArray)
        {

            new Task((Action)(async () =>
            {
                try
                {
                    var fluxTaskStatusChanged = JsonConvert.DeserializeObject<TaskChangedModel>(jsonArray.ToString()); ;
                    logger.Info(string.Format("{0} => {1}", "Task Status changed to VIR Api", jsonArray.ToString()));
                    var TaskStChanged = await UTILS.RetourLivraisonTrait.TaskStatusChangedToAkanea(fluxTaskStatusChanged);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }

            })).Start();
            return (IHttpActionResult)Ok();


        }

    }
}
