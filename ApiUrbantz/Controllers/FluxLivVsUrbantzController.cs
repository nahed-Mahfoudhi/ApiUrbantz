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
using static ApiUrbantz.Models.FluxAOptimiser;

namespace ApiUrbantz.Models
{
    [BasicAuthentication]
    [System.Web.Mvc.RequireHttps]
    public class FluxLivVsUrbantzController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]dynamic jsonn)
        {

            new Task((Action)(async () =>
            {
                try
                {
                    var flux = JsonConvert.DeserializeObject<FluxAOptimiser>(jsonn.First.ToString()); ;
                    logger.Info(string.Format("{0} => {1}", "json mrod", jsonn.First.ToString()));

                    var msg = await UTILS.EmissionLivraisonTrait.GetTaskByBorderau(flux);

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
