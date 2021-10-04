using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class HitCommand : ICommand
    {
        private SharedViewModel SharedViewModel;
        public event EventHandler CanExecuteChanged;

        public HitCommand(SharedViewModel sharedViewModel)
        {
            SharedViewModel = sharedViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.Hit();
        }
    }
}
