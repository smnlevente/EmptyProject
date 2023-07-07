namespace MVC
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public abstract class Controller<T> : Singleton<T> where T : MonoBehaviour
    {
        private HashSet<IObserver> observers = new HashSet<IObserver>();

        public void Attach(IObserver observer)
        {
            this.observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this.observers.Remove(observer);
        }

        public void NotifyAll(string notification, params object[] parameters)
        {
            string methodName = string.Format("On{0}", notification);
            foreach (IObserver observer in this.observers.ToArray())
            {
                if (observer.Equals(default(IObserver)))
                {
                    this.observers.Remove(observer);
                    continue;
                }

                MethodInfo method = observer.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(observer, parameters);
                }
            }
        }

        public void NotifyAll(string notification, object parameter)
        {
            this.NotifyAll(notification, new object[] { parameter });
        }

        public void NotifyAll(string notification)
        {
            this.NotifyAll(notification, null);
        }
    }
}
