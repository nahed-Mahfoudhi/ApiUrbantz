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
            #region creation de l'objet Interventions contenant le meme nombre d'intervention que le trajet list de l'objet Borderau
            ListFluxLivUrbants FluxRdvUrb = new ListFluxLivUrbants();
            List<FluxLivUrbantz> FluxRdvListUrbz = new List<FluxLivUrbantz>();
            foreach (var item in FluxRdv)
            {
                FluxRdvListUrbz.Add(new FluxLivUrbantz());
            }
            FluxRdvUrb.ListFluxLivUrbantz = FluxRdvListUrbz;
            #endregion

            for (int i = 0; i < FluxRdvListUrbz.Count; i++)
            {
                //Traitement du numero de téléphone
                var telSms = "";
                if ((FluxRdv[i].Commande.PortableDestinataire != null) && ((FluxRdv[i].Commande.PortableDestinataire.Substring(0, 2) == "06") || (FluxRdv[i].Commande.PortableDestinataire.Substring(0, 2) == "07")))

                    telSms = FluxRdv[i].Commande.PortableDestinataire;

                else if ((FluxRdv[i].Commande.PortableDestinataire != null) && ((FluxRdv[i].Commande.PortableDestinataire.Substring(0, 4) == "+337") || (FluxRdv[i].Commande.PortableDestinataire.Substring(0, 4) == "+336")))

                    telSms = FluxRdv[i].Commande.PortableDestinataire;

                else if ((FluxRdv[i].Commande.PortableDestinataire != null) && (FluxRdv[i].Commande.PortableDestinataire.Substring(0, 4) == "0044"))

                    telSms = FluxRdv[i].Commande.PortableDestinataire;

                else if ((FluxRdv[i].Commande.TelephoneDestinataire != null) && ((FluxRdv[i].Commande.TelephoneDestinataire.Substring(0, 2) == "06") || (FluxRdv[i].Commande.TelephoneDestinataire.Substring(0, 2) == "07")))
                    telSms = FluxRdv[i].Commande.TelephoneDestinataire;

                else if ((FluxRdv[i].Commande.TelephoneDestinataire != null) && ((FluxRdv[i].Commande.TelephoneDestinataire.Substring(0, 4) == "+337") || (FluxRdv[i].Commande.TelephoneDestinataire.Substring(0, 4) == "+336")))
                    telSms = FluxRdv[i].Commande.TelephoneDestinataire;

                else if ((FluxRdv[i].Commande.TelephoneDestinataire != null) && (FluxRdv[i].Commande.TelephoneDestinataire.Substring(0, 4) == "0044"))

                    telSms = FluxRdv[i].Commande.TelephoneDestinataire;

                else if ((FluxRdv[i].Commande.PortableDestinataire == null) && (FluxRdv[i].Commande.TelephoneDestinataire == null))
                    telSms = null;

                var TelDest = "";
                if ((telSms != null) && ((telSms.Substring(0, 2) == "06") || (telSms.Substring(0, 2) == "07")))
                {

                    TelDest = string.Concat("+33", telSms.Substring(1, telSms.Length - 1));
                }
                else if ((telSms != null) && (telSms.Substring(0, 3) == "+33"))
                {

                    TelDest = telSms;
                }

                else if ((telSms != null) && (telSms.Substring(0, 4) == "0044"))
                {

                    TelDest = string.Concat("+", telSms.Substring(2, telSms.Length - 2));
                }
                // Traitement du nom du donneur d'ordre

                string Do = "";
                if (FluxRdv[i].Commande.DonneurOrdre.ServiceTiersList?.Count > 0)

                    if (FluxRdv[i].Commande.DonneurOrdre.ServiceTiersList[0].Interlocuteur == null)
                    {

                        Do = (FluxRdv[i].Commande.DonneurOrdre.Nom);
                    }
                    else Do = FluxRdv[i].Commande.DonneurOrdre.ServiceTiersList[0].Interlocuteur;


                FluxRdvListUrbz[i].taskId = FluxRdv[i].Commande.IdCommandeEntete;
                FluxRdvListUrbz[i].hubName = string.Concat("Agence ", FluxRdv[i].CodeAgence.ToString());
                FluxRdvListUrbz[i].client = Do;
                //infos du contact
                FluxRdvListUrbz[i].contact.person = FluxRdv[i].Commande.NomDestinataire;
                FluxRdvListUrbz[i].contact.name = FluxRdv[i].Commande.NomDestinataire;
                FluxRdvListUrbz[i].contact.phone = TelDest;
                FluxRdvListUrbz[i].contact.email = FluxRdv[i].Commande.MailDestinataire;
                FluxRdvListUrbz[i].contact.language = "Fr";
                //infos adresse 
                FluxRdvListUrbz[i].address.street = "rue les chemins";
                FluxRdvListUrbz[i].address.zip = "35120";
                FluxRdvListUrbz[i].address.city = "Rennes";
                FluxRdvListUrbz[i].address.country = "Fr";
                FluxRdvListUrbz[i].price = null;
                FluxRdvListUrbz[i].dimensions.weight = "";
                FluxRdvListUrbz[i].dimensions.volume = "";
                FluxRdvListUrbz[i].dimensions.price = "";
                FluxRdvListUrbz[i].quantity = "";
                FluxRdvListUrbz[i].type = "delivery";
                FluxRdvListUrbz[i].serviceTime = 0;



            }
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