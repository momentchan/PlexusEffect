using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlexusEffect.Job
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private float maxDistanceSqr = 1.0f;
        [SerializeField] private LineRenderer prefab;

        private List<LineRenderer> lines = new List<LineRenderer>();
        private ParticleSpawner spawner;

        void Start()
        {
            spawner = GetComponent<ParticleSpawner>();
        }

        private void LateUpdate()
        {
            int index = 0;
            int count = lines.Count;


            for (int i = 0; i < spawner.Trs.length; i++)
            {
                var p1 = spawner.Trs[i].position;

                for (int j = i + 1; j < spawner.Trs.length; j++)
                {
                    var p2 = spawner.Trs[j].position;

                    var distSqr = Vector3.SqrMagnitude(p2 - p1);
                    if (distSqr < maxDistanceSqr)
                    {
                        if(index == count)
                        {
                            var newLine = Instantiate(prefab, transform);
                            lines.Add(newLine);
                            count++;
                        }

                        var line = lines[index];
                        line.enabled = true;
                        line.SetPosition(0, p1);
                        line.SetPosition(1, p2);

                        index++;
                    }
                }
            }

            for(int i = index; i< lines.Count; i++)
            {
                lines[i].enabled = false;
            }
        }
    }
}