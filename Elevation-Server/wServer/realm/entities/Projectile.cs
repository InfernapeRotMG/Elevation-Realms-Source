#region

using System;
using System.Collections.Generic;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities
{
    public interface IProjectileOwner
    {
        Projectile[] Projectiles { get; }
        Entity Self { get; }
    }

    public class Projectile : Entity
    {
        public Projectile(RealmManager manager, ProjectileDesc desc)
            : base(manager, manager.GameData.IdToObjectType[desc.ObjectId])
        {
            Descriptor = desc;
        }

        public IProjectileOwner ProjectileOwner { get; set; }
        public new byte ProjectileId { get; set; }
        public short Container { get; set; }
        public int Damage { get; set; }
        public Position BeginPos { get; set; }
        public float Angle { get; set; }

        public ProjectileDesc Descriptor { get; set; }

        public void Destroy(bool immediate)
        {
            if (!immediate)
                Manager.Logic.AddPendingAction(_ => Destroy(true), PendingPriority.Destruction);
            if (Owner != null)
            {
                if (ProjectileOwner is Player)
                    (ProjectileOwner as Player).FameCounter.RemoveProjectile(this);
                Owner.LeaveWorld(this);
            }
        }

        public void ForceHit(Entity entity, RealmTime time)
        {
            if (entity.HitByProjectile(this, time))
            {
                Destroy(true);
                ProjectileOwner.Self.ProjectileHit(this, entity);
            }
        }
    }
}