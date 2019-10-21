using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICue
{
    void AdjustCue(float value);
    void Release();
}
