using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{
    

    public class Service
    {

        [JsonProperty("Libelle")]
        public string Libelle { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }
    }

    public class ServiceTiersList
    {

        [JsonProperty("Service")]
        public Service Service { get; set; }

        [JsonProperty("Tel")]
        public string Tel { get; set; }

        [JsonProperty("Gsm")]
        public object Gsm { get; set; }

        [JsonProperty("Email")]
        public object Email { get; set; }

        [JsonProperty("Interlocuteur")]
        public string Interlocuteur { get; set; }
    }

    public class DonneurOrdre
    {

        [JsonProperty("Code")]
        public int Code { get; set; }

        [JsonProperty("Nom")]
        public string Nom { get; set; }

        [JsonProperty("ServiceTiersList")]
        public List<ServiceTiersList> ServiceTiersList { get; set; }
        public DonneurOrdre()
        {
            ServiceTiersList = new List<ServiceTiersList>();

        }
    }

   

    public class Commande
    {
        [JsonProperty("DonneurOrdre")]
        public DonneurOrdre DonneurOrdre { get; set; }

        [JsonProperty("Payeur")]
        public Payeur Payeur { get; set; }

        [JsonProperty("IdCommandeEntete")]
        public string IdCommandeEntete { get; set; }

        [JsonProperty("NumeroRecepisse")]
        public int NumeroRecepisse { get; set; }

        [JsonProperty("NomDestinataire")]
        public string NomDestinataire { get; set; }

        [JsonProperty("PortableDestinataire")]
        public string PortableDestinataire { get; set; }

        [JsonProperty("TelephoneDestinataire")]
        public string TelephoneDestinataire { get; set; }

        [JsonProperty("MailDestinataire")]
        public string MailDestinataire { get; set; }

        public Commande()
        {
            DonneurOrdre = new DonneurOrdre();

        }
    }

    public class ElementEnvoiSms
    {

        [JsonProperty("Commande")]
        public Commande Commande { get; set; }

        [JsonProperty("CodeEvenement")]
        public string CodeEvenement { get; set; }

        [JsonProperty("CodeAgence")]
        public int CodeAgence { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}