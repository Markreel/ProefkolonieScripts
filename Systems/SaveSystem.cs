using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveData SaveData = new SaveData();

    public static void ActivateEasterEgg(bool _value)
    {
        LoadGame();
        SaveData.EasterEggtivated = _value;
        SaveGame();
    }

    public static void UpdateSaveData()
    {
        SaveData.Day = GameManager.Instance.TimeSystem.CurrentDate.Day;
        SaveData.Month = GameManager.Instance.TimeSystem.CurrentDate.Month;
        SaveData.Year = GameManager.Instance.TimeSystem.CurrentDate.Year;

        SaveData.ColonistContentmentLevel = GameManager.Instance.ColonistContentmentLevel;
        SaveData.SubcommissionContentmentLevel = GameManager.Instance.SubCommissionLevel;
        SaveData.PreviousIncome = GameManager.Instance.MoneyOfPreviousMonth;
        SaveData.Income = GameManager.Instance.Money;
    }

    public static void SaveGame()
    {
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(Application.persistentDataPath + "/SaveFile.dat");

        _bf.Serialize(_file, SaveData);
        _file.Close();
    }

    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);

            SaveData = (SaveData)bf.Deserialize(file);
            file.Close();

            if(GameManager.Instance != null)
            {
                if(SaveData.Day != -1 && SaveData.Month != -1 && SaveData.Year != -1)
                {
                    //Save new date
                    Date _newDate = new Date();
                    _newDate.Day = SaveData.Day;
                    _newDate.Month = SaveData.Month;
                    _newDate.Year = SaveData.Year;

                    //Set new date
                    GameManager.Instance.TimeSystem.SetDate(_newDate);
                }


                //Set money and contentment levels
                if (SaveData.ColonistContentmentLevel != -1) { GameManager.Instance.ColonistContentmentLevel = SaveData.ColonistContentmentLevel; }
                if (SaveData.SubcommissionContentmentLevel != -1) {GameManager.Instance.SubCommissionLevel = SaveData.SubcommissionContentmentLevel; }
                if (SaveData.PreviousIncome != -1) {GameManager.Instance.MoneyOfPreviousMonth = SaveData.PreviousIncome; }
                if (SaveData.Income != -1) { GameManager.Instance.Money = SaveData.Income; }
            }

        }
    }

    public static void DeleteSaveFile()
    {
        SaveData = new SaveData();

        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            File.Delete(Application.persistentDataPath + "/SaveFile.dat");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public int Day = -1;
    public int Month = -1;
    public int Year = -1;

    public float ColonistContentmentLevel = -1;
    public float SubcommissionContentmentLevel = -1;
    public float PreviousIncome = -1;
    public float Income = -1;

    public bool EasterEggtivated;
}
