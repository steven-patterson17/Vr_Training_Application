using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MusicPlaylistUI : MonoBehaviour
{
    public AudioSource[] speakers;
    public AudioClip[] playlist;
    public Sprite[] albumArts;

    // UI
    public TextMeshProUGUI songNameText;
    public TextMeshProUGUI songLengthText;
    public TextMeshProUGUI timeRemainingText;
    public Image albumArtImage;
    public Slider progressBar;

    // Buttons + Icons
    public Button pauseButton;
    public Button skipButton;
    public Image pauseButtonImage;
    public Sprite pauseIcon;
    public Sprite playIcon;

    private List<int> unplayedSongs = new List<int>();
    private double nextStartTime;
    private int currentSongIndex = -1;

    private bool isPaused = false;
    private double pauseStartTime;
    private double pausedDuration;

    void Start()
    {
        ResetSongPool();

        nextStartTime = AudioSettings.dspTime + 0.5;
        PlayNextSong(true);

        pauseButton.onClick.AddListener(TogglePause);
        skipButton.onClick.AddListener(() => PlayNextSong(true));
    }

    void Update()
    {
        if (currentSongIndex < 0 || isPaused)
            return;

        double timeRemaining = nextStartTime - AudioSettings.dspTime;
        float songLength = playlist[currentSongIndex].length;
        float timePlayed = songLength - (float)timeRemaining;

        // Update UI
        timeRemainingText.text = FormatTime((float)timeRemaining);
        progressBar.value = timePlayed / songLength;

        if (timeRemaining < 1.0)
        {
            PlayNextSong(false);
        }
    }

    void ResetSongPool()
    {
        unplayedSongs.Clear();
        for (int i = 0; i < playlist.Length; i++)
            unplayedSongs.Add(i);
    }

    void PlayNextSong(bool immediate)
    {
        if (unplayedSongs.Count == 0)
            ResetSongPool();

        int randomIndex = Random.Range(0, unplayedSongs.Count);
        currentSongIndex = unplayedSongs[randomIndex];
        unplayedSongs.RemoveAt(randomIndex);

        AudioClip clip = playlist[currentSongIndex];

        // If skipping or starting fresh → play immediately
        if (immediate)
            nextStartTime = AudioSettings.dspTime + 0.1;

        foreach (AudioSource s in speakers)
        {
            s.clip = clip;
            s.PlayScheduled(nextStartTime);
        }

        // Update UI
        songNameText.text = clip.name;
        songLengthText.text = FormatTime(clip.length);
        albumArtImage.sprite = albumArts[currentSongIndex];
        progressBar.value = 0;

        nextStartTime += clip.length;

        // If we were paused, reset pause state
        if (isPaused)
        {
            isPaused = false;
            pauseButtonImage.sprite = pauseIcon;
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseStartTime = AudioSettings.dspTime;

            foreach (AudioSource s in speakers)
                s.Pause();

            // Change button icon to PLAY
            pauseButtonImage.sprite = playIcon;
        }
        else
        {
            pausedDuration = AudioSettings.dspTime - pauseStartTime;
            nextStartTime += pausedDuration;

            foreach (AudioSource s in speakers)
                s.UnPause();

            // Change button icon back to PAUSE
            pauseButtonImage.sprite = pauseIcon;
        }
    }

    string FormatTime(float seconds)
    {
        seconds = Mathf.Max(0, seconds);
        int mins = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{mins:00}:{secs:00}";
    }
}
