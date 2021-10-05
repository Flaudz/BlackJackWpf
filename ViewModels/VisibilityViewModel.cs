using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.ViewModels
{
    public class VisibilityViewModel : INotifyPropertyChanged
    {
            // Visibility
        private string loginVisibility = "Visible";
        private string betVisibility = "Hidden";
        private string gameVisibility = "Hidden";
        private string dealerTempCardVisibility = "Visible";
        private string showWinner = "Hidden";
        private string doubbleDown = "Hidden";
        private string splitVisibility = "Hidden";
        private string splitWinnerVisibility = "Hidden";

        // Visibility
        public string LoginVisibility { get => loginVisibility;
            set
            {
                if (loginVisibility != value)
                {
                    loginVisibility = value;
                    RaisePropertyChanged("LoginVisibility");
                }
            }
        }
        public string BetVisibility { get => betVisibility;
            set
            {
                if (betVisibility != value)
                {
                    betVisibility = value;
                    RaisePropertyChanged("BetVisibility");
                }
            }
        }
        public string GameVisibility { get => gameVisibility;
            set
            {
                if (gameVisibility != value)
                {
                    gameVisibility = value;
                    RaisePropertyChanged("GameVisibility");
                }
            }
        }
        public string ShowWinner { get => showWinner;
            set
            {
                if (showWinner != value)
                {
                    showWinner = value;
                    RaisePropertyChanged("ShowWinner");
                }
            }
        }
        public string DealerTempCardVisibility
        {
            get => dealerTempCardVisibility;
            set
            {
                if (dealerTempCardVisibility != value)
                {
                    dealerTempCardVisibility = value;
                    RaisePropertyChanged("DealerTempCardVisibility");
                }
            }
        }
        public string SplitVisibility { get => splitVisibility;
            set
            {
                if (splitVisibility != value)
                {
                    splitVisibility = value;
                    RaisePropertyChanged("SplitVisibility");
                }
            }
        }
        public string SplitWinnerVisibility { get => splitWinnerVisibility;
            set
            {
                if (splitWinnerVisibility != value)
                {
                    splitWinnerVisibility = value;
                    RaisePropertyChanged("SplitWinnerVisibility");
                }
            }
        }
        public string DoubbleDownVisibility { get => doubbleDown;
            set
            {
                if (doubbleDown != value)
                {
                    doubbleDown = value;
                    RaisePropertyChanged("DoubbleDownVisibility");
                }
            }
        }
        public string ChangeVisibility()
        {

            SplitWinnerVisibility = "Hidden";
            SplitVisibility = "Hidden";
            LoginVisibility = "Hidden";
            GameVisibility = "Hidden";
            BetVisibility = "Hidden";
            ShowWinner = "Hidden";

            return "Visible";
        }
            // Property Changed
            public event PropertyChangedEventHandler PropertyChanged;

            // This fuction is what informors the thing that it is changed
            public void RaisePropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }

    }

