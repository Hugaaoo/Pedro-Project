using UnityEngine;

public class MusicVisualizer : MonoBehaviour
{
    public AudioSource musicSource;
    public Transform player;
    public float levelLengthInUnits; // Longueur du niveau en unités Unity

    void OnGUI()
    {
        // Bar de progression
        float progress = musicSource.time / musicSource.clip.length;
        float playerPos = player.position.x / levelLengthInUnits;

        GUI.Box(new Rect(10, 10, 200, 25), "Music Progress");
        GUI.HorizontalScrollbar(new Rect(10, 35, 200, 25), 0, progress, 0, 1);

        GUI.Box(new Rect(10, 60, 200, 25), "Player Progress");
        GUI.HorizontalScrollbar(new Rect(10, 85, 200, 25), 0, playerPos, 0, 1);
    }
}
