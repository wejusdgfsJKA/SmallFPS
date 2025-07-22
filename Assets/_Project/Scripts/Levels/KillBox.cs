using UnityEngine;
namespace Levels
{
    public class KillBox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.SetActive(false);
        }
    }
}