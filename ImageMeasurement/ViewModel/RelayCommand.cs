using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageMeasurement.ViewModel
{
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        readonly Action<T> _execute;
        readonly Predicate<T> _canExecute;
        readonly string _commandName;
        public string Name { get; protected set; }
        #endregion 

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(string name, Action<T> execute)
            : this(name, execute, null, true)
        {
        }

        public RelayCommand(string name, Action<T> execute, Predicate<T> canExecute, bool logCmd)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            Name = name;
            _execute = execute;
            _canExecute = canExecute;
            _commandName = name;
        }

        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            bool canExecute = false;
            if (_canExecute == null)
            {
                return true;
            }
            if (parameter == null && typeof(T) != typeof(Object))
            {
                return true;
            }
            try
            {
                canExecute = _canExecute((T)parameter);
            }
            catch (Exception /*e*/)
            {
            }

            return canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            try
            {
                _execute((T)parameter);
            }
            catch (Exception /*e*/)
            {
                throw;
            }
        }


        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        #endregion 
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(string name, Action<object> execute)
            : base(name, execute)
        {
        }

        public RelayCommand(string name, Action<object> execute, Predicate<object> canExecute)
            : base(name, execute, canExecute, true)
        {
        }

        public RelayCommand(string name, Action<object> execute, Predicate<object> canExecute, bool logCmd)
            : base(name, execute, canExecute, logCmd)
        {
        }

    }
}
