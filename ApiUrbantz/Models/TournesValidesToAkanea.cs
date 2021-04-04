using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{
    public class TournesValidesToAkanea
    {

        //Id de commande
        [JsonProperty("IdCommande")]
        public string IdCommande { get; set; }
        //Code d'agence
        [JsonProperty("CodeAgence")]
        public string CodeAgence { get; set; }

        //Code de direction
        [JsonProperty("CodeDirection")]
        public string CodeDirection { get; set; }

        //Action
        [JsonProperty("Action")]
        public string Action { get; set; }

        //Ordre
        [JsonProperty("Ordre")]
        public string Ordre { get; set; }

        //Date et heure de debut de commande
        [JsonProperty("DateHeureDebutCommande")]
        public string DateHeureDebutCommande { get; set; }

        //Date et heure d'arrivee de client
        [JsonProperty("DateHeureArriveeClient")]
        public string DateHeureArriveeClient { get; set; }

        //Date et heure de départ du client
        [JsonProperty("DateHeureDepartClient")]
        public string DateHeureDepartClient { get; set; }
    }
    public class LTournesValidesToAkanea
    {
        public List<TournesValidesToAkanea> listTournesValidesToAkanea { get; set; }
    }

    public class LTournesValidesToAkaneaDupl
    {
        public List<TournesValidesToAkanea> listTournesValidesToAkaneaDupl { get; set; }
    }

}