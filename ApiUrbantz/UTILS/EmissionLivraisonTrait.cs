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

namespace ApiUrbantz.UTILS
{
    public static class EmissionLivraisonTrait
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task<HttpResponseMessage> GetTaskByBorderau(FluxAOptimiser FluxMroad)
        {
            //TODO faire le traitement 
            #region creation de l'objet FluxLivUrbant contenant la liste de l'objet FluxMroad
            ListFluxLivUrbants ListFluxLivUrbant = new ListFluxLivUrbants();
            List<FluxLivUrbantz> FluxVsUrbantz = new List<FluxLivUrbantz>();
            foreach (var item in FluxMroad.TrajetList)
            {
                FluxVsUrbantz.Add(new FluxLivUrbantz());
            }
            ListFluxLivUrbant.ListFluxLivUrbantz = FluxVsUrbantz;
            #endregion

            #region Remplir le modèle flux livraison de Mroad à Urbantz
            for (int i = 0; i < FluxVsUrbantz.Count; i++)
            {

                var TypeLivraison = "";
                if (FluxMroad.TrajetList[i].CommandeEntete.Trafic.Code == "LIV")
                {
                    TypeLivraison = "delivery";

                }
                else if (FluxMroad.TrajetList[i].CommandeEntete.Trafic.Code == "REP")
                {
                    TypeLivraison = "pickup";
                }
                else if (FluxMroad.TrajetList[i].CommandeEntete.Trafic.Code == "SAV")
                {
                    FluxMroad.TrajetList[i].CommandeEntete.Trafic.Code = "LIV";
                    TypeLivraison = "delivery";
                }
                #region 
                //calcul de numero de telephone
                var Telephone = "";
                var TelInternational = "";
                if (FluxMroad.TrajetList[i].LocaliteArrivee.CodePays=="FR")
                {
                         if ((FluxMroad.TrajetList[i].CommandeEntete.PortableDestinataire != null) && ((FluxMroad.TrajetList[i].CommandeEntete.PortableDestinataire.Substring(0, 2) == "06")
                    || (FluxMroad.TrajetList[i].CommandeEntete.PortableDestinataire.Substring(0, 2) == "07"))
                    )

                {
                    Telephone = FluxMroad.TrajetList[i].CommandeEntete.PortableDestinataire;
                }
                else if (FluxMroad.TrajetList[i].CommandeEntete.TelephoneDestinataire != null)
                {
                    if ((FluxMroad.TrajetList[i].CommandeEntete.TelephoneDestinataire.Substring(0, 2) == "06")
                    || (FluxMroad.TrajetList[i].CommandeEntete.TelephoneDestinataire.Substring(0, 2) == "07"))

                    {
                        Telephone = FluxMroad.TrajetList[i].CommandeEntete.TelephoneDestinataire;
                    }
                }
                    //changement du téléphone au format international

                  
                    if (Telephone != "")
                    {
                        if ((Telephone.Substring(0, 2) == "06") || (Telephone.Substring(0, 2) == "07"))
                        {
                            TelInternational = string.Concat("+33", Telephone.Substring(1, Telephone.Length - 1));
                        }
                        else if (Telephone.Substring(0, 3) == "+33")
                            TelInternational = Telephone;
                    }
                }


                else if ((FluxMroad.TrajetList[i].LocaliteArrivee.CodePays == "LU")|| (FluxMroad.TrajetList[i].LocaliteArrivee.CodePays == "BE"))

                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.PortableDestinataire != null)
                    {
                        TelInternational = FluxMroad.TrajetList[i].CommandeEntete.PortableDestinataire;
                    }
                    else
                    {
                        TelInternational = FluxMroad.TrajetList[i].CommandeEntete.TelephoneDestinataire;
                    }
                }
                
                #endregion

                
                FluxVsUrbantz[i].taskId = FluxMroad.TrajetList[i].CommandeEntete.NumeroRecepisse;
                FluxVsUrbantz[i].hubName = string.Concat("VIR", FluxMroad.Agence.CodeNumerique);
                FluxVsUrbantz[i].type = TypeLivraison;
                FluxVsUrbantz[i].taskReference = FluxMroad.TrajetList[i].CommandeEntete.IdCommandeEntete.ToString();
                //Nom du Do
                FluxVsUrbantz[i].client = FluxMroad.TrajetList[i].CommandeEntete.Payeur.Nom;
                FluxVsUrbantz[i].contact.email = FluxMroad.TrajetList[i].CommandeEntete.MailDestinataire;
                FluxVsUrbantz[i].contact.language = FluxMroad.TrajetList[i].LocaliteArrivee.CodePays;
                FluxVsUrbantz[i].contact.phone = TelInternational;
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

                #region calcul temps de montage 
                int tmps_poids = 0;
                if(FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code=="LX")
                     { tmps_poids = 15; }
                if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                { tmps_poids = 15; }
                if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                { tmps_poids = 30; }
                if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                { tmps_poids = 30; }
                if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                { tmps_poids = 30; }
                if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                { tmps_poids = 45; }
                if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                { tmps_poids = 30; }
                if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                { tmps_poids = 15; }

                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 400)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 15; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 15; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 20; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 20; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 20; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 32; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 20; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 15; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 300)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 10; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 10; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 15; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 15; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 15; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 27; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 15; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 15; }
                    
                }

                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 200)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 10; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 10; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 15; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 12; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 12; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 22; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 12; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 15; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 150)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 12; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 12; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 22; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 12; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 10; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 120)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 12; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 12; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 19; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 12; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 10; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 100)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 10; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 15; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 10; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 80)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 5; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 5; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 7; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 10; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 15; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 10; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 50)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 5; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 5; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS")
                    { tmps_poids = 7; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 7; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 7; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 7; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 10; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.CommandeTotaux.TotalPoids < 30)
                {
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LX")
                    { tmps_poids = 3; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LC") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "PDC"))
                    { tmps_poids = 3; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LS")|| (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LIV"))
                    { tmps_poids = 5; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LD")
                    { tmps_poids = 5; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "RS")
                    { tmps_poids = 5; }
                    if ((FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LI") || (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "DMI"))
                    { tmps_poids = 10; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LM")
                    { tmps_poids = 5; }
                    if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "LE")
                    { tmps_poids = 10; }

                }
                if (FluxMroad.TrajetList[i].CommandeEntete.Prestation.Code == "CC")
                {
                    tmps_poids = 3;
                
                }
                    #endregion





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

            #endregion


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
        public  static void email_send()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("n.mahfoudhi@vir.fr");
            mail.To.Add("n.mahfoudhi@vir.fr");
            mail.Subject = "Test Mail - 1";
            mail.Body = "mail with attachment";
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment("C:/Users/n.mahfoudhi/source/repos/ApiUrbantz/ApiUrbantz/SaveFile/LOGFILE.txt");
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("n.mahfoudhi@vir.fr", "NAHED456789");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}

