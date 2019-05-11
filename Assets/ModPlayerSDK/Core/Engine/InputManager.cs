using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager ActiveDevice;

    public InputDirection Direction;

    void Awake()
    {
        ActiveDevice = this;

        Direction = new InputDirection();
    }
    void Update()
    {
        var w = Input.GetKey(KeyCode.W);
        var a = Input.GetKey(KeyCode.A);
        var s = Input.GetKey(KeyCode.S);
        var d = Input.GetKey(KeyCode.D);

        Direction.IsPressed = w || a || s || d;

        if (w && d)
        {
            Direction.Angle = 45;
            Direction.X = 1;
            Direction.Y = 1;
        }
        else if (w && a)
        {
            Direction.Angle = -45;
            Direction.X = -1;
            Direction.Y = 1;
        }
        else if (w)
        {
            Direction.Angle = 0;
            Direction.X = 0;
            Direction.Y = 1;
        }
        else if (s)
        {
            Direction.Angle = 180;
            Direction.X = 0;
            Direction.Y = -1;
        }
        else if (a)
        {
            Direction.Angle = -90;
            Direction.X = -1;
            Direction.Y = 0;
        }
        else if (d)
        {
            Direction.Angle = 90;
            Direction.X = 1;
            Direction.Y = 0;
        }
    }
}
public class InputDirection
{
    public float Angle;
    public float X, Y;
    public bool IsPressed;
}
