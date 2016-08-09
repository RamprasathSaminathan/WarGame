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
    static class Deck
    {
        static public List<Card> cards;

        static public List<Card> player1;

        static public List<Card> player2;

        static public List<Card> warCards;

        public static Card NextOf<Card>(this IList<Card> list, Card item)
        {
            return list[(list.IndexOf(item) + 1) == list.Count ? 0 : (list.IndexOf(item) + 1)];
        }

        static public void CreateDeck()
        {
            cards = new List<Card>();
            player1 = new List<Card>();
            player2 = new List<Card>();
            warCards = new List<Card>();

            for (int suit = (int)Card.SuitType.Clubs; suit <= (int)Card.SuitType.Diamonds; suit++)
            {
                for (int rank = (int)Card.Rank.Ace; rank <= (int)Card.Rank.King; rank++)
                {
                    Card card = new Card((Card.Rank)rank, (Card.SuitType)suit);
                    cards.Add(card);
                }
            }
        }

        //check out Fisher-Yates shuffle
        //https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        static public void Shuffle(int numTimes)
        {
            int cardCount = cards.Count();
            Random rand = new Random();

            for(int time = 0; time < numTimes; time++)
            {
                for(var index = 0; index < cardCount; index++)
                {
                    int indexSwapPosition = rand.Next(cardCount);
                    Card temp = cards[indexSwapPosition];
                    cards[indexSwapPosition] = cards[index];
                    cards[index] = temp;
                }
            }

            //split the deck into two piles for player1 and player 2
            SplitDeck();
        }

        static private void SplitDeck()
        {
            for (int i = 0; i < 26; i++)
            {
                player1.Add(cards[i]);
            }

            for(int i = 26; i < cards.Count; i++)
            {
                player2.Add(cards[i]);
            }
        }
    }
}