using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    const int SAVE_VERSION = 1;

    public static SaveManager instance = null;
    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one SaveManager.");
            Destroy(instance);
            return;
        }

        instance = this;
    }

    public void SaveGame(int slot)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memStream);

        writer.Write(SAVE_VERSION);

        writer.Write(GameManager.instance.twoPlayer);

        //more stuff in here <--

        string savePath = Application.persistentDataPath + "/slot" + slot + ".dat";
        Debug.Log("Savegame, savePath = " + savePath);

        FileStream fileStream = new FileStream(savePath, FileMode.OpenOrCreate);
        memStream.WriteTo(fileStream);
        fileStream.Close();

        writer.Close();
        memStream.Close();
    }

    public void LoadSave(int slot)
    {
        string loadPath = Application.persistentDataPath + "/slot" + slot + ".dat";

        MemoryStream memStream = new MemoryStream();

        FileStream fileStream = new FileStream(loadPath, FileMode.Open);
        if(fileStream != null)
        {
            BinaryReader reader = new BinaryReader(memStream);
            fileStream.CopyTo(memStream);
            memStream.Position = 0;

            int version = reader.ReadInt32();
            if(version == SAVE_VERSION)
            {
                GameManager.instance.twoPlayer = reader.ReadBoolean();

                reader.Close();
                fileStream.Close();
            }
            else
            {
                Debug.LogError("SaveFile version is not correct");
            }
        }

        memStream.Close();
    }
}
