using ApiUrbantz.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text.RegularExpressions;
using static ApiUrbantz.UTILS.Utils;

namespace ApiUrbantz.UTILS
{
    public static class EmissionLivraisonTrait
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task<HttpResponseMessage> GetTaskByBorderau(FluxAOptimiser FluxMroad)
        {
            //TODO faire le traitement 
            
            ListFluxLivUrbants ListFluxLivUrbant = new ListFluxLivUrbants();
            List<FluxLivUrbantz> FluxVsUrbantz = new List<FluxLivUrbantz>();
            foreach (var item in FluxMroad.TrajetList)
            {
                FluxVsUrbantz.Add(new FluxLivUrbantz());
            }
            ListFluxLivUrbant.ListFluxLivUrbantz = FluxVsUrbantz;
            
 
            for (int i = 0; i < FluxVsUrbantz.Count; i++)
            {
                
                FluxVsUrbantz[i].taskId = FluxMroad.TrajetList[i].CommandeEntete.NumeroRecepisse;
                FluxVsUrbantz[i].hubName = string.Concat("VIR", FluxMroad.Agence.CodeNumerique);
                FluxVsUrbantz[i].type = GetDeliveryTypeByTraficCode(FluxMroad.TrajetList[i].CommandeEntete.Trafic.Code);
                FluxVsUrbantz[i].taskReference = FluxMroad.TrajetList[i].CommandeEntete.IdCommandeEntete.ToString();
                //Nom du Do
                FluxVsUrbantz[i].client = FluxMroad.TrajetList[i].CommandeEntete.Payeur.Nom;
                FluxVsUrbantz[i].contact.email = FluxMroad.TrajetList[i].CommandeEntete.MailDestinataire;
                FluxVsUrbantz[i].contact.language = FluxMroad.TrajetList[i].LocaliteArrivee.CodePays;
                FluxVsUrbantz[i].contact.phone = GetPhoneNumberByCodePays(FluxMroad.TrajetList[i].LocaliteArrivee.CodePays, FluxMroad.TrajetList[i].CommandeEntete);
                FluxVsUrbantz[i].contact.name = FluxMroad.TrajetList[i].CommandeEntete.NomDestinataire;
                FluxVsUrbantz[i].contact.person = FluxMroad.TrajetList[i].NomArrivee;
                FluxVsUrbantz[i].address.street = FluxMroad.TrajetList[i].Adresse1Arrivee;
                //FluxVsUrbantz[i].address.street = "";
                FluxVsUrbantz[i].address.country = FluxMroad.TrajetList[i].LocaliteArrivee.CodePays;
                FluxVsUrbantz[i].address.city = FluxMroad.TrajetList[i].LocaliteArrivee.Nom;
                FluxVsUrbantz[i].address.zip = FluxMroad.TrajetList[i].LocaliteArrivee.CodePostal;
                if (FluxMroad.TrajetList[i].CommandeEntete.Ascenseur == false)
                {
                    FluxVsUrbantz[i].metadata.libelle_adresse = "ASC=NON";
                }
                else if (FluxMroad.TrajetList[i].CommandeEntete.Ascenseur == true)
                { FluxVsUrbantz[i].metadata.libelle_adresse = "ASC=OUI"; }

                FluxVsUrbantz[i].metadata.Tel_supplementaire = FluxMroad.TrajetList[i].CommandeEntete.TelephoneDestinataire;
                FluxVsUrbantz[i].metadata.Agence = string.Concat("VIR", FluxMroad.Agence.CodeNumerique);
                FluxVsUrbantz[i].instructions = FluxMroad.TrajetList[i].CommandeEntete.InstructionLivraison;
                FluxVsUrbantz[i].timeWindow.start = FluxMroad.TrajetList[i].CommandeEntete.THeureDebutRDV.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.000Z");
                 FluxVsUrbantz[i].timeWindow.stop = FluxMroad.TrajetList[i].CommandeEntete.THeureFinRDV.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.000Z"); 
                FluxVsUrbantz[i].date = DateTime.UtcNow;
               
                FluxVsUrbantz[i].price = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotauxMontant.ChiffreAffaire;
                FluxVsUrbantz[i].dimensions.price = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotauxMontant.ChiffreAffaire.ToString().Replace(",", ".");

               
                int tmps_poids = 0;
                tmps_poids = Utils.GetPoidsByPrestation(FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids, FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code);

              

                    //en dur pour le moment 
                FluxVsUrbantz[i].serviceTime = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalTempsMontage+tmps_poids;
                FluxVsUrbantz[i].dimensions.weight = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids.ToString().Replace(",","."); 
                FluxVsUrbantz[i].dimensions.volume = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalVolume.ToString().Replace(",", ".");
                FluxVsUrbantz[i].quantity = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalColis.ToString();

                for (int j = 0; j < FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList?.Count; j++)
                {
                    if (j == 0)
                    {
                        FluxVsUrbantz[i].items.Add(new Item
                        {
                            barcode = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CodeBarre,
                            description = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.Article.Libelle,
                            name = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.LibelleRubrique,
                            type = "COL",
                            quantity = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.NbArticle.ToString(),
                            dimensions = new Dimensions()
                            {
                                volume = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalVolume.ToString().Replace(",", "."),
                                weight = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids.ToString().Replace(",", "."),
                                price = FluxMroad.TrajetList[i].CommandeEntete.CommandeTotauxMontant.ChiffreAffaire !=null ? FluxMroad.TrajetList[i].CommandeEntete.CommandeTotauxMontant.ChiffreAffaire.ToString().Replace(",", ".") : string.Empty
                            },
                            labels = new List<string>() { FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.Prestation.Code }

                        });
                    }else
                    FluxVsUrbantz[i].items.Add(new Item
                    {
                        barcode = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CodeBarre,
                        description = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.Article.Libelle,
                        name = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.LibelleRubrique,
                        type = "COL",
                        quantity = FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.NbArticle.ToString(),
                        dimensions = new Dimensions() { volume = "",
                        weight = "",
                        price = "" 
                        },
                        labels = new List<string>() { FluxMroad.TrajetList[i].CommandeEntete.CommandeColisList[j].CommandeDetail.Prestation.Code }

                    });
                }
                
            }

           

            var json = new JavaScriptSerializer().Serialize(ListFluxLivUrbant.ListFluxLivUrbantz);
                json = json.Replace("\u0027", "");
                var passflux = SendData(ListFluxLivUrbant.ListFluxLivUrbantz);
                await System.Threading.Tasks.Task.Delay(500);
                return await await System.Threading.Tasks.Task.FromResult(passflux);
            }

        
        
    //Envoie de flux livraison à Urbantz
       public static async Task<HttpResponseMessage> SendData(List<FluxLivUrbantz> FluxVsUrbantz)
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
                    Convert.ToBase64String(Encoding.ASCII.GetBytes("P_ziTStgeCckfG7DLYbNWNg0YWaHO0z8j8:E_Ey4okEnz1WR09k0ts00wwAAQf0bmsIas"))
                );
                JavaScriptSerializer jss = new JavaScriptSerializer();

                var myContent = jss.Serialize(FluxVsUrbantz);
                logger.Info(string.Format("{0} => {1}", "json to Urbantz", myContent));
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

