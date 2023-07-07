namespace State
{
    using System;
    using System.Collections.Generic;

    public interface IState<T>
    {
        List<Type> TransitionIn();

        void Enter(T owner);

        void Update(T owner);

        void Exit(T owner);
    }
}