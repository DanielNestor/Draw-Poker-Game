using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawPokerV1
{
    public partial class Form1 : Form
    {
        private object panel1;
        //list of public booleans
        Boolean card1Held = false;
        Boolean card2Held = false;
        Boolean card3Held = false;
        Boolean card4Held = false;
        Boolean card5Held = false;

        //store card values up here
        Card c1 = null;
        Card c2 = null;
        Card c3 = null;
        Card c4 = null;
        Card c5 = null;

        //Store the score here
        int score = 0;

        bool newGameBool = true;

        //declare the new deck
        Deck d1 = new Deck();

        int drawCount = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew)) 
                using (StreamWriter sw = new StreamWriter(s)) {
                    sw.Write(score);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(newGameBool == true)
            {
                score = 500;
                newGameBool = false;

            }
            //first draw the user's Initial Hand
            if (drawCount == 1)
            {
                //re-enable the button if it was disabled
                button1.Enabled = true;
                button6.Visible = false;
                //clear the game over label if it is there
                GameOverLbl.Text = "";
                button1.Text = "Draw Again";
                //then actually play the game
                DrawHand();
                DisplayGameStats();
                StartGame();
                drawCount++;
                return;
            }else if (drawCount == 2) {
                button1.Enabled = false;
                RedrawHand();
                CheckHand();
                GameOverLbl.Text = "Game Over";
                button6.Visible = true;
                resetHoldVariables();
                clearCardVariables();
                drawCount--;
                return;
            }
            //change the buttons to say draw
            button1.Text = "Draw";
        }

        //this function checks the hand
        private void CheckHand()
        {
            if (isStraightFlush()) {
                score = score + 250;
                MessageBox.Show("You Have a Straight Flush");
                return;
            }
            if (isFourOfAKind()) {
                score = score + 100;
                MessageBox.Show("You Have 4 of a Kind");
                return;
            }
            if (isFullHouse()) {
                score = score + 60;
                MessageBox.Show("You Have a full house");
                return;
            }
            if (isFlush()) {
                score = score + 40;
                MessageBox.Show("You Have a Flush");
                return;
            }
            if (isStraight()) {
                score = score + 25;
                MessageBox.Show("You Have a Straight");
                return;
            }
            if (is3ofKind()) {
                score = score + 10;
                MessageBox.Show("You Have 3 of a kind");
                return;
            }
            if (is2Pair()) {
                score = score + 5;
                MessageBox.Show("You Have Two Pair");
                return;
            }
            if (isJacksOrBetter()) {
                score = score + 3;
                MessageBox.Show("You Have A High Pair");
                return;
            }
            if (isPair()) {
                score = score + 2;
                MessageBox.Show("You Have A Pair");
                return;
            }
            //remove 2 points if the user gets nothing
            score = score - 10;
        }













        #region Hand Functions 

        private bool isStraightFlush()
        {
            if (isStraight() && isFlush()) {
                return true;
            }
            return false;
        }

        private bool isFourOfAKind()
        {
            int[] tempArray = new int[5];
            tempArray[0] = c1.getValue();
            tempArray[1] = c2.getValue();
            tempArray[2] = c3.getValue();
            tempArray[3] = c4.getValue();
            tempArray[4] = c5.getValue();

            int cardTypesCount = 0;
            int cardValue = 0;
            bool fourofakindfound = false;
            while (cardValue < 15)
            {
                cardTypesCount = 0;
                for (int x = 0; x < tempArray.Length; x++)
                {
                    if (tempArray[x] == cardValue)
                    {
                        cardTypesCount++;
                        
                    }
                    
                }
                if (cardTypesCount >= 4)
                {
                    fourofakindfound = true;
                }

                cardValue++;
            }

            return fourofakindfound;
        }
        private bool isFullHouse()
        {
            int[] tempArray = new int[5];
            tempArray[0] = c1.getValue();
            tempArray[1] = c2.getValue();
            tempArray[2] = c3.getValue();
            tempArray[3] = c4.getValue();
            tempArray[4] = c5.getValue();

            int cardTypesCount = 0;
            int cardValue = 0;
            while (cardValue < 15)
            {
                for (int x = 0; x < tempArray.Length; x++)
                {
                    if (tempArray[x] == cardValue) {
                        cardTypesCount++;
                        break;
                    }
                }
                cardValue++;
            }
            if (cardTypesCount == 2) {
                return true;
            }

            return false;
           
        }
        private bool is2Pair()
        {
            //copy the values into a temp array
            int[] tempArray = new int[5];
            tempArray[0] = c1.getValue();
            tempArray[1] = c2.getValue();
            tempArray[2] = c3.getValue();
            tempArray[3] = c4.getValue();
            tempArray[4] = c5.getValue();

            int CardValue = 0;
            int occuranceCount = 0;
            int pairCount = 0;

            //first loop goes over the card values
            while (CardValue < 15) {

                //now loop over the array
                for (int x = 0; x < tempArray.Length; x++)
                {
                    if (tempArray[x] == CardValue) {
                        occuranceCount++;
                    }

                }
                if (occuranceCount >= 2) {
                    pairCount++;
                }

                //reset the occurance count
                occuranceCount = 0;
                    CardValue++;
            }

            if(pairCount == 2)
            {
                return true;
            }
            return false;
        }

        private bool isFlush()
        {
            if (compareCardSuit(c1,c2) && compareCardSuit(c2,c3) && compareCardSuit(c3,c4) && compareCardSuit(c4,c5)) {
                return true;
            }

            return false;
        }
        //check to see if is 3 of a kind
        private bool is3ofKind()
        {
            if (compareCardValue(c1, c2, c3) || compareCardValue(c1, c2, c4) || compareCardValue(c1, c2, c5) || compareCardValue(c1, c3, c4) || compareCardValue(c1, c3, c5)
                || compareCardValue(c1, c4, c5) || compareCardValue(c2, c3, c4) || compareCardValue(c2, c3, c5) || compareCardValue(c2, c4, c5) || compareCardValue(c3, c4, c5)) {
                return true;
            }
            return false;
        }

        //check to see if it is a pair
        private bool isPair()
        {
            int x = 0;
            if (compareCardValue(c1,c2) || compareCardValue(c1, c3) || compareCardValue(c1, c4) || compareCardValue(c1, c5) || compareCardValue(c2, c3)
                || compareCardValue(c2, c4) || compareCardValue(c2, c5) || compareCardValue(c3, c4) || compareCardValue(c3, c5) || compareCardValue(c4, c5)) {
                return true;
            }
            return false;
        }

        private bool isStraight()
        {
            int[] tempArray = new int[5];
            tempArray[0] = c1.getValue();
            tempArray[1] = c2.getValue();
            tempArray[2] = c3.getValue();
            tempArray[3] = c4.getValue();
            tempArray[4] = c5.getValue();
            Boolean isStraight = true;

            //replace any aces in the array first
            for (int x = 0; x < tempArray.Length; x++) {
                if (tempArray[x] == 0) {
                    tempArray[x] = 13;
                }
            }

            //sort the array(with bubble sort because it is so small)
            int swapCount = 0;
            do
            {
                swapCount = 0;
                int index = 0;
                while (index < 4) {
                    if (tempArray[index] > tempArray[index + 1]) {
                        //swap the values
                        int temp = tempArray[index];
                        tempArray[index] = tempArray[index + 1];
                        tempArray[index + 1] = temp;
                        swapCount++;
                    }
                    index++;
                }
                
                index = 0;
            } while (swapCount != 0);

            //now that the list is sorted, verify that the list is good
            int cardValue = tempArray[0];
            for (int x = 1; x < tempArray.Length;x++) {
                if (tempArray[x] != cardValue + 1) {
                    isStraight = false;
                }
                cardValue = tempArray[x];
            }

            return isStraight;
        }

        //at the end of a game this function sets all
        // of the card variables values to false
        private void clearCardVariables()
        {
            c1 = null;
            c2 = null;
            c3 = null;
            c4 = null;
            c5 = null;
        }

        private bool isJacksOrBetter()
        {
            int jackCount = 0;
            int queenCount = 0;
            int kingCount = 0;
            int aceCount = 0;

            //copy the values into a temp array
            int[] tempArray = new int[5];
            tempArray[0] = c1.getValue();
            tempArray[1] = c2.getValue();
            tempArray[2] = c3.getValue();
            tempArray[3] = c4.getValue();
            tempArray[4] = c5.getValue();

            for (int x = 0; x < tempArray.Length; x++) {
                int tempInt = tempArray[x];
                if (tempInt == 10) {
                    jackCount++;
                }
                if (tempInt == 11)
                {
                    queenCount++;
                }
                if (tempInt == 12)
                {
                    kingCount++;
                }
                if (tempInt == 0)
                {
                    aceCount++;
                }


            }

            //now check to see if there is a pair
            if (kingCount == 2 || jackCount == 2 || queenCount == 2 || aceCount== 2) {
                return true;
            }

            return false;
        }
        //This function compares 2 cards to see if they are equal
        private bool compareCardValue(Card c1, Card c2) {

            if (c1.getValue() == c2.getValue()) {
                return true;
            }

            return false;
        }
        private bool compareCardValue(Card c1, Card c2,Card c3)
        {

            if (c1.getValue() == c2.getValue())
            {
                if (c2.getValue() == c3.getValue())
                {
                    return true;
                }
            }

            return false;
        }
        //This function compares 2 cards to see if they are equal
        private bool compareCardSuit(Card c1, Card c2)
        {

            if (c1.getSuit() == c2.getSuit())
            {
                return true;
            }

            return false;
        }


        #endregion
        //this function resets all of the hold variables to 
        //false
        private void resetHoldVariables()
        {
            card1Held = false;
            card2Held = false;
            card3Held = false;
            card4Held = false;
            card5Held = false;
        }

        private void RedrawHand()
        {
            
             
            

            if (card1Held == false)
            {
                c1 = d1.drawCard();
                drawCard(30, 120, c1.getValue(), c1.getSuit());
                card1Held = false;
               
            }
            if (card2Held == false)
            {
                c2 = d1.drawCard();
                drawCard(170, 120, c2.getValue(), c2.getSuit());
                card2Held = false;
                
            }
            if (card3Held == false)
            {
                c3 = d1.drawCard();
                drawCard(320, 120, c3.getValue(), c3.getSuit());
                card3Held = false;
                
            }
            if (card4Held == false)
            {
                c4 = d1.drawCard();
                drawCard(460, 120, c4.getValue(), c4.getSuit());
                card4Held = false;
                
            }
            if (card5Held == false)
            {
                c5 = d1.drawCard();
                drawCard(600, 120, c5.getValue(), c5.getSuit());
                card5Held = false;
                
            }

            SetAllButtonsToHold();
        }

        //this function sets the text on all of the card buttons to hold
        private void SetAllButtonsToHold()
        {
            HoldButton5.Text = "Hold";
            HoldButton6.Text = "Hold";
            HoldButton7.Text = "Hold";
            HoldButton8.Text = "Hold";
            button5.Text = "Hold";
        }

        private void StartGame()
        {
            displayHoldButtons();
           
        }

        private void displayHoldButtons()
        {
            HoldButton5.Visible = true;
            HoldButton6.Visible = true;
            HoldButton7.Visible = true;
            HoldButton8.Visible = true;
            button5.Visible = true;
        }

        private void DisplayGameStats()
        {
            label1.Text = "Cash: " + score;
        }

        private void DrawHand()
        {
           
            //populate the deck
            d1.populateDeck();

            c1 = d1.drawCard();
            c2 = d1.drawCard();
            c3 = d1.drawCard();
            c4 = d1.drawCard();
            c5 = d1.drawCard();

            
            //now decide which cards get replaced
            drawCard(30, 120, c1.getValue(), c1.getSuit());
            drawCard(170, 120, c2.getValue(), c2.getSuit());
            drawCard(320, 120, c3.getValue(), c3.getSuit());
            drawCard(460, 120, c4.getValue(), c4.getSuit());
            drawCard(600, 120, c5.getValue(), c5.getSuit());
           
        }

        private void drawCard(int x, int y, int value, int suit)
        {

            
            //create the graphics object
            Graphics gObject = this.CreateGraphics();
            Brush white = new SolidBrush(Color.White);
            Brush blue = new SolidBrush(Color.Blue);
            Pen whitePen = new Pen(white, 8);
            Pen bluePen = new Pen(white, 8);
            //if value and suit are 0 then draw the card's back at said location
            if (value >= 0 && suit >= 0)
            {
                gObject.FillRectangle(white, x, y, 120, 180);

                //now draw some text on the back
                Rectangle r1 = new Rectangle();
                PaintEventArgs e = new PaintEventArgs(gObject, r1);
                Font drawFont = new Font("Arial", 40);
                SolidBrush drawBrushBlack = new SolidBrush(Color.Black);
                SolidBrush drawBrushRed = new SolidBrush(Color.Red);
                PointF drawPoint = new PointF(x + 15, y + 50);

                //generate the string to draw on the card
                char suitChar = getSuitChar(suit);
                String cardContentsString = getValueChar(value) + "" + suitChar;

                //if the suit is 3 or 4, paint the contents of the card black, else paint them red
                if (suit > 2)
                {
                    e.Graphics.DrawString(cardContentsString, drawFont, drawBrushBlack, drawPoint);
                }
                else
                {
                    e.Graphics.DrawString(cardContentsString, drawFont, drawBrushRed, drawPoint);
                }

            }
        }

        //this function gets the character for the suit of the card
        private char getSuitChar(int suit)
        {
            if (suit == 1) {
                return '♦';
            }
            if (suit == 2)
            {
                return '♥';
            }
            if (suit == 3)
            {
                return '♣';
            }
            if (suit == 4)
            {
                return '♠';
            }

            return '.';
        }

        private String getValueChar(int val) {
            if (val == 0) {
                return "A";
            }
            if (val <= 9) {
                return (val + 1) + "";
            }
            if (val == 10) {
                return "J";
            }
            if (val == 11) {
                return "Q";
            }
            if (val == 12)
            {
                return "K";
            }
            return "null";
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void HoldButton5_Click(object sender, EventArgs e)
        {
            if (card1Held)
            {
                card1Held = false;
                HoldButton5.Text = "Hold";
            }
            else
            {
                card1Held = true;
                HoldButton5.Text = "Held";
            }

        }

        private void HoldButton6_Click(object sender, EventArgs e)
        {
            if (card2Held)
            {
                card2Held = false;
                HoldButton6.Text = "Hold";
            }
            else
            {
                card2Held = true;
                HoldButton6.Text = "Held";
            }
        }

        private void HoldButton7_Click(object sender, EventArgs e)
        {
            if (card3Held)
            {
                card3Held = false;
                HoldButton7.Text = "Hold";
            }
            else
            {
                card3Held = true;
                HoldButton7.Text = "Held";
            }
        }

        private void HoldButton8_Click(object sender, EventArgs e)
        {
            if (card4Held)
            {
                card4Held = false;
                HoldButton8.Text = "Hold";
            }
            else
            {
                card4Held = true;
                HoldButton8.Text = "Held";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (card5Held)
            {
                card5Held = false;
                button5.Text = "Hold";
            }
            else
            {
                card5Held = true;
                button5.Text = "Held";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //open up the text file
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                String strfilename = openFileDialog1.FileName;
                MessageBox.Show(strfilename);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }


}
