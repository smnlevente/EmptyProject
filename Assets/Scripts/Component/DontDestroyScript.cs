using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    private static DontDestroyScript instance;

    private void Awake()
    {
        if (instance != null && instance.gameObject.name == this.gameObject.name)
        {
            Object.DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
            Object.DontDestroyOnLoad(this.gameObject);
        }
    }
}
