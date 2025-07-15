using EventBus;

namespace Entity
{
    /// <summary>
    /// This is meant to fire whenever the entity takes damage.
    /// </summary>
    public struct OnDamageTaken : IEvent
    {
        public DmgInfo dmgInfo;
        public EntityBase entityBase;
        public OnDamageTaken(DmgInfo dmgInfo, EntityBase entityBase)
        {
            this.dmgInfo = dmgInfo;
            this.entityBase = entityBase;
        }
    }

    /// <summary>
    /// This is meant to fire whenever the entity dies.
    /// </summary>
    public struct OnDeath : IEvent
    {
        public DmgInfo dmgInfo;
        public EntityBase entityBase;
        public OnDeath(DmgInfo dmgInfo, EntityBase entityBase)
        {
            this.dmgInfo = dmgInfo;
            this.entityBase = entityBase;
        }
    }
}