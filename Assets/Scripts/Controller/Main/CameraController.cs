using System;
using System.Collections.Generic;
using MVC;
using State;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : Controller<CameraController>, IObserver
{
    [SerializeField]
    private float glideSpeed = 5.0f;
    private Vector2 extent;
    private Range softBoundary = new Range(-18f, 20f);
    private Range hardBoundary = new Range(-23f, 25f);
    private StateMachine<CameraController> stateMachine;
    private Physics2DRaycaster raycaster;
    private LayerMask? layerMask;

    public bool IsIdle
    {
        get
        {
            return this.stateMachine.CurrentState.GetType().Equals(typeof(IdleState));
        }
    }

    public Vector2 Extent
    {
        get
        {
            if (Vector2.zero.Equals(this.extent))
            {
                this.extent = new Vector2(5.5f, 5.5f / Camera.main.aspect);
            }

            return this.extent;
        }
    }

    private Physics2DRaycaster Raycaster
    {
        get
        {
            if (this.raycaster == null)
            {
                this.raycaster = Camera.main.GetComponent<Physics2DRaycaster>();
            }

            return this.raycaster;
        }
    }

    public void MoveBy(float x, float y)
    {
        this.MoveBy(new Vector3(x, y, 0.0f));
    }

    public void MoveBy(Vector2 delta)
    {
        this.MoveBy(new Vector3(delta.x, delta.y, 0.0f));
    }

    public void MoveBy(Vector3 delta)
    {
        this.MoveTo(Camera.main.transform.position + (-1.0f * delta * Camera.main.PixelRatio()));
    }

    public void MoveTo(float y)
    {
        this.MoveTo(new Vector3(Camera.main.transform.position.x, y, Camera.main.transform.position.z));
    }

    public void MoveTo(Vector2 location)
    {
        this.MoveTo(new Vector3(location.x, location.y, Camera.main.transform.position.z));
    }

    public void MoveTo(Vector3 location)
    {
        location = this.hardBoundary.Clamp(location);
        this.stateMachine.ChangeState(new MoveState(location));
    }

    public void FocusBy(float y)
    {
        this.FocusBy(new Vector3(0.0f, y, 0.0f));
    }

    public void FocusBy(Vector2 delta)
    {
        this.FocusBy(new Vector3(delta.x, delta.y, 0.0f));
    }

    public void FocusBy(Vector3 delta)
    {
        this.FocusTo(Camera.main.transform.position + (-1.0f * delta * Camera.main.PixelRatio()));
    }

    public void FocusTo(float y)
    {
        this.FocusTo(new Vector3(Camera.main.transform.position.x, y, Camera.main.transform.position.z));
    }

    public void FocusTo(Vector2 location)
    {
        this.FocusTo(new Vector3(location.x, location.y, Camera.main.transform.position.z));
    }

    public void FocusTo(Vector3 location)
    {
        location = this.softBoundary.Clamp(location);
        this.stateMachine.ChangeState(new FocusState(location));
    }

    public void GlideBy(float y)
    {
        this.GlideBy(new Vector3(0.0f, y, 0.0f));
    }

    public void GlideBy(Vector2 delta)
    {
        this.GlideBy(new Vector3(delta.x, delta.y, 0.0f));
    }

    public void GlideBy(Vector3 delta)
    {
        this.GlideTo(Camera.main.transform.position + (-this.glideSpeed * delta * Camera.main.PixelRatio()));
    }

    public void GlideTo(float y)
    {
        this.GlideTo(new Vector3(Camera.main.transform.position.x, y, Camera.main.transform.position.z));
    }

    public void GlideTo(Vector2 location)
    {
        this.GlideTo(new Vector3(location.x, location.y, Camera.main.transform.position.z));
    }

    public void GlideTo(Vector3 location)
    {
        location = this.softBoundary.Clamp(location);
        this.stateMachine.ChangeState(new GlideState(location));
    }

    public void SwipeTo(float y)
    {
        this.SwipeTo(new Vector3(Camera.main.transform.position.x, y, Camera.main.transform.position.z));
    }

    public void SwipeTo(Vector2 location)
    {
        this.SwipeTo(new Vector3(location.x, location.y, Camera.main.transform.position.z));
    }

    public void SwipeTo(Vector3 location)
    {
        location = this.softBoundary.Clamp(location);
        this.stateMachine.ChangeState(new SwipeState(location));
    }

    public void FocusToScreen(float y)
    {
        this.FocusToScreen(new Vector3(Camera.main.transform.position.x, y, Camera.main.transform.position.z));
    }

    public void FocusToScreen(Vector2 location)
    {
        this.FocusToScreen(new Vector3(location.x, location.y, Camera.main.transform.position.z));
    }

    public void FocusToScreen(Vector3 location)
    {
        location = this.softBoundary.Clamp(location);
        this.stateMachine.ChangeState(new FocusToScreenState(location));
    }

    public void FocusToScreen(View view)
    {
        this.FocusToScreen(view.transform.position.y);
    }

    public void Decelerate(float delta)
    {
        this.stateMachine.ChangeState(new DecelerateState(delta));
    }

    public void Stop()
    {
        this.stateMachine.ChangeState(IdleState.Instance);
    }

    public void SetRaycastLayer(string layer)
    {
        this.layerMask = this.layerMask ?? (int)this.Raycaster.eventMask;
        this.Raycaster.eventMask = LayerMask.GetMask(layer);
    }

    public void SetRaycastLayers(string[] layers)
    {
        this.layerMask = this.layerMask ?? (int)this.Raycaster.eventMask;
        this.Raycaster.eventMask = LayerMask.GetMask(layers);
    }

    public void RemoveRaycastLayers()
    {
        this.layerMask = this.layerMask ?? (int)this.Raycaster.eventMask;
        this.Raycaster.eventMask = LayerMask.GetMask(string.Empty);
    }

    public void RestoreRaycastLayers()
    {
        this.Raycaster.eventMask = (int)(this.layerMask ?? this.Raycaster.eventMask);
    }

    public void SetCameraBoundary()
    {
    }

    public void SetCameraBoundary(Range softBoundary, Range hardBoundary)
    {
        this.softBoundary = softBoundary;
        this.hardBoundary = hardBoundary;
    }

    public void ExpansionUnlocked()
    {
        this.SetCameraBoundary();
    }

    public void WaterUnlocked()
    {
        this.SetCameraBoundary();
    }

    private void OnMainSceneActive()
    {
        this.SetCameraBoundary();
    }

    private void OnRightSceneLoaded()
    {
        this.SetCameraBoundary(new Range(-18.5f, 83.5f), new Range(-23.5f, 88.5f));
    }

    private void Start()
    {
        this.stateMachine = new StateMachine<CameraController>(this);
        this.stateMachine.ChangeState(IdleState.Instance);
        Camera.main.orthographicSize = this.Extent.y;
    }

    private sealed class IdleState : IState<CameraController>
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
            return new List<Type>()
            {
                typeof(MoveState), typeof(FocusState), typeof(FocusToScreenState), typeof(GlideState), typeof(SwipeState)
            };
        }

        public void Enter(CameraController owner)
        {
            if (owner.softBoundary == null)
            {
                return;
            }

            if (owner.softBoundary.Max < Camera.main.transform.position.y)
            {
                owner.GlideTo(owner.softBoundary.Max);
            }
            else if (owner.softBoundary.Min > Camera.main.transform.position.y)
            {
                owner.GlideTo(owner.softBoundary.Min);
            }
        }

        public void Update(CameraController owner)
        {
        }

        public void Exit(CameraController owner)
        {
        }
    }

    private class MoveState : IState<CameraController>
    {
        private Vector3 target;

        public MoveState(Vector3 target)
        {
            this.target = target;
        }

        public List<Type> TransitionIn()
        {
            return new List<Type>()
            {
                typeof(GlideState), typeof(MoveState), typeof(DecelerateState), typeof(SwipeState)
            };
        }

        public void Enter(CameraController owner)
        {
            Camera.main.transform.position = this.target;
        }

        public void Update(CameraController owner)
        {
        }

        public void Exit(CameraController owner)
        {
        }
    }

    private sealed class FocusState : SmoothDampState
    {
        private const float SmoothTime = 0.3f;

        public FocusState(Vector3 target) : base(target, SmoothTime)
        {
        }
    }

    private sealed class FocusToScreenState : IState<CameraController>
    {
        private float smoothTime = 0.3f;
        private Vector3 target;
        private Vector3 velocity;

        public FocusToScreenState(Vector3 target)
        {
            this.target = target;
        }

        public List<Type> TransitionIn()
        {
            return new List<Type>()
            {
                typeof(IdleState), typeof(FocusState)
            };
        }

        public void Enter(CameraController owner)
        {
            this.velocity = Vector3.zero;
        }

        public void Update(CameraController owner)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, this.target, ref this.velocity, this.smoothTime);
            if (Vector3.Distance(Camera.main.transform.position, this.target) < 3.5f)
            {
                owner.stateMachine.ChangeState(IdleState.Instance);
            }
        }

        public void Exit(CameraController owner)
        {
        }
    }

    private class SwipeState : IState<CameraController>
    {
        private Vector3 target;
        private float speed;

        public SwipeState(Vector3 target)
        {
            this.target = target;
        }

        public void Enter(CameraController owner)
        {
            this.speed = Vector3.Distance(Camera.main.transform.position, this.target);
            owner.RemoveRaycastLayers();
        }

        public void Exit(CameraController owner)
        {
            owner.NotifyAll("camera_swipe_finished");
            owner.RestoreRaycastLayers();
        }

        public List<Type> TransitionIn()
        {
            return new List<Type>()
            {
                typeof(GlideState), typeof(SwipeState)
            };
        }

        public void Update(CameraController owner)
        {
            float diff = Vector3.Distance(Camera.main.transform.position, this.target);
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, this.target, Mathf.Pow(diff, 0.5f) * this.speed * Time.deltaTime);

            if (Vector3.Distance(Camera.main.transform.position, this.target) < 0.1f)
            {
                Camera.main.transform.position = this.target;
                owner.GlideTo(this.target);
            }
        }
    }

    private class GlideState : IState<CameraController>
    {
        private Vector3 target;
        private float speed = 10f;

        public GlideState(Vector3 target)
        {
            this.target = target;
        }

        public void Enter(CameraController owner)
        {
        }

        public void Exit(CameraController owner)
        {
        }

        public List<Type> TransitionIn()
        {
            return new List<Type>()
            {
                typeof(IdleState), typeof(FocusState), typeof(MoveState), typeof(SwipeState)
            };
        }

        public void Update(CameraController owner)
        {
            float diff = Vector3.Distance(Camera.main.transform.position, this.target);
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, this.target, diff * this.speed * Time.deltaTime);

            if (Vector3.Distance(Camera.main.transform.position, this.target) < 0.1f)
            {
                Camera.main.transform.position = this.target;
                owner.stateMachine.ChangeState(IdleState.Instance);
            }
        }
    }

    private abstract class SmoothDampState : IState<CameraController>
    {
        private Vector3 target;
        private float smoothTime;
        private Vector3 velocity;

        public SmoothDampState(Vector3 target, float smoothTime)
        {
            this.target = target;
            this.smoothTime = smoothTime;
        }

        public List<Type> TransitionIn()
        {
            return new List<Type>()
            {
                typeof(IdleState), typeof(FocusState), typeof(MoveState)
            };
        }

        public void Enter(CameraController owner)
        {
            this.velocity = Vector3.zero;
        }

        public void Update(CameraController owner)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, this.target, ref this.velocity, this.smoothTime);
            if (Vector3.Distance(Camera.main.transform.position, this.target) < 0.1f)
            {
                owner.stateMachine.ChangeState(IdleState.Instance);
            }
        }

        public void Exit(CameraController owner)
        {
        }
    }

    private class DecelerateState : IState<CameraController>
    {
        private static float? coefficient;
        private static Vector2 firstPoint = new Vector2(25f, 5f);
        private static Vector2 secondPoint = new Vector2(70f, 3f);
        private float deceleration = 0f;
        private Vector3 currentDeceleration = Vector3.zero;
        private float limit = 0.3f;
        private float hardDecelerat = 10f;

        public DecelerateState(float delta)
        {
            this.currentDeceleration = Camera.main.velocity;

            if (Camera.main.velocity.magnitude < this.limit)
            {
                this.currentDeceleration = new Vector3(0f, delta, 0f);
            }
        }

        private float Coefficient
        {
            get
            {
                if (!coefficient.HasValue)
                {
                    coefficient = (secondPoint.y - firstPoint.y) / (secondPoint.x - firstPoint.x);
                }

                return coefficient.Value;
            }
        }

        public List<Type> TransitionIn()
        {
            return new List<Type>()
            {
                typeof(GlideState), typeof(IdleState), typeof(SwipeState)
            };
        }

        public void Enter(CameraController owner)
        {
            this.deceleration = this.GetStartDeceleration(owner.softBoundary.Max - owner.softBoundary.Min);
        }

        public void Update(CameraController owner)
        {
            Vector3 newPosition = Camera.main.transform.position + (this.currentDeceleration * Time.deltaTime);

            if (this.OutOfBounds(owner, newPosition))
            {
                return;
            }

            Camera.main.transform.position = newPosition;
            this.Decelerate(owner);
        }

        public void Exit(CameraController owner)
        {
        }

        private float GetStartDeceleration(float distance)
        {
            return (this.Coefficient * (distance - firstPoint.x)) + firstPoint.y;
        }

        private bool OutOfBounds(CameraController owner, Vector3 newPosition)
        {
            if (owner.hardBoundary.Max < newPosition.y && this.currentDeceleration.y >= 0f)
            {
                owner.GlideTo(owner.softBoundary.Max);
                return true;
            }

            if (owner.hardBoundary.Min >= newPosition.y && this.currentDeceleration.y <= 0f)
            {
                owner.GlideTo(owner.softBoundary.Min);
                return true;
            }

            return false;
        }

        private void Decelerate(CameraController owner)
        {
            this.currentDeceleration = Vector3.Slerp(this.currentDeceleration, Vector3.zero, Time.deltaTime * this.deceleration);

            if (!owner.softBoundary.IsInRange(Camera.main.transform.position.y))
            {
                this.deceleration *= this.hardDecelerat;
            }

            if (Camera.main.velocity.magnitude < this.limit)
            {
                owner.stateMachine.ChangeState(IdleState.Instance);
            }
        }
    }
}