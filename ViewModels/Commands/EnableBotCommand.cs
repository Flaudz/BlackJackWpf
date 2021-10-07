using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class EnableBotCommand : ICommand
    {
        SharedViewModel SharedViewModel;

        public event EventHandler CanExecuteChanged;

        public EnableBotCommand(SharedViewModel viewModel)
        {
            SharedViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.EnableBot();
        }
    }
}
