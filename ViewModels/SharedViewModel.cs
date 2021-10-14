using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using WpfPrac.Models;
using WpfPrac.ViewModels.Commands;
using AiTest2ML.Model;

namespace WpfPrac.ViewModels
{
    public class SharedViewModel : CommandViewModel
    {
        private bool bothWon = false;
        public List<DataModel> DataModel = new();

        public int PlayerStartValue, DealersStartValue = 0;


        public bool BothWon { get => bothWon; set => bothWon = value; }

        // Constructor
        public SharedViewModel()
        {
            this.StartGameCommand = new StartGameCommand(this);
            this.LoginCommand = new LoginCommand(this);
            this.HitCommand = new HitCommand(this);
            this.StayCommand = new StayCommand(this);
            this.GoAgainCommand = new GoAgainCommand(this);
            this.ResetCommand = new ResetCommand(this);
            this.DoubbleDownCommand = new DoubbleDownCommand(this);
            this.SplitCommand = new SplitCommand(this);
            this.HandHitCommand = new HandHitCommand(this);
            this.SplitStayCommand = new SplitStayCommand(this);
            this.EnableBotCommand = new EnableBotCommand(this);
            this.BotGoAgainCommand = new BotGoAgainCommand(this);
        }

        // Early Game
        public void SetName(string name)
        {
            MiniReset();
            RoundNubmer++;
            if (EnabledBot)
            {
                Player.Name = name;
                Deck.MakeDeck();
                if (Deck.PlayDeck.Count == 52)
                {
                    CountModel.Count = 0;
                    CountModel.UsedCards.Clear();

                }
                BotSetBet();
            }
            else
            {
                Player.Name = name;
                BetVisibility = ChangeVisibility();
                Deck.MakeDeck();
            }
        }

        public void Start(float bet)
        {
            if(bet < Player.Money + 1)
            {
                Player.SetBet(bet);
                if (Player.Money >= Player.Bet)
                    DoubbleDownVisibility = "Visible";
                GameVisibility = ChangeVisibility();

                // Give starter card
                StartDeal();
            }
        }

        private void StartDeal()
        {
            BeforeMoney = Player.Money + Player.Bet;

            Dealer.AddCard(Deck);

            Player.AddCard(Deck);

            DealerTempCard = Deck.PickCard();

            Player.AddCard(Deck);

            PlayerStartValue = (int)Player.Value;
            DealersStartValue = (int)Dealer.Value;

            if (Player.Cards[0].Value == Player.Cards[1].Value)
                CanSplit = "True";

            MakeCountFromRound();

            Count = Deck.PlayDeck.Count;
        }

        public async Task DoubbleDown(string choice)
        {
            if(choice == "yes")
            {
                Player.Money -= Player.Bet;
                Player.Bet *= 2;
                Hit();
                if(Player.Value > 21)
                {
                    CheckWinner();
                }
                else
                {
                    await DealersTurn();
                    CheckWinner();
                }
            }
            else if(choice == "no")
            {
                DoubbleDownVisibility = "Hidden";
            }
        }


        // Mid Game
        public void Split()
        {
            Player.Hand1.StartCard(Player);
            Player.Hand1.AddCardFromDeck(Deck);

            Player.Money -= Player.Bet;

            Player.Hand2.StartCard(Player);
            Player.Hand2.AddCardFromDeck(Deck);


            SplitVisibility = ChangeVisibility();
        }

        public void Hit()
        {
            PlayerStartValue = (int)Player.Value;
            DealersStartValue = (int)Dealer.Value;
            Player.AddCard(Deck);
            MakeCountFromRound();

            Count = Deck.PlayDeck.Count;

            if(Player.Value > 21)
            {
                DataModel.Add(new DataModel() { });
                DealDealerTempCard();
                CheckWinner();
            }
        }

        public async Task Stay()
        {
            DealerTempCardVisibility = "Visible";
            DealersTurn();
            CheckWinner();
        }


        // Late Game
        private async Task DealersTurn()
        {
            while (Dealer.Value < 17)
            {
                DealDealerTempCard();
                if (Dealer.Cards.Count != 1 && Dealer.Value < 17)
                {
                    Dealer.AddCard(Deck);
                }
            }
            MakeCountFromRound();
            Count = Deck.PlayDeck.Count;
        }

