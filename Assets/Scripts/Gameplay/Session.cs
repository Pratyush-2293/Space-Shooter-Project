using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Session
{
    public enum Hardness { Easy, Normal, Hard, Insane};
    public CraftData[] craftDatas = new CraftData[2];

    public Hardness hardness = Hardness.Normal;
    public int stage = 2;
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

    public void Save(BinaryWriter writer)
    {
        craftDatas[0].Save(writer);
        if (GameManager.instance.twoPlayer)
        {
            craftDatas[1].Save(writer);
        }

        writer.Write((byte)hardness);
        writer.Write(stage);
    }

    public void Load(BinaryReader reader)
    {
        craftDatas[0].Load(reader);
        if (GameManager.instance.twoPlayer)
        {
            craftDatas[1].Load(reader);
        }

        hardness = (Hardness)reader.ReadByte();
        stage = reader.ReadInt32();
    }
}

[Serializable]
public class CraftData
{
    public float positionX = 0;
    public float positionY = 0;

    public byte shotPower;
    public byte numberOfEnabledOptions;
    public byte optionsLayout;

    public bool beamFiring;
    public byte beamPower;    // Damage and Width
    public byte beamCharge;   // Picked by charge
    public byte beamTimer;    // Current Charge Level - How Much Beam is Left

    public byte smallBombs;
    public byte largeBombs;

    public void Save(BinaryWriter writer)
    {
        writer.Write(shotPower);

        writer.Write(numberOfEnabledOptions);
        writer.Write(optionsLayout);

        writer.Write(beamPower);
        writer.Write(beamCharge);

        writer.Write(smallBombs);
        writer.Write(largeBombs);
    }

    public void Load(BinaryReader reader)
    {
        shotPower = reader.ReadByte();

        numberOfEnabledOptions = reader.ReadByte();
        optionsLayout = reader.ReadByte();

        beamPower = reader.ReadByte();
        beamCharge = reader.ReadByte();

        smallBombs = reader.ReadByte();
        largeBombs = reader.ReadByte();
    }
}