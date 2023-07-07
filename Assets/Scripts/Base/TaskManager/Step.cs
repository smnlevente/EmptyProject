namespace TaskManager
{
    using UnityEngine;

    public abstract class Step
    {
        private float timer;

        public float Timer
        {
            get
            {
                return this.timer;
            }
        }

        public void Update()
        {
            this.timer += Time.deltaTime;
        }

        public abstract bool IsCompleted();
    }
}