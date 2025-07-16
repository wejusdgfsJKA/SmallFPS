using EventBus;
using UnityEngine;
namespace Detection
{
    /// <summary>
    /// This is meant to fire whenever the entity's sound level changes.
    /// </summary>
    public struct OnSoundChanged : IEvent
    {
        public float Value;
        public OnSoundChanged(float Value)
        {
            this.Value = Value;
        }
    }
    public abstract class DetectableTarget : MonoBehaviour
    {
        /// <summary>
        /// How loud the entity is.
        /// </summary>
        public float CurrentSound { get; protected set; } = 0;
        protected virtual void OnEnable()
        {
            RegisterEventBindings();
        }
        public void UpdateSound(OnSoundChanged @event)
        {
            CurrentSound += @event.Value;
        }
        public virtual void RegisterEventBindings()
        {
            EventBus<OnSoundChanged>.AddBinding(transform.GetInstanceID());
        }
    }
}