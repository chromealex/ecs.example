using System.Linq;

namespace Example.Modules {
    
    using ME.ECS;
    using TState = ExampleState;
    
    /// <summary>
    /// We need to implement our own NetworkModule class without any logic just to catch your State type into ECS.Network
    /// You can use some overrides to setup history config for your project
    /// </summary>
    public class NetworkModule : ME.ECS.Network.NetworkModule<TState> {

        private int orderId;
        private PhotonTransporter photonTransporter;

        protected override int GetRPCOrder() {

            return this.orderId;// + Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber;

        }

        protected override ME.ECS.Network.NetworkType GetNetworkType() {

            return ME.ECS.Network.NetworkType.SendToNet | ME.ECS.Network.NetworkType.RunLocal;

        }

        public void SetOrderId(int orderId) {

            this.orderId = orderId;

        }

        public void AddToQueue(byte[] bytes) {
            
            this.photonTransporter.AddToQueue(bytes);
            
        }

        public void AddToSystemQueue(byte[] bytes) {
            
            this.photonTransporter.AddToSystemQueue(bytes);
            
        }

        public void SetRoom(Photon.Realtime.Room room) {

            this.photonTransporter.SetRoom(room);

        }

        protected override void OnInitialize() {

            var tr = new PhotonTransporter(this.world.id);
            var instance = (ME.ECS.Network.INetworkModuleBase)this;
            instance.SetTransporter(tr);
            instance.SetSerializer(new FSSerializer());
            
            this.photonTransporter = tr;

        }

        protected override void OnDeInitialize() {

            this.photonTransporter.OnRelease();

        }

    }
    
    public class PhotonReceiver : Photon.Pun.MonoBehaviourPunCallbacks {

        [Photon.Pun.PunRPC]
        public void RPC_HISTORY_CALL(byte[] bytes) {

            var world = ME.ECS.Worlds.currentWorld;
            var storageNetworkModule = world.GetModule<NetworkModule>();
            var networkModule = world.GetModule<ME.ECS.Network.INetworkModuleBase>();
            var storage = storageNetworkModule.GetSerializer().DeserializeStorage(bytes); 
            networkModule.LoadHistoryStorage(storage);

        }

        [Photon.Pun.PunRPC]
        public void RPC_CALL(byte[] bytes) {

            var world = ME.ECS.Worlds.currentWorld;
            var networkModule = world.GetModule<NetworkModule>();
            networkModule.AddToQueue(bytes);

        }

        [Photon.Pun.PunRPC]
        public void RPC_SYSTEM_CALL(byte[] bytes) {

            var world = ME.ECS.Worlds.currentWorld;
            var networkModule = world.GetModule<NetworkModule>();
            networkModule.AddToSystemQueue(bytes);

        }

        public void Initialize() {

            Photon.Pun.PhotonNetwork.ConnectUsingSettings();
    
        }

        public void DeInitialize() {
            
            Photon.Pun.PhotonNetwork.Disconnect();
            
        }

