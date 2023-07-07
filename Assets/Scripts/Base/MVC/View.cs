namespace MVC
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class View : MonoBehaviour
    {
        private int? layer = null;

        private List<GameObject> runtimeChildren = new List<GameObject>();

        public static T[] FindViews<T>() where T : View
        {
            return GameObject.FindObjectsOfType<T>();
        }

        public static T FindView<T>() where T : View
        {
            return FindViews<T>().FirstOrDefault();
        }

        public static T AssertView<T>(T view) where T : View
        {
            return (view == null) ? FindView<T>() : view;
        }

        public static T FindView<T>(string viewID) where T : View
        {
            return FindViews<T>().FirstOrDefault(item => item.GetID() == viewID);
        }

        public static T AssertView<T>(T view, string viewID) where T : View
        {
            return (view == null) ? FindView<T>(viewID) : view;
        }

        public T[] FindViewsInChildren<T>() where T : View
        {
            return this.GetComponentsInChildren<T>(true);
        }

        public T FindViewInChildren<T>() where T : View
        {
            return this.FindViewsInChildren<T>().FirstOrDefault();
        }

        public T FindLastViewInChildren<T>() where T : View
        {
            return this.FindViewsInChildren<T>().LastOrDefault();
        }

        public T AssertViewInChildren<T>(T view) where T : View
        {
            return (view == null) ? this.FindViewInChildren<T>() : view;
        }

        public T FindViewInChildren<T>(string viewID) where T : View
        {
            return this.FindViewsInChildren<T>().FirstOrDefault(item => item.GetID() == viewID);
        }

        public T AssertViewInChildren<T>(T view, string viewID) where T : View
        {
            return (view == null) ? this.FindViewInChildren<T>(viewID) : view;
        }

        public GameObject AddObject(GameObject prefab)
        {
            GameObject newObject = GameObject.Instantiate(prefab);
            newObject.transform.SetParent(this.transform);
            newObject.transform.localScale = prefab.transform.localScale;
            this.runtimeChildren.Add(newObject);
            return newObject;
        }

        public GameObject AddObject(GameObject prefab, Vector2 position)
        {
            GameObject newObject = GameObject.Instantiate(prefab, new Vector3(position.x, position.y, prefab.transform.position.z), Quaternion.identity) as GameObject;
            newObject.transform.SetParent(this.transform);
            this.runtimeChildren.Add(newObject);
            return newObject;
        }

        public GameObject AddObject(GameObject prefab, Vector3 position)
        {
            GameObject newObject = GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;
            newObject.transform.SetParent(this.transform);
            this.runtimeChildren.Add(newObject);
            return newObject;
        }

        public T AddObject<T>(GameObject prefab) where T : View
        {
            GameObject newObject = this.AddObject(prefab);
            return newObject.GetComponentInChildren<T>(true);
        }

        public T AddObject<T>(GameObject prefab, Vector2 position)
        {
            GameObject newObject = this.AddObject(prefab, new Vector3(position.x, position.y, prefab.transform.position.z));
            return newObject.GetComponentInChildren<T>(true);
        }

        public T AddObject<T>(GameObject prefab, Vector3 position)
        {
            GameObject newObject = this.AddObject(prefab, position);
            return newObject.GetComponentInChildren<T>(true);
        }

        public virtual void CleanUp()
        {
            foreach (GameObject child in this.runtimeChildren.ToArray())
            {
                this.runtimeChildren.Remove(child);
                GameObject.Destroy(child);
            }
        }

        public void ChangeLayer(string layer)
        {
            this.layer = this.layer ?? (int)this.gameObject.layer;
            this.SetLayer(this.gameObject, LayerMask.NameToLayer(layer));
        }

        public void RestoreLayer()
        {
            this.SetLayer(this.gameObject, (int)(this.layer ?? this.gameObject.layer));
        }

        public virtual string GetID()
        {
            return string.Empty;
        }

        private void SetLayer(GameObject parent, int layer)
        {
            if (parent == null)
            {
                return;
            }

            parent.layer = layer;
            foreach (Transform child in parent.transform)
            {
                if (child == null)
                {
                    continue;
                }

                this.SetLayer(child.gameObject, layer);
            }
        }
    }
}