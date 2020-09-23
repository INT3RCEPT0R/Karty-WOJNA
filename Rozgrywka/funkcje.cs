using System;
using Uczestnicy;
using TaliaKart;
using System.Collections.Generic;

namespace Rozgrywka
{
    public static class funkcje
    {
        public static Gracz AktualnieWygrywającyGracz = new Gracz();
        public static List<Gracz> graczedousuniecia = new List<Gracz>();
        public static short WywołajWojnę(List<Gracz> gracze, int poziomZagnieżdżenia)
        {
            int AktualnaWartośćKarty = 0;
            int iloscpowtorzen = 0;
            List<Gracz> wojnagraczy = new List<Gracz>();

            Console.WriteLine("WOJNA!");

            foreach (Gracz gracz in gracze)
            {
                if (gracz.Cards.Count< 3*poziomZagnieżdżenia)
                {
                    Console.WriteLine("Gracz: " + gracz.ID + " odpada z gry.");
                    graczedousuniecia.Add(gracz);
                }

            }
            foreach (Gracz usun in graczedousuniecia)
            {
                gracze.Remove(usun);
            }

            if(gracze.Count == 0)
            {
                Console.WriteLine("WOJNA NIE UDANA!\n");
                return 1;
            }


            foreach (Gracz player in gracze)
            {
                Console.WriteLine("Gracz {0} z {1} i ma {2} kart", player.ID, player.Cards[3 * poziomZagnieżdżenia -1].Name, player.Cards.Count);
            }

            Card NajWyższaKarta = funkcje.SprawdźNajwyższąWartość(gracze, poziomZagnieżdżenia*3-1, out iloscpowtorzen);

            Console.WriteLine("Najwyższa karta w tej rundzie to: " + NajWyższaKarta.Name + " i powtórzyła się {0} razy", iloscpowtorzen);

            if (iloscpowtorzen != 1)
            {
                foreach (Gracz player in gracze)
                {
                    if (player.Cards[0].Value == NajWyższaKarta.Value)
                    {
                        wojnagraczy.Add(player);
                    }
                }
                funkcje.WywołajWojnę(wojnagraczy, poziomZagnieżdżenia+1);
                wojnagraczy.Clear();
            }

            foreach (Gracz gracz in gracze)
            {
                if (gracz.Cards[2].Value == AktualnaWartośćKarty)
                {

                }
                else if (gracz.Cards[2].Value > AktualnaWartośćKarty)
                {
                    AktualnaWartośćKarty = gracz.Cards[2].Value;
                    AktualnieWygrywającyGracz = gracz;
                }
            }

            Console.WriteLine("Wygrywa gracz: " + AktualnieWygrywającyGracz.ID + " z kartą : " + AktualnaWartośćKarty);

            foreach (Gracz gracz in gracze)
            {
                if (gracz != AktualnieWygrywającyGracz)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (gracz.Cards.Count != 0)
                        {
                            AktualnieWygrywającyGracz.Cards.Add(gracz.Cards[0]);
                            gracz.Cards.RemoveAt(0);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        gracz.Cards.Add(gracz.Cards[0]);
                        gracz.Cards.RemoveAt(0);
                    }
                }
            }
            Console.WriteLine(AktualnieWygrywającyGracz.ID + " wielkośc talii:" + AktualnieWygrywającyGracz.Cards.Count);
            Console.WriteLine("KONIEC WOJNY!\n");
            return 0;
        }


        public static Card SprawdźNajwyższąWartość(List<Gracz> gracze, int NaPozycji, out int Powtórzenia)
        {
            int MaxValue = 0;
            int Count = 0;
            foreach (Gracz gracz in gracze)
            {
                if (gracz.Cards[NaPozycji].Value > MaxValue)
                {
                    MaxValue = gracz.Cards[NaPozycji].Value;
                }
            }
            foreach (Gracz gracz in gracze)
            {
                if (gracz.Cards[NaPozycji].Value == MaxValue)
                {
                    Count++;
                }
            }
            Powtórzenia = Count;
            foreach (Gracz gracz in gracze)
            {
                if (gracz.Cards[NaPozycji].Value == MaxValue)
                {
                    return gracz.Cards[NaPozycji];
                }
            }
            return null;
        }

        public static int InicjalizacjaGry(out int limitrund)
        {
            //
            //Powitanie i pobranie parametrów związanych z grą
            //

            Console.WriteLine("Witam w grze karcianej o nazwie \"WOJNA!\"");

            bool CzyWybranoTryb = false;
            int trybgry;

            do
            {
                Console.WriteLine("Wybierz liczbę graczy:");

                bool CzyPodanoLiczbę = Int32.TryParse(Console.ReadLine(), out trybgry);

                if (CzyPodanoLiczbę)
                {
                    Console.WriteLine("Liczba graczy: " + trybgry);
                    CzyWybranoTryb = true;
                }
                else
                {
                    Console.WriteLine("To nie liczba!");
                }
                Console.WriteLine("");
            }
            while (!CzyWybranoTryb);

            bool CzyPodanoRundy = false;

            do
            {
                Console.WriteLine("Podaj limit rund");
                bool CzyPodanoLiczbę = Int32.TryParse(Console.ReadLine(), out limitrund);

                if (CzyPodanoLiczbę)
                {
                    CzyPodanoRundy = true;
                }
                else
                {
                    Console.WriteLine("To nie liczba!");
                }
                Console.WriteLine("");
            }
            while (!CzyPodanoRundy);

            return trybgry;
        }
    }
}
