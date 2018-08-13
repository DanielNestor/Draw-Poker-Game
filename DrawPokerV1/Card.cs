using System;

namespace DrawPokerV1
{
    public class Card
    {
        int Value = 0;
        int Suit = 0;

        public void setValue(int x)
        {
            Value = x;
        }

        public void setSuit(int y)
        {
            Suit = y;
        }



        public int getValue()
        {
            return Value;
        }

        public int getSuit()
        {
            return Suit;
        }
    }
}