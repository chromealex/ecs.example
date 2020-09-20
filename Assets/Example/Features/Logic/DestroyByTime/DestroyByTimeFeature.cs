using ME.ECS;

namespace Example.Features.Logic {

    using Components; using Modules; using Systems; using Features; using Markers;
    using DestroyByTime.Components; using DestroyByTime.Modules; using DestroyByTime.Systems; using DestroyByTime.Markers;
    
    namespace DestroyByTime.Components {}
    namespace DestroyByTime.Modules {}
    namespace DestroyByTime.Systems {}
    namespace DestroyByTime.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class DestroyByTimeFeature : Feature {

        protected override void OnConstruct() {

            this.AddSystem<DestroyByTimeSystem>();

        }

        protected override void OnDeconstruct() {
            
        }

    }

}