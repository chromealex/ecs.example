using ME.ECS;

namespace Example.Features.Logic {

    using Components; using Modules; using Systems; using Features; using Markers;
    using ForceAtPoint.Components; using ForceAtPoint.Modules; using ForceAtPoint.Systems; using ForceAtPoint.Markers;
    
    namespace ForceAtPoint.Components {}
    namespace ForceAtPoint.Modules {}
    namespace ForceAtPoint.Systems {}
    namespace ForceAtPoint.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class ForceAtPointFeature : Feature {

        protected override void OnConstruct() {

            this.AddSystem<AddForceAtPointSystem>();
            this.AddSystem<ApplyForceSystem>();

        }

        protected override void OnDeconstruct() {
            
        }

    }

}