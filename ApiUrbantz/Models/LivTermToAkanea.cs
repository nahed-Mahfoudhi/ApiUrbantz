using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{
    public class BL
    {
        public string Type { get; set; }
        public string Base64 { get; set; }
    }

    public class Photo
    {
        public string Type { get; set; }
        public string Base64 { get; set; }
    }

    public class Signature
    {
        public string name { get; set; }
        public string data { get; set; }
        public string Type { get; set; }
        public string Base64 { get; set; }
    }


    public class LivTermToAkanea
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("CodeEvenement")]
        public string CodeEvenement { get; set; }

        [JsonProperty("Motif")]
        public string Motif { get; set; }

        [JsonProperty("Note")]
        public string Note { get; set; }

        [JsonProperty("Commentaire")]
        public string Commentaire { get; set; }

        [JsonProperty("BL")]
        public BL BL { get; set; }

        [JsonProperty("Photos")]
        public IList<Photo> Photos { get; set; }

        [JsonProperty("Signature")]
        public Signature Signature { get; set; }

        [JsonProperty("DatePriseEnCharge")]
        public string DatePriseEnCharge { get; set; }

        [JsonProperty("DateArrivee")]
        public string DateArrivee { get; set; }

        [JsonProperty("DateDebut")]
        public string DateDebut { get; set; }

        [JsonProperty("DateFin")]
        public string DateFin { get; set; }

        [JsonProperty("DateDebutRDV")]
        public string  DateDebutRDV { get; set; }

        [JsonProperty("DateFinRDV")]
        public string DateFinRDV { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        public LivTermToAkanea()
        {
            Signature = new Signature();
            Photos = new List<Photo>();
            BL = new BL();
        }
    }
    public class ListLivTermToAkanea
    {
        public List<LivTermToAkanea> LivTermToAkaneaList { get;set;}
    }
}