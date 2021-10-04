using System;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class LoginCommand : ICommand
    {
        private SharedViewModel SharedViewModel;
        public event EventHandler CanExecuteChanged;

        public LoginCommand(SharedViewModel viewmodel)
        {
            this.SharedViewModel = viewmodel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.SetName(parameter.ToString());
        }
    }
}
