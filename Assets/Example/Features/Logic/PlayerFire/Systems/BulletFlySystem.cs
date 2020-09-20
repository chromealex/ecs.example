using ME.ECS;
using UnityEngine.Assertions.Must;

namespace Example.Features.PlayerFire.Systems {

    #pragma warning disable
    using Example.Components; using Example.Modules; using Example.Systems; using Example.Markers;
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class BulletFlySystem : ISystemFilter {

        private PlayerFireFeature feature;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.feature = this.world.GetFeature<PlayerFireFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-BulletFlySystem")
                         .WithStructComponent<IsBullet>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            var bulletData = entity.GetData<IsBullet>();
            ref var fly = ref entity.GetData<BulletFly>();
            fly.timer += deltaTime / this.feature.bulletSpeed;
            
            var v0 = bulletData.@from;
            var v3 = bulletData.to;
            var v1 = new UnityEngine.Vector3(v0.x, v0.y + 3f, v0.z) + bulletData.dir * (this.feature.shootDistance * 0.25f);
            var v2 = new UnityEngine.Vector3(v3.x, v3.y + 3f, v3.z) - bulletData.dir * (this.feature.shootDistance * 0.25f);

            var a0 = UnityEngine.Vector3.Lerp(v0, v1, fly.timer);
            var a1 = UnityEngine.Vector3.Lerp(v1, v2, fly.timer);
            var a2 = UnityEngine.Vector3.Lerp(v2, v3, fly.timer);
            var b0 = UnityEngine.Vector3.Lerp(a0, a1, fly.timer);
            var b1 = UnityEngine.Vector3.Lerp(a1, a2, fly.timer);
            var res = UnityEngine.Vector3.Lerp(b0, b1, fly.timer);

            entity.SetPosition(res);

            if (fly.timer >= 1f) {

                var force = new Entity("Force");
                force.SetPosition(res);
                force.SetData(new Example.Features.Logic.ForceAtPoint.Components.AddForce() {
                    value = this.feature.explosionForce,
                    range = this.feature.explosionRange,
                    time = this.feature.effectTime
                });
                
                var effect = new Entity("BulletEffect");
                effect.SetPosition(res);
                effect.SetData(new Example.Features.Logic.DestroyByTime.Components.DestroyByTime() {
                    time = this.feature.effectTime
                });
                effect.InstantiateView(this.feature.bulletEffectViewId);
                
                this.world.RemoveEntity(entity);

            }

        }
    
    }
    
}