using System;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Forward,
    Down,
    Up,
    Back,
    Aim,
    None
}

[Serializable]
public class ActionState {
    public Agent.Action action;
    public bool state;
}
