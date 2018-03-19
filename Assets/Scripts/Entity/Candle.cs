using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public Light Flame;
    public float MaxIntensity;
    public float MinIntensity;
    public float ChangeDelay = 0.1f;

    private float TimeOfLastChange = -999f;

	public void Update ()
	{
	    if (Time.time > TimeOfLastChange + ChangeDelay)
	    {
	        Flame.intensity = Random.Range(MinIntensity, MaxIntensity);
	        TimeOfLastChange = Time.time;
	    }
	}
}
