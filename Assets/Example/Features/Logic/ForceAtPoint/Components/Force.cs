using ME.ECS;

namespace Example.Features.Logic.ForceAtPoint.Components {

    public struct Force : IStructComponent {

        public UnityEngine.Vector3 dir;
        public float time;
        public float value;

    }
    
}