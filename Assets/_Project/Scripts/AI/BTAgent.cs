using Entity;
using EventBus;
using KBCore.Refs;
using MBT;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : ValidatedMonoBehaviour
{
    [SerializeField, Self] Blackboard blackboard;
    [SerializeField, Anywhere] NavMeshAgent agent;
    [SerializeField] float attackCooldown = 1;
    [SerializeField] TransformReference player;
    [SerializeField] BoolReference los;
    float lastAttacked;
    private void Awake()
    {
        lastAttacked = -attackCooldown;
    }
    private void Update()
    {
        player.Value = EntityManager.Instance.Player;
        if (player.Value != null)
        {
            los.Value = !Physics.Linecast(transform.position, player.Value.position, 1 << 0);
        }
    }
    public void Melee(Transform target)
    {
        if (Time.time - lastAttacked >= attackCooldown)
        {
            EventBus<TakeDamage>.Raise(target.GetInstanceID(), new TakeDamage(new DmgInfo(1, transform.root)));
            lastAttacked = Time.time;
        }
    }
    public void StopMoving()
    {
        agent.isStopped = true;
    }
    public void GoIdle()
    {
        StopMoving();
    }
}
