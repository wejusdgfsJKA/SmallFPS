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
    float lastAttacked;
    private void Awake()
    {
        lastAttacked = -attackCooldown;
    }
    private void Update()
    {
        blackboard.GetVariable<TransformVariable>("Player").Value = EntityManager.Instance.Player;
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
