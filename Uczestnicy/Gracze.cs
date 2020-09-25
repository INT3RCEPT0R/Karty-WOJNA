using System;
using TaliaKart;
using System.Collections.Generic;

namespace Uczestnicy
{
    public class Gracz
    {
        public int ID { get; }
        static int Pop = 0;
        public List<Card> Cards = new List<Card>();
        public Gracz()
        {
            Pop += 1;
            this.ID = Pop;
        }
        public void ShowHand()
        {
            Console.WriteLine("Gracz numer: " + this.ID);
            foreach (Card card in this.Cards)
            {
                Console.WriteLine(card.Name);
            }
            Console.WriteLine();
        }
        public override string ToString()
        {
            return "Gracz " + this.ID.ToString();
        }

    }
}
