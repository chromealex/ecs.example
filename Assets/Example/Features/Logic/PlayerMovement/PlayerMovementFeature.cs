using ME.ECS;

namespace Example.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using PlayerMovement.Components; using PlayerMovement.Modules; using PlayerMovement.Systems; using PlayerMovement.Markers;
    
    namespace PlayerMovement.Components {}
    namespace PlayerMovement.Modules {}
    namespace PlayerMovement.Systems {}
    namespace PlayerMovement.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PlayerMovementFeature : Feature {

        public float speed;
        
        protected override void OnConstruct() {

            this.AddSystem<PlayerMovementSystem>();

        }

        protected override void OnDeconstruct() {
            
        }

    }

}