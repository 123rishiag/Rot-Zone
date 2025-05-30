using System;
using System.Collections.Generic;

namespace Game.Utility
{
    public class GenericObjectPool<T> where T : class
    {
        public List<PooledItem<T>> pooledItems = new List<PooledItem<T>>();

        public virtual T GetItem<U>() where U : T
        {
            if (pooledItems.Count > 0)
            {
                PooledItem<T> item = pooledItems.Find(item => !item.isUsed && item.Item is U);
                if (item != null)
                {
                    item.isUsed = true;
                    return item.Item;
                }
            }
            return CreateNewPooledItem<U>();
        }

        private T CreateNewPooledItem<U>() where U : T
        {
            PooledItem<T> newItem = new PooledItem<T>();
            newItem.Item = CreateItem<U>();
            newItem.isUsed = true;
            pooledItems.Add(newItem);
            return newItem.Item;
        }

        protected virtual T CreateItem<U>() where U : T
        {
            throw new NotImplementedException("CreateItem() method not implemented in derived class");
        }

        public virtual void ReturnItem(T _item)
        {
            PooledItem<T> pooledItem = pooledItems.Find(i => i.Item.Equals(_item));
            pooledItem.isUsed = false;
        }

        public class PooledItem<T>
        {
            public T Item;
            public bool isUsed;
        }
    }
}