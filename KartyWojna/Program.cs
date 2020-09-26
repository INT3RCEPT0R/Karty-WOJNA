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
            for (int i = 0; i < trybgry; i++)                           //Inicjalizacja odpowiedniej ilosci graczy
            {
                Players.Add(new Gracz());
            }

            Deck Talia = new Deck();                                    //Utworzenie talii 52 kart 

            Talia.Cards = Card.MixCards(Talia.Cards);                   //Wymieszanie kart

            int j = 0;
            foreach (Card card in Talia.Cards)                          //Rozdanie wszystkich kart pomiędzy graczy
            {
                if (j == trybgry) j = 0;
                Players[j].Cards.Add(card);
                j++;
            }

            Players = funkcje.Play(limitrund, Players);                    //Rozpoczęcie rozgrywki

            int wielkosctalii = 0;                                       //Podsumowanie skończonej gry
            int wygrany = 0;
            foreach (Gracz player in Players)
            {
                if (player.Cards.Count > wielkosctalii)
                {
                    wielkosctalii = player.Cards.Count;
                    wygrany = player.ID;
                }
            }
            Console.WriteLine("\nWygrał gracz: " + wygrany + " z ilością kart: " + wielkosctalii);  //Wynik końcowy


            Console.ReadKey();
        }
    }

}
