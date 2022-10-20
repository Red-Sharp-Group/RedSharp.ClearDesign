using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using RedSharp.ClearDesign.Wpf.Interfaces;

namespace RedSharp.ClearDesign.Utils
{
    public class ResourceSchema : ICollection<IResourceModifier>
    {
        private ResourceDictionary _dictionary;
        private Dictionary<object, ResourceNotifier> _notifiers;
        private Dictionary<object, List<IResourceModifier>> _modifiers;
        private HashSet<IResourceModifier> _allModifiers;

        public ResourceSchema(ResourceDictionary dictionary)
        {
            _dictionary = dictionary;
            _notifiers = new Dictionary<object, ResourceNotifier>();
            _modifiers = new Dictionary<object, List<IResourceModifier>>();
            _allModifiers = new HashSet<IResourceModifier>();
        }

        public static ResourceSchema ApplicationResources { get; private set; }

        public int Count => _allModifiers.Count();

        public bool IsReadOnly => false;

        public void InitializeFromInternalDictionary()
        {
            InitializeFromDictionary(_dictionary);
        }

        public void InitializeFromDictionary(ResourceDictionary dictionary)
        {
            foreach (var key in dictionary.Keys)
            {
                if (!(key is String keyString) || !keyString.Contains("modifier", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var value = _dictionary[key];

                if (value is IResourceModifier modifier)
                    Add(modifier);
            }

            foreach (var item in dictionary.MergedDictionaries)
                InitializeFromDictionary(item);
        }

        public void Add(IResourceModifier item)
        {
            if (!_allModifiers.Add(item))
                return;

            var tempNotifiers = new List<ResourceNotifier>();

            foreach (var key in item.InputKeys)
            {
                if (!_modifiers.TryGetValue(key, out List<IResourceModifier> modifiersList))
                {
                    modifiersList = new List<IResourceModifier>();

                    _modifiers.Add(key, modifiersList);
                }

                modifiersList.Add(item);

                if (!_notifiers.ContainsKey(key))
                {
                    var notifier = new ResourceNotifier(this, key);

                    tempNotifiers.Add(notifier);

                    _notifiers.Add(key, notifier);
                }
            }

            foreach(var notifier in tempNotifiers)
                notifier.Initialize(_dictionary);
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

        internal static void InitializeApplicationResources()
        {
            if (ApplicationResources != null)
                return;

            ApplicationResources = new ResourceSchema(Application.Current.Resources);

            Application.Current.Startup += ApplicationLoadCompleted;
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
                    arguments.Add(_dictionary[targetKey]);

                _dictionary[item.ResultKey] = item.Modify(arguments);
            }
        }

        private static void ApplicationLoadCompleted(object sender, StartupEventArgs arguments)
        {
            ((Application)sender).Startup -= ApplicationLoadCompleted;

            ApplicationResources.InitializeFromInternalDictionary();
        }
    }
}
