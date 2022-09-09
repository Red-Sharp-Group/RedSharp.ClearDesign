using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedSharp.ClearDesign.Abstractions
{
    public abstract class DefinitionBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ResourceDictionary _resources;

        public DefinitionBase(ResourceDictionary resources)
        {
            _resources = resources;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TValue GetValue<TValue>(String key)
        {
            return (TValue)_resources[key];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetValue<TValue>(String key, TValue value)
        {
            _resources[key] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaisePropertyChanged([CallerMemberName] String property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool SetAndRaise<TValue>(String key, TValue value, [CallerMemberName] String property = null)
        {
            if (EqualityComparer<TValue>.Default.Equals(GetValue<TValue>(key), value))
                return false;

            SetValue(key, value);

            RaisePropertyChanged(property);

            return true;
        }
    }
}
