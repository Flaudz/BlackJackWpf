using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPrac.ViewModels.Commands
{
    public class HandHitCommand : ICommand
    {
        protected SharedViewModel SharedViewModel;
        public event EventHandler CanExecuteChanged;

        public HandHitCommand(SharedViewModel viewModel)
        {
            SharedViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            if (parameter.ToString().Contains("hit"))
            {
                SharedViewModel.SplitHit(parameter.ToString());
            }
            else if (parameter.ToString().Contains("stay"))
            {
                // Stay
            }
        }
    }
}
