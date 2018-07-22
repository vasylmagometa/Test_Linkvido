using Likvido.CreditRisk.Domain.Enums;
using Likvido.CreditRisk.Services.Abstraction.Scoring;

namespace Likvido.CreditRisk.Services.Scoring
{
    public class ScoringDescriptionService : IScoringDescriptionService
    {
        public string DescriptionAge(decimal data)
        {
            if (data < 0)
            {
                return "Virksomheden er forholdsvis ny, hvilket trækker kredit indikationen ned";
            }
            else if (data <= 10)
            {
                return "Virksomheden har været aktiv i flere år, hvilket giver lidt lavere kredit indikation";
            }

            return "Virksomheden har været aktiv i mange år, hvilket løfter kredit indikationen";
        }

        public string DescriptionEmployees(decimal data)
        {
            if (data < 0)
            {
                return "Virksomheden har ingen eller meget få medarbejdere, hvilket trækker kredit indikationen ned";
            }
            else if (data <= 10)
            {
                return "Virksomheden har en del medarbejdere, hvilket giver lavere kredit indikation";
            }

            return "Virksomheden har mange medarbejdere, hvilket løfter kredit indikationen";
        }

        public string DescriptionType(CompanyType companyType)
        {
            switch (companyType)
            {
                case CompanyType.APS:
                    return "Virksomheden er et anpartsselskab (ApS), hvilket er neutralt";
                case CompanyType.IS:
                    return "Virksomheden er et Interessentskab (I/S), hvilket giver lidt kredit indikation, da der er personlig hæftelse over flere personer";
                case CompanyType.AS:
                    return "Virksomheden er et aktieselskab (A/S), hvilket giver meget kredit indikation";
                case CompanyType.IVS:
                    return "Virksomheden er et iværksætterselskab (IVS), hvilket trækker meget ned i kredit indikationen";
                case CompanyType.Personal:
                    return "Virksomheden er personligt ejet, hvilket giver lidt mindre kredit indikation, da der er personlig hæftelse";
                default:
                    return string.Empty;
            }
        }

        public string DescriptionSituation(decimal data)
        {
            if (data == 0)
            {
                return "Virksomheden er aktiv hvilket er forventeligt";
            }

            return "Virksomheden er ikke aktiv (konkurs eller ophørt). Dette gør du IKKE skal handle med virksomheden";
        }

        public string DescriptionEquity(decimal data)
        {
            if (data < 0)
            {
                return "Virksomhedens egenkapital trækker ned på den samlede kredit indikation";
            }
            else if (data <= 10)
            {
                return "Virksomheden har en acceptabel egenkapital, hvilket giver lidt kredit indikation";
            }

            return "Virksomheden har en fin egenkapital, hvilket trækker op i kredit indikationen";
        }

        public string DescriptionResult(decimal data)
        {
            if (data < 0)
            {
                return "Virksomhedens seneste årsresultat trækker ned på den samlede kredit indikation";
            }
            else if (data <= 10)
            {
                return "Virksomhedens seneste årsresultat var acceptabelt, hvilket giver lidt kredit indikation";
            }

            return "Virksomhedens seneste årsresultat var godt, hvilket trækker op i kredit indikationen";
        }
    }
}
