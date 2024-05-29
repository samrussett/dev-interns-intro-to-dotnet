﻿using Blackjack;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using static Blackjack.ConsoleControlHandler;

namespace BlackjackUpdated
{
    class Program
    {
        static Random cardRandomizer = new Random();

        static readonly List<Card> playerCards = new List<Card>();
        static int playerTotal = 0;
        private static readonly List<Card> dealerCards = new List<Card>();
        static int dealerTotal = 0;
        static int dealerCardCount = 0;

        static List<string> suits = new List<string> { "Diamond", "Spade", "Heart", "Club" };
        static List<string> cardNames = new List<string> { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };
        static List<Card> deck = new List<Card>();

        //users to store the player choice (hit or stay)
        static string playerChoice = "";

        static string playAgain = "Y";

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            CreateDeck();

            while (playAgain.ToUpper() == "Y")
            {
                //StartGame
                try
                {
                    Console.WriteLine("Welcome to Blackjack - are you ready to play? (Y)esss (N)o");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("hi");
                }

                var decision = Console.ReadLine().ToUpper();

                if (decision == "Y")
                {
                    // With line below, just get a value between 16-21 for the dealer
                    // dealerTotal = cardRandomizer.Next(15, 22);
                    playerCards.Add(DealCard());
                    playerCards.Add(DealCard());

                    playerCards.FirstOrDefault();

                    // playerCards[0] = DealCard();
                    // playerCards[1] = DealCard();

                    int pCard1Num = NameToCardNum(playerCards[0].Name);
                    playerTotal += GetCardValue(pCard1Num);
                    int pCard2Num = NameToCardNum(playerCards[1].Name);
                    playerTotal += GetCardValue(pCard2Num);


                    dealerCardCount = 2;

                    // The dealer is dealt one card face up, one card face down.
                    // dealerCards[0] = DealCard();
                    // dealerCards[1] = DealCard();
                    dealerCards.Add(DealCard());
                    dealerCards.Add(DealCard());

                    int dCard1Num = NameToCardNum(dealerCards[0].Name);
                    dealerTotal += GetCardValue(dCard1Num);
                    int dCard2Num = NameToCardNum(dealerCards[0].Name);
                    dealerTotal += GetCardValue(dCard2Num);

                    DisplayWelcomeMessage();
                }
                else
                {
                    Environment.Exit(0);
                }

                /* START GAME LOOP */
                do
                {
                    Console.WriteLine("Would you like to (H)it or (S)tay?");
                    playerChoice = Console.ReadLine().ToUpper();
                }
                while (!playerChoice.Equals("H") && !playerChoice.Equals("S"));

                if (playerChoice.Equals("H"))
                {
                    //hit will get them a card / check the total and ask for another hit
                    Hit();
                }

                if (playerChoice.Equals("S"))
                {
                    if (playerTotal > dealerTotal && playerTotal <= 21)
                    {
                        Console.WriteLine("Congrats! You won the game! The dealer's total is {0} ", dealerTotal);
                    }
                    else if (playerTotal < dealerTotal)
                    {
                        Console.WriteLine("Sorry, you lost! The dealer's total was {0}", dealerTotal);
                    }
                }

                /* END GAME LOOP */

                Console.WriteLine("Would you like to play again? (Y)es or (N)o?");
                PlayAgain();
            }
        }

        /// <summary>
        /// Displays a friendly message to the user and shows their current hand.
        /// </summary>
        private static void DisplayWelcomeMessage()
        {
            Console.WriteLine("You were dealt the cards : {0} and {1} ", playerCards[0].Name, playerCards[1].Name);
            Console.WriteLine("Your playerTotal is {0} ", playerTotal);
            // Inform the player the value of the dealer's visible card.
            Console.WriteLine("Dealer was dealt two cards, one of them being : {0} ", dealerCards[1].Name);
        }

        static void Hit()
        {
            Card newCard = DealCard();
            int dealedCardNum = NameToCardNum(newCard.Name);
            playerTotal += GetCardValue(dealedCardNum);
            playerCards.Add(newCard);


            while (dealerTotal < 17)
            {
                dealerCardCount += 1;

                newCard = DealCard();
                int newCardNum = NameToCardNum(newCard.Name);
                dealerTotal += GetCardValue(newCardNum);
                dealerCards.Add(newCard);
            }

            //Is this true? I don't think it is.
            if (playerTotal.Equals(21))
            {
                Console.WriteLine("You got Blackjack! The dealer's Total was {0}. ", dealerTotal);

            }
            else if (playerTotal > 21)
            {
                Console.WriteLine("You busted! Sorry! The dealer's Total was {0}", dealerTotal);

            }
            else if (playerTotal < 21)
            {
                do
                {
                    Console.WriteLine("Would you like to hit or stay? h for hit s for stay");
                    playerChoice = Console.ReadLine().ToUpper();
                }
                while (!playerChoice.Equals("H") && !playerChoice.Equals("S"));
                if (playerChoice.ToUpper() == "H")
                {
                    Hit();
                }
            }
        }

        /*         private List<Cards> cards = new List<Cards>()
                {
                    public string Name;
                    public int Value;
                } */

        //Move this class to its own file.
        /*         private class Card
                {
                    public int Value;
                    public string Name;
                } */

        static void CreateDeck()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    deck.Add(new Card() { Name = cardNames[j], Suit = suits[i], Removed = false });
                }
            }



        }

        static Card DealCard()
        {
            int cardNum = cardRandomizer.Next(1, 53);
            // playerTotal += cardValue;
            deck[cardNum].Removed = true;
            return deck[cardNum];
        }


        static int GetCardValue(int cardNum)
        {
            return cardNum switch
            {
                1 => 11,
                2 => 2,
                3 => 3,
                4 => 4,
                5 => 5,
                6 => 6,
                7 => 7,
                8 => 8,
                9 => 9,
                10 => 10,
                11 => 10,
                12 => 10,
                13 => 10,
                _ => 2,
            };
        }

        static int NameToCardNum(string name)
        {
            return name switch
            {
                "Two" => 2,
                "Three" => 3,
                "Four" => 4,
                "Five" => 5,
                "Six" => 6,
                "Seven" => 7,
                "Eight" => 8,
                "Nine" => 9,
                "Ten" => 10,
                "Jack" => 11,
                "Queen" => 12,
                "King" => 13,
                "Ace" => 1,
                _ => 0
            };
        }

        static void PlayAgain()
        {
            //Loop until they make a valid choice
            do
            {
                playAgain = Console.ReadLine().ToUpper();
            }
            while (!playAgain.Equals("Y") && !playAgain.Equals("N"));

            if (playAgain.Equals("Y"))
            {
                Console.WriteLine("Press enter to restart the game!");
                Console.ReadLine();
                Console.Clear();
                dealerTotal = 0;
                playerTotal = 0;
            }
            else if (playAgain.Equals("N"))
            {
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.Key == ConsoleKey.Enter)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.Read();
                    Environment.Exit(0);
                }
            }
        }

        
    }
}