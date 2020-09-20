using ME.ECS;

namespace Example.Features.Logic.Input.Modules {
    
    using Components; using Modules; using Systems; using Features; using Markers;
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class KeyboardInputModule : IModule, IUpdate {
        
        public World world { get; set; }
        
        void IModuleBase.OnConstruct() {}
        
        void IModuleBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime) {

            var dir = UnityEngine.Vector3.zero;
            
            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow) == true) {
                
                dir += UnityEngine.Vector3.left;
                
            }

            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow) == true) {
                
                dir += UnityEngine.Vector3.right;

            }

            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow) == true) {
                
                dir += UnityEngine.Vector3.forward;

            }

            if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow) == true) {
                
                dir += UnityEngine.Vector3.back;

            }

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space) == true) {

                this.world.AddMarker(new InputFire());

            }
            
            if (dir != UnityEngine.Vector3.zero) {

                this.world.AddMarker(new InputDir() {
                    dir = dir
                });

            }

        }
        
    }
    
}
