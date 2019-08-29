using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Playnite.SDK
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RelayCommand : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object parameter);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : RelayCommand
    {
        private readonly Predicate<T> canExecute;
        private readonly Action<T> execute;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <param name="gesture"></param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return canExecute((T)parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
