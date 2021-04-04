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
        // GET: api/FluxLivVsUrbantz
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/FluxLivVsUrbantz/5
        public string Get(int id)
        {
            return "value";
        }


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

                    logger.Error(ex.Message);
                    logger.Error("====================================================================================");

                }

            })).Start();
            return (IHttpActionResult)Ok();


        }

        // PUT: api/FluxLivVsUrbantz/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FluxLivVsUrbantz/5
        public void Delete(int id)
        {
        }
    }
}
