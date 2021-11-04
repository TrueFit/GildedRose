using System;
using System.Windows.Input;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// A simple command that can be executed from the user interface
    /// </summary>
    public class SimpleCommand : ICommand
    {
        private Action _action;

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="action">The action that shall be executed when the command is executed.</param>
        public SimpleCommand(Action action)
        {
            _action = action;
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            if (_action != null)
                _action();
        }
    }
}
