using UnityEngine;

public interface IDraggable
{
    void OnDragStart(Vector3 position);

    void OnDragUpdate(Vector3 position);

    void OnDragEnd(Vector3 position);

    float DragStartDelay();
}
