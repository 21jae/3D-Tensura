using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper
{
    private static Dictionary<float, WaitForSeconds> _WaitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!_WaitForSeconds.TryGetValue(seconds, out var waitForSeconds))
            _WaitForSeconds.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
        
        return waitForSeconds;
    }
}
