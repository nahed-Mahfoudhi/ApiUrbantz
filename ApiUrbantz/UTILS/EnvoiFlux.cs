using ApiUrbantz.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;

namespace ApiUrbantz.UTILS
{
    public class EnvoiFlux
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Envoi de flux livraison à Mroad
        public static async Task<HttpResponseMessage> SendData(List<TournesValidesToAkanea> tournesValidesToAkanea, string intervention)
        {

            TimeSpan timeout = new TimeSpan(0, 0, 30);
            HttpResponseMessage response = null;
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://1807-tms-messagerie.akanea.com/json/");
                string accessId = "VIRTRP";
                string userId = "VIRAPI";
                string password = "Virapi1@";
                string token = null;

                Task<System.Net.Http.HttpResponseMessage> taskRetour = client.GetAsync($"Login/GetToken?accessId={accessId}&userId={userId}&password={password}");
                taskRetour.Wait();

                using (response = taskRetour.Result)
                {
                    Task<string> task2 = response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<string>(task2.Result);
                    logger.Info($"Token gotten from Akanea : {token}");
                    response.EnsureSuccessStatusCode();
                }

                try
                {
                    // pour l'exemple
                    //  object bordereaux = null;
                    List<TournesValidesToAkanea> bordereaux = tournesValidesToAkanea;

                    var ss = JsonConvert.SerializeObject(bordereaux);
                    logger.Info(string.Format("{0} => {1}", "json to Akanea", ss));
                    byte[] datas = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bordereaux));

                    using (System.Net.Http.HttpContent byteContent = new System.Net.Http.ByteArrayContent(datas))
                    {
                        byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        byteContent.Headers.ContentLength = datas.Length;
                        logger.Info($"Envoi vers Akanea : {"https://1807-tms-messagerie.akanea.com/json/"}{intervention}?token={token}");
                        taskRetour = client.PostAsync($"{intervention}?token={token}", byteContent);
                        taskRetour.Wait();

                        using (response = taskRetour.Result)
                        {
                            Task<string> task2 = response.Content.ReadAsStringAsync();
                            string message = task2.Result;

                            logger.Info($"Retour message Akanea: {message}");

                            //  response.EnsureSuccessStatusCode();

                        }
                    }




                }
                finally
                {
                    // libération jeton
                    taskRetour = client.GetAsync($"Login/ReleaseToken?token={token}");

                    using (response = taskRetour.Result)
                    {
                        logger.Info($"Liberation de Token : {taskRetour.Result}");
                        // response.EnsureSuccessStatusCode();
                    }
                }
            }
            return response;
        }

    }
}