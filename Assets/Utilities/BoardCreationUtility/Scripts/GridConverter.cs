using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridConverter
{
    // Round X coordinate to nearest int
    public static int RoundXCoordToInt(float location)
    {
        return (int)Math.Round(location, 0);
    }

    // Round Y coordinate to nearest int, returns positive int
    public static int RoundYCoordToPosInt(float location)
    {
        return -1 * (int)Math.Round(location, 0);
    }
}
