using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GenericApp
{

    public class MultiDictionary<K, V> : IMultiDictionary<K, V>, IEnumerable<KeyValuePair<K, V>> where V : new() //Missing: where K : struct
    {
        
        Dictionary<K, LinkedList<V>> _dic = new Dictionary<K, LinkedList<V>>();
        
        public void Add(K key, V value)
        {
            var type = key.GetType();
            var res = type.GetCustomAttribute<KeyAttribute>();
            if (value != null && null != res)
            {            
                if (!_dic.ContainsKey(key))
                {
                    LinkedList<V> l = new LinkedList<V>();
                    l.AddLast(value);
                    _dic.Add(key, l);
                }
                else
                {
                    if (!_dic[key].Contains(value))
                    {
                        _dic[key].AddLast(value);
                    }    
                }
            }
            else
            {
                throw new ArgumentNullException();
            }             
        }


        public void CreateNewValue(K key)
        {
            V newValue = new V();
            Add(key, (V) newValue);
        }


        public bool Remove(K key)
        {
            return _dic.Remove(key);
        }


        public bool Remove(K key, V value)
        {
            if (value != null && key != null)
                return _dic[key].Remove(value);
            else
            {
                throw new ArgumentNullException();
            }
        }


        public void Clear()
        {
            foreach (KeyValuePair<K, LinkedList<V>> a in _dic)
            {
                a.Value.Clear();
            }
            _dic.Clear();
        }


        public bool ContainsKey(K key)
        {
            return _dic.ContainsKey(key);
        }


        public bool Contains(K key, V value)
        {
            if (value!=null)
            {
                if (ContainsKey(key))
                    return _dic[key].Contains(value);
                else
                    return false;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }


        public ICollection<K> Keys
        {
            get
            {
                LinkedList<K> keys = new LinkedList<K>(_dic.Keys);
                return keys;
            }
        }


        public ICollection<V> Values
        {
            get
            {
                LinkedList<V> keys = new LinkedList<V>();
                foreach (KeyValuePair<K, LinkedList<V>> a in _dic)
                {
                    LinkedList<V> keysB = new LinkedList<V>();
                    keysB = a.Value;
                    foreach(var k in keysB)
                        if (!keys.Contains(k))
                        {
                            keys.AddFirst(k);
                        }
                }
                return keys;
            }
        }


        public int Count
        {
            get
            {
                int count=0;
                foreach (KeyValuePair<K, LinkedList<V>> a in _dic)
                {
                    count+= a.Value.Count();
                }
                return count;
            }
        }        
        

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (var d in _dic)
            {               
                foreach (var v in d.Value)
                    yield return new KeyValuePair<K, V>(d.Key, v);
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }        
    }


    [AttributeUsage(AttributeTargets.All)]
    public class KeyAttribute : Attribute
    {
    }

}
