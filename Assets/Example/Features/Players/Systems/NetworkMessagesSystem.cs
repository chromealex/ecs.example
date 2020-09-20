using ME.ECS;

namespace Example.Features.Players.Systems {

    #pragma warning disable
    using Example.Components; using Example.Modules; using Example.Systems; using Example.Markers;
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class NetworkMessagesSystem : ISystem, IUpdate {
        
        private PlayersFeature playersFeature;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {

            this.playersFeature = this.world.GetFeature<PlayersFeature>();

        }
        
        void ISystemBase.OnDeconstruct() {}

        void IUpdate.Update(in float deltaTime) {

            if (this.world.GetMarker(out NetworkPlayerConnectedTimeSynced markerTimeSynced) == true) {
                
                this.playersFeature.OnPlayerConnectedTimeSynced();
                
            }

            if (this.world.GetMarker(out NetworkPlayerDisconnected markerPlayerDisconnected) == true) {
                
                this.playersFeature.OnPlayerDisconnected(markerPlayerDisconnected.player.ActorNumber);
                
            }

            if (this.world.GetMarker(out NetworkSetActivePlayer markerSetActivePlayer) == true) {
                
                this.playersFeature.SetActivePlayer(markerSetActivePlayer.player.ActorNumber);
                
            }

        }

    }
    
}