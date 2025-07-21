using EventBus;

namespace Entity
{
    public struct TakeDamage : IEvent
    {
        public DmgInfo DmgInfo;
        public TakeDamage(DmgInfo DmgInfo)
        {
            this.DmgInfo = DmgInfo;
        }
    }
    /// <summary>
    /// This is meant to fire whenever the entity takes damage.
    /// </summary>
    public struct OnDamageTaken : IEvent
    {
        public DmgInfo DmgInfo;
        public EntityBase EntityBase;
        public OnDamageTaken(DmgInfo DmgInfo, EntityBase EntityBase)
        {
            this.DmgInfo = DmgInfo;
            this.EntityBase = EntityBase;
        }
    }
    public struct OnHealthUpdated : IEvent
    {
        public EntityBase EntityBase;
        public OnHealthUpdated(EntityBase entityBase)
        {
            EntityBase = entityBase;
        }
    }
    /// <summary>
    /// This is meant to fire whenever the entity dies.
    /// </summary>
    public struct OnDeath : IEvent
    {
        public DmgInfo DmgInfo;
        public EntityBase EntityBase;
        public OnDeath(DmgInfo DmgInfo, EntityBase EntityBase)
        {
            this.DmgInfo = DmgInfo;
            this.EntityBase = EntityBase;
        }
    }
}