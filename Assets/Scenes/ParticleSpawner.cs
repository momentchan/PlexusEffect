using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

namespace PlexusEffect.Job
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private Particle prefab;
        [SerializeField] private float spawnRadius = 2f;
        [SerializeField] private int initalSpawnCount = 1000;
        [SerializeField] private List<Texture2D> textures;

        private TransformAccessArray trs;
        private NativeList<Vector3> pos;
        private NativeList<float> seeds;

        public TransformAccessArray Trs => trs;

        void Start()
        {
            trs = new TransformAccessArray(0);
            seeds = new NativeList<float>(0, Allocator.Persistent);
            pos = new NativeList<Vector3>(0, Allocator.Persistent);

            for (var i = 0; i < initalSpawnCount; ++i)
                Spawn();
        }

        void Update()
        {
            if(Input.GetMouseButton(0))
                Spawn();

            var job = new UpdateTRS()
            {
                Seeds = seeds.AsDeferredJobArray(),
                Positions = pos.AsDeferredJobArray(),
                Time = Time.time
            };

            var handle = job.Schedule(trs);
            handle.Complete();
        }

        private void Spawn()
        {
            var go = Instantiate(prefab);
            go.transform.position = Random.insideUnitSphere * spawnRadius;
            go.Setup(textures.RandomPick());

            trs.Add(go.transform);
            pos.Add(go.transform.position);
            seeds.Add(Random.value);
        }

        private void OnDestroy()
        {
            trs.Dispose();
            seeds.Dispose();
            pos.Dispose();
        }
    }

    [BurstCompile]
    partial struct UpdateTRS : IJobParallelForTransform
    {
        [ReadOnly] public NativeArray<float> Seeds;
        [ReadOnly] public NativeArray<Vector3> Positions;
        public float Time;

        public void Execute(int index, TransformAccess t)
        {
            float seed = Seeds[index];
            float3 pos = Positions[index];
            pos.y += math.sin(Time + seed * math.PI * 2);
            t.position = pos;
        }
    }
}