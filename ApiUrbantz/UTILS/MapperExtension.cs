using ApiUrbantz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.UTILS
{
    public static class MapperExtension
    {
        public static IEnumerable<FluxLivUrbantz> Convert(this List<ElementEnvoiSms> FluxRdv)
        {
            return FluxRdv.Select(x => new FluxLivUrbantz()
            {
                taskId = x.Commande.IdCommandeEntete ,
                hubName = string.Concat("Agence ",x.CodeAgence.ToString()),
                client = x.Commande.DonneurOrdre.ServiceTiersList?.Count > 0 ? x.Commande.DonneurOrdre.ServiceTiersList[0].Interlocuteur == null ? x.Commande.DonneurOrdre.Nom : x.Commande.DonneurOrdre.ServiceTiersList[0].Interlocuteur :string.Empty ,
                contact = new Contact { person = x.Commande.NomDestinataire , name = x.Commande.NomDestinataire , phone = Utils.GetFormattedPhoneNumber(x.Commande.PortableDestinataire) == null ? Utils.GetFormattedPhoneNumber(x.Commande.TelephoneDestinataire) == null ? string.Empty : Utils.GetFormattedPhoneNumber(x.Commande.TelephoneDestinataire) : Utils.GetFormattedPhoneNumber(x.Commande.PortableDestinataire) , email = x.Commande.MailDestinataire , language = "Fr" } ,
                address = new Address { street = "rue les chemins"  , zip = "35120" , city = "Rennes" , country = "Fr" },
                price = null ,
                type = "delivery",
                serviceTime = 0 
            });
        }
    }
}