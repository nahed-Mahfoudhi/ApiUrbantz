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
using  ApiUrbantz.Models;

namespace ApiUrbantz.Models
{
    [BasicAuthentication]
    [System.Web.Mvc.RequireHttps]
    public class RoundValidatedController : ApiController

    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]dynamic jsonn)
        {

            new Task((Action)(async () =>
            {
                try
                {
                    var flux = JsonConvert.DeserializeObject<RoundValid>(jsonn.ToString()); 
                    logger.Info(string.Format("{0} => {1}", "json to VIR", jsonn.ToString()));

                    var Round = await UTILS.RoudValidatedRetourTrait.RoundValidesToAkanea(flux);

                }
                catch (Exception ex)
                {

                    logger.Error(ex);

                }

            })).Start();
            return (IHttpActionResult)Ok();


        }

    }
}
