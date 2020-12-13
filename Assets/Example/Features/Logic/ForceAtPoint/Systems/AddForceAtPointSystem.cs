using ME.ECS;

namespace Example.Features.Logic.ForceAtPoint.Systems {

    #pragma warning disable
    using Example.Components; using Example.Modules; using Example.Systems; using Example.Markers;
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class AddForceAtPointSystem : ISystemFilter {

        private Filter allPlayers;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {
            
            Filter.Create("Filter-AddForceAtPointSystem-Players")
                  .WithStructComponent<Example.Features.Players.Components.IsPlayer>()
                  .Push(ref this.allPlayers);
            
        }
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-AddForceAtPointSystem")
                         .WithStructComponent<AddForce>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            var force = entity.GetData<AddForce>();
            var sqrForceRange = force.range * force.range;
            
            var pos = entity.GetPosition();
            foreach (var player in this.allPlayers) {

                var p = player.GetPosition();
                if ((p - pos).sqrMagnitude <= sqrForceRange) {

                    ref var currentForce = ref player.GetData<Force>();
                    currentForce.dir += (p - pos).normalized;
                    currentForce.time = UnityEngine.Mathf.Max(currentForce.time, force.time);
                    currentForce.value = UnityEngine.Mathf.Max(currentForce.value, force.value);

                }

            }

            entity.Destroy();

        }
    
    }
    
}