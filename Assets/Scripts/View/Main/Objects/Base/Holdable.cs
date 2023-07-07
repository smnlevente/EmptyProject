using UnityEngine;
using UnityEngine.EventSystems;

public class Holdable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private IHold hold;
    private bool down = false;
    private float currentTime = 0f;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.currentTime = 0;
        this.down = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.down = false;
    }

    public void AttachHold(IHold hold)
    {
        this.hold = hold;
    }

    public void DetachHold()
    {
        this.hold = null;
    }

    private void Update()
    {
        if (!this.down || this.hold == null)
        {
            return;
        }

        this.currentTime += Time.deltaTime;

        if (this.currentTime >= this.hold.GetActivateTime())
        {
            this.hold.OnHold();
            this.currentTime = 0;
        }
    }
}
