﻿using System.Collections;
using UnityEngine;

/*
 * AUTHOR: Trenton Pottruff
 */

public class CreditsHolder : MonoBehaviour {
    public MenuManager menuManager;

    public void BackToMain() {
        StartCoroutine(AnimTimer());
    }

    IEnumerator AnimTimer() {
        yield return new WaitForSeconds(60f);
        menuManager.ChangeMenu(0);
    }
}
