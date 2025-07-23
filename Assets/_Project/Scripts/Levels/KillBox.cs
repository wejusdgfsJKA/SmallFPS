using Entity;
using EventBus;
using UnityEngine;
namespace Levels
{
    public class KillBox : MonoBehaviour
    {
        static TakeDamage @event = new TakeDamage(new DmgInfo(9999, null));
        private void OnTriggerEnter(Collider other)
        {
            EventBus<TakeDamage>.Raise(other.transform.root.GetInstanceID(), @event);
        }
    }
}