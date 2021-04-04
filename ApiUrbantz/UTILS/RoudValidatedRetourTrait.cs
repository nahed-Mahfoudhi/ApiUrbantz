using ApiUrbantz.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ApiUrbantz.UTILS
{
    public static class RoudValidatedRetourTrait
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task<HttpResponseMessage> RoundValidesToAkanea(RoundValid RoundValides)
        {
            //TODO faire le traitement ici le mapping ....

            LTournesValidesToAkanea FluxTournesVsAkanea = new LTournesValidesToAkanea();
            List<TournesValidesToAkanea> tournesValidesToAkanea = new List<TournesValidesToAkanea>();
            List<TournesValidesToAkanea> tournesValidesToAkaneaDupl = new List<TournesValidesToAkanea>();
            LTournesValidesToAkaneaDupl FluxTournesVsAkaneaDupl = new LTournesValidesToAkaneaDupl();
            if (RoundValides.round.metadata != null && RoundValides.round.metadata.Agence_tournee != null && RoundValides.round.metadata.Agence_tournee[0].Substring(3, 2) == "26")
            {
                for (int i = 0; i < RoundValides.round.stops.Count() - 1; i++)
                {
                    tournesValidesToAkanea.Add(new TournesValidesToAkanea());
                    tournesValidesToAkaneaDupl.Add(new TournesValidesToAkanea());
                }

            // si on 6 element on prepare 5 elements seulemet a remplir après ...


            #region Remplir le modèle flux livraison de Mroad à Urbantz
            
                for (int i = 1; i <= tournesValidesToAkanea.Count; i++)
                {
                    tournesValidesToAkanea[i - 1].IdCommande = RoundValides.round.stops[i].taskReference;
                    tournesValidesToAkanea[i - 1].Action = RoundValides.round.stops[i].type;
                    tournesValidesToAkanea[i - 1].Ordre = RoundValides.round.stops[i].sequence.ToString();
                    if (RoundValides.round.metadata != null && RoundValides.round.metadata.Agence_tournee != null)
                    {
                        tournesValidesToAkanea[i - 1].CodeAgence = RoundValides.round.metadata.Agence_tournee[0].Substring(3, 2);
                    }
                    tournesValidesToAkanea[i - 1].CodeDirection = RoundValides.round.name;
                    //tournesValidesToAkanea[i - 1].DateHeureArriveeClient = RoundValides.round.stops[i].arriveTime.ToString("yyyy/MM/dd HH:00:00");
                    //tournesValidesToAkanea[i - 1].DateHeureDepartClient = (RoundValides.round.stops[i].arriveTime.AddHours(2)).ToString("yyyy/MM/dd HH:00:00");
                    tournesValidesToAkanea[i - 1].DateHeureArriveeClient = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkanea[i - 1].DateHeureDebutCommande = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkanea[i - 1].DateHeureDepartClient = string.Concat(RoundValides.round.stops[i].departTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].departTime.ToLocalTime().ToString("HH:00:00"));

                    tournesValidesToAkaneaDupl[i - 1].IdCommande = RoundValides.round.stops[i].taskReference;
                    if (tournesValidesToAkanea[i - 1].Action == "delivery")
                    {
                        tournesValidesToAkaneaDupl[i - 1].Action = "pickup";
                    }
                    else if (tournesValidesToAkanea[i - 1].Action == "pickup")
                    {
                        tournesValidesToAkaneaDupl[i - 1].Action = "delivery";
                    }

                    tournesValidesToAkaneaDupl[i - 1].Ordre = RoundValides.round.stops[i].sequence.ToString();
                    if (RoundValides.round.metadata != null && RoundValides.round.metadata.Agence_tournee != null)
                    {
                        tournesValidesToAkaneaDupl[i - 1].CodeAgence = RoundValides.round.metadata.Agence_tournee[0].Substring(3, 2);
                    }
                    tournesValidesToAkaneaDupl[i - 1].CodeDirection = RoundValides.round.name;
                    tournesValidesToAkaneaDupl[i - 1].DateHeureArriveeClient = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkaneaDupl[i - 1].DateHeureDebutCommande = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkaneaDupl[i - 1].DateHeureDepartClient = string.Concat(RoundValides.round.stops[i].departTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].departTime.ToLocalTime().ToString("HH:00:00"));
                }
            }
            else
            {


                for (int i = 0; i < RoundValides.round.stops.Count() - 2; i++)
                {
                    tournesValidesToAkanea.Add(new TournesValidesToAkanea());
                    tournesValidesToAkaneaDupl.Add(new TournesValidesToAkanea());
                }

                for (int i = 1; i <= tournesValidesToAkanea.Count; i++)
                {
                    tournesValidesToAkanea[i - 1].IdCommande = RoundValides.round.stops[i].taskReference;
                    tournesValidesToAkanea[i - 1].Action = RoundValides.round.stops[i].type;
                    tournesValidesToAkanea[i - 1].Ordre = RoundValides.round.stops[i].sequence.ToString();
                    if (RoundValides.round.metadata != null && RoundValides.round.metadata.Agence_tournee != null)
                    {
                        tournesValidesToAkanea[i - 1].CodeAgence = RoundValides.round.metadata.Agence_tournee[0].Substring(3, 2);
                    }
                    tournesValidesToAkanea[i - 1].CodeDirection = RoundValides.round.name;
                    //tournesValidesToAkanea[i - 1].DateHeureArriveeClient = RoundValides.round.stops[i].arriveTime.ToString("yyyy/MM/dd HH:00:00");
                    //tournesValidesToAkanea[i - 1].DateHeureDepartClient = (RoundValides.round.stops[i].arriveTime.AddHours(2)).ToString("yyyy/MM/dd HH:00:00");
                    tournesValidesToAkanea[i - 1].DateHeureArriveeClient = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkanea[i - 1].DateHeureDebutCommande = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkanea[i - 1].DateHeureDepartClient = string.Concat(RoundValides.round.stops[i].departTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].departTime.ToLocalTime().ToString("HH:00:00"));

                    tournesValidesToAkaneaDupl[i - 1].IdCommande = RoundValides.round.stops[i].taskReference;
                    if (tournesValidesToAkanea[i - 1].Action == "delivery")
                    {
                        tournesValidesToAkaneaDupl[i - 1].Action = "pickup";
                    }
                    else if (tournesValidesToAkanea[i - 1].Action == "pickup")
                    {
                        tournesValidesToAkaneaDupl[i - 1].Action = "delivery";
                    }

                    tournesValidesToAkaneaDupl[i - 1].Ordre = RoundValides.round.stops[i].sequence.ToString();
                    if (RoundValides.round.metadata != null && RoundValides.round.metadata.Agence_tournee != null)
                    {
                        tournesValidesToAkaneaDupl[i - 1].CodeAgence = RoundValides.round.metadata.Agence_tournee[0].Substring(3, 2);
                    }
                    tournesValidesToAkaneaDupl[i - 1].CodeDirection = RoundValides.round.name;
                    tournesValidesToAkaneaDupl[i - 1].DateHeureArriveeClient = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkaneaDupl[i - 1].DateHeureDebutCommande = string.Concat(RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].arriveTime.ToLocalTime().ToString("HH:00:00"));
                    tournesValidesToAkaneaDupl[i - 1].DateHeureDepartClient = string.Concat(RoundValides.round.stops[i].departTime.ToLocalTime().ToString("yyyy-MM-dd"), "T", RoundValides.round.stops[i].departTime.ToLocalTime().ToString("HH:00:00"));
                }
            }

            FluxTournesVsAkanea.listTournesValidesToAkanea = tournesValidesToAkanea;
            FluxTournesVsAkanea.listTournesValidesToAkanea.AddRange(tournesValidesToAkaneaDupl);


            #endregion

            //***

            var json = new JavaScriptSerializer().Serialize(FluxTournesVsAkanea.listTournesValidesToAkanea);
            json = json.Replace("\u0027", "");
            var passflux =EnvoiFlux.SendData(FluxTournesVsAkanea.listTournesValidesToAkanea, "WebApi/FluxCamionnage");
            await System.Threading.Tasks.Task.Delay(500);
            return await await System.Threading.Tasks.Task.FromResult(passflux);
        }

    }

}
