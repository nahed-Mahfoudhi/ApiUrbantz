using ApiUrbantz.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ApiUrbantz.UTILS
{
    public class FluxRdvTrait
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Creation du flux à envoyer à Notico
        public static async Task<HttpResponseMessage> FluxRdvVersNotico(List<ElementEnvoiSms> FluxRdv)
        {
            //TODO faire le traitement ici le mapping ....
           
            ListFluxLivUrbants FluxRdvUrb = new ListFluxLivUrbants();
            List<FluxLivUrbantz> FluxRdvListUrbz = new List<FluxLivUrbantz>();

            FluxRdvUrb.ListFluxLivUrbantz = FluxRdv.Convert().ToList();


            var json = new JavaScriptSerializer().Serialize(FluxRdvUrb.ListFluxLivUrbantz);
            json = json.Replace("\u0027", "");
            var passflux = PassComplexDat(FluxRdvUrb.ListFluxLivUrbantz);
            await System.Threading.Tasks.Task.Delay(500);
            return await await System.Threading.Tasks.Task.FromResult(passflux);
        }

        public static async Task<HttpResponseMessage> PassComplexDat(List<FluxLivUrbantz> ListFluxLivUrbantz)
        {
            HttpResponseMessage response = null;
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://api.urbantz.com/");
                    client.Timeout = timeout;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(Encoding.ASCII.GetBytes("P_z73RVoEqCH4QmlbTYjYjuMuCfWxyc03x:E_VMfzcIFrA6vJYOBa19lqPvZOz42aHW0h"))
                    );
                    JavaScriptSerializer jss = new JavaScriptSerializer();

                    var myContent = jss.Serialize(ListFluxLivUrbantz);
                    logger.Info(string.Format("{0} => {1}", "flux rdv to urbantz", myContent));
                    byte[] datas = Encoding.ASCII.GetBytes(myContent);

                    using (HttpContent byteContent = new ByteArrayContent(datas))
                    {
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        byteContent.Headers.ContentLength = datas.Length;

                        Task<HttpResponseMessage> task = client.PostAsync("v2/task", byteContent);
                        task.Wait(timeout);

                        response = task.Result;
                        logger.Info(string.Format("{0} ", response));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("{0}", ex.Message));
                }

            }

            return response;
        }
    }
}
