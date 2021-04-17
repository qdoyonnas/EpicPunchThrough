using System;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Forward,
    Down,
    Up,
    Back,
    Aim
}

[Serializable]
public class ActionState {
    public Agent.Action action;
    public bool state;
}
