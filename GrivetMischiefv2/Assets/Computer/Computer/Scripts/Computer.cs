using Photon.Pun;
using Photon.VR; // i fucking DESPISE photon vr
using System;
using TMPro;
using UnityEngine;
public class Computer : MonoBehaviour
{
    public static Computer instance;
    //mango
    public ComputerSettings computerSettings;

    public PageData[] screenPages;

    public TextMeshPro screenText;

    public TextMeshPro pageText;
    [Header("Read only")]
    public float lastScreenUpdate = 0;
    
    public int currentPage = 0;

    public string newName;

    public string newCode;

    public Color playerColor;
    [Header("Debug")]
    public bool forcefullyUpdateScreen = false;

    private string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    private string currentName;

    private int redValue;

    private int greenValue;

    private int blueValue;

    private int colorRow = 0;

    private PageData currentPageData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GetSettings();
    }

    private void GetSettings()
    {
        redValue = PlayerPrefs.GetInt("RedValue", UnityEngine.Random.Range(0, 9));
        greenValue = PlayerPrefs.GetInt("GreenValue", UnityEngine.Random.Range(0, 9));
        blueValue = PlayerPrefs.GetInt("BlueValue", UnityEngine.Random.Range(0, 9));

        newName = PlayerPrefs.GetString("PlayerName", "Gorilla" + UnityEngine.Random.Range(0, 10000).ToString("D4"));
        UpdateName(newName);

        PlayerPrefs.Save();
    }

    private void Update()
    {
        playerColor = new Color(redValue / 9f, greenValue / 9f, blueValue / 9f);
        PhotonVRManager.SetColour(playerColor);

        if (computerSettings.onlyUpdateWhenConnectedToPhoton && !PhotonNetwork.IsConnectedAndReady) return;

        if (lastScreenUpdate + computerSettings.screenUpdateInterval < Time.time)
        {
            UpdateScreen();
            lastScreenUpdate = Time.time;
        }
        if (forcefullyUpdateScreen)
        {
            forcefullyUpdateScreen = false;
            UpdateScreen();
        }
        currentPageData = screenPages[currentPage];
    }
    public void KeyFunction(ComputerKey key)
    {
        switch (key.keyCode)
        {
            case "PageUp":
                currentPage = (currentPage - 1 + screenPages.Length) % screenPages.Length;
                break;
            case "PageDown":
                currentPage = (currentPage + 1) % screenPages.Length;
                break;
        }
        switch (currentPageData.extraStuff)
        {
            case ExtraStuff.Name:
                switch (key.keyCode)
                {
                    default:
                        if (newName.Length < computerSettings.maxCharacters && allowedCharacters.Contains(key.keyCode.ToUpper()))
                        {
                            newName += key.keyCode;
                        }
                        break;
                    case "Delete":
                        if (newName.Length > 0)
                        {
                            newName = newName.Remove(newName.Length - 1);
                        }
                        break;
                    case "Enter": 
                        if (newName.Length > 0 && currentName != newName)
                        {
                            UpdateName(newName);
                        }
                        else
                        {
                            UpdateName("Gorilla" + UnityEngine.Random.Range(0,10000).ToString("D4"));
                        }
                        break;
                }
                break;
            case ExtraStuff.Code:
                switch (key.keyCode)
                {
                    default:
                        if (newCode.Length < computerSettings.maxCharacters && allowedCharacters.Contains(key.keyCode.ToUpper()))
                        {
                            newCode += key.keyCode;
                        }
                        break;
                    case "Delete":
                        if (newCode.Length > 0)
                        {
                            newCode = newCode.Remove(newCode.Length - 1);
                        }
                        break;
                    case "Enter":
                        if (newCode.Length > 0)
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                if (PhotonNetwork.CurrentRoom.Name != newCode)
                                {
                                    PhotonVRManager.JoinPrivateRoom(newCode);
                                    return;
                                }
                            }
                            PhotonVRManager.JoinPrivateRoom(newCode);
                        }
                        break;
                    case "Option1":
                        if (PhotonNetwork.InRoom)
                        {
                            PhotonNetwork.LeaveRoom();
                        }
                        break;
                }
                break;
            case ExtraStuff.Color:
                if (int.TryParse(key.keyCode, out var num))
                {
                    switch (colorRow)
                    {
                        case 0:
                            redValue = num;
                            break;
                        case 1:
                            greenValue = num;
                            break;
                        case 2:
                            blueValue = num;
                            break;
                    }
                    PlayerPrefs.SetInt("RedValue", redValue);
                    PlayerPrefs.SetInt("GreenValue", greenValue);
                    PlayerPrefs.SetInt("BlueValue", blueValue);

                    PlayerPrefs.Save();
                }
                switch (key.keyCode)
                {
                    case "Option1":
                        colorRow = 0;
                        break;
                    case "Option2":
                        colorRow = 1;
                        break;
                    case "Option3":
                        colorRow = 2;
                        break;
                }
                break;
        }
        UpdateScreen();
    }
    private void UpdateName(string name)
    {
        PhotonVRManager.SetUsername(name);
        currentName = name;
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
    }

    private void UpdateScreen()
    {
        currentPage = Mathf.Clamp(currentPage, 0, screenPages.Length - 1);

        string currentCode = PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : "Not in a room";
        
        string extraStuffText = "";
        
        switch (screenPages[currentPage].extraStuff)
        {
            case ExtraStuff.Name:
                extraStuffText = $"Current Name: {currentName}\r\nNew Name: {newName}";
                break;
            case ExtraStuff.Code:
                extraStuffText = $"Current Code: {currentCode}\r\nNew Code: {newCode}";
                break;
            case ExtraStuff.Color:
                extraStuffText = $"Red: {redValue}\r\nGreen: {greenValue}\r\nBlue: {blueValue}";
                break;
            default:
                extraStuffText = "";
                break;
        }

        screenText.text = screenPages[currentPage].pageText + $"\r\n\n{extraStuffText}";

        string allPagesText = "";
        for (int i = 0; i < screenPages.Length; i++)
        {
            allPagesText += screenPages[i].pageName;
            if (i == currentPage)
            {
                allPagesText += " <-";
            }
            allPagesText += "\n";

        }

        pageText.text = allPagesText;

    }

    [Serializable]
    public class ComputerSettings
    {
        [Header("Computer Settings")]
        [Header("Hover over the variable name to see what it is")]

        [Tooltip("This is the interval (time) at which the screen updates whenever you are not using the keyboard")]
        public float screenUpdateInterval = 1.5f;
        [Tooltip("Only makes it so the computer updates if the player is connected to photon")]
        public bool onlyUpdateWhenConnectedToPhoton = true;
        [Tooltip("self explanitory")]
        public int maxCharacters = 12;
    }

    [Serializable]
    public class PageData
    {
        [Header("Page Data")]
        [Header("Hover over the variable name to see what it is")]

        [Tooltip("This is the name of the page for organization")]
        public string pageName;
        [Tooltip("This is the text that will be displayed on the page also PLEASE use line breakers (enter press)"), TextArea]
        public string pageText;
        [Tooltip("This is extra stuff you can add on to it")]
        public ExtraStuff extraStuff; 
    }

    public enum ExtraStuff
    {
        None,
        Name,
        Code,
        Color,
    }
}
