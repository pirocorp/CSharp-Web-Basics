namespace SimpleMvc.Framework.Models
{
    using System.Collections.Generic;

    public class ViewModel
    {
        private readonly IDictionary<string, string> _data;

        public ViewModel()
        {
            this._data = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data => this._data;

        public string this[string key]
        {
            get => this._data[key];
            set => this._data[key] = value;
        }
    }
}
