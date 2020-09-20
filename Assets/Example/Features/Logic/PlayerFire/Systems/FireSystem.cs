using ME.ECS;

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
    public sealed class FireSystem : ISystemFilter {

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
            
            return Filter.Create("Filter-FireSystem")
                         .WithStructComponent<Example.Features.Players.Components.IsPlayer>()
                         .WithStructComponent<FireAction>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            var lastDir = entity.GetData<Example.Features.PlayerMovement.Components.LastMovementDirection>().dir;

            var bullet = new Entity("Bullet");
            bullet.SetData(new IsBullet() {
                dir = lastDir,
                from = entity.GetPosition(),
                to = entity.GetPosition() + this.feature.shootDistance * lastDir
            });
            bullet.SetData(new BulletFly() {
                timer = 0f,
            });
            bullet.SetPosition(entity.GetPosition());
            bullet.InstantiateView(this.feature.bulletViewId);
            
        }
    
    }
    
}