using ME.ECS;

namespace Example.Features.Logic.DestroyByTime.Systems {

    #pragma warning disable
    using Example.Components; using Example.Modules; using Example.Systems; using Example.Markers;
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class DestroyByTimeSystem : ISystemFilter {
        
        public World world { get; set; }
        
        void ISystemBase.OnConstruct() {}
        
        void ISystemBase.OnDeconstruct() {}
        
        bool ISystemFilter.jobs => false;
        int ISystemFilter.jobsBatchCount => 64;
        Filter ISystemFilter.filter { get; set; }
        Filter ISystemFilter.CreateFilter() {
            
            return Filter.Create("Filter-DestroyByTimeSystem")
                         .WithStructComponent<DestroyByTime>()
                         .Push();
            
        }

        void ISystemFilter.AdvanceTick(in Entity entity, in float deltaTime) {

            ref var timer = ref entity.GetData<DestroyByTime>();
            timer.time -= deltaTime;
            if (timer.time <= 0f) {

                this.world.RemoveEntity(entity);

            }

        }
    
    }
    
}