using System;
using System.Collections.Generic;
using System.Linq;
using State;
using TaskManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private IClickable click;
    private float currentPressTime = 0;
    private float totalPressTime = 0;
    private bool dragEnabled = false;
    private bool clicked = false;
    private Vector2 delta;
    private Task tap;
    private StateMachine<Selectable> tapState;
    private PointerEventData eventData;
    private bool offscreen;
    private Draggable draggable;

    private Draggable Draggable
    {
        get
        {
            return this.draggable = this.AssertComponent(this.draggable);
        }
    }

    public void AttachClick(IClickable click)
    {
        this.click = click;
        this.totalPressTime = this.click.ClickTime();
    }

    public void DettachClick()
    {
        this.click = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.delta = eventData.delta;
        this.eventData = eventData;
        this.StopTapTask();
        this.ChangeTapState(DragState.Instance);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.offscreen)
        {
            return;
        }

        Vector2 viewport = Camera.main.ScreenToViewportPoint(eventData.position);
        if (viewport.y > 1f || viewport.y < 0f)
        {
            CameraController.Instance.Stop();
            this.OnPointerUp(this.eventData);
            this.OnEndDrag(this.eventData);
            this.offscreen = true;
            return;
        }

        this.delta = eventData.delta;
        this.eventData = eventData;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.offscreen)
        {
            return;
        }

        this.delta = eventData.delta;
        this.eventData = eventData;
        this.StopTapTask();
        this.ChangeTapState(IdleState.Instance);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CameraController.Instance.Stop();
        this.offscreen = false;
        this.delta = eventData.delta;
        this.tap = TaskController.Instance.WaitFor(0.1f).Then(() => this.ChangeTapState(PressState.Instance));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.offscreen)
        {
            return;
        }

        if (this.tapState.CurrentState is DragState)
        {
            return;
        }

        this.delta = eventData.delta;
        this.eventData = eventData;
        if (this.tap != null && !this.tap.IsComplete)
        {
            this.ChangeTapState(PressState.Instance);
        }

        this.StopTapTask();
        this.ChangeTapState(IdleState.Instance);
    }

    private void ChangeTapState(IState<Selectable> state)
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
        this.tapState = new StateMachine<Selectable>(this);
        this.ChangeTapState(IdleState.Instance);
    }

    private sealed class IdleState : IState<Selectable>
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

        public void Enter(Selectable owner)
        {
            if (!owner.dragEnabled && owner.clicked)
            {
                if (owner.click != null)
                {
                    owner.click.OnClick(owner.transform.position);
                }
            }
        }

        public void Update(Selectable owner)
        {
        }

        public void Exit(Selectable owner)
        {
        }
    }

    private sealed class DragState : IState<Selectable>
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

        public void Enter(Selectable owner)
        {
            owner.dragEnabled = true;
        }

        public void Update(Selectable owner)
        {
        }

        public void Exit(Selectable owner)
        {
            CameraController.Instance.Decelerate(owner.delta.y);
        }
    }

    private sealed class PressState : IState<Selectable>
    {
        private bool tooLong = false;

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

        public void Enter(Selectable owner)
        {
            owner.currentPressTime = 0f;
            this.tooLong = false;
            owner.dragEnabled = false;
        }

        public void Update(Selectable owner)
        {
            owner.currentPressTime += Time.deltaTime;
            if (owner.currentPressTime >= owner.totalPressTime)
            {
                this.tooLong = true;
            } 
        }

        public void Exit(Selectable owner)
        {
            owner.clicked = !this.tooLong;
        }
    }
}
