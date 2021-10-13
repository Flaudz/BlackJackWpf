using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class BotGoAgainCommand : ICommand
    {
        private SharedViewModel SharedViewModel;

        public event EventHandler CanExecuteChanged;

        public BotGoAgainCommand(SharedViewModel viewModel)
        {
            SharedViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.EnabledBot = true;
            SharedViewModel.RoundNubmer = 0;
            SharedViewModel.SetName(SharedViewModel.Player.Name);
        }
    }
}
