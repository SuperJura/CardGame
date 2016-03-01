using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class ObservableList<T> : List<T>
    {
        public delegate void OnAddHandler(T item);
        public event OnAddHandler OnAdd;

        public delegate void OnRemoveHandler(T item);
        public event OnRemoveHandler OnRemove;

        public new void Add(T item)
        {
            base.Add(item);
            OnAdd(item);
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            OnRemove(item);
        }
    }
}
