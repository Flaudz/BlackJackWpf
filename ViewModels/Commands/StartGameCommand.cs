using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class StartGameCommand : ICommand
    {
        private SharedViewModel SharedViewModel;
        public event EventHandler CanExecuteChanged;

        public StartGameCommand(SharedViewModel viewmodel)
        {
            SharedViewModel = viewmodel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.Start(Int32.Parse(parameter.ToString()));
        }
    }
}
