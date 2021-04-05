using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using ApiUrbantz.Models;
using ApiUrbantz.UTILS;
using Newtonsoft.Json;
using NLog;

namespace ApiUrbantz.UTILS
{
	public static class RetourLivraisonTrait
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task<HttpResponseMessage> TaskStatusChangedToAkanea(TaskChangedModel TaskChangedModel)
        {
            //TODO faire le traitement ici le mapping ....

            List<LivTermToAkanea> FluxLivTerm = new List<LivTermToAkanea>();
            ListLivTermToAkanea ListLivTermToAk = new ListLivTermToAkanea();

         
                #region calcul de code Evenement
                var CodEvmt = "";
            if ((TaskChangedModel.progress == "COMPLETED") && (TaskChangedModel.status == "DELIVERED"))
            {
                CodEvmt = "IE78";
            }
         

          if ((TaskChangedModel.progress == "COMPLETED") && (TaskChangedModel.status == "DELIVERED") && (TaskChangedModel.metadata != null))
            {
                if (TaskChangedModel.metadata.Motif_livraison_effectuee_avec_probleme == "Produits non démontés/désinstallés")
                {
                    CodEvmt = "IE90";
                }
                else if (TaskChangedModel.metadata.Motif_livraison_effectuee_avec_probleme == "Montage/Installation non effectuée")
                {
                    CodEvmt = "IE85";
                }
                else if (TaskChangedModel.metadata.Motif_livraison_effectuee_avec_probleme == "Colis manquant")
                {
                    CodEvmt = "IE83";
                }
                else if (TaskChangedModel.metadata.Motif_livraison_effectuee_avec_probleme == "Colis cassé")
                {
                    CodEvmt = "IE84";
                }
            }

            if ((TaskChangedModel.progress == "COMPLETED") && (TaskChangedModel.status == "NOT_DELIVERED") && (TaskChangedModel.execution != null)
                && (TaskChangedModel.execution.failedReason != null))
            {
                if (TaskChangedModel.execution.failedReason.reason == "Client absent")
                {
                    CodEvmt = "IE77";
                }


                else if (TaskChangedModel.execution.failedReason.reason == "Ne passe pas")
                {
                    CodEvmt = "IE79";
                }

                else if (TaskChangedModel.execution.failedReason.reason == "Refusé partie manquante")
                {
                    CodEvmt = "IE82";
                }
                else if (TaskChangedModel.execution.failedReason.reason == "Refusé colis cassé")
                {
                    CodEvmt = "IE81";
                }
                else if (TaskChangedModel.execution.failedReason.reason == "Refusé non conforme")
                {
                    CodEvmt = "IE280";
                }
                else if (TaskChangedModel.execution.failedReason.reason == "Point relais en surcharge")
                {
                    CodEvmt = "IE99";
                }

            }
            if ((TaskChangedModel.progress == "COMPLETED") && (TaskChangedModel.status == "PICKUD_UP"))
            {
                CodEvmt = "IE89";
            }

            #endregion
            #region Remplir le modèle flux livraison de Mroad à Urbantz

            for (int i = 0; i<1; i++)
            {

                FluxLivTerm.Add(new LivTermToAkanea());
            }

            for (int i = 0; i < FluxLivTerm.Count; i++)
            {

                FluxLivTerm[i].Id = TaskChangedModel.taskReference;
                FluxLivTerm[i].CodeEvenement = CodEvmt;
                if (TaskChangedModel.execution.failedReason.picture!=null&& TaskChangedModel.execution.failedReason!= null)
                {
                    FluxLivTerm[i].Url = string.Concat(TaskChangedModel.imagePath, TaskChangedModel.execution.failedReason.picture);
                }
                else if( TaskChangedModel.metadata !=null && TaskChangedModel.metadata.Photos_livraison!=null)
                    {
                        FluxLivTerm[i].Url = string.Concat(TaskChangedModel.imagePath, TaskChangedModel.metadata.Photos_livraison[0]);
                    }
                else if (TaskChangedModel.execution!=null && TaskChangedModel.execution.successPicture!=null )
                {
                    FluxLivTerm[i].Url = string.Concat(TaskChangedModel.imagePath, TaskChangedModel.execution.successPicture);
                }

                if (TaskChangedModel.execution!=null && TaskChangedModel.execution.failedReason!=null && TaskChangedModel.execution.failedReason.reason!= null)
                {
                    FluxLivTerm[i].Motif = TaskChangedModel.execution.failedReason.reason;
                }
                else if (TaskChangedModel.metadata != null && TaskChangedModel.metadata.Motif_livraison_effectuee_avec_probleme != null)
                {
                    FluxLivTerm[i].Motif = TaskChangedModel.metadata.Motif_livraison_effectuee_avec_probleme;
                }

                FluxLivTerm[i].Note = null;
                //en dur
                FluxLivTerm[i].DatePriseEnCharge = "";
                FluxLivTerm[i].DateDebut = string.Concat(TaskChangedModel.actualTime.arrive.when.ToLocalTime().ToString("yyyy-MM-dd"), "T", (TaskChangedModel.actualTime.arrive.when.ToLocalTime()).ToString("HH:mm:00"));
                FluxLivTerm[i].DateArrivee = "";
                FluxLivTerm[i].DateFin = string.Concat(TaskChangedModel.closureDate.ToLocalTime().ToString("yyyy-MM-dd"), "T", (TaskChangedModel.closureDate.ToLocalTime()).ToString("HH:mm:00"));

                //en dur
                FluxLivTerm[i].DateDebutRDV = string.Concat((DateTime.Parse(TaskChangedModel.timeWindow.start).ToLocalTime()).ToString("yyyy-MM-dd"), "T", (DateTime.Parse(TaskChangedModel.timeWindow.start)).ToLocalTime().ToString("HH:mm:00"));
                FluxLivTerm[i].DateFinRDV = string.Concat((DateTime.Parse(TaskChangedModel.timeWindow.stop).ToLocalTime()).ToString("yyyy-MM-dd"), "T", (DateTime.Parse(TaskChangedModel.timeWindow.stop).ToLocalTime()).ToString("HH:mm:00"));
                FluxLivTerm[i].Signature = null;
                FluxLivTerm[i].Photos = null;
                FluxLivTerm[i].BL = null;
                if (TaskChangedModel.execution!=null && TaskChangedModel.execution.successComment!=null)
                {
                    FluxLivTerm[i].Commentaire = TaskChangedModel.execution.successComment;
                }
             
                
            }

            #endregion
            //***
            ListLivTermToAk.LivTermToAkaneaList = FluxLivTerm;
            //***
            var json = new JavaScriptSerializer().Serialize(ListLivTermToAk.LivTermToAkaneaList);
            logger.Info(string.Format("{0} => {1}", "json to Mroad", json));
            var passflux = SendDataToAkanea(ListLivTermToAk.LivTermToAkaneaList, "WebApi/RetourLivraison");

            await System.Threading.Tasks.Task.Delay(500);
            return await await System.Threading.Tasks.Task.FromResult(passflux);
        }
        public static async Task<HttpResponseMessage> TaskChangedToAkanea(List<TaskChangedModel> TaskChangedModel)
        {
            //TODO faire le traitement ici le mapping ....

           
            List<LivTermToAkanea> FluxLivTerm = new List<LivTermToAkanea>();
            ListLivTermToAkanea ListLivTermToAk = new ListLivTermToAkanea();

            #region Remplir le modèle flux livraison de Mroad à Urbantz
            for (int i = 0; i < TaskChangedModel.Count; i++)
            {

                FluxLivTerm.Add(new LivTermToAkanea());
            }

            for (int i = 0; i < FluxLivTerm.Count; i++)
            {
                FluxLivTerm[i].Id = TaskChangedModel[i].taskReference;
                FluxLivTerm[i].CodeEvenement = "INF";
                FluxLivTerm[i].Note = TaskChangedModel[i].metadata.Note.ToString();
                FluxLivTerm[i].Commentaire = TaskChangedModel[i].metadata.Commentaire_note;
                FluxLivTerm[i].BL = null;
                FluxLivTerm[i].Signature = null;
                FluxLivTerm[i].Photos = null;
                FluxLivTerm[i].DateDebut= string.Concat(TaskChangedModel[i].actualTime.arrive.when.ToLocalTime().ToString("yyyy-MM-dd"), "T", (TaskChangedModel[i].actualTime.arrive.when.ToLocalTime()).ToString("HH:mm:00"));
            }
            #endregion
            ListLivTermToAk.LivTermToAkaneaList = FluxLivTerm;
            //***
            var json = new JavaScriptSerializer().Serialize(ListLivTermToAk.LivTermToAkaneaList);
            logger.Info(string.Format("{0} => {1}", "json to Mroad", json));
            var passflux = SendDataToAkanea(ListLivTermToAk.LivTermToAkaneaList, "WebApi/RetourLivraison");
          
            await System.Threading.Tasks.Task.Delay(500);
            return await await System.Threading.Tasks.Task.FromResult(passflux);
        }

        //Envoie de flux retour livraison à Mroad

        public static async Task<HttpResponseMessage> SendDataToAkanea(List<LivTermToAkanea> FluxLivTerm, string intervention)
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
                    
                    List<LivTermToAkanea> FluxLiv = FluxLivTerm;

                    var ss = JsonConvert.SerializeObject(FluxLiv);
                    logger.Info(string.Format("{0} => {1}", "json to Akanea", ss));
                    byte[] datas = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(FluxLiv));

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