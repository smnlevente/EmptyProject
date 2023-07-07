using System;
using System.Collections.Generic;
using State;
using TaskManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private IDraggable drag;
    private IPressable press;
    private Vector2 delta;
    private Vector2 screenDelta;
    private Task tap;
    private StateMachine<Draggable> tapState;
    private float currentPressTime = 0;
    private float totalPressTime = 0;
    private bool dragEnabled = false;
    private Vector2 currentPosition;
    private Vector2 size;
    private bool applicationQuitting = false;

    public bool IsDragged
    {
        get
        {
            return this.dragEnabled;
        }
    }

    public void AttachDrag(IDraggable drag, Vector2 size)
    {
        this.size = size;
        this.drag = drag;
        this.totalPressTime = this.drag.DragStartDelay();
    }

    public void AttachPress(IPressable press)
    {
        this.press = press;
    }

    public void SetDragState(bool state)
    {
        this.dragEnabled = state;
        if (this.dragEnabled)
        {
            if (this.drag != null)
            {
                this.drag.OnDragStart(transform.position);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            }
        }
        else
        {
            if (this.drag != null)
            {
                this.drag.OnDragEnd(transform.position);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.currentPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        this.screenDelta = eventData.delta;
        this.StopTapTask();
        this.ChangeTapState(DragState.Instance);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.delta = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position) - this.currentPosition;
        this.currentPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        this.screenDelta = eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.screenDelta = eventData.delta;
        this.StopTapTask();
        this.ChangeTapState(IdleState.Instance);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.screenDelta = eventData.delta;
        this.tap = TaskController.Instance.WaitFor(0.1f).Then(() => this.ChangeTapState(PressState.Instance));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.currentPosition = Camera.main.ScreenToWorldPoint(Vector2.one);
        this.screenDelta = eventData.delta;
        if (this.tap != null && !this.tap.IsComplete)
        {
            this.ChangeTapState(PressState.Instance);
        }

        this.StopTapTask();
        this.ChangeTapState(IdleState.Instance);
    }

    private void ChangeTapState(IState<Draggable> state)
    {
        if (this.tapState != null)
        {
            this.tapState.ChangeState(state);
        }
    }

    private void StopTapTask()
    {
        if (this.tap != null)
        {
            this.tap.Stop();
        }
    }
    
    private void Start()
    {
        this.tapState = new StateMachine<Draggable>(this);
        this.ChangeTapState(IdleState.Instance);
    }

    private void OnDestroy()
    {
        if (this.applicationQuitting)
        {
            return;
        }

        this.ChangeTapState(IdleState.Instance);
    }

    private void OnApplicationQuit()
    {
        this.applicationQuitting = true;
    }

    private sealed class IdleState : IState<Draggable>
    {
        private IdleState()
        {
        }

        public static IdleState Instance
        {
            get
            {
                return new IdleState();
            }
        }

        public List<Type> TransitionIn()
        {
            return null;
        }

        public void Enter(Draggable owner)
        {
        }

        public void Update(Draggable owner)
        {
        }

        public void Exit(Draggable owner)
        {
        }
    }

    private sealed class DragState : IState<Draggable>
    {
        private DragState()
        {
        }

        public static DragState Instance
        {
            get
            {
                return new DragState();
            }
        }

        public List<Type> TransitionIn()
        {
            return null;
        }

        public void Enter(Draggable owner)
        {
        }

        public void Update(Draggable owner)
        {
            if (owner.dragEnabled)
            {
                Vector3 newPosition = owner.transform.position + (Vector3)owner.delta;
                if (this.InLimits(newPosition, owner))
                {
                    owner.transform.position = newPosition;
                    if (owner.drag != null)
                    {
                        owner.drag.OnDragUpdate(owner.transform.position);
                    }
                }
            }
            else
            {
                CameraController.Instance.MoveBy(owner.screenDelta.x, owner.screenDelta.y);
            }

            owner.delta = Vector2.zero;
            owner.screenDelta = Vector2.zero;
        }

        public void Exit(Draggable owner)
        {
            if (!owner.dragEnabled)
            {
                CameraController.Instance.MoveBy(owner.screenDelta.x, owner.screenDelta.y);
            }
        }

        private bool InLimits(Vector3 position, Draggable owner)
        {
            Vector2 centerPosition = position;
            Vector2 inputPosition = owner.currentPosition;
            Bounds bounds = new Bounds(centerPosition + (owner.size / 2f), owner.size);
            return bounds.Contains(inputPosition);
        }
    }

    private sealed class PressState : IState<Draggable>
    {
        private PressState()
        {
        }

        public static PressState Instance
        {
            get
            {
                return new PressState();
            }
        }

        public List<Type> TransitionIn()
        {
            return null;
        }

        public void Enter(Draggable owner)
        {
            if (!owner.dragEnabled)
            {
                owner.currentPressTime = 0f;
                if (owner.press != null)
                {
                    if (owner.GetComponent<Collider2D>() != null)
                    {
                        Vector3 position = owner.GetComponent<Collider2D>().bounds.center;
                        owner.press.OnPressStart(position);
                        return;
                    }

                    owner.press.OnPressStart(owner.transform.position);
                }
            }
        }

        public void Update(Draggable owner)
        {
            if (!owner.dragEnabled)
            {
                owner.currentPressTime += Time.deltaTime;
                float percent = owner.currentPressTime / owner.totalPressTime;
                if (owner.press != null)
                {
                    owner.press.OnPressUpdate(owner.transform.position, percent);
                }

                if (percent >= 1f)
                {
                    if (owner.press != null)
                    {
                        owner.press.OnPressEnd(owner.transform.position);
                    }

                    owner.dragEnabled = true;
                    if (owner.drag != null)
                    {
                        owner.drag.OnDragStart(owner.transform.position);
                    }

                    owner.transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y, owner.transform.position.z - 1);
                }
            }
        }

        public void Exit(Draggable owner)
        {
            if (!owner.dragEnabled)
            {
                if (owner.press != null)
                {
                    owner.press.OnPressEnd(owner.transform.position);
                }
            }
        }
    }
}