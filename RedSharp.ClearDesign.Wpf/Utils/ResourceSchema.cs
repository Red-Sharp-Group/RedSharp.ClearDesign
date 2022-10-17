using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using RedSharp.ClearDesign.Shared.Interfaces;

namespace RedSharp.ClearDesign.Utils
{
    public class ResourceSchema : ICollection<IResourceModifier>
    {
        private ResourceDictionary _dictionary;
        private Dictionary<object, ResourceNotifier> _notifiers;
        private Dictionary<object, List<IResourceModifier>> _modifiers;
        private HashSet<IResourceModifier> _allModifiers;
        private HashSet<Type> _initializedDictionaries;

        public ResourceSchema(ResourceDictionary dictionary)
        {
            _dictionary = dictionary;
            _notifiers = new Dictionary<object, ResourceNotifier>();
            _modifiers = new Dictionary<object, List<IResourceModifier>>();
            _allModifiers = new HashSet<IResourceModifier>();
        }

        public static ResourceSchema ApplicationResources { get; } = new ResourceSchema(Application.Current.Resources);

        public bool IsFilledFrom(ResourceDictionary dictionary)
        {
            return _initializedDictionaries != null && _initializedDictionaries.Contains(dictionary.GetType());
        }

        public void FillFrom(ResourceDictionary dictionary)
        {
            if (_initializedDictionaries == null)
                _initializedDictionaries = new HashSet<Type>();

            if (_initializedDictionaries.Add(dictionary.GetType()))
                foreach (DictionaryEntry item in dictionary)
                    if (item.Value is IResourceModifier modifier)
                        Add(modifier);
        }

        public int Count => _allModifiers.Count();

        public bool IsReadOnly => false;

        public void Add(IResourceModifier item)
        {
            if (!_allModifiers.Add(item))
                return;

            foreach (var key in item.InputKeys)
            {
                if (!_modifiers.TryGetValue(key, out List<IResourceModifier> modifiersList))
                {
                    modifiersList = new List<IResourceModifier>();

                    _modifiers.Add(key, modifiersList);
                }

                modifiersList.Add(item);

                if (!_notifiers.ContainsKey(key))
                    _notifiers.Add(key, new ResourceNotifier(this, _dictionary, key));
            }
        }

        public bool Contains(IResourceModifier item)
        {
            return _allModifiers.Contains(item);
        }

        public bool Remove(IResourceModifier item)
        {
            if (!_allModifiers.Remove(item))
                return false;

            foreach (var key in item.InputKeys)
            {
                var modifiersList = _modifiers[key];

                modifiersList.Remove(item);

                if (modifiersList.Count == 0)
                    _modifiers.Remove(key);
            }

            return true;
        }

        public void Clear()
        {
            _allModifiers.Clear();
            _modifiers.Clear();
        }

        public void CopyTo(IResourceModifier[] array, int arrayIndex)
        {
            _allModifiers.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _allModifiers.GetEnumerator();
        }

        public IEnumerator<IResourceModifier> GetEnumerator()
        {
            return _allModifiers.GetEnumerator();
        }

        internal void UpdateResource(object key)
        {
            if (!_modifiers.TryGetValue(key, out List<IResourceModifier> modifiers))
                return;

            var arguments = new List<object>();

            foreach (var item in modifiers)
            {
                arguments.Clear();

                foreach (var targetKey in item.InputKeys)
                    arguments.Add(_notifiers[targetKey].Value);

                _dictionary[item.ResultKey] = item.Modify(arguments);
            }
        }
    }
}
