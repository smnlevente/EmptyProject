namespace State
{
    using System;
    using System.Linq;

    public class StateMachine<T>
    {
        private T owner;

        private IState<T> currentState;
        private IState<T> previousState;
        private IState<T> nextState;
        private IState<T> globalState;

        public StateMachine(T owner)
        {
            this.owner = owner;
        }

        public IState<T> CurrentState
        {
            get { return this.currentState; }
        }

        public IState<T> PreviousState
        {
            get { return this.previousState; }
        }

        public IState<T> NextState
        {
            get { return this.nextState; }
        }

        public IState<T> GlobalState
        {
            get { return this.globalState; }
        }

        public bool IsInState(string type)
        {
            return this.IsInState(Type.GetType(type));
        }

        public bool IsInState(IState<T> state)
        {
            return this.IsInState(state.GetType());
        }

        public bool IsInState(Type type)
        {
            return (this.currentState != null) && (this.currentState.GetType() == type);
        }

        public bool IsInGlobalState(string type)
        {
            return this.IsInGlobalState(Type.GetType(type));
        }

        public bool IsInGlobalState(IState<T> state)
        {
            return this.IsInGlobalState(state.GetType());
        }

        public bool IsInGlobalState(Type type)
        {
            return (this.globalState != null) && (this.globalState.GetType() == type);
        }

        public void ChangeState(IState<T> newState)
        {
            if (!this.CanChangeState(this.currentState, newState))
            {
                return;
            }

            this.nextState = newState;
            this.previousState = this.currentState;
            if (this.currentState != null)
            {
                this.currentState.Exit(this.owner);
            }

            this.currentState = newState;
            this.nextState = null;
            this.currentState.Enter(this.owner);
            TaskController.Instance.WaitUntil(time =>
            {
                if (this.currentState != newState)
                {
                    return true;
                }

                this.currentState.Update(this.owner);
                return false;
            });
        }

        public void RevertToPreviousState()
        {
            this.ChangeState(this.previousState);
        }

        public void ChangeGlobalState(IState<T> newGlobalState)
        {
            if (!this.CanChangeState(this.globalState, newGlobalState))
            {
                return;
            }

            if (this.globalState != null)
            {
                this.globalState.Exit(this.owner);
            }

            this.globalState = newGlobalState;
            this.globalState.Enter(this.owner);
            TaskController.Instance.WaitUntil(time =>
            {
                if (this.globalState != newGlobalState)
                {
                    return true;
                }

                this.globalState.Update(this.owner);
                return false;
            });
        }

        private bool CanChangeState(IState<T> current, IState<T> next)
        {
            if (next == null)
            {
                return false;
            }

            if (current != null)
            {
                if ((current.TransitionIn() != null || current.GetType().Equals(next.GetType()))
                    && (current.TransitionIn() == null || !current.TransitionIn().Contains(next.GetType())))
                {
                    return false;
                }
            }

            return true;
        }
    }
}