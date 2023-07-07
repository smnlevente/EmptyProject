using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static T AssertComponent<T>(this MonoBehaviour currentObject, T checkAgainst) where T : Component
    {
        return checkAgainst != null ? checkAgainst : currentObject.GetComponent<T>();
    }

    public static T AssertComponentInChildren<T>(this MonoBehaviour currentObject, T checkAgainst) where T : MonoBehaviour
    {
        return checkAgainst != null ? checkAgainst : currentObject.GetComponentInChildren<T>();
    }

    public static T AssertComponentInChildren<T>(this MonoBehaviour currentObject, T checkAgainst, bool includeInactive) where T : MonoBehaviour
    {
        return checkAgainst != null ? checkAgainst : currentObject.GetComponentInChildren<T>(includeInactive);
    }
}