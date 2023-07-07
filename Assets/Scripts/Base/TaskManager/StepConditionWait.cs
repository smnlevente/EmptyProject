namespace TaskManager
{
    public delegate bool UntilTaskPredicate(float elapsedTime);

    public class StepConditionWait : Step
    {
        private UntilTaskPredicate condition;

        public StepConditionWait(UntilTaskPredicate condition)
        {
            this.condition = condition;
        }

        public override bool IsCompleted()
        {
            if (this.condition == null || this.condition(this.Timer))
            {
                return true;
            }

            return false;
        }
    }
}