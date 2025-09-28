using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; }

    [SerializeField] int playingTimeMinutes;

    private enum State
    {
        WaitingToStart,
        Playing,
        GameOver,
    }

    private State state;

    private float timeRemainingSeconds;

    public event EventHandler OnTournamentStart;
    public event EventHandler OnTournamentEnd;

    private void Awake()
    {
        Singleton = this;
        state = State.WaitingToStart;

        timeRemainingSeconds = playingTimeMinutes * 60;
    }

    private void Update()
    {
        switch (state)
        {
            case (State.WaitingToStart):
                break;
            case (State.Playing):

                timeRemainingSeconds -= Time.deltaTime;

                if (timeRemainingSeconds <= 0)
                {
                    state = State.GameOver;
                    OnTournamentEnd?.Invoke(this, EventArgs.Empty);
                }

                break;
            case (State.GameOver):
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.Playing;
    }

    public float GetRemainingTimeInSeconds()
    {
        return timeRemainingSeconds;
    }

    public void StartTournament()
    {
        state = State.Playing;

        OnTournamentStart?.Invoke(this, EventArgs.Empty);
    }

    public void SetPlayingTimeMinutes(int minutes)
    {
        timeRemainingSeconds = minutes * 60;
    }
}
