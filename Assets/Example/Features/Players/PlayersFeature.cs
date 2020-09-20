using ME.ECS;

namespace Example.Features {

    using Components; using Modules; using Systems; using Features; using Markers;
    using Players.Components; using Players.Modules; using Players.Systems; using Players.Markers;
    
    namespace Players.Components {}
    namespace Players.Modules {}
    namespace Players.Systems {}
    namespace Players.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class PlayersFeature : Feature {

        public Example.Features.Players.Views.PlayerView playerView;
        private ViewId playerViewId;
        
        private RPCId createPlayerRpcId;
        private int activePlayerId;
        
        protected override void OnConstruct() {

            this.playerViewId = this.world.RegisterViewSource(this.playerView);

            var net = this.world.GetModule<NetworkModule>();
            net.RegisterObject(this, 1);
            this.createPlayerRpcId = net.RegisterRPC(new System.Action<int>(this.CreatePlayer_RPC).Method);

            this.AddSystem<NetworkMessagesSystem>();

        }

        protected override void OnDeconstruct() {
            
        }

        public int GetActivePlayerId() {

            return this.activePlayerId;

        }

        private void CreatePlayer_RPC(int actorId) {

            UnityEngine.Debug.Log("CreatePlayer_RPC: " + actorId);
            
            var entity = new Entity("Player " + actorId);
            entity.SetData(new IsPlayer());
            entity.InstantiateView(this.playerViewId);
            entity.SetPosition(new UnityEngine.Vector3(actorId * 2f, 0f, 0f));
            entity.SetRotation(UnityEngine.Quaternion.identity);
            
            this.world.GetState<ExampleState>().playerEntities.Add(actorId, entity);
            
        }

        public void OnPlayerConnectedTimeSynced() {
            
            UnityEngine.Debug.Log("Players matched, local player id: " + this.activePlayerId);
            
            var net = this.world.GetModule<NetworkModule>();
            net.RPC(this, this.createPlayerRpcId, this.activePlayerId);
            
        }

        public void OnPlayerDisconnected(int actorId) {
            
            UnityEngine.Debug.Log("Player disconnected: " + actorId);
            
            this.world.GetState<ExampleState>().playerEntities.Remove(actorId);

        }

        public void SetActivePlayer(int actorId) {
            
            UnityEngine.Debug.Log("Set active player: " + actorId);

            this.activePlayerId = actorId;

        }

        public Entity GetEntityByActorId(int actorId) {

            if (this.world.GetState<ExampleState>().playerEntities.TryGetValue(actorId, out var entity) == true) {

                return entity;

            }
            
            return Entity.Empty;
            
        }

    }

}