using System;
using System.Collections.Generic;
using System.Linq;
using State;
using TaskManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stationary : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private IClickable click;
    private IDraggable drag;
    private Vector2 delta;
    private Task tap;
    private StateMachine<Stationary> tapState;
    private bool clicked = false;
    private PointerEventData eventData;
    private bool offscreen;

    public void AttachClick(IClickable click)
    {
        this.click = click;
    }

    public void AttachDrag(IDraggable drag)
    {
        this.drag = drag;
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

    private void OnDisable()
    {
        if (this.tapState != null && this.tapState.CurrentState.GetType().Equals(typeof(DragState)))
        {
            this.OnEndDrag(this.eventData);
        }
    }

    private void ChangeTapState(IState<Stationary> state)
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
        this.tapState = new StateMachine<Stationary>(this);
        this.ChangeTapState(IdleState.Instance);
    }

    private sealed class IdleState : IState<Stationary>
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

        public void Enter(Stationary owner)
        {
            if (owner.clicked)
            {
                if (owner.click != null)
                {
                    owner.click.OnClick(Camera.main.ScreenToWorldPoint(owner.eventData.position));
                }
            }

            owner.clicked = false;
        }

        public void Update(Stationary owner)
        {
        }

        public void Exit(Stationary owner)
        {
        }
    }

    private sealed class DragState : IState<Stationary>
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

        public void Enter(Stationary owner)
        {
            owner.clicked = false;
            if (owner.drag != null)
            {
                owner.drag.OnDragStart(owner.transform.position);
            }
        }

        public void Update(Stationary owner)
        {
            CameraController.Instance.MoveBy(owner.delta.x, owner.delta.y);
            owner.delta = Vector2.zero;
        }

        public void Exit(Stationary owner)
        {
            CameraController.Instance.Decelerate(owner.delta.y);
        }
    }

    private sealed class PressState : IState<Stationary>
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

        public void Enter(Stationary owner)
        {
            owner.clicked = true;
        }

        public void Update(Stationary owner)
        {
        }

        public void Exit(Stationary owner)
        {
        }
    }
}
