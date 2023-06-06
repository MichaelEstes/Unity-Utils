using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageParticles : MonoBehaviour
{
    public List<ParticleSystem> particleSystems;

    void OnEnable()
    {
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
    }

    void OnDisable()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Stop();
        }
    }
}
