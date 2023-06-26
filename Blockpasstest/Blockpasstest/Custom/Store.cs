using System;
using System.Threading.Tasks;
using Blockpasstest.Custom;
using System.Collections.Generic;

namespace Blockpasstest.Custom
{
	public class Store
	{
        private Dictionary<string, object> EDS = new Dictionary<string, object>();
        private Dictionary<string, List<Action<object>>> subscribers = new Dictionary<string, List<Action<object>>>();

        public Store() {
            var storeItems = new List<dynamic>();

            var groupsSlice = CreateSlice("groups", new
            {
                loading = false,
                groups = new List<Custom.RPC.Group> { }
            });
            storeItems.Add(groupsSlice);

            var passwordsSlice = CreateSlice("passwords", new
            {
                loading = false,
                passwords = new List<Custom.RPC.Password> { }
            });
            storeItems.Add(passwordsSlice);

            this.SetupStore(storeItems);
        }

        public async Task InitialLoad() {
            var rpc = new RPC();

            this.SetDataInStore("passwords", new
            {
                loading = true,
                passwords = new List<Custom.RPC.Password> { }
            });
            var userPasswords = await rpc.getUserPasswords();
            this.SetDataInStore("passwords", new
            {
                loading = false,
                passwords = userPasswords
            });

            this.SetDataInStore("groups", new
            {
                loading = true,
                groups = new List<Custom.RPC.Group> { },
            });
            var userGroups = await rpc.getUserGroups();

            this.SetDataInStore("groups", new {
                loading = false,
                groups = userGroups
            });
        }

        public void SetupStore(List<object> storeItems)
        {
            foreach (var item in storeItems)
            {
                var slice = item as Slice;
                if (slice != null)
                {
                    EDS[slice.Name] = slice.InitialState;
                }
            }
        }

        public Slice CreateSlice(string name, object initialState)
        {
            return new Slice(name, initialState);
        }

        public void SetDataInStore(string name, object data)
        {
            if (EDS.ContainsKey(name))
            {
                EDS[name] = data;
                NotifySubscribers(name, data);
            }
        }

        public void AddDataInArrayByObjectKey<TState>(string name, string key, TState data)
        {
            var storedSlice = this.GetDataFromStore(name);
            //if (storedSlice != null)
            //{
            var slice = storedSlice.GetType().GetProperty(key)?.GetValue(storedSlice);
                if (slice is List<TState> list)
                {
                    list.Add(data);
                    this.SetDataInStore(name, storedSlice);
                }
                else
                {
                    Console.WriteLine("Error");
                }
            //}
        }

        public object GetDataFromStore(string name)
        {
            if (EDS.ContainsKey(name))
            {
                return EDS[name];
            }

            return null;
        }

        public void Subscribe(string name, Action<object> callback)
        {
            if (!subscribers.ContainsKey(name))
            {
                subscribers[name] = new List<Action<object>>();
            }

            subscribers[name].Add(callback);
        }

        public void Unsubscribe(string name, Action<object> callback)
        {
            if (subscribers.ContainsKey(name))
            {
                subscribers[name].Remove(callback);
            }
        }

        private void NotifySubscribers(string name, object value)
        {
            if (subscribers.ContainsKey(name))
            {
                foreach (var callback in subscribers[name])
                {
                    callback(value);
                }
            }
        }

        public class Slice
        {
            public string Name { get; }
            public object InitialState { get; }

            public Slice(string name, object initialState)
            {
                Name = name;
                InitialState = initialState;
            }
        }

        public TState useStoreState<TState>(string slice, string key)
        {
            var storedSlice = App.Storage.GetDataFromStore(slice);
            var sliceValueByKey = storedSlice.GetType().GetProperty(key)?.GetValue(storedSlice);
            if (sliceValueByKey != null && sliceValueByKey is TState converted)
            {
                return converted;
            }
            return (TState)Activator.CreateInstance(typeof(TState));
        }
    }
}

