using ME.ECS;

namespace Example.Features.Logic.Input.Systems {

    #pragma warning disable
    using Example.Components; using Example.Modules; using Example.Systems; using Example.Markers;
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class MarkerToRPCSystem : ISystem, IAdvanceTick, IUpdate {

        private InputFeature feature;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.feature = this.world.GetFeature<InputFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}
        
        void IAdvanceTick.AdvanceTick(in float deltaTime) {}

        void IUpdate.Update(in float deltaTime) {

            if (this.world.GetMarker(out InputDir dir) == true) {

                this.feature.SendDir(dir);

            }

            if (this.world.GetMarker(out InputFire fire) == true) {
                
                this.feature.SendFire(fire);

            }
            
        }
        
    }
    
}