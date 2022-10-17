using System;
using System.Collections.Generic;
using RedSharp.ClearDesign.Shared.Interfaces;

namespace RedSharp.ClearDesign.Shared.Abstracts
{
    public abstract class SingleInputModifierBase : IResourceModifier
    {
        private object[] _inputKeys;

        public SingleInputModifierBase()
        {
            _inputKeys = new object[1];
        }

        public object ResultKey { get; set; }

        IEnumerable<object> IResourceModifier.InputKeys => _inputKeys;

        public object InputKey 
        { 
            get => _inputKeys[0]; 
            set => _inputKeys[0] = value; 
        }

        object IResourceModifier.Modify(IList<object> values)
        {
            if (InputKey == null)
                throw new Exception("Input keys wasn't initialized.");

            if (values.Count != 1)
                throw new ArgumentException("Values number doesn't match to parameters number.");

            return InternalModify(values[0]);
        }

        protected abstract object InternalModify(object value);
    }
}
