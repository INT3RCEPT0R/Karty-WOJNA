using System;
using Uczestnicy;
using TaliaKart;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rozgrywka
{
    public static class funkcje
    {        
        public static List<Gracz> graczedousuniecia = new List<Gracz>();
        public static List<Card> stos = new List<Card>();

        public static List<Gracz> Play(int limitrund, List<Gracz> Players)
        {
            //Główna część programu, rozgrywka pomiędzy graczami

            //Inicjzalizowanie zmiennych do rozgrywki
            int runda = 0;
            bool wygrana = false;
            int PlayerActivity = 0;
            int iloscpowtorzen = 0;
            List<Gracz> wojnagraczy = new List<Gracz>();
            Gracz AktualnieWygrywającyGracz = new Gracz();

            do
            {

                foreach (Gracz player in Players)                        
                {
                    if (player.Cards.Count == 0)
                    {
                        Console.WriteLine("Gracz: " + player.ID + " odpada z gry.");
                        funkcje.graczedousuniecia.Add(player);
                    }
                }
                foreach (Gracz usun in funkcje.graczedousuniecia)
                {
                    Players.Remove(usun);
                }       //Usunięcie graczy którzy odpadli
                funkcje.graczedousuniecia.Clear();



                runda++;                                                    //Sprawdzenie warunków końca gry: limit rund, ilośc graczy
                if (runda > limitrund)
                {
                    Console.WriteLine("Koniec tur!");
                    wygrana = true;
                }
                else if (Players.Count == 1)
                {
                    Console.WriteLine("Pozostał ostatni gracz!");
                    wygrana = true;
                }
                else if(Players.Count < 1)
                {
                    Console.WriteLine("Wszyscy gracze przegrali!");
                    wygrana = true;
                }
                if (wygrana == true)
                {
                    break;
                }


                if (runda % 25 == 0)                                    
                {
                    foreach (Gracz player in Players)
                    {
                        player.Cards = Card.MixCards(player.Cards);
                    }
                }                                       //Po 25 rundach wymiesznie kart na rękach wszystkich graczy

                
                Console.WriteLine("\nRunda numer: " + runda);
                foreach (Gracz player in Players)                           
                {
                    Console.WriteLine("Gracz {0} z {1} i ma {2} kart", player.ID, player.Cards[0].Name, player.Cards.Count);
                }                                                               //Wyłożenie kart przez wszystkich graczy i aktywność użytkownika
                if (Players.Exists(Player => Player.ID == 1))
                {
                    do
                    {
                        PlayerActivity = funkcje.DecyzjaGracza();
                        if (PlayerActivity == 2 || PlayerActivity == 1)
                        {
                            break;
                        }else if(PlayerActivity == 3)
                        {
                            Players[0].ShowHand();
                        }
                        else if (PlayerActivity == 4)
                        {                            
                            Players[0].Cards = TaliaKart.Card.MixCards(Players[0].Cards);
                            Console.WriteLine();
                        }
                    }
                    while (true);

                    if (PlayerActivity == 2)
                    {
                        break;
                    }
                }                                                                   //Aktywność gracza

                Card NajWyższaKarta = funkcje.SprawdźNajwyższąWartość(Players, 0, out iloscpowtorzen);         //Sprawdzenie najwyższej karty



                Console.WriteLine("Najwyższa karta w tej rundzie to: " + NajWyższaKarta.Name + " i powtórzyła się {0} razy", iloscpowtorzen);



                if (iloscpowtorzen != 1)                                                                        
                {
                    foreach (Gracz player in Players)
                    {
                        if (player.Cards[0].Value == NajWyższaKarta.Value)
                        {
                            wojnagraczy.Add(player);
                        }
                    }
                    funkcje.WywołajWojnę(wojnagraczy, 1);
                    wojnagraczy.Clear();
                }                                   //Sprawdzenie powtarzających się kart i wywołanie wojny lub normalna runda
                else
                {
                    foreach (Gracz player in Players)
                    {
                        if (player.Cards[0].Value == NajWyższaKarta.Value)
                        {
                            AktualnieWygrywającyGracz = player;
                        }
                    }

                    foreach (Gracz player in Players)
                    {
                        if (player != AktualnieWygrywającyGracz)
                        {
                            if (player.Cards.Count != 0)
                            {
                                AktualnieWygrywającyGracz.Cards.Add(player.Cards[0]);
                                player.Cards.RemoveAt(0);
                            }
                        }
                        else
                        {
                            player.Cards.Add(player.Cards[0]);
                            player.Cards.RemoveAt(0);
                        }

                    }
                    Console.WriteLine("Wygrywa gracz: " + AktualnieWygrywającyGracz.ID + " i ma " + AktualnieWygrywającyGracz.Cards.Count + " karty!\n");
                }                                                       //W przeciwnym wypadku przerowadź zwykłą potyczkę
                    

            } while (true);
            return Players;                                                         //Zwrócenie listy graczy
        }

        public static short WywołajWojnę(List<Gracz> gracze, int poziomZagnieżdżenia)
        {

            //
            //W przypadku remisu, gracze kładą po jednej karcie zakrytej. A następnie odkryte które są porównywane.
            //

            int iloscpowtorzen = 0;
            List<Gracz> wojnagraczy = new List<Gracz>();
            Gracz AktualnieWygrywającyGracz = new Gracz();

            Console.WriteLine("WOJNA!");

            foreach (Gracz gracz in gracze)
            {
                if (gracz.Cards.Count < 3 * poziomZagnieżdżenia)
                {
                    Console.WriteLine("Gracz: " + gracz.ID + " odpada z gry.");
                    graczedousuniecia.Add(gracz);
                }

            }                                     //Gracze którzy nie mają odpowiedniej ilości kart przegrywają
            foreach (Gracz usun in graczedousuniecia)
            {
                gracze.Remove(usun);
            }

            if (gracze.Count == 0)
            {
                Console.WriteLine("WOJNA NIE UDANA!\n");
                foreach (Gracz usun in graczedousuniecia)
                {
                    foreach (Card karta in usun.Cards)
                    {
                        stos.Add(karta);
                    }
                }
                return 1;
            }                                               //W przypadku gdy wszyscy gracze mają za mało kart. Wojna się kończy a karty odkładane sa na stos.

            foreach (Gracz player in gracze)
            {
                Console.WriteLine("Gracz {0} z {1} i ma {2} kart", player.ID, player.Cards[3 * poziomZagnieżdżenia - 1].Name, player.Cards.Count);
            }                                       //Gracze którzy biora udział w grze ukazują karty
                
            Card NajWyższaKarta = funkcje.SprawdźNajwyższąWartość(gracze, poziomZagnieżdżenia * 3 - 1, out iloscpowtorzen); //Sprawdzanie najwyższej karty w zestawieniu

            Console.WriteLine("Najwyższa karta w tej rundzie to: " + NajWyższaKarta.Name + " i powtórzyła się {0} razy", iloscpowtorzen);

            if (iloscpowtorzen != 1)                                                    
            {
                foreach (Gracz player in gracze)
                {
                    if (player.Cards[3 * poziomZagnieżdżenia - 1].Value == NajWyższaKarta.Value)
                    {
                        wojnagraczy.Add(player);
                    }
                }
                funkcje.WywołajWojnę(wojnagraczy, poziomZagnieżdżenia + 1);
                wojnagraczy.Clear();
            }                                               //W przypadku remisu kolejne wywołanie wojny po przez rekurencję

            foreach (Gracz gracz in gracze)
            {
                if(gracz.Cards[3 * poziomZagnieżdżenia - 1] == NajWyższaKarta)
                    AktualnieWygrywającyGracz = gracz;
            }                               //Zebranie informacji o wygrywającym

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
            }                           //Zebranie kart od przegrany i dodanie ich wygranemu

            foreach (Card karta in stos)                                        
            {
                AktualnieWygrywającyGracz.Cards.Add(karta);
            }                           //Dodanie kart z stosu graczy który odpadli z gry
            stos.Clear();

            Console.WriteLine("Wygrywa gracz: " + AktualnieWygrywającyGracz.ID + " z kartą : " + NajWyższaKarta.Name + " i wielkością talii " + AktualnieWygrywającyGracz.Cards.Count);

            Console.WriteLine("KONIEC WOJNY!");
            return 0;
        }

        public static Card SprawdźNajwyższąWartość(List<Gracz> gracze, int NaPozycji, out int Powtórzenia)
        {

            //Znalezienie najwyższej karty w talii graczy

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

            Console.WriteLine("Witam w grze karcianej o nazwie \"WOJNA!\"\nGrasz jako gracz nr 1!\n");

            bool CzyWybranoTryb = false;
            int trybgry;

            do
            {
                Console.WriteLine("Wybierz liczbę graczy:");

                bool CzyPodanoLiczbę = Int32.TryParse(Console.ReadLine(), out trybgry);

                if (CzyPodanoLiczbę && trybgry > 1)
                {
                    Console.WriteLine("Liczba graczy: " + trybgry);                    
                    CzyWybranoTryb = true;
                }
                else
                {
                    Console.WriteLine("Niepoprawna wartość!");
                }
                Console.WriteLine("");
            }
            while (!CzyWybranoTryb);

            bool CzyPodanoRundy = false;

            do
            {
                Console.WriteLine("Podaj limit rund");
                bool CzyPodanoLiczbę = Int32.TryParse(Console.ReadLine(), out limitrund);

                if (CzyPodanoLiczbę && limitrund != 0)
                {
                    if (limitrund < 0)
                    {
                        limitrund = -limitrund;
                    }
                    CzyPodanoRundy = true;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa wartość!");
                }
                Console.WriteLine("");
            }
            while (!CzyPodanoRundy);

            return trybgry;
        }

        public static int DecyzjaGracza()
        {
            int odpowiedz;
            bool wyjdz = true;
            Console.WriteLine("Co chcesz zrobić?");
            Console.WriteLine("1 - Kontynuuj grę\n2 - Zakończ grę\n3 - Zobacz karty na ręce\n4 - Przetasuj karty na ręce");
            do
            {
                bool czyliczba = int.TryParse(Console.ReadLine(), out odpowiedz);
                switch (odpowiedz)
                {
                    case 1:
                        wyjdz = false;
                        break;
                    case 2:
                        wyjdz = false;
                        break;
                    case 3:
                        wyjdz = false;
                        break;
                    case 4:
                        wyjdz = false;
                        break;
                    default:
                        Console.WriteLine("Podaj prawidłową wartość!");
                        break;
                }
            }
            while (wyjdz);
            return odpowiedz;
        }
    }
}
