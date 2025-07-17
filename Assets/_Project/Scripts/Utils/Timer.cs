using System;

namespace Utilities
{
    public abstract class Timer
    {
        protected float initialTime;
        protected float currentTime { get; set; }
        public bool IsRunning { get; protected set; }

        public float Progress => currentTime / initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            if (initialTime <= 0)
            {
                throw new ArgumentException("Timer duration cannot be set below 0!");
            }
            IsRunning = false;
        }
        /// <summary>
        /// Resets timer duration. Starts the timer if the timer is not running.
        /// </summary>
        public void Start()
        {
            currentTime = initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;

        public abstract void Tick(float deltaTime);
    }

    public class CountdownTimer : Timer
    {
        public CountdownTimer(float value) : base(value) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && currentTime > 0)
            {
                currentTime -= deltaTime;
            }

            if (IsRunning && currentTime <= 0)
            {
                Stop();
            }
        }

        public bool IsFinished => currentTime <= 0;

        public void Reset() => currentTime = initialTime;

        public void Reset(float newTime)
        {
            initialTime = newTime;
            Reset();
        }
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                currentTime += deltaTime;
            }
        }

        public void Reset() => currentTime = 0;

        public float GetTime() => currentTime;
    }
}