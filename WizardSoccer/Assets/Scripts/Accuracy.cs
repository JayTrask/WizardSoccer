using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuracy : MonoBehaviour {

    private const int maxAccuracy = 100;
    private int accuracy = maxAccuracy;

    public void reduceAccuracy(int reduction)
    {
        accuracy -= reduction;

        if (accuracy < 0)
        {
            accuracy = 0;
        }
    }
}
