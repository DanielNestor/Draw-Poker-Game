using System;
using System.Collections;
using System.Collections.Generic;

namespace DrawPokerV1
{
    public class Deck
    {
        Random rnd = new Random();
        Stack<Card> deck = new Stack<Card>();
        Card[] initialDeck = new Card[53];
        public Card drawCard()
        {
            int x = 0;
            while (deck.Peek() == null)
            {
                deck.Pop();
            }
            return deck.Pop();
        }
        public void populateDeck() {

            //put the cards into the list in order
            int suitValue = 1;
            int listIndex = 0;
            for (int x = 0; x < 13; x++) {
                Card tempCard = new Card();

               
                 tempCard.setValue(x);
                
                
                tempCard.setSuit(suitValue);

                //add to the list
                initialDeck[listIndex] = tempCard;

                if (suitValue != 4 && x == 12) {
                    x = -1;
                    suitValue++;
                }
                listIndex++;
            }

            //this loop shuffles the deck
            for (int x = 0; x < 10000; x++) {
                Random rnd1 = new Random();
                int firstIndex = rnd.Next(0,52);
                int secondIndex = rnd.Next(0,52);
                //now perform the actual swap
                Card tempCard = initialDeck[firstIndex];
                initialDeck[firstIndex] = initialDeck[secondIndex];
                initialDeck[secondIndex] = tempCard;

            }
            //finally push the elements onto the deck
            for (int x = 0; x <= 51; x++) {
                deck.Push(initialDeck[x]);
            }
        }
    }
}