using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPrac.ViewModels.Commands;

namespace WpfPrac.ViewModels
{
    public class CommandViewModel : VisibilityViewModel
    {
        // Commands
        private LoginCommand loginCommand;
        private StartGameCommand startGameCommand;
        private HitCommand hitCommand;
        private StayCommand stayCommand;
        private GoAgainCommand goAgainCommand;
        private ResetCommand resetCommand;
        private DoubbleDownCommand doubbleDownCommand;
        private SplitCommand splitCommand;
        private HandHitCommand handHitCommand;
        private SplitStayCommand splitStayCommand;
        private EnableBotCommand enableBotCommand;
        private BotGoAgainCommand botGoAgainCommand;

        // Commands
        public LoginCommand LoginCommand { get => loginCommand; set => loginCommand = value; }
        public StartGameCommand StartGameCommand { get => startGameCommand; set => startGameCommand = value; }
        public HitCommand HitCommand { get => hitCommand; set => hitCommand = value; }
        public StayCommand StayCommand { get => stayCommand; set => stayCommand = value; }
        public GoAgainCommand GoAgainCommand { get => goAgainCommand; set => goAgainCommand = value; }
        public ResetCommand ResetCommand { get => resetCommand; set => resetCommand = value; }
        public DoubbleDownCommand DoubbleDownCommand { get => doubbleDownCommand; set => doubbleDownCommand = value; }
        public SplitCommand SplitCommand { get => splitCommand; set => splitCommand = value; }
        public HandHitCommand HandHitCommand { get => handHitCommand; set => handHitCommand = value; }
        public SplitStayCommand SplitStayCommand { get => splitStayCommand; set => splitStayCommand = value; }
        public EnableBotCommand EnableBotCommand { get => enableBotCommand; set => enableBotCommand = value; }
        public BotGoAgainCommand BotGoAgainCommand { get => botGoAgainCommand; set => botGoAgainCommand = value; }
    }
}
