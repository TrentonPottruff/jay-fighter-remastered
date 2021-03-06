﻿using System.Collections.Generic;
using UnityEngine;

/*
 * AUTHOR: Trenton Pottruff
 */

//TODO Debate whether to keep in-game music or not
[RequireComponent(typeof(AudioSource))]
[System.Obsolete("Implements a class that uses old Unity networking")]
public class MusicManager : MonoBehaviour {
    public List<AudioClip> songs;
    public MusicNotification musicNotification;

    private AudioSource aSource;
    
    private int[] songQueue = new int[0];
    private int nextSong = 0;

    private void Start() {
        aSource = GetComponent<AudioSource>();
    }

    private void Update() {
        //aSource.volume = Game.MUSIC_VOLUME;
        aSource.volume = Game.GetMusicVolume();

        if (nextSong >= songQueue.Length) {
            songQueue = new int[songs.Count];

            for (int i = 0; i < songQueue.Length; i++) {
                songQueue[i] = i;
            }

            Shuffle();
            DebugQueue();

            nextSong = 0;
            return;
        }

        if (!aSource.isPlaying) {
            AudioClip nextClip = songs[songQueue[nextSong]];
            aSource.PlayOneShot(nextClip);
            Debug.Log("Now playing " + nextClip.name);
            string[] parts = nextClip.name.Split('-');
            musicNotification.artist = parts[0];
            musicNotification.song = parts[1].Remove(0, 1);
            musicNotification.Open();
            nextSong++;

            return;
        }
    }

    private void Shuffle() {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int i = 0; i < songQueue.Length; i++) {
            int tmp = songQueue[i];
            int r = Random.Range(i, songQueue.Length);
            songQueue[i] = songQueue[r];
            songQueue[r] = tmp;
        }
    }

    private void DebugQueue() {
        Debug.Log("Song Queue is:");
        for (int i = 0; i < songQueue.Length; i++) {
            Debug.Log(songs[songQueue[i]].name);
        }
    }
}