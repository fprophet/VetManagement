using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Data;

namespace VetManagement.Commands
{
    public class DatabaseRelayCommand<TEntity> where TEntity : BaseEntity
    {
        private readonly Action<TEntity> _execute;
        private readonly bool _canExecute;
        private readonly TEntity _entity;

        public DatabaseRelayCommand(TEntity entity, Action<TEntity> execute)
        {
            _entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = _entity != null && _entity.GetType().GetProperty("Id") != null;
        }

        public bool CanExecute() =>  _canExecute;
        public virtual void Execute(object? parameters) => _execute((TEntity)parameters);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
