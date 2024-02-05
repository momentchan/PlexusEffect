using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

namespace PlexusEffect.Job
{
    public class Particle : MonoBehaviour
    {
        private Block block;
        public void Setup(Texture tex)
        {
            block = new Block(GetComponent<Renderer>());
            block.SetTexture("_Texture", tex);
            block.Apply();
        }
    }
}