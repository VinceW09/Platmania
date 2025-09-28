using UnityEngine;

public static class TimeFormatter
{
    public static string FormatTime(float time)
    {
        string formatedTime;
        int minutes = 0;

        while (time >= 60f)
        {
            time -= 60f;
            minutes += 1;
        }

        if (minutes == 0)
        {
            formatedTime = "00:" + string.Format("{0:00.00}", time);
        }
        else if (minutes < 10)
        {
            formatedTime = "0" + minutes + ":" + string.Format("{0:00.00}", time);
        }
        else
        {
            formatedTime = minutes + ":" + string.Format("{0:00.00}", time);
        }

        return formatedTime;
    }

    public static string FormatLeaderTime(float playerTime, float leaderTime)
    {
        string formatedLeaderTime;
        float timeDelta;
        int minutes = 0;

        timeDelta = playerTime - leaderTime;

        while (timeDelta >= 60f)
        {
            timeDelta -= 60f;
            minutes += 1;
        }

        if (minutes == 0)
        {
            formatedLeaderTime = "+" + string.Format("{0:00.00}", timeDelta);
        }
        else if (minutes < 10)
        {
            formatedLeaderTime = "+0" + minutes + ":" + string.Format("{0:00.00}", timeDelta);
        }
        else
        {
            formatedLeaderTime = "+" + minutes + ":" + string.Format("{0:00.00}", timeDelta);
        }

        return formatedLeaderTime;
    }
}
