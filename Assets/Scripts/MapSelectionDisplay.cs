﻿using UnityEngine;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// AUTHOR: Taliyah Pottruff
/// </summary>
public class MapSelectionDisplay : MonoBehaviour {
    public MenuManager menuManager;
    public GameLoader gameLoader;

    [SerializeField]
    private Transform officialMapHolder;
    [SerializeField]
    private Transform customMapHolder;

    public GameObject mapSelectButtonPrefab;

    private ToggleGroup tg;

    private void Start() {
        tg = GetComponent<ToggleGroup>();

        PopulateList(); //Populate the list as soon as possible
    }

    private void PopulateList() {
        Map[] maps = new Map[Game.MAPS.Count];
        Game.MAPS.Values.CopyTo(maps, 0);
        string[] mapPaths = new string[] { };

        if (Directory.Exists(Application.persistentDataPath + "/maps"))
            mapPaths = Directory.GetFiles(Application.persistentDataPath + "/maps");

        string[] names = new string[mapPaths.Length];

        for (int i = 0; i < mapPaths.Length; i++) {
            names[i] = mapPaths[i].Replace(Application.persistentDataPath, "").Replace(".map", "").Replace("/maps\\", "");
        }

        //Populate the official maps
        Utilities.ClearChildren(officialMapHolder);
        for (int i = 0; i < maps.Length; i++) {
            GameObject go = Instantiate(mapSelectButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            go.name = mapSelectButtonPrefab.name;
            go.transform.SetParent(officialMapHolder);
            MapSelectButton msb = go.GetComponent<MapSelectButton>();
            
            msb.SetInfo(maps[i].name, "This is a map!");
            if (tg != null) {
                go.GetComponent<Toggle>().group = tg;

                if (maps[i].name.Equals(Game.CURRENT_MAP)) go.GetComponent<Toggle>().isOn = true;
            }
        }

        //Populate the custom maps
        Utilities.ClearChildren(customMapHolder);
        for (int i = 0; i < names.Length; i++) {
            GameObject go = Instantiate(mapSelectButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            go.name = mapSelectButtonPrefab.name;
            go.transform.SetParent(customMapHolder);
            MapSelectButton msb = go.GetComponent<MapSelectButton>();

            msb.SetInfo(names[i], "This is a map!");
            go.GetComponent<Toggle>().group = tg;
        }
    }

    /// <summary>
    /// Plays the game
    /// </summary>
    public void PlayGame() {
        gameLoader.LoadGame();
    }

    /// <summary>
    /// Returns the user to the main menu
    /// </summary>
    public void BackToMain() {
        menuManager.ChangeMenu(0);
    }
}