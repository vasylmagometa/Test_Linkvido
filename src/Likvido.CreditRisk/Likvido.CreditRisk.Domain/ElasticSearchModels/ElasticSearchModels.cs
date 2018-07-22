using System;

namespace Likvido.CreditRisk.Domain.ElasticSearchModels
{
    // TODO remane
    public class ElasticCompanyModelDTO
    {
        public Vrvirksomhed Vrvirksomhed { get; set; }

        public Virksomhedmetadata Data
        {
            get
            {
                return Vrvirksomhed.virksomhedMetadata;
            }
        }
    }

    public class ElasticCompanyTypeaheadModelDTO
    {
        public VrvirksomhedTypeahead Vrvirksomhed { get; set; }
    }

    public class VrvirksomhedTypeahead
    {
        public string cvrNummer { get; set; }
        public VirksomhedmetadataTypeahead virksomhedMetadata { get; set; }
    }

    public class VirksomhedmetadataTypeahead
    {
        public NyestenavnTypeahead nyesteNavn { get; set; }
    }

    public class NyestenavnTypeahead
    {
        public string navn { get; set; }
    }

    public class Vrvirksomhed
    {
        public string cvrNummer { get; set; }
        public bool? reklamebeskyttet { get; set; }
        public Navne[] navne { get; set; }
        public Navne[] binavne { get; set; }
        public Beliggenhedsadresse[] beliggenhedsadresse { get; set; }
        public Telefonnummer[] telefonNummer { get; set; }
        public Telefonnummer[] sekundaertTelefonNummer { get; set; }
        public Elektroniskpost[] elektroniskPost { get; set; }
        public Hjemmeside[] hjemmeside { get; set; }
        public Obligatoriskemail[] obligatoriskEmail { get; set; }
        public Livsforloeb[] livsforloeb { get; set; }
        public Hovedbranche[] hovedbranche { get; set; }
        public Hovedbranche[] bibranche1 { get; set; }
        public Hovedbranche[] bibranche2 { get; set; }
        public Hovedbranche[] bibranche3 { get; set; }
        public Virksomhedsstatu[] virksomhedsstatus { get; set; }
        public Virksomhedsform[] virksomhedsform { get; set; }
        public Aarsbeskaeftigelse[] aarsbeskaeftigelse { get; set; }
        public Kvartalsbeskaeftigelse[] kvartalsbeskaeftigelse { get; set; }
        public Maanedsbeskaeftigelse[] maanedsbeskaeftigelse { get; set; }
        public Attributter[] attributter { get; set; }
        public Penheder[] penheder { get; set; }
        public Deltagerrelation[] deltagerRelation { get; set; }
        public object[] fusioner { get; set; }
        public object[] spaltninger { get; set; }
        public Virksomhedmetadata virksomhedMetadata { get; set; }
        public int samtId { get; set; }
        public bool? fejlRegistreret { get; set; }
        public string dataAdgang { get; set; }
        public string enhedsNummer { get; set; }
        public string enhedstype { get; set; }
        public DateTime? sidstIndlaest { get; set; }
        public DateTime? sidstOpdateret { get; set; }
        public bool? fejlVedIndlaesning { get; set; }
        public string virkningsAktoer { get; set; }
    }

    public class Virksomhedmetadata
    {
        public Nyestenavn nyesteNavn { get; set; }
        public Nyestevirksomhedsform nyesteVirksomhedsform { get; set; }
        public Nyestebeliggenhedsadresse nyesteBeliggenhedsadresse { get; set; }
        public Nyestehovedbranche nyesteHovedbranche { get; set; }
        public Nyestehovedbranche nyesteBibranche1 { get; set; }
        public Nyestehovedbranche nyesteBibranche2 { get; set; }
        public Nyestehovedbranche nyesteBibranche3 { get; set; }
        public object nyesteStatus { get; set; }
        public string[] nyesteKontaktoplysninger { get; set; }
        public int antalPenheder { get; set; }
        public Nyesteaarsbeskaeftigelse nyesteAarsbeskaeftigelse { get; set; }
        public Nyestekvartalsbeskaeftigelse nyesteKvartalsbeskaeftigelse { get; set; }
        public Nyestemaanedsbeskaeftigelse nyesteMaanedsbeskaeftigelse { get; set; }
        public string sammensatStatus { get; set; }
        public string stiftelsesDato { get; set; }
        public string virkningsDato { get; set; }
    }

