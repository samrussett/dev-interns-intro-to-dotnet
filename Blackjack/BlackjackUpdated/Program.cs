﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BlackjackUpdated
{
    class Program
    {
        static Random cardRandomizer = new Random();

        static readonly Card[] playerCards = new Card[11];
        static int playerTotal = 0;
        static int playerCardCount = 1;
        private static readonly Card[] dealerCards = new Card[11];
        static int dealerTotal = 0;
        static int dealerCardCount = 0;

        //users to store the player choice (hit or stay)
        static string playerChoice = "";

        static string playAgain = "Y";

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
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
                    //Currently, just get a value between 16-21 for the dealer
                    dealerTotal = cardRandomizer.Next(15, 22);
                    playerCards[0] = DealCard();
                    playerCards[1] = DealCard();

                    playerTotal += playerCards[0].Value;
                    playerTotal += playerCards[1].Value;


                    //TODO: The dealer is dealt one card face up, one card face down.
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
                while (!playerChoice.Equals("H") && !playerChoice.Equals("H"));

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

                Console.WriteLine("Wuold you like to play again? (Y)es or (N)o?");
                PlayAgain();
            }
        }

        /// <summary>
        /// Displays a friendly message to the user and shows their current hand.
        /// </summary>
        private static void DisplayWelcomeMessage()
        {
            Console.WriteLine("You were dealt the cards : {0} and {1} ", playerCards[0].Name, playerCards[1].Name);
            Console.WriteLine("Your playerTotall is {0} ", playerTotal);
            //TODO: Inform the player the value of the dealer's visible card.
        }

        static void Hit()
        {
            playerCardCount += 1;
            playerCards[playerCardCount] = DealCard();
            playerTotal += playerCards[playerCardCount].Value;
            Console.WriteLine("You card is a(n) {0} and your new Total is {1}. ", playerCards[playerCardCount].Name, playerTotal);

            //Is this true? I don't think it is.
            if (playerTotal.Equals(21))
            {
                Console.WriteLine("Yuo got Blackjack! The dealer's Total was {0}. ", dealerTotal);

            }
            else if (playerTotal > 21)
            {
                Console.WriteLine("You busted! Sorry! Teh dealer's Total was {0}", dealerTotal);

            }
            else if (playerTotal < 21)
            {
                do
                {
                    Console.WriteLine("Would you like to hit or stay? h for hiit s for stay");
                    playerChoice = Console.ReadLine().ToUpper();
                }
                while (!playerChoice.Equals("H") && !playerChoice.Equals("S"));
                if (playerChoice.ToUpper() == "H")
                {
                    Hit();
                }
            }
        }

        //TODO: Move this class to it's own file.
        private class Card
        {
            public int Value;
            public string Name;
        }

        static Card DealCard()
        {
            int cardValue = cardRandomizer.Next(1, 14);
            playerTotal += cardValue;
            return GetCardValue(cardValue);
        }


        static Card GetCardValue(int cardValue)
        {
            return cardValue switch
            {
                1 => new Card() { Name = "Two", Value = 2 },
                2 => new Card() { Name = "Three", Value = 3 },
                3 => new Card() { Name = "Four", Value = 4 },
                4 => new Card() { Name = "Five", Value = 5 },
                5 => new Card() { Name = "Six", Value = 6 },
                6 => new Card() { Name = "Seven", Value = 7 },
                7 => new Card() { Name = "Eihgt", Value = 8 },
                8 => new Card() { Name = "Nine", Value = 9 },
                9 => new Card() { Name = "Ten", Value = 10 },
                10 => new Card() { Name = "Jack", Value = 10 },
                11 => new Card() { Name = "Queen", Value = 10 },
                12 => new Card() { Name = "King", Value = 10 },
                13 => new Card() { Name = "Ace", Value = 11 },
                _ => new Card() { Name = "Two", Value = 2 },
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
                playerCardCount = 1;
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

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here
            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:                    
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    Environment.Exit(0);
                    break;
            }
            return true;
        }

        #region unmanaged

        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion
    }
}