using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Contact
    {
        public IList<object> extraEmails { get; set; }
        public IList<object> extraPhones { get; set; }
        public string person { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string language { get; set; }
    }

    public class Metadata
    {
        public string libelle_adresse { get; set; }
        public string Tel_supplementaire { get; set; }
        public string Agence { get; set; }
        public IList<string> Photos_echec_livraison { get; set; }
        public string Code_chauffeur { get; set; }
        public IList<string> Photos_livraison { get; set; }
        public string Statut_livraison { get; set; }
        public string D3E { get; set; }
        public string Reprise { get; set; }
        public string Motif_livraison_effectuee_avec_probleme { get; set; }
        public int Nombre_de_colis_recus { get; set; }
        public string Etat_colis { get; set; }
        public int Note { get; set; }
        public string Motif_note { get; set; }
        public string Commentaire_note { get; set; }
        public string Signature_client { get; set; }
        //Agence 
        public IList<string> Agence_tournee { get; set; }


    }

    public class Address
    {
        public string street { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }

    public class TimeWindow
    {
        public string start { get; set; }
        public string stop { get; set; }
    }

    public class Dimensions
    {
        public string weight { get; set; }
        public string volume { get; set; }
        public string price { get; set; }
    }
    public class Damaged
    {
        public bool confirmed { get; set; }
        public IList<object> pictures { get; set; }
    }
  
    public class Item
    {
        public string type { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string barcode { get; set; }
        public Dimensions dimensions { get; set; }
        public IList<string> labels { get; set; }
        public Damaged damaged { get; set; }
        public string status { get; set; }
        public IList<object> skills { get; set; }
        public object lastOfflineUpdatedAt { get; set; }
        public string _id { get; set; }
        public string barcodeEncoding { get; set; }

     
        public Item()
        {
            dimensions = new Dimensions();
           
        }
    }

    public class FluxLivUrbantz
    {
        public string taskId { get; set; }
        public string hubName { get; set; }
        public string client { get; set; }
        public Contact contact { get; set; }
        public Metadata metadata { get; set; }
        public Address address { get; set; }
        public string instructions { get; set; }
        public TimeWindow timeWindow { get; set; }
        public DateTime date { get; set; }
        public double? price { get; set; }
        public IList<Item> items { get; set; }
        public Dimensions dimensions { get; set; }
        public string quantity { get; set; }
        public string type { get; set; }
        public string taskReference { get; set; }
        public int serviceTime { get; set; }
        public FluxLivUrbantz()
        {
            contact = new Contact();
            timeWindow = new TimeWindow();
            metadata = new Metadata();
            address = new Address();
            dimensions = new Dimensions();
            items = new List<Item>();
        }

    }

    public class ListFluxLivUrbants

    {
        public List<FluxLivUrbantz> ListFluxLivUrbantz { get; set; }
    }
}


    