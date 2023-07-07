namespace TaskManager
{
    public class StepTimeWait : Step
    {
        private float length;

        public StepTimeWait(float length)
        {
            this.length = length;
        }

        public override bool IsCompleted()
        {
            return this.Timer >= this.length;
        }
    }
}