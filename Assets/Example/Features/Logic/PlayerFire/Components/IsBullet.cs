using ME.ECS;

namespace Example.Features.PlayerFire.Components {

    public struct IsBullet : IStructComponent {

        public UnityEngine.Vector3 dir;
        public UnityEngine.Vector3 from;
        public UnityEngine.Vector3 to;

    }

    public struct BulletFly : IStructComponent {

        public float timer;

    }
    
}