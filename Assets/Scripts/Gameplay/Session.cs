using System;
using UnityEngine;

[Serializable]
public class Session
{
    public enum Hardness { Easy, Normal, Hard, Insane};

    public Hardness hardness = Hardness.Normal;
    public int stage = 1;
    public bool practice = false;
    public bool arenaPractice = false;
    public bool stagePractice = false;

    //Cheats
    public bool infiniteLives = false;
    public bool infiniteContinues = false;
    public bool infiniteBombs = false;
    public bool invincible = false;
    public bool halfSpeed = false;
    public bool doubleSpeed = false;
}