        private void CheckWinner()
        {
            if(Player.Value > 21 || Dealer.Value > 21)
            {
                if(Player.Value > 21)
                {
                    // Player Bustede - Dealer vinder
                    Winner.Name = Dealer.Name;
                }
                if(Dealer.Value > 21)
                {
                    // Dealer Bustede - Player vinder
                    Winner.Name = Player.Name;
                    Player.Money += Player.Bet * 2;
                }
            }
            else
            {
                if(Player.CheckBlackJack() || Dealer.CheckBlackJack())
                {
                    if (Dealer.CheckBlackJack() && Player.CheckBlackJack())
                    {
                        Winner.Name = Dealer.Name;
                    }
                    else
                    {
                        if (Player.CheckBlackJack())
                        {
                            Winner.Name = Player.Name;
                            Player.Money += Convert.ToInt32(Math.Ceiling(Player.Bet * 2.5));
                        }

                        if (Dealer.CheckBlackJack())
                        {
                            Winner.Name = Dealer.Name;
                        }
                    }
                }
                else
                {
                    if (Dealer.Value > Player.Value)
                    {
                        // Dealer Vinder
                        Winner.Name = Dealer.Name;
                    }
                    else if(Dealer.Value < Player.Value)
                    {
                        // Player vinder
                        Winner.Name = Player.Name;
                        Player.Money += Player.Bet * 2;
                    }
                    else if(Dealer.Value == Player.Value)
                    {
                        Winner.Name = $"{Player.Name} + {Dealer.Name}";
                        Player.Money += Player.Bet;
                    }
                }
            }

            if (Winner.Name == Player.Name)
                Player.JustWon = true;
            else if(Winner.Name == Dealer.Name)
                Player.JustWon = false;
            else
            {
                BothWon = true;
                Player.JustWon = true;
            }

            if (Player.JustWon)
            {
                DataModel.Add(new DataModel() { CardCount = Player.Cards.Count, PlayerValue = (int)Player.Value, DealerValue = Dealer.Cards[0].Value, RealCount = CountModel.Count, ShouldStay = true});
            }
            else if(Player.JustWon == false && Player.Value < 22)
            {
                DataModel.Add(new DataModel() { CardCount = Player.Cards.Count, PlayerValue = (int)Player.Value, DealerValue = Dealer.Cards[0].Value, RealCount = CountModel.Count, ShouldStay = false });
            }
            else if(Player.JustWon == false && Player.Value > 21)
            {
                DataModel.Add(new DataModel() { CardCount = Player.Cards.Count, PlayerValue = (int)Player.Value - Player.Cards[Player.Cards.Count-1].Value, DealerValue = Dealer.Cards[0].Value, RealCount = CountModel.Count-1, ShouldStay = true });
            }

            RoundNubmer++;
            HaveAWinner = "false";
            ShowWinner = ChangeVisibility();

            if(Player.Money < 1)
            {
                NoMoney = "false";
            }
        } 

        // MiniReset will reset the bet, value, cards and insurance so player can start a new round
        public void MiniReset()
        {

            HaveAWinner = "true";
            Winner.Name = "Unknown";

            Player.Bet = 0;
            Player.Value = 0;
            Player.Insurance = 0;
            Player.Hand1.Cards.Clear();
            Player.Hand1.Bet = 0;
            Player.Hand2.Cards.Clear();
            Player.Hand2.Bet = 0;

            Player.Cards.Clear();

            Dealer.Bet = 0;
            Dealer.Value = 0;
            Dealer.Insurance = 0;
            Dealer.Hand1 = new Hand();
            Dealer.Hand2 = new Hand();
            Dealer.Cards.Clear();

            BetVisibility = ChangeVisibility();
        }

        // This will reset the player completly so the player can start from a new start.
        public void FullReset()
        {
            Player.PreviousBet = 0;
            EnabledBot = false;

            NoMoney = "true";
            HaveAWinner = "true";
            Winner.Name = "Unknown";

            Player.Money = 250;
            Player.Name = "Unknown";
            Player.Bet = 0;
            Player.Value = 0;
            Player.Insurance = 0;
            Player.Hand1.Cards.Clear();
            Player.Hand1.Bet = 0;
            Player.Hand2.Cards.Clear();
            Player.Hand2.Bet = 0;
            Player.Cards.Clear();

            Dealer.Money = 250;
            Dealer.Name = "Dealer";
            Dealer.Bet = 0;
            Dealer.Value = 0;
            Dealer.Insurance = 0;
            Dealer.Hand1 = new Hand();
            Dealer.Hand2 = new Hand();
            Dealer.Cards.Clear();

            CountModel = new CountModel();

            for (int i = 0; i < DataModel.Count; i++)
            {
                if(DataModel[i].PlayerValue == 0 && DataModel[i].DealerValue == 0)
                {
                    DataModel.Remove(DataModel[i]);
                }
            }

            using (var writer = new StreamWriter(@"C:\Users\nico936d\Desktop\MyData3.csv"))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(DataModel);
            }

            LoginVisibility = ChangeVisibility();
        }

        protected async Task DealDealerTempCard()
        {
            if (Dealer.Cards.Count == 1)
            {
                Dealer.Cards.Add(DealerTempCard);
                Dealer.Value += DealerTempCard.Value;
                if (Dealer.CheckBlackJack())
                {
                    CheckWinner();
                }
                if(!EnabledBot)
                    await Task.Delay(500);
            }
        }

