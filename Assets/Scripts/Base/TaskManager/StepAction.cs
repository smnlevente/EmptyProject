namespace TaskManager
{
    using System;

    public class StepAction : Step
    {
        private Action action;

        public StepAction(Action action)
        {
            this.action = action;
        }

        public override bool IsCompleted()
        {
            if (this.action != null)
            {
                this.action();
            }

            return true;
        }
    }
}
