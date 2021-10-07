using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class GoAgainCommand : ICommand
    {
        public SharedViewModel SharedViewModel;

        public event EventHandler CanExecuteChanged;

        public GoAgainCommand(SharedViewModel viewModel)
        {
            SharedViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.EnabledBot = false;
            SharedViewModel.MiniReset();
        }
    }
}