        // Split Game
        public void SplitHit(string input)
        {
            if (input.Contains("Hand1"))
            {
                if(Player.Hand1.Value < 22)
                {
                    Player.Hand1.AddCardFromDeck(Deck);
                }
            }
            else if (input.Contains("Hand2"))
            {
                if(Player.Hand2.Value < 22)
                {
                    Player.Hand2.AddCardFromDeck(Deck);
                }
                else
                {
                    SplitStay(input);
                }
            }
        }

        public void SplitStay(string input)
        {
            if (input.Contains("Hand2"))
            {
                SplitResult();
            }
        }

        protected async Task SplitResult()
        {
            await DealersTurn();
            List<Hand> hands = new();
            hands.Add(Player.Hand1);
            hands.Add(Player.Hand2);

            foreach (Hand hand in hands)
            {
                if (hand.Value > 21 || Dealer.Value > 21)
                {
                    if (hand.Value > 21)
                    {
                        // hand Bustede - Dealer vinder
                        Winner.Name = Dealer.Name;
                    }
                    if (Dealer.Value > 21)
                    {
                        // Dealer Bustede - hand vinder
                        Winner.Name = Player.Name;
                        Player.Money += hand.Bet * 2;
                    }
                }
                else
                {
                    if (hand.CheckBlackJack() || Dealer.CheckBlackJack())
                    {
                        if (Dealer.CheckBlackJack() && hand.CheckBlackJack())
                        {
                            Winner.Name = Dealer.Name;
                        }
                        else
                        {
                            if (hand.CheckBlackJack())
                            {
                                Winner.Name = Player.Name;
                                Player.Money += Convert.ToInt32(Math.Ceiling(hand.Bet * 2.5));
                            }

                            if (Dealer.CheckBlackJack())
                            {
                                Winner.Name = Dealer.Name;
                            }
                        }
                    }
                    else
                    {
                        if (Dealer.Value > hand.Value)
                        {
                            // Dealer Vinder
                            Winner.Name = Dealer.Name;
                        }
                        if (Dealer.Value < hand.Value)
                        {
                            // hand vinder
                            Winner.Name = Player.Name;
                            Player.Money += hand.Bet * 2;
                        }
                        else if (Dealer.Value == hand.Value)
                        {
                            Winner.Name = $"{Player.Name} + {Dealer.Name}";
                            Player.Money += hand.Bet;
                        }
                    }
                }
                hand.Value = 0;
                
                HaveAWinner = "false";
                SplitWinnerVisibility = ChangeVisibility();
            }
        }

        // Card Counting Bot
        public void EnableBot()
        {
            if (EnabledBot)
                EnabledBot = false;
            else
                EnabledBot = true;
        }

        protected void MakeCountFromRound()
        {
            foreach (Card card in Player.Cards)
            {
                if(CountModel.UsedCards.Contains(card) == false)
                {

                    if(card.Value < 7 && card.Value != 1)
                    {
                        CountModel.Count++;
                        CountModel.UsedCards.Add(card);
                    }
                    else if(card.Value > 9 || card.Value == 1)
                    {
                        CountModel.Count--;
                        CountModel.UsedCards.Add(card);
                    }
                }
            }
            foreach (Card card in Dealer.Cards)
            {
                if (CountModel.UsedCards.Contains(card) == false)
                {

                    if (card.Value < 7 && card.Value != 1)
                    {
                        CountModel.Count++;
                        CountModel.UsedCards.Add(card);
                    }
                    else if (card.Value > 9 || card.Value == 1)
                    {
                        CountModel.Count--;
                        CountModel.UsedCards.Add(card);
                    }
                    else
                    {
                        CountModel.UsedCards.Add(card);
                    }
                }
            }
        }

        public void BotSetBet()
        {
            if (!BothWon)
            {
                if(Player.PreviousBet == 0 || Player.JustWon)
                {
                    Player.SetBet(1);
                }
                else if(Player.JustWon == false)
                {
                    if(Player.PreviousBet*2%2 == 0)
                        Player.SetBet(Player.PreviousBet * 2);
                    else
                    {
                        Player.SetBet(Player.Money);
                    }
                }
            }
            else
            {
                Player.SetBet(Player.PreviousBet);
                BothWon = false;
            }
            StartDeal();
            BotStayOrHit();
        }

        public void BotStayOrHit()
        {
            bool stillGoing = true;
            while (Player.Value < 22 && stillGoing)
            {
                // Add input data
                var input = new ModelInput() { PlayerValue = Player.Value, DealerValue = Dealer.Value, RealCount = CountModel.Count, CardCount = 3 };

                // Load model and predict output of sample data
                ModelOutput result = ConsumeModel.Predict(input);

                if(result.Prediction == "True")
                {
                    Stay();
                    stillGoing = false;
                }
                else if(result.Prediction == "False")
                {
                    Hit();
                    Player.FixZeroError();
                }
            }

            stillGoing = true;
        }
    }
}
