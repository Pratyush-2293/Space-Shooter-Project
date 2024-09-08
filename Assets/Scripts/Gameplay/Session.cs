using System;
using UnityEngine;

[Serializable]
public class Session
{
    public enum Hardness { Easy, Normal, Hard, Insane};
    public CraftData[] craftDatas = new CraftData[2];

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

[Serializable]
public class CraftData
{
    public float positionX;
    public float positionY;

    public byte shotPower;
    public byte numberOfEnabledOptions;
    public byte optionsLayout;

    public bool beamFiring;
    public byte beamPower;    // Damage and Width
    public byte beamCharge;   // Picked by charge
    public byte beamTimer;    // Current Charge Level - How Much Beam is Left

    public byte smallBombs;
    public byte largeBombs;
}