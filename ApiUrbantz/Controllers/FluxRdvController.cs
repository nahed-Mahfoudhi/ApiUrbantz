using ApiUrbantz.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiUrbantz.Controllers
{
    public class FluxRdvController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: api/FluxRdv
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/FluxRdv/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FluxRdv
        [HttpPost]

        public async Task<IHttpActionResult> Post([FromBody]dynamic JsonArray)
        {
            try
            {

                var FluRdv = JsonConvert.DeserializeObject<List<ElementEnvoiSms>>(JsonArray.Root.ToString());

                logger.Error("====================================================================================");
                logger.Info(string.Format("{0} => {1}", "json mrod", JsonArray.First.ToString()));

                var FluxRdvEmission = await UTILS.FluxRdvTrait.FluxRdvVersNotico(FluRdv);
                logger.Error("====================================================================================");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error("====================================================================================");

            }
       
            return (IHttpActionResult)Ok();


        }
        // PUT: api/FluxRdv/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FluxRdv/5
        public void Delete(int id)
        {
        }
    }
}
