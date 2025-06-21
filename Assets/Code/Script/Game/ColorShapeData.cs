using UnityEngine;

// En enum med våre 4 mulige farger
public enum BallColor
{
    Red,
    Blue,
    Yellow,
    Green
}

[System.Serializable]
public class ColorShapeData
{
    public BallColor ballColor; // i stedet for en flytende Color
}
