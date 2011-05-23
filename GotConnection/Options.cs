using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GotConnection
{
    /// <summary>
    ///
    /// </summary>
    public class Options
    {
        private readonly Dictionary<string, object> _options;

        public Options()
        {
            _options = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public Options(IDictionary<string, object> options)
        {
            _options = new Dictionary<string, object>(options, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Inspiration taken from System.Web.Routing.RouteValueDictionary to allow for anonymous object consumption
        /// </summary>
        /// <param name="options"></param>
        public Options(object options)
        {
            _options = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            AddValues(options);
        }

        private void AddValues(object options)
        {
            if (options != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(options))
                {
                    Add(descriptor.Name, descriptor.GetValue(options));
                }
            }
        }

        public void Add(string key, object value)
        {
            _options.Add(key, value);
        }

        public object this[string key]
        {
            get { return _options.ContainsKey(key) ? _options[key] : null; }
            set { _options[key] = value; }
        }
        
        public int Count
        {
            get { return _options.Count; }
        }

        public bool Contains(string key)
        {
            return _options.ContainsKey(key);
        }
    }
}
