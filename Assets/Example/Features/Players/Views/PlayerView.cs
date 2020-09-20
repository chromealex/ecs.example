using ME.ECS;

namespace Example.Features.Players.Views {
    
    using ME.ECS.Views.Providers;
    
    public class PlayerView : MonoBehaviourView {
        
        public override bool applyStateJob => true;

        public override void OnInitialize() {
            
        }
        
        public override void OnDeInitialize() {
            
        }
        
        public override void ApplyStateJob(UnityEngine.Jobs.TransformAccess transform, float deltaTime, bool immediately) {
            
        }
        
        public override void ApplyState(float deltaTime, bool immediately) {

            this.transform.position = this.entity.GetPosition();
            this.transform.rotation = this.entity.GetRotation();

        }
        
    }
    
}