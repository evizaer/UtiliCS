using System;
using System.Diagnostics;
using System.Windows.Input;

namespace UtiliCS
{
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _Execute;
        readonly Predicate<object> _CanExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _Execute = execute;
            _CanExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _CanExecute == null || _CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _Execute(parameter);
        }

        #endregion // ICommand Members


        public event EventHandler CanExecuteChanged;
    }
}
