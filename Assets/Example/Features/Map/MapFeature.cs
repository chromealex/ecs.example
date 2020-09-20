using ME.ECS;

namespace Example.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using Map.Components; using Map.Modules; using Map.Systems; using Map.Markers;
    
    namespace Map.Components {}
    namespace Map.Modules {}
    namespace Map.Systems {}
    namespace Map.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class MapFeature : Feature {

        public Example.Features.Map.Views.MapView mapView;
        
        protected override void OnConstruct() {

            var viewId = this.world.RegisterViewSource(this.mapView);
            
            var map = new Entity("Map");
            map.SetData(new IsMap());
            map.InstantiateView(viewId);

        }

        protected override void OnDeconstruct() {
            
        }

    }

}