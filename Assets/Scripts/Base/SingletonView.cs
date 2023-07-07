using UnityEngine;

public abstract class SingletonView<T> : Singleton<T> 
    where T : MonoBehaviour
{
}
