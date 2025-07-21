using System;
using System.Collections.Generic;

namespace EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        static Dictionary<int, EventBinding<T>> bindings = new();
        static void Clear()
        {
            foreach (var binding in bindings.Values)
            {
                binding.Clear();
            }
            bindings.Clear();
        }
        public static bool Raise(T @event)
        {
            return Raise(0, @event);
        }
        /// <summary>
        /// Raise a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="event">The value of the IEvent parameter.</param>
        /// <returns>True if the binding was found and raised.</returns>
        public static bool Raise(int bindingId, T @event)
        {
            EventBinding<T> binding;
            if (bindings.TryGetValue(bindingId, out binding))
            {
                binding?.Invoke(@event);
                return true;
            }
            return false;
        }
        public static bool AddBinding(int id)
        {
            return bindings.TryAdd(id, new());
        }
        public static bool ClearBinding(int id)
        {
            EventBinding<T> e;
            if (bindings.TryGetValue(id, out e))
            {
                e.Clear();
                return true;
            }
            return false;
        }
        public static bool RemoveBinding(int id)
        {
            EventBinding<T> binding;
            if (bindings.TryGetValue(id, out binding))
            {
                binding.Clear();
                binding = null;
                bindings.Remove(id);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add actions to a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <returns>True if the binding was found.</returns>
        public static bool AddActions(int bindingId, Action<T> action = null, Action actionNoArgs = null)
        {
            EventBinding<T> e;
            if (bindings.TryGetValue(bindingId, out e))
            {
                e.Add(action);
                e.Add(actionNoArgs);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes actions from a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <returns></returns>
        public static bool RemoveActions(int bindingId, Action<T> action = null,
            Action actionNoArgs = null)
        {
            EventBinding<T> e;
            if (bindings.TryGetValue(bindingId, out e))
            {
                e.Remove(action);
                e.Remove(actionNoArgs);
                return true;
            }
            return false;
        }
    }
}