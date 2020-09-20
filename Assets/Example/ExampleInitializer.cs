using UnityEngine;

#region Namespaces
namespace Example.Systems {} namespace Example.Components {} namespace Example.Modules {} namespace Example.Features {} namespace Example.Markers {} namespace Example.Views {}
#endregion

namespace Example {
    
    using TState = ExampleState;
    using ME.ECS;
    using ME.ECS.Views.Providers;
    using Example.Modules;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class ExampleInitializer : InitializerBase {

        private World world;

        public void Update() {

            if (this.world == null) {

                // Initialize world with 0.033 time step
                WorldUtilities.CreateWorld<TState>(ref this.world, 0.033f);
                {
                    #if FPS_MODULE_SUPPORT
                    this.world.AddModule<FPSModule>();
                    #endif
                    this.world.AddModule<StatesHistoryModule>();
                    this.world.AddModule<NetworkModule>();
                    
                    // Add your custom modules here
                    
                    // Create new state
                    this.world.SetState<TState>(WorldUtilities.CreateState<TState>());
                    ComponentsInitializer.DoInit();
                    this.Initialize(this.world);

                    // Add your custom systems here
                    
                }
                // Save initialization state
                this.world.SaveResetState<TState>();

            }

            if (this.world != null) {

                var dt = Time.deltaTime;
                this.world.PreUpdate(dt);
                this.world.Update(dt);

            }

        }

        public void LateUpdate() {
            
            if (this.world != null) this.world.LateUpdate(Time.deltaTime);
            
        }

        public void OnDestroy() {
            
            if (this.world == null || this.world.isActive == false) return;
            
            this.DeInitializeFeatures(this.world);
            // Release world
            WorldUtilities.ReleaseWorld<TState>(ref this.world);

        }

    }
    
}

namespace ME.ECS {
    
    public static partial class ComponentsInitializer {

        public static void DoInit() {
            
            ComponentsInitializer.Init(ref Worlds.currentWorld.GetStructComponents());
            
        }

        static partial void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer);

    }

}