        public override void OnConnectedToMaster() {
        
            base.OnConnectedToMaster();

            Photon.Pun.PhotonNetwork.JoinLobby(Photon.Realtime.TypedLobby.Default);

        }

        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause) {
            
            base.OnDisconnected(cause);

            if (UnityEngine.Application.isPlaying == false) return;
            
            UnityEngine.Debug.Log("Disconnected because of " + cause);

            var go = UnityEngine.GameObject.Find("ExampleInitializer");
            go.gameObject.SetActive(false);
            UnityEngine.GameObject.DestroyImmediate(go);

            this.timeSyncedConnected = false;
            this.timeSynced = false;
            this.timeSyncDesiredTick = Tick.Zero;
            ME.ECS.Worlds.currentWorld = null;
            
            UnityEngine.Debug.Log("World destroyed");

            var name = "Main";
            UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> action = null;
            action = (arg0, mode) => {

                if (arg0.name == "Empty") {
                    
                    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= action;
                    UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> action2 = null;
                    action2 = (arg1, mode1) => {

                        if (arg1.name == name) {

                            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= action2;

                            
                            
                        }

                    };

                    UnityEngine.SceneManagement.SceneManager.sceneLoaded += action2;
                    UnityEngine.SceneManagement.SceneManager.LoadScene(name, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    
                }

            };
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += action;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Empty", UnityEngine.SceneManagement.LoadSceneMode.Single);
            
        }

        public override void OnJoinedRoom() {
            
            base.OnJoinedRoom();

            if (Photon.Pun.PhotonNetwork.InRoom == true) {
                
                UnityEngine.Debug.Log("OnJoinedRoom. IsMaster: " + Photon.Pun.PhotonNetwork.IsMasterClient);

                var world = ME.ECS.Worlds.currentWorld;
                if (world == null) return;
                var hash = Photon.Pun.PhotonNetwork.CurrentRoom.Name.GetHashCode();
                if (hash < 0) hash = -hash;
                if (hash == 0) hash = 1;
                world.SetSeed((uint)hash);
                UnityEngine.Debug.Log("Seed: " + hash);
                var networkModule = world.GetModule<NetworkModule>();
                networkModule.SetRoom(Photon.Pun.PhotonNetwork.CurrentRoom);

                if (Photon.Pun.PhotonNetwork.IsMasterClient == true) {
                    
                    // Put server time into the room properties
                    var serverTime = Photon.Pun.PhotonNetwork.Time;
                    Photon.Pun.PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
                        { "t", serverTime },
                        { "cc", 1 },
                    });
                    
                }

                this.timeSyncedConnected = false;
                this.timeSynced = false;
                this.UpdateTime();

                world.AddMarker(new Example.Markers.NetworkSetActivePlayer() {
                    player = Photon.Pun.PhotonNetwork.LocalPlayer
                });
                
            }

        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
            
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            
            var world = ME.ECS.Worlds.currentWorld;
            var networkModule = world.GetModule<NetworkModule>();
            if ((networkModule as ME.ECS.Network.INetworkModuleBase).GetRPCOrder() == 0) {

                var orderId = (int)Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties["cc"];
                networkModule.SetOrderId(orderId);

            }

        }

        private bool timeSyncedConnected = false;
        private bool timeSynced = false;
        private Tick timeSyncDesiredTick;
        public void UpdateTime() {
            
            if (Photon.Pun.PhotonNetwork.InRoom == false) return;

            if (Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("t") == true) {

                // Set current time since start from master client
                var world = ME.ECS.Worlds.currentWorld;
                var serverTime = Photon.Pun.PhotonNetwork.Time;
                var gameStartTime = serverTime - (double)Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties["t"];

                world.SetTimeSinceStart(gameStartTime);
                var timeSinceGameStart = (long)(world.GetTimeSinceStart() * 1000L);
                var desiredTick = (Tick)System.Math.Floor(timeSinceGameStart / (world.GetTickTime() * 1000d));
                this.timeSynced = true;
                this.timeSyncDesiredTick = desiredTick;

            }

        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
            
            base.OnPlayerEnteredRoom(newPlayer);

            if (Photon.Pun.PhotonNetwork.IsMasterClient == true) {

                var world = ME.ECS.Worlds.currentWorld;
                var props = Photon.Pun.PhotonNetwork.CurrentRoom.CustomProperties;
                props["cc"] = (int)props["cc"] + 1;
                Photon.Pun.PhotonNetwork.CurrentRoom.SetCustomProperties(props);

                // Send all history events to client
                var networkModule = world.GetModule<NetworkModule>();
                var history = world.GetModule<ME.ECS.StatesHistory.IStatesHistoryModuleBase>().GetHistoryStorage();
                this.photonView.RPC("RPC_HISTORY_CALL", newPlayer, networkModule.GetSerializer().SerializeStorage(history));
                
            }

        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
            
            base.OnPlayerLeftRoom(otherPlayer);

            if (Photon.Pun.PhotonNetwork.IsMasterClient == true) {

                var world = ME.ECS.Worlds.currentWorld;
                world.AddMarker(new Example.Markers.NetworkPlayerDisconnected() {
                    player = otherPlayer
                });

            }

        }

        public override void OnRoomListUpdate(System.Collections.Generic.List<Photon.Realtime.RoomInfo> roomList) {

            var guid = string.Format("{0}-{1}", System.Guid.NewGuid(), UnityEngine.Random.Range(0, 1000000));
            var roomOptions = new Photon.Realtime.RoomOptions() { MaxPlayers = 2, PublishUserId = true };
            var room = roomList.FirstOrDefault(x => x.IsOpen == true);
            if (room != null) {

                Photon.Pun.PhotonNetwork.JoinRoom(room.Name);

            } else {

                Photon.Pun.PhotonNetwork.CreateRoom(guid, roomOptions, Photon.Realtime.TypedLobby.Default);

            }

        }

        public void LateUpdate() {

            this.UpdateTime();

            var world = ME.ECS.Worlds.currentWorld;
            if (this.timeSynced == true && this.timeSyncedConnected == false /*&& world.GetCurrentTick() > this.timeSyncDesiredTick*/) {

                var networkModule = world.GetModule<NetworkModule>();
                if (((ME.ECS.Network.INetworkModuleBase)networkModule).GetRPCOrder() > 0) {

                    // Turn off this check to run game locally without awaiting for other player to join
                    if (Photon.Pun.PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                    
                        this.timeSyncedConnected = true;
                        
                        world.AddMarker(new Example.Markers.NetworkPlayerConnectedTimeSynced());

                    }
                    
                }

            }

        }

    }

    public class PhotonTransporter : ME.ECS.Network.ITransporter {

        private System.Collections.Generic.Queue<byte[]> queue = new System.Collections.Generic.Queue<byte[]>();
        private System.Collections.Generic.Queue<byte[]> queueSystem = new System.Collections.Generic.Queue<byte[]>();
        private Photon.Pun.PhotonView photonView;
        private PhotonReceiver photonReceiver;
        private Photon.Realtime.Room room;
        
        private int sentCount;
        private int sentBytesCount;
        private int receivedCount;
        private int receivedBytesCount;

        public PhotonTransporter(int id) {

            var photon = new UnityEngine.GameObject("PhotonTransporter", typeof(Photon.Pun.PhotonView), typeof(PhotonReceiver));
            this.photonReceiver = photon.GetComponent<PhotonReceiver>();
            var view = photon.GetComponent<Photon.Pun.PhotonView>();
            view.ViewID = id;
            Photon.Pun.PhotonNetwork.RegisterPhotonView(view);

            this.photonView = view;

            this.photonReceiver.Initialize();

        }

        public void OnRelease() {
            
            this.photonReceiver.DeInitialize();
            if (this.photonReceiver != null) UnityEngine.GameObject.Destroy(this.photonReceiver.gameObject);
            
        }
        
        public void SetRoom(Photon.Realtime.Room room) {

            this.room = room;

        }
        
        public bool IsConnected() {

            return Photon.Pun.PhotonNetwork.IsConnectedAndReady == true && this.room != null;

        }

        public void Send(byte[] bytes) {

            this.photonView.RPC("RPC_CALL", Photon.Pun.RpcTarget.Others, bytes);

            this.sentBytesCount += bytes.Length;
            ++this.sentCount;

        }

        public void SendSystem(byte[] bytes) {

            this.photonView.RPC("RPC_SYSTEM_CALL", Photon.Pun.RpcTarget.Others, bytes);
            
            this.sentBytesCount += bytes.Length;
            //++this.sentCount;

        }

        public void AddToQueue(byte[] bytes) {
            
            this.queue.Enqueue(bytes);
            
        }

        public void AddToSystemQueue(byte[] bytes) {
            
            this.queueSystem.Enqueue(bytes);
            
        }

        public byte[] Receive() {

            if (this.queue.Count == 0) {

                if (this.queueSystem.Count == 0) return null;
                
                var bytes = this.queueSystem.Dequeue();
            
                //++this.receivedCount;
                this.receivedBytesCount += bytes.Length;
            
                return bytes;

            } else {

                var bytes = this.queue.Dequeue();

                ++this.receivedCount;
                this.receivedBytesCount += bytes.Length;

                return bytes;

            }

        }

        public int GetEventsSentCount() {

            return this.sentCount;

        }

        public int GetEventsBytesSentCount() {

            return this.sentBytesCount;

        }

        public int GetEventsReceivedCount() {

            return this.receivedCount;

        }

        public int GetEventsBytesReceivedCount() {
            
            return this.receivedBytesCount;
            
        }

    }

    public class FSSerializer : ME.ECS.Network.ISerializer {

        public byte[] SerializeWorld(World.WorldState worldState) {
            
            return ME.ECS.Serializer.Serializer.Pack(worldState);
            
        }

        public World.WorldState DeserializeWorld(byte[] bytes) {
            
            return ME.ECS.Serializer.Serializer.Unpack<World.WorldState>(bytes);

        }

        public byte[] SerializeStorage(ME.ECS.StatesHistory.HistoryStorage historyStorage) {

            return ME.ECS.Serializer.Serializer.Pack(historyStorage);

        }

        public ME.ECS.StatesHistory.HistoryStorage DeserializeStorage(byte[] bytes) {

            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryStorage>(bytes);

        }

        public byte[] Serialize(ME.ECS.StatesHistory.HistoryEvent historyEvent) {

            return ME.ECS.Serializer.Serializer.Pack(historyEvent);

        }

        public ME.ECS.StatesHistory.HistoryEvent Deserialize(byte[] bytes) {

            return ME.ECS.Serializer.Serializer.Unpack<ME.ECS.StatesHistory.HistoryEvent>(bytes);

        }

    }

}