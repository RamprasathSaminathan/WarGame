using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WarGame
{
    public class Card
    {
        public Card(Rank value, SuitType suit)
        {
            if (!Enum.IsDefined(typeof(SuitType), suit))
                throw new ArgumentOutOfRangeException("suit");
            if (!Enum.IsDefined(typeof(Rank), value))
                throw new ArgumentOutOfRangeException("rank");

            Suit = suit;
            FaceValue = value;
        }

        public enum SuitType
        {
            Clubs,
            Spades,
            Hearts,
            Diamonds
        }

        public enum Rank
        {
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
        }

        public Rank FaceValue { get; set; }
        
        public SuitType Suit { get; set; }


    }
}