using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HR_Management.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class RelayCommand<T> : ICommand
        {
            protected readonly Predicate<T> _canExecute;
            protected readonly Action<T> _execute;

            public RelayCommand(Predicate<T> canExecute, Action<T> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }
                _canExecute = canExecute;
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                if (parameter == null)
                {
                    return false;
                }

                try
                {
                    return _canExecute == null ? true : _canExecute((T)parameter);
                }
                catch
                {
                    return true;
                }
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }

        public class RelayDoubleParamCommand<T, V> : ICommand
        {
            protected readonly Func<T, V, bool> _canExecute;
            protected readonly Func<T, V, bool> _execute;

            public RelayDoubleParamCommand(Func<T, V, bool> canExecute, Func<T, V, bool> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }
                _canExecute = canExecute;
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                if (parameter == null)
                {
                    return false;
                }

                try
                {
                    var values = parameter as object[];
                    return _canExecute == null ? true : (bool)_canExecute((T)values[0], (V)values[1]);
                }
                catch
                {
                    return true;
                }
            }

            public void Execute(object parameter)
            {
                var values = parameter as object[];
                _execute((T)values[0], (V)values[1]);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }

        public class RelayTripleParamCommand<T, V, U> : ICommand
        {
            protected readonly Func<T, V, U, bool> _canExecute;
            protected readonly Func<T, V, U, bool> _execute;

            public RelayTripleParamCommand(Func<T, V, U, bool> canExecute, Func<T, V, U, bool> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }
                _canExecute = canExecute;
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                if (parameter == null)
                {
                    return false;
                }

                try
                {
                    var values = parameter as object[];
                    return _canExecute == null ? true : (bool)_canExecute((T)values[0], (V)values[1], (U)values[2]);
                }
                catch
                {
                    return true;
                }
            }

            public void Execute(object parameter)
            {
                var values = parameter as object[];
                _execute((T)values[0], (V)values[1], (U)values[2]);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }


        public class AsyncNoParamCommand : ICommand
        {
            private readonly Func<bool> _canExecute;
            private readonly Func<bool> _execute;

            public AsyncNoParamCommand(Func<bool> canExecute, Func<bool> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }

                _canExecute = canExecute;
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public bool CanExecute(object parameter)
            {
                try
                {
                    return _canExecute == null ? true : _canExecute();
                }
                catch
                {
                    return true;
                }
            }

            public async void Execute(object parameter)
            {
                await Task.Factory.StartNew(() => { _execute(); });
            }
        }

        public class AsyncCommand<T> : ICommand
        {
            private readonly Predicate<T> _canExecute;
            private readonly Action<T> _execute;

            public AsyncCommand(Predicate<T> canExecute, Action<T> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }

                _canExecute = canExecute;
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public bool CanExecute(object parameter)
            {
                if (parameter == null)
                {
                    return false;
                }

                try
                {
                    return _canExecute == null ? true : _canExecute((T)parameter);
                }
                catch
                {
                    return true;
                }
            }

            public async void Execute(object parameter)
            {
                await Task.Factory.StartNew(() => { _execute((T)parameter); });
            }
        }

        public class AsyncDoubleParamCommand<T, V> : ICommand
        {
            private readonly Func<T, V, bool> _canExecute;
            private readonly Func<T, V, bool> _execute;

            public AsyncDoubleParamCommand(Func<T, V, bool> canExecute, Func<T, V, bool> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }

                _canExecute = canExecute;
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public bool CanExecute(object parameter)
            {
                if (parameter == null)
                {
                    return false;
                }

                try
                {
                    var values = parameter as object[];
                    return _canExecute == null ? true : _canExecute((T)values[0], (V)values[1]);
                }
                catch
                {
                    return true;
                }
            }

            public async void Execute(object parameter)
            {
                var values = parameter as Object[];
                await Task.Factory.StartNew(() => { _execute((T)values[0], (V)values[1]); });
            }
        }

        public class AsyncQuadraParamCommand<T, V, X, W> : ICommand
        {
            private readonly Func<T, V, X, W, bool> _canExecute;
            private readonly Func<T, V, X, W, bool> _execute;

            public AsyncQuadraParamCommand(Func<T, V, X, W, bool> canExecute, Func<T, V, X, W, bool> execute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException("execute");
                }

                _canExecute = canExecute;
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public bool CanExecute(object parameter)
            {
                if (parameter == null)
                {
                    return false;
                }

                try
                {
                    var values = parameter as object[];
                    return _canExecute == null ? true : _canExecute((T)values[0], (V)values[1], (X)values[2], (W)values[3]);
                }
                catch
                {
                    return true;
                }
            }

            public async void Execute(object parameter)
            {
                var values = parameter as Object[];
                await Task.Factory.StartNew(() => { _execute((T)values[0], (V)values[1], (X)values[2], (W)values[3]); });
            }
        }
    }
}
