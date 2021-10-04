using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class Deck
    {
        public Queue playDeck = new();
        private ObservableCollection<Card> allCards = new();
        public ObservableCollection<Card> AllCards { get => allCards; set => allCards = value; }

        public Deck()
        {
            // Clubs
            AllCards.Add(new Card("Ace♣️", 11, @$"{Environment.CurrentDirectory}/Images/Clubs/Ace.png"));
            AllCards.Add(new Card("2♣️", 2, @$"{Environment.CurrentDirectory}/Images/Clubs/2.png"));
            AllCards.Add(new Card("3♣️", 3, @$"{Environment.CurrentDirectory}/Images/Clubs/3.png"));
            AllCards.Add(new Card("4♣️", 4, @$"{Environment.CurrentDirectory}/Images/Clubs/4.png"));
            AllCards.Add(new Card("5♣️", 5, @$"{Environment.CurrentDirectory}/Images/Clubs/5.png"));
            AllCards.Add(new Card("6♣️", 6, @$"{Environment.CurrentDirectory}/Images/Clubs/6.png"));
            AllCards.Add(new Card("7♣️", 7, @$"{Environment.CurrentDirectory}/Images/Clubs/7.png"));
            AllCards.Add(new Card("8♣️", 8, @$"{Environment.CurrentDirectory}/Images/Clubs/8.png"));
            AllCards.Add(new Card("9♣️", 9, @$"{Environment.CurrentDirectory}/Images/Clubs/9.png"));
            AllCards.Add(new Card("10♣️", 10, @$"{Environment.CurrentDirectory}/Images/Clubs/10.png"));
            AllCards.Add(new Card("Jack♣️", 10, @$"{Environment.CurrentDirectory}/Images/Clubs/Jack.png"));
            AllCards.Add(new Card("King♣️", 10, @$"{Environment.CurrentDirectory}/Images/Clubs/King.png"));
            AllCards.Add(new Card("Queen♣️", 10, @$"{Environment.CurrentDirectory}/Images/Clubs/Queen.png"));

            // Diamond
            AllCards.Add(new Card("Ace♦️", 11, @$"{Environment.CurrentDirectory}/Images/Diamond/Ace.png"));
            AllCards.Add(new Card("2♦️", 2, @$"{Environment.CurrentDirectory}/Images/Diamond/2.png"));
            AllCards.Add(new Card("3♦️", 3, @$"{Environment.CurrentDirectory}/Images/Diamond/3.png"));
            AllCards.Add(new Card("4♦️", 4, @$"{Environment.CurrentDirectory}/Images/Diamond/4.png"));
            AllCards.Add(new Card("5♦️", 5, @$"{Environment.CurrentDirectory}/Images/Diamond/5.png"));
            AllCards.Add(new Card("6♦️", 6, @$"{Environment.CurrentDirectory}/Images/Diamond/6.png"));
            AllCards.Add(new Card("7♦️", 7, @$"{Environment.CurrentDirectory}/Images/Diamond/7.png"));
            AllCards.Add(new Card("8♦️", 8, @$"{Environment.CurrentDirectory}/Images/Diamond/8.png"));
            AllCards.Add(new Card("9♦️", 9, @$"{Environment.CurrentDirectory}/Images/Diamond/9.png"));
            AllCards.Add(new Card("10♦️", 10, @$"{Environment.CurrentDirectory}/Images/Diamond/10.png"));
            AllCards.Add(new Card("Jack♦️", 10, @$"{Environment.CurrentDirectory}/Images/Diamond/Jack.png"));
            AllCards.Add(new Card("King♦️", 10, @$"{Environment.CurrentDirectory}/Images/Diamond/King.png"));
            AllCards.Add(new Card("Queen♦️", 10, @$"{Environment.CurrentDirectory}/Images/Diamond/Queen.png"));

            // Hearts
            AllCards.Add(new Card("Ace♥️", 11, @$"{Environment.CurrentDirectory}/Images/Heart/Ace.png"));
            AllCards.Add(new Card("2♥️", 2, @$"{Environment.CurrentDirectory}/Images/Heart/2.png"));
            AllCards.Add(new Card("3♥️", 3, @$"{Environment.CurrentDirectory}/Images/Heart/3.png"));
            AllCards.Add(new Card("4♥️", 4, @$"{Environment.CurrentDirectory}/Images/Heart/4.png"));
            AllCards.Add(new Card("5♥️", 5, @$"{Environment.CurrentDirectory}/Images/Heart/5.png"));
            AllCards.Add(new Card("6♥️", 6, @$"{Environment.CurrentDirectory}/Images/Heart/6.png"));
            AllCards.Add(new Card("7♥️", 7, @$"{Environment.CurrentDirectory}/Images/Heart/7.png"));
            AllCards.Add(new Card("8♥️", 8, @$"{Environment.CurrentDirectory}/Images/Heart/8.png"));
            AllCards.Add(new Card("9♥️", 9, @$"{Environment.CurrentDirectory}/Images/Heart/9.png"));
            AllCards.Add(new Card("10♥️", 10, @$"{Environment.CurrentDirectory}/Images/Heart/10.png"));
            AllCards.Add(new Card("Jack♥️", 10, @$"{Environment.CurrentDirectory}/Images/Heart/Jack.png"));
            AllCards.Add(new Card("King♥️", 10, @$"{Environment.CurrentDirectory}/Images/Heart/King.png"));
            AllCards.Add(new Card("Queen♥️", 10, @$"{Environment.CurrentDirectory}/Images/Heart/Queen.png"));
                    
            // Spades
            AllCards.Add(new Card("Ace♠️", 11, @$"{Environment.CurrentDirectory}/Images/Spade/Ace.png"));
            AllCards.Add(new Card("2♠️", 2, @$"{Environment.CurrentDirectory}/Images/Spade/2.png"));
            AllCards.Add(new Card("3♠️", 3, @$"{Environment.CurrentDirectory}/Images/Spade/3.png"));
            AllCards.Add(new Card("4♠️", 4, @$"{Environment.CurrentDirectory}/Images/Spade/4.png"));
            AllCards.Add(new Card("5♠️", 5, @$"{Environment.CurrentDirectory}/Images/Spade/5.png"));
            AllCards.Add(new Card("6♠️", 6, @$"{Environment.CurrentDirectory}/Images/Spade/6.png"));
            AllCards.Add(new Card("7♠️", 7, @$"{Environment.CurrentDirectory}/Images/Spade/7.png"));
            AllCards.Add(new Card("8♠️", 8, @$"{Environment.CurrentDirectory}/Images/Spade/8.png"));
            AllCards.Add(new Card("9♠️", 9, @$"{Environment.CurrentDirectory}/Images/Spade/9.png"));
            AllCards.Add(new Card("10♠️", 10, @$"{Environment.CurrentDirectory}/Images/Spade/10.png"));
            AllCards.Add(new Card("Jack♠️", 10, @$"{Environment.CurrentDirectory}/Images/Spade/Jack.png"));
            AllCards.Add(new Card("King♠️", 10, @$"{Environment.CurrentDirectory}/Images/Spade/King.png"));
            AllCards.Add(new Card("Queen♠️", 10, @$"{Environment.CurrentDirectory}/Images/Spade/Queen.png"));

            this.MakeDeck();
        }


        public void MakeDeck()
        {
            this.playDeck = new Queue();
            Random random = new();
            Card[] tempArr = AllCards.OrderBy(x => random.Next()).ToArray();
            foreach (Card item in tempArr)
            {
                this.playDeck.Enqueue(item);
            }
        }

        public Card PickCard()
        {
            return (Card)this.playDeck.Dequeue();
        }
    }
}
