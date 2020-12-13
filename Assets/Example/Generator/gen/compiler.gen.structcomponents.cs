namespace ME.ECS {

    public static partial class ComponentsInitializer {
    
        static partial void InitTypeIdPartial() {
            
            WorldUtilities.ResetTypeIds();
            CoreComponentsInitializer.InitTypeId();
            

            WorldUtilities.InitComponentTypeId<Example.Features.Players.Components.IsPlayer>(true);
            WorldUtilities.InitComponentTypeId<Example.Features.Map.Components.IsMap>(true);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerMovement.Components.LastMovementDirection>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerMovement.Components.MoveAction>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerFire.Components.FireAction>(true);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerFire.Components.IsBullet>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerFire.Components.BulletFly>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.Logic.ForceAtPoint.Components.AddForce>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.Logic.ForceAtPoint.Components.Force>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.Logic.DestroyByTime.Components.DestroyByTime>(false);

        }
        
        static partial void Init(ref ME.ECS.StructComponentsContainer structComponentsContainer) {
    
            WorldUtilities.ResetTypeIds();
            CoreComponentsInitializer.InitTypeId();
            

            WorldUtilities.InitComponentTypeId<Example.Features.Players.Components.IsPlayer>(true);
            WorldUtilities.InitComponentTypeId<Example.Features.Map.Components.IsMap>(true);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerMovement.Components.LastMovementDirection>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerMovement.Components.MoveAction>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerFire.Components.FireAction>(true);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerFire.Components.IsBullet>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.PlayerFire.Components.BulletFly>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.Logic.ForceAtPoint.Components.AddForce>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.Logic.ForceAtPoint.Components.Force>(false);
            WorldUtilities.InitComponentTypeId<Example.Features.Logic.DestroyByTime.Components.DestroyByTime>(false);

            ComponentsInitializerWorld.Setup(ComponentsInitializerWorldGen.Init);
            CoreComponentsInitializer.Init(ref structComponentsContainer);

            structComponentsContainer.Validate<Example.Features.Players.Components.IsPlayer>(true);
            structComponentsContainer.Validate<Example.Features.Map.Components.IsMap>(true);
            structComponentsContainer.Validate<Example.Features.PlayerMovement.Components.LastMovementDirection>(false);
            structComponentsContainer.Validate<Example.Features.PlayerMovement.Components.MoveAction>(false);
            structComponentsContainer.Validate<Example.Features.PlayerFire.Components.FireAction>(true);
            structComponentsContainer.Validate<Example.Features.PlayerFire.Components.IsBullet>(false);
            structComponentsContainer.Validate<Example.Features.PlayerFire.Components.BulletFly>(false);
            structComponentsContainer.Validate<Example.Features.Logic.ForceAtPoint.Components.AddForce>(false);
            structComponentsContainer.Validate<Example.Features.Logic.ForceAtPoint.Components.Force>(false);
            structComponentsContainer.Validate<Example.Features.Logic.DestroyByTime.Components.DestroyByTime>(false);
    
        }
    
    }
    
    public static class ComponentsInitializerWorldGen {

        public static void Init(Entity entity) {


            entity.ValidateData<Example.Features.Players.Components.IsPlayer>(true);
            entity.ValidateData<Example.Features.Map.Components.IsMap>(true);
            entity.ValidateData<Example.Features.PlayerMovement.Components.LastMovementDirection>(false);
            entity.ValidateData<Example.Features.PlayerMovement.Components.MoveAction>(false);
            entity.ValidateData<Example.Features.PlayerFire.Components.FireAction>(true);
            entity.ValidateData<Example.Features.PlayerFire.Components.IsBullet>(false);
            entity.ValidateData<Example.Features.PlayerFire.Components.BulletFly>(false);
            entity.ValidateData<Example.Features.Logic.ForceAtPoint.Components.AddForce>(false);
            entity.ValidateData<Example.Features.Logic.ForceAtPoint.Components.Force>(false);
            entity.ValidateData<Example.Features.Logic.DestroyByTime.Components.DestroyByTime>(false);

        }

    }

}