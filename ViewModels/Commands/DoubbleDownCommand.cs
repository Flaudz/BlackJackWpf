using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class DoubbleDownCommand : ICommand
    {
        public SharedViewModel SharedViewModel;
        public event EventHandler CanExecuteChanged;

        public DoubbleDownCommand(SharedViewModel viewModel)
        {
            SharedViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SharedViewModel.DoubbleDown(parameter.ToString().ToLower());
        }
    }
}
