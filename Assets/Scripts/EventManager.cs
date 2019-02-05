using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void GameEvent();
    public static event GameEvent GameStart, GameOver, GamePause;
    public static event GameEvent LevelUp, CoinCollected;
    public static event GameEvent PowerCharged, ActivatePower, StopPower;

    public static void TriggerGameStart()
    {
        if (GameStart != null)
            GameStart();
    }

    public static void TriggerGameOver()
    {
        if (GameOver != null)
            GameOver();
    }

    public static void TriggerGamePause()
    {
        if (GamePause != null)
            GamePause();
    }

    public static void TriggerLevelUp()
    {
        if (LevelUp != null)
            LevelUp();
    }

    public static void TriggerCoinCollected()
    {
        if (CoinCollected != null)
            CoinCollected();
    }

    public static void TriggerPowerCharged()
    {
        if (PowerCharged != null)
            PowerCharged();
    }

    public static void TriggerActivatePower()
    {
        if (ActivatePower != null)
            ActivatePower();
    }

    public static void TriggerStopPower()
    {
        if (StopPower != null)
            StopPower();
    }
}
