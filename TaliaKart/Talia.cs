using System;
using System.Collections.Generic;

/// <summary>
/// from https://stackoverflow.com/questions/53383468/creating-a-deck-of-cards-in-c-sharp
/// </summary>

namespace TaliaKart
{
    public class Card
    {
        public enum Suites
        {
            Hearts = 0,
            Diamonds,
            Clubs,
            Spades
        }

        public int Value
        {
            get;
        }

        public Suites Suite
        {
            get;
        }

        //Used to get full name, also useful 
        //if you want to just get the named value
        string NamedValue
        {
            get
            {
                string name = string.Empty;
                switch (Value)
                {
                    case (14):
                        name = "Ace";
                        break;
                    case (13):
                        name = "King";
                        break;
                    case (12):
                        name = "Queen";
                        break;
                    case (11):
                        name = "Jack";
                        break;
                    default:
                        name = Value.ToString();
                        break;
                }

                return name;
            }
        }

        public string Name
        {
            get
            {
                return NamedValue + " of " + Suite.ToString();
            }
        }
        public Card()
        {
        }
        public Card(int Value, Suites Suite)
        {
            this.Value = Value;
            this.Suite = Suite;
        }

        private static Random rng = new Random();
        public static List<Card> MixCards(List<Card> karty)
        {
            List<Card> list = karty;
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }

    public class Deck
    {
        int ID;
        static int Pop = 0;
        public List<Card> Cards = new List<Card>();
        void FillDeck()
        {
            //Can use a single loop utilising the mod operator % and Math.Floor
            //Using divition based on 13 cards in a suited
            for (int i = 0; i < 52; i++)
            {
                Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i / 13));
                //Add 2 to value as a cards start a 2
                int val = i % 13 + 2;
                Cards.Add(new Card(val, suite));
            }
        }      

        public void PrintDeck()
        {
            Console.WriteLine("Talia numer: " + this.ID);
            foreach (Card card in this.Cards)
            {
                Console.WriteLine(card.Name);
            }
            Console.WriteLine();
        }
        public Deck()
        {
            Pop += 1;
            this.ID = Pop;
            this.FillDeck();
        }
    }

}
