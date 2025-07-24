using System;

namespace EventBus
{
    public interface IEvent { }

    public class EventBinding<T> where T : IEvent
    {
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };

        public EventBinding() { }
        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;
        /// <summary>
        /// Add an action with no arguments to this binding.
        /// </summary>
        /// <param name="onEvent">The action to add.</param>
        public void Add(Action onEvent) => onEventNoArgs += onEvent;
        /// <summary>
        /// Remove an action with no arguments from this binding.
        /// </summary>
        /// <param name="onEvent">The action to remove.</param>
        public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

        /// <summary>
        /// Add an action to this binding.
        /// </summary>
        /// <param name="onEvent">The action to add.</param>
        public void Add(Action<T> onEvent) => this.onEvent += onEvent;
        /// <summary>
        /// Remove an action from this binding.
        /// </summary>
        /// <param name="onEvent">The action to remove.</param>
        public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;

        /// <summary>
        /// Invoke an event on this binding.
        /// </summary>
        /// <param name="event">The event to invoke.</param>
        public void Invoke(T @event)
        {
            onEvent?.Invoke(@event);
            onEventNoArgs?.Invoke();
        }
        /// <summary>
        /// Clear this binding. Sets all delegates to null.
        /// </summary>
        public void Clear()
        {
            onEvent = null;
            onEventNoArgs = null;
        }
    }
}