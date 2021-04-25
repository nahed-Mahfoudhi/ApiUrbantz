using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{


    public class Agence
    {
        public int CodeNumerique { get; set; }
        public string NomCourt { get; set; }
    }

    public class Conducteur1
    {
        public string Code { get; set; }
        public string Nom { get; set; }
    }

    public class VehiculeMoteur
    {
        public string Code { get; set; }
        public string Immatriculation { get; set; }
    }

    public class CommandeTotaux
    {
        public int TotalColis { get; set; }
        public double TotalPoids { get; set; }
        public int TotalTempsMontage { get; set; }
        public double TotalUniteManutention { get; set; }
        public double TotalVolume { get; set; }
    }
    public class CommandeTotauxMontant
    {
        public double? ChiffreAffaire { get; set; }
    }
    public class DirectionTd
    {
        public string Code { get; set; }
    }



    public class Payeur
    {
        public int Code { get; set; }
        public string Nom { get; set; }
        public object SpecifiqueChaine1 { get; set; }
    }

    public class Prestation
    {
        public string Code { get; set; }
    }

    public class Trafic
    {
        public string Code { get; set; }
    }

    public class Article
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }

    public class CommandeDetail
    {
        public Article Article { get; set; }
        public string LibelleRubrique { get; set; }
        public int? NbArticle { get; set; }
        public double Poids { get; set; }
        public Prestation Prestation { get; set; }
    public double Volume { get; set; }
}

public class CommandeColisList
{
    public string CodeBarre { get; set; }
    public CommandeDetail CommandeDetail { get; set; }
    public int IdColis { get; set; }
    //public int IdCommandeEntete { get; set; }
}

public class CommandeEntete
{
    public bool Ascenseur { get; set; }
    public CommandeTotaux CommandeTotaux { get; set; }
    public CommandeTotauxMontant CommandeTotauxMontant { get; set; }
    public DateTime DateDebutLivraison { get; set; }
    public object DateRDV { get; set; }
    public object Digicode1 { get; set; }
    public object Digicode2 { get; set; }
    public DirectionTd DirectionTd { get; set; }
    public DonneurOrdre DonneurOrdre { get; set; }
    public object Etage { get; set; }
    public int HeureDebutRDV { get; set; }
    public int HeureFinRDV { get; set; }
    public int IdCommandeEntete { get; set; }
    public string InstructionLivraison { get; set; }
    public string Interphone { get; set; }
    public bool IsEnvoyeMobilite { get; set; }
    public string LotArrivageRamasse { get; set; }
    public string MailDestinataire { get; set; }
    public string MontantContreRemboursement { get; set; }
    public string NomDestinataire { get; set; }
    public string NumeroRecepisse { get; set; }
    public int OrdreNumeroBordereauDepart { get; set; }
    public Payeur Payeur { get; set; }
    public string PortableDestinataire { get; set; }
    public Prestation Prestation { get; set; }
    public string ProgrammeOrigineSaisie { get; set; }
    public string Reference { get; set; }
    public string Reference2 { get; set; }
    public string TelephoneDestinataire { get; set; }
    public DateTime THeureDebutRDV { get; set; }
    public DateTime THeureFinRDV { get; set; }
    public Trafic Trafic { get; set; }
    public IList<CommandeColisList> CommandeColisList { get; set; }

}

public class LocaliteArrivee
{
    public string CodeDepartement { get; set; }
    public string CodePays { get; set; }
    public string CodePostal { get; set; }
    public string CodeRegion { get; set; }
    public string Nom { get; set; }
}

public class TrajetList
{
    public string Adresse1Arrivee { get; set; }
    public string Adresse2Arrivee { get; set; }
    public object Adresse3Arrivee { get; set; }
    public CommandeEntete CommandeEntete { get; set; }
    public LocaliteArrivee LocaliteArrivee { get; set; }
    public string NomArrivee { get; set; }
   
}

public class FluxAOptimiser
    {
    public Agence Agence { get; set; }
    public string CodeDossier { get; set; }
    public Conducteur1 Conducteur1 { get; set; }
    public object Conducteur2 { get; set; }
    public object Conducteur3 { get; set; }
    public object Conducteur4 { get; set; }
    public DateTime DateDossier { get; set; }
    public string DirectionBordereau { get; set; }
    public VehiculeMoteur VehiculeMoteur { get; set; }
    public object VehiculeTracte1 { get; set; }
    public object VehiculeTracte2 { get; set; }
    public IList<TrajetList> TrajetList { get; set; }
}
}



    
