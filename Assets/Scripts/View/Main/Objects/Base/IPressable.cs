using UnityEngine;

public interface IPressable
{
    void OnPressStart(Vector3 position);

    void OnPressUpdate(Vector3 position, float percent);

    void OnPressEnd(Vector3 position);
}