    public class Nyestenavn
    {
        public string navn { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Nyestevirksomhedsform
    {
        public int virksomhedsformkode { get; set; }
        public string kortBeskrivelse { get; set; }
        public string langBeskrivelse { get; set; }
        public string ansvarligDataleverandoer { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Periode
    {
        public DateTime? gyldigFra { get; set; }
        public DateTime? gyldigTil { get; set; }
    }

    public class Nyestebeliggenhedsadresse
    {
        public string landekode { get; set; }
        public object fritekst { get; set; }
        public int vejkode { get; set; }
        public Kommune kommune { get; set; }
        public int husnummerFra { get; set; }
        public string adresseId { get; set; }
        public DateTime sidstValideret { get; set; }
        public object husnummerTil { get; set; }
        public object bogstavFra { get; set; }
        public object bogstavTil { get; set; }
        public string etage { get; set; }
        public string sidedoer { get; set; }
        public object conavn { get; set; }
        public object postboks { get; set; }
        public string vejnavn { get; set; }
        public object bynavn { get; set; }
        public int postnummer { get; set; }
        public string postdistrikt { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Kommune
    {
        public int kommuneKode { get; set; }
        public string kommuneNavn { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Nyestehovedbranche
    {
        public string branchekode { get; set; }
        public string branchetekst { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Nyesteaarsbeskaeftigelse
    {
        public int aar { get; set; }
        public object antalInklusivEjere { get; set; }
        public object antalAarsvaerk { get; set; }
        public object antalAnsatte { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public string intervalKodeAntalInklusivEjere { get; set; }
        public string intervalKodeAntalAarsvaerk { get; set; }
        public string intervalKodeAntalAnsatte { get; set; }
    }

    public class Nyestekvartalsbeskaeftigelse
    {
        public int aar { get; set; }
        public int kvartal { get; set; }
        public object antalAarsvaerk { get; set; }
        public object antalAnsatte { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public string intervalKodeAntalAarsvaerk { get; set; }
        public string intervalKodeAntalAnsatte { get; set; }
    }

    public class Nyestemaanedsbeskaeftigelse
    {
        public int aar { get; set; }
        public int maaned { get; set; }
        public object antalAarsvaerk { get; set; }
        public object antalAnsatte { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public string intervalKodeAntalAarsvaerk { get; set; }
        public string intervalKodeAntalAnsatte { get; set; }
    }

    public class Navne
    {
        public string navn { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Beliggenhedsadresse
    {
        public string landekode { get; set; }
        public object fritekst { get; set; }
        public int vejkode { get; set; }
        public Kommune1 kommune { get; set; }
        public int husnummerFra { get; set; }
        public string adresseId { get; set; }
        public DateTime? sidstValideret { get; set; }
        public object husnummerTil { get; set; }
        public object bogstavFra { get; set; }
        public object bogstavTil { get; set; }
        public string etage { get; set; }
        public string sidedoer { get; set; }
        public object conavn { get; set; }
        public object postboks { get; set; }
        public string vejnavn { get; set; }
        public object bynavn { get; set; }
        public int postnummer { get; set; }
        public string postdistrikt { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Kommune1
    {
        public int kommuneKode { get; set; }
        public string kommuneNavn { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Telefonnummer
    {
        public string kontaktoplysning { get; set; }
        public bool hemmelig { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Elektroniskpost
    {
        public string kontaktoplysning { get; set; }
        public bool hemmelig { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Hjemmeside
    {
        public string kontaktoplysning { get; set; }
        public bool hemmelig { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Obligatoriskemail
    {
        public string kontaktoplysning { get; set; }
        public bool hemmelig { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Livsforloeb
    {
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Hovedbranche
    {
        public string branchekode { get; set; }
        public string branchetekst { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Virksomhedsstatu
    {
        public string status { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Virksomhedsform
    {
        public int virksomhedsformkode { get; set; }
        public string kortBeskrivelse { get; set; }
        public string langBeskrivelse { get; set; }
        public string ansvarligDataleverandoer { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Aarsbeskaeftigelse
    {
        public int aar { get; set; }
        public object antalInklusivEjere { get; set; }
        public object antalAarsvaerk { get; set; }
        public object antalAnsatte { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public string intervalKodeAntalInklusivEjere { get; set; }
        public string intervalKodeAntalAarsvaerk { get; set; }
        public string intervalKodeAntalAnsatte { get; set; }
    }

    public class Kvartalsbeskaeftigelse
    {
        public int aar { get; set; }
        public int kvartal { get; set; }
        public object antalAarsvaerk { get; set; }
        public object antalAnsatte { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public string intervalKodeAntalAarsvaerk { get; set; }
        public string intervalKodeAntalAnsatte { get; set; }
    }

    public class Maanedsbeskaeftigelse
    {
        public int aar { get; set; }
        public int maaned { get; set; }
        public object antalAarsvaerk { get; set; }
        public object antalAnsatte { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public string intervalKodeAntalAarsvaerk { get; set; }
        public string intervalKodeAntalAnsatte { get; set; }
    }

    public class Attributter
    {
        public int sekvensnr { get; set; }
        public string type { get; set; }
        public string vaerditype { get; set; }
        public Vaerdier[] vaerdier { get; set; }
    }

    public class Vaerdier
    {
        public string vaerdi { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Penheder
    {
        public int pNummer { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Deltagerrelation
    {
        public Deltager deltager { get; set; }
        public object[] kontorsteder { get; set; }
        public Organisationer[] organisationer { get; set; }
    }

    public class Deltager
    {
        public long enhedsNummer { get; set; }
        public string enhedstype { get; set; }
        public object forretningsnoegle { get; set; }
        public object organisationstype { get; set; }
        public DateTime sidstIndlaest { get; set; }
        public DateTime sidstOpdateret { get; set; }
        public Navne1[] navne { get; set; }
        public Beliggenhedsadresse1[] beliggenhedsadresse { get; set; }
    }

    public class Navne1
    {
        public string navn { get; set; }
        public Periode periode { get; set; }
        public object sidstOpdateret { get; set; }
    }

    public class Beliggenhedsadresse1
    {
        public string landekode { get; set; }
        public object fritekst { get; set; }
        public int vejkode { get; set; }
        public Kommune2 kommune { get; set; }
        public int husnummerFra { get; set; }
        public object adresseId { get; set; }
        public object sidstValideret { get; set; }
        public object husnummerTil { get; set; }
        public string bogstavFra { get; set; }
        public object bogstavTil { get; set; }
        public string etage { get; set; }
        public string sidedoer { get; set; }
        public string conavn { get; set; }
        public object postboks { get; set; }
        public string vejnavn { get; set; }
        public object bynavn { get; set; }
        public int postnummer { get; set; }
        public string postdistrikt { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Kommune2
    {
        public int kommuneKode { get; set; }
        public string kommuneNavn { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Organisationer
    {
        public long enhedsNummerOrganisation { get; set; }
        public string hovedtype { get; set; }
        public Organisationsnavn[] organisationsNavn { get; set; }
        public Attributter1[] attributter { get; set; }
        public Medlemsdata[] medlemsData { get; set; }
    }

    public class Organisationsnavn
    {
        public string navn { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Attributter1
    {
        public int sekvensnr { get; set; }
        public string type { get; set; }
        public string vaerditype { get; set; }
        public Vaerdier1[] vaerdier { get; set; }
    }

    public class Vaerdier1
    {
        public string vaerdi { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }

    public class Medlemsdata
    {
        public Attributter2[] attributter { get; set; }
    }

    public class Attributter2
    {
        public int sekvensnr { get; set; }
        public string type { get; set; }
        public string vaerditype { get; set; }
        public Vaerdier2[] vaerdier { get; set; }
    }

    public class Vaerdier2
    {
        public string vaerdi { get; set; }
        public Periode periode { get; set; }
        public DateTime sidstOpdateret { get; set; }
    }
}
