namespace TaskManager
{
    using System;
    using System.Collections.Generic;

    public class Task
    {
        private readonly Queue<Step> steps = new Queue<Step>();

        public bool IsComplete 
        { 
            get 
            { 
                return this.steps.Count == 0; 
            }
        }

        public Task Then(Action action)
        {
            this.steps.Enqueue(new StepAction(action));
            return this;
        }

        public Task ThenWaitFor(float seconds)
        {
            this.steps.Enqueue(new StepTimeWait(seconds));
            return this;
        }
            
        public Task ThenWaitUntil(UntilTaskPredicate condition)
        {
            this.steps.Enqueue(new StepConditionWait(condition));
            return this;
        }

        public Task WaitForNextFrame()
        {
            this.steps.Enqueue(new StepTimeWait(0.01f));
            return this;
        }

        public void Stop()
        {
            this.steps.Clear();
        }

        public void Update()
        {
            if (this.IsComplete)
            {
                return;
            }

            Step step = this.steps.Peek();
            step.Update();
            if (step.IsCompleted()) 
            {
                if (this.IsComplete)
                {
                    return;
                }

                this.steps.Dequeue();
            }
        }
    }
}