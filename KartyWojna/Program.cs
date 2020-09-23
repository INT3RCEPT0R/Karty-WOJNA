using System;
using System.Collections.Generic;
using TaliaKart;
using Uczestnicy;
using Rozgrywka;

namespace KartyWojna
{
    static class Program
    {
        static void Main()
        {
            int trybgry = funkcje.InicjalizacjaGry(out int limitrund);  //Pobranie zmiennych związanych z gra od użytkownika

            List<Gracz> Players = new List<Gracz>();                    //Inizjalizacja wszystkich graczy
            for (int i = 0; i < trybgry; i++)
            {
                Players.Add(new Gracz());
            }

            Deck Talia = new Deck();                                    //Utworzenie talii 52 kart 

            Talia.MixDeck();                                            //Wymieszanie kart

            int j = 0;
            foreach (Card card in Talia.Cards)                          //Rozdanie wszystkich kart pomiędzy graczy
            {
                if (j == trybgry) j = 0;
                Players[j].Cards.Add(card);
                j++;
            }
            Talia.Cards.Clear();                                         //Wyczyszczenie talii po rozdaniu kart

                                                                         //Inicjzalizowanie zmiennych do rozgrywki
            int runda = 0;
            bool wygrana = false;
            List<Gracz> wojnagraczy = new List<Gracz>();            

            do
            {
                runda++;

                foreach (Gracz player in Players)                        //Usunięcie graczy którzy odpadli
                {
                    if(player.Cards.Count == 0)
                    {
                        Console.WriteLine("Gracz: " + player.ID + " odpada z gry.");
                        funkcje.graczedousuniecia.Add(player);
                    }
                }
                foreach (Gracz usun in funkcje.graczedousuniecia)
                {
                    Players.Remove(usun);
                }


                if (runda > limitrund)                                  //Sprawdzenie warunków końca gry
                {
                    Console.WriteLine("Koniec tur!");
                    wygrana = true;
                }else if (Players.Count == 1)
                {
                    Console.WriteLine("Pozostał ostatni gracz!");
                    wygrana = true;
                }
                funkcje.graczedousuniecia.Clear();
                if (wygrana == true)
                {
                    break;
                }

                int iloscpowtorzen = 0;

                Console.WriteLine("Runda numer: " + runda);
                foreach(Gracz player in Players)
                {
                    Console.WriteLine("Gracz {0} z {1} i ma {2} kart", player.ID,player.Cards[0].Name,player.Cards.Count);
                }

                Card NajWyższaKarta = funkcje.SprawdźNajwyższąWartość(Players,0, out iloscpowtorzen);

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
                    funkcje.WywołajWojnę(wojnagraczy,1);
                    wojnagraczy.Clear();
                }
                else
                {
                    foreach (Gracz player in Players)
                    {
                        if (player.Cards[0].Value == NajWyższaKarta.Value)
                        {
                            funkcje.AktualnieWygrywającyGracz = player;
                        }
                    }
                    
                    foreach (Gracz player in Players)
                    {
                        if (player != funkcje.AktualnieWygrywającyGracz)
                        {
                            if (player.Cards.Count != 0)
                            {
                                funkcje.AktualnieWygrywającyGracz.Cards.Add(player.Cards[0]);
                                player.Cards.RemoveAt(0);
                            }
                        }
                        else
                        {
                            player.Cards.Add(player.Cards[0]);
                            player.Cards.RemoveAt(0);
                        }

                    }
                    Console.WriteLine("Wygrywa gracz: " + funkcje.AktualnieWygrywającyGracz.ID + " i ma " + funkcje.AktualnieWygrywającyGracz.Cards.Count + " kart!\n");
                }

                
            } while (true);

            int wielkosctalii = 0;
            int wygrany = 0;
            foreach (Gracz player in Players)
            {
                if (player.Cards.Count > wielkosctalii)
                {
                    wielkosctalii = player.Cards.Count;
                    wygrany = player.ID;
                }
            }
            Console.WriteLine("\nWygrał gracz: " + wygrany + " z ilością kart: " + wielkosctalii);


            Console.ReadKey();
        }
    }

}
