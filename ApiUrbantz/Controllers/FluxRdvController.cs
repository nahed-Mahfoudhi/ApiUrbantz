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

        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]dynamic JsonArray)
        {
            try
            {
                var FluRdv = JsonConvert.DeserializeObject<List<ElementEnvoiSms>>(JsonArray.Root.ToString());
                var FluxRdvEmission = await UTILS.FluxRdvTrait.FluxRdvVersNotico(FluRdv);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
       
            return (IHttpActionResult)Ok();
        }

    }
}
