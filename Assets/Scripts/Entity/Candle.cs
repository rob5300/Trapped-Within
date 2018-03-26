using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public Light Flame;
    public ParticleSystem FlameParticle;
    public float MaxIntensity;
    public float MinIntensity;
    public float ChangeDelay = 0.1f;
    public bool IsOn = true;

    private float TimeOfLastChange = -999f;

	public void Update ()
	{
        if (IsOn)
        {
            if (Time.time > TimeOfLastChange + ChangeDelay)
            {
                Flame.intensity = Random.Range(MinIntensity, MaxIntensity);
                TimeOfLastChange = Time.time;
            } 
        }
	}

    public void TurnOn()
    {
        ParticleSystem.MainModule mainModule = FlameParticle.main;
        mainModule.loop = true;
        IsOn = true;
        FlameParticle.Play();
    }

    public void TurnOff()
    {
        ParticleSystem.MainModule mainModule = FlameParticle.main;
        mainModule.loop = false;
        IsOn = false;
        FlameParticle.Stop();
    }
}
