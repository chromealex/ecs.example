using ME.ECS;

namespace Example.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using PlayerFire.Components; using PlayerFire.Modules; using PlayerFire.Systems; using PlayerFire.Markers;
    
    namespace PlayerFire.Components {}
    namespace PlayerFire.Modules {}
    namespace PlayerFire.Systems {}
    namespace PlayerFire.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PlayerFireFeature : Feature {

        public float explosionForce;
        public float explosionRange;
        public float effectTime = 3f;
        
        public float bulletSpeed;
        public float shootDistance;
        
        public Example.Features.PlayerFire.Views.BulletView bulletView;
        public Example.Features.PlayerFire.Views.BulletEffectView bulletEffectView;

        public ViewId bulletViewId;
        public ViewId bulletEffectViewId;
        
        protected override void OnConstruct() {

            this.bulletViewId = this.world.RegisterViewSource(this.bulletView);
            this.bulletEffectViewId = this.world.RegisterViewSource(this.bulletEffectView);
            
            this.AddSystem<FireSystem>();
            this.AddSystem<BulletFlySystem>();

        }

        protected override void OnDeconstruct() {
            
        }

    }

}