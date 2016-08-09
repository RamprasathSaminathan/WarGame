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
    [Activity(Label = "GamePlayActivity")]
    public class GamePlayActivity : Activity
    {
        public int player1Score;
        public int player2Score;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GamePlay);
           
            //create a new deck of cards
            Deck.CreateDeck();

            //shuffle that deck of cards
            Deck.Shuffle(3);

            

            Button button = FindViewById<Button>(Resource.Id.dealHand);
            button.Click += delegate
            {
                //deal out the first two cards to be displayed
                ImageView card1 = FindViewById<ImageView>(Resource.Id.card1);
                ImageView card2 = FindViewById<ImageView>(Resource.Id.card2);

                if (Deck.player1.Count < 52 || Deck.player2.Count < 52)
                {
                    //update the score at the beginning of each hand
                    TextView score1 = FindViewById<TextView>(Resource.Id.player1Score);
                    score1.Text = "Player 1: " + player1Score;
                    TextView score2 = FindViewById<TextView>(Resource.Id.player2Score);
                    score2.Text = "Player 2: " + player2Score;

                    //grab the top two cards from the deck
                    Card selectedCard1 = Deck.player1.NextOf(null);
                    Card selectedCard2 = Deck.player2.NextOf(null);

                    card1.SetImageResource(CardID(selectedCard1));
                    card2.SetImageResource(CardID(selectedCard2));

                    //determine which card is higher?
                    if(selectedCard1.FaceValue > selectedCard2.FaceValue)
                    {
                        //we know selectedCard1 has a higher value
                        //player 1 wins the hand
                        //notify the player
                        Toast.MakeText(this, "Winner: Player 1", ToastLength.Short).Show();

                        //assign the cards to the appropriate deck
                       
                        Deck.player1.Add(selectedCard2);
                        Deck.player2.Remove(selectedCard2);

                        Deck.player1.Remove(selectedCard1);
                        Deck.player1.Add(selectedCard1);

                        //award 1 point for every hand won
                        player1Score += 1;
                        
                    }
                    else if( selectedCard2.FaceValue > selectedCard1.FaceValue)
                    {
                        //we know selectedCard2 has a higher value
                        //player 2 wins the hand
                        //notify the player
                        Toast.MakeText(this, "Winner: Player 2", ToastLength.Short).Show();

                        //assign the cards to the appropriate deck
                        Deck.player2.Add(selectedCard1);
                        Deck.player1.Remove(selectedCard1);

                        Deck.player2.Remove(selectedCard2);
                        Deck.player2.Add(selectedCard2);

                        //award 1 point for every hand won
                        player2Score += 1;
                    }
                    else if (selectedCard1.FaceValue == selectedCard2.FaceValue)
                    {
                        //we know the two cards have the same rank
                        //it's war!
                        WarLogic(selectedCard1, selectedCard2);
                    }

                }
                else //someone has 52 cards The game is over
                //we need to declare a winner
                {
                    if (Deck.player1.Count >= 52)
                    {
                        //player 1 wins!
                        button.Enabled = false;

                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Player 1 Wins!");
                        alert.SetPositiveButton("OK!", (senderAlert, args) =>
                        {

                        });
                    }
                    else
                    {
                        //player 2 wins!
                        button.Enabled = false;

                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Player 2 Wins!");
                        alert.SetPositiveButton("OK!", (senderAlert, args) =>
                        {

                        });
                    }
                }
            

            };
            
        }

        public void WarLogic(Card selectedCard1, Card selectedCard2)
        {
            ImageView card1 = FindViewById<ImageView>(Resource.Id.card1);
            ImageView card2 = FindViewById<ImageView>(Resource.Id.card2);
            Button button = FindViewById<Button>(Resource.Id.dealHand);

            //we know the two cards have the same rank
            //it's war!
            button.Enabled = false;

            //turn up two cards (one for each player)
            //the winner gets 6 cards (points) from the first set of cards, the "face down" set
            //of cards, and the face up set for war.
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("War!");
            alert.SetPositiveButton("Let's Play!", (senderAlert, args) => {
                //this is where we put the war logic

                //take both cards that triggered the war and put them in the war list
                Deck.warCards.Add(selectedCard1);
                Deck.warCards.Add(selectedCard2);

                //remove the trigger cards from the player's hands
                Deck.player1.Remove(selectedCard1);
                Deck.player2.Remove(selectedCard2);

                //pull out one card from each player's deck to put into the war list as the 
                //facedown cards
                //grab the top two cards from the deck

                selectedCard1 = Deck.player1.NextOf(null);
                selectedCard2 = Deck.player2.NextOf(null);
                Deck.warCards.Add(selectedCard1);
                Deck.warCards.Add(selectedCard2);
                //remove the  cards from the player's hands
                Deck.player1.Remove(selectedCard1);
                Deck.player2.Remove(selectedCard2);

                //one card from the top of each player's deck to play war
                //these cards will be displayed
                selectedCard1 = Deck.player1.NextOf(null);
                selectedCard2 = Deck.player2.NextOf(null);

                card1.SetImageResource(CardID(selectedCard1));
                card2.SetImageResource(CardID(selectedCard2));

                //determine which card is higher?
                if (selectedCard1.FaceValue > selectedCard2.FaceValue)
                {
                    //player 1 wins the war
                    Toast.MakeText(this, "Player 1 Wins the War!", ToastLength.Short).Show();
                    Deck.player1.Add(selectedCard2);
                    Deck.player2.Remove(selectedCard2);

                    foreach (Card c in Deck.warCards)
                    {
                        //add the card into the player's hand
                        Deck.player1.Add(c);
                    }

                    //clear the war list
                    Deck.warCards.Clear();

                    //award 6 points
                    player1Score += 6;

                    //turn the deal button back on 
                    button.Enabled = true;


                }
                else if (selectedCard2.FaceValue > selectedCard1.FaceValue)
                {
                    //player 2 wins the war
                    Toast.MakeText(this, "Player 2 Wins the War!", ToastLength.Short).Show();
                    Deck.player2.Add(selectedCard1);
                    Deck.player1.Remove(selectedCard1);

                    foreach (Card c in Deck.warCards)
                    {
                        Deck.player2.Add(c);
                    }

                    //clear the war list
                    Deck.warCards.Clear();

                    //award 6 points
                    player2Score += 6;

                    //turn the deal button back on 
                    button.Enabled = true;
                }
                else if (selectedCard1.FaceValue == selectedCard2.FaceValue)
                {
                    //the cards are the same and we play war again
                    WarLogic(selectedCard1, selectedCard2);
                }
            });

            //run the alert
            alert.Show();
        }

        public int CardID(Card card)
        {
            switch (card.FaceValue)
            {
                #region Aces
                case Card.Rank.Ace:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.ClubsAce;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.DiamondsAce;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.HeartsAce;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.SpadesAce;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                       
                    }
                #endregion

                #region Two
                case Card.Rank.Two:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs2;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds2;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts2;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades2;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Three
                case Card.Rank.Three:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs3;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds3;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts3;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades3;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Four
                case Card.Rank.Four:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs4;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds4;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts4;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades4;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Five
                case Card.Rank.Five:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs5;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds5;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts5;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades5;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Six
                case Card.Rank.Six:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs6;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds6;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts6;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades6;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Seven
                case Card.Rank.Seven:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs7;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds7;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts7;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades7;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }

                #endregion

                #region Eight
                case Card.Rank.Eight:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs8;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds8;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts8;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades8;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }

                #endregion

                #region Nine
                case Card.Rank.Nine:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs9;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds9;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts9;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades9;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Ten
                case Card.Rank.Ten:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.Clubs10;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.Diamonds10;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.Hearts10;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.Spades10;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Jack
                case Card.Rank.Jack:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.ClubsJ;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.DiamondsJ;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.HeartsJ;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.SpadesJ;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region King
                case Card.Rank.King:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.ClubsK;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.DiamondsK;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.HeartsK;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.SpadesK;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                #region Queen
                case Card.Rank.Queen:
                    {
                        switch (card.Suit)
                        {
                            case Card.SuitType.Clubs:
                                {
                                    return Resource.Drawable.ClubsQ;
                                }
                            case Card.SuitType.Diamonds:
                                {
                                    return Resource.Drawable.DiamondsQ;
                                }
                            case Card.SuitType.Hearts:
                                {
                                    return Resource.Drawable.HeartsQ;
                                }
                            case Card.SuitType.Spades:
                                {
                                    return Resource.Drawable.SpadesQ;
                                }
                            default:
                                {
                                    return Resource.Drawable.Joker;
                                }
                        }
                        break;
                    }
                #endregion

                default:
                    {
                        return Resource.Drawable.Joker;
                    }
            }
        }
    }
}