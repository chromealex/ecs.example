using ME.ECS;

namespace Example.Markers {
    
    public struct NetworkSetActivePlayer : IMarker {

        public Photon.Realtime.Player player;

    }
    
    public struct NetworkPlayerDisconnected : IMarker {

        public Photon.Realtime.Player player;

    }

    public struct NetworkPlayerConnectedTimeSynced : IMarker { }

}