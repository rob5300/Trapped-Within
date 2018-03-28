using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialLock : MonoBehaviour {

    public int CurrentDigit = 1;
    public float RotateAmount = 36f;

    public CodeBox box;

	public void RotateUp()
    {
        CurrentDigit--;
        if (CurrentDigit < 0) CurrentDigit = 9;
        transform.Rotate(0, -RotateAmount, 0);
        box.CheckCode();
    }

    public void RotateDown()
    {
        CurrentDigit++;
        if (CurrentDigit > 9) CurrentDigit = 0;
        transform.Rotate(0, RotateAmount, 0);
        box.CheckCode();
    }
}
