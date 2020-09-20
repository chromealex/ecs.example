namespace Example {

    using ME.ECS;
    
    public class ExampleState : ME.ECS.State {

        public System.Collections.Generic.Dictionary<int, Entity> playerEntities;

        public override void Initialize(World world, bool freeze, bool restore) {
            
            base.Initialize(world, freeze, restore);
            
            if (this.playerEntities == null) this.playerEntities = PoolDictionary<int, Entity>.Spawn(2);

        }

        public override void CopyFrom(State other) {
            
            base.CopyFrom(other);

            var _other = (ExampleState)other;

            if (this.playerEntities != null) PoolDictionary<int, Entity>.Recycle(ref this.playerEntities);
            this.playerEntities = PoolDictionary<int, Entity>.Spawn(_other.playerEntities.Count);
            foreach (var item in _other.playerEntities) {
                
                this.playerEntities.Add(item.Key, item.Value);
                
            }

        }

        public override void OnRecycle() {
            
            base.OnRecycle();

            PoolDictionary<int, Entity>.Recycle(ref this.playerEntities);

        }

    }

}