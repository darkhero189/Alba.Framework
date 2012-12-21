using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Alba.Framework.Attributes;

namespace Alba.Framework.Mvvm.Models
{
    public abstract class ModelBase : IModel, INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected ModelBase ()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public Dispatcher Dispatcher { get; protected set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanging ([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanging;
            if (handler != null)
                handler(this, new PropertyChangingEventArgs(propName));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged ([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propName));
        }

        [NotifyPropertyChangedInvocator ("propName")]
        protected bool Set<T> (ref T field, T value, [CallerMemberName] string propName = null)
        {
            VerifyAccess();
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            OnPropertyChanging(propName);
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        [NotifyPropertyChangedInvocator ("propNames")]
        protected bool Set<T> (ref T field, T value, params string[] propNames)
        {
            VerifyAccess();
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            foreach (string prop in propNames)
                OnPropertyChanging(prop);
            field = value;
            foreach (string prop in propNames)
                OnPropertyChanged(prop);
            return true;
        }

        protected T Get<T> (ref T field)
        {
            VerifyAccess();
            return field;
        }

        protected void VerifyAccess ()
        {
            if (Dispatcher != null)
                Dispatcher.VerifyAccess();
        }
    }
}