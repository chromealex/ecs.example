using ME.ECS;

namespace Example.Features.PlayerMovement.Systems {

    #pragma warning disable
    using Example.Components; using Example.Modules; using Example.Systems; using Example.Markers;
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PlayerMovementSystem : ISystemFilter {

        private PlayerMovementFeature feature;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.feature = this.world.GetFeature<PlayerMovementFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-PlayerMovementSystem")
                         .WithStructComponent<Example.Features.Players.Components.IsPlayer>()
                         .WithStructComponent<MoveAction>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            var action = entity.GetData<MoveAction>();
            var dir = action.dir.normalized;

            var pos = entity.GetPosition();
            pos += dir * (this.feature.speed * deltaTime);
            entity.SetPosition(pos);
            entity.SetRotation(UnityEngine.Quaternion.LookRotation(dir, UnityEngine.Vector3.up));

            entity.SetData(new LastMovementDirection() {
                dir = dir
            });

        }
    
    }
    
}