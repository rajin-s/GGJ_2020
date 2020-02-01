using System;
using System.Diagnostics;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    private static RhythmManager singleton;
    private static void DoNothing() { }

    private Action wholeCallback = DoNothing;
    private Action halfCallback = DoNothing;
    private Action quarterCallback = DoNothing;
    private Action eighthCallback = DoNothing;
    private Action sixteenthCallback = DoNothing;

    private Action halfTripletCallback = DoNothing;
    private Action quarterTripletCallback = DoNothing;
    private Action eighthTripletCallback = DoNothing;

    [SerializeField] private float beatsPerMinute;
    private float secondsPerSixteenth;
    private float secondsPerEighthTriplet;

    private bool playing = false;
    private float time;
    private int sixteenthCount;
    private int eighthTripletCount;

    private Stopwatch timer;

    private void Awake()
    {
        singleton = this;
        secondsPerSixteenth = 60f / beatsPerMinute / 4f;
        secondsPerEighthTriplet = 60f / beatsPerMinute / 6f;
    }

    private void OnDestroy()
    {
        singleton = null;
    }

    private void Update()
    {
        if (playing)
        {
            UpdateTime();
        }
    }

    public void StartTime()
    {
        playing = true;

        timer = new Stopwatch();
        timer.Start();

        sixteenthCount = 0;
        eighthTripletCount = 0;
    }
    public void EndTime()
    {
        playing = false;
        timer.Stop();
    }

    private void UpdateTime()
    {
        time = (float)(((double)timer.ElapsedMilliseconds) / 1000.0);
        if (time > sixteenthCount * secondsPerSixteenth)
        {
            if (sixteenthCount % 16 == 0)
            {
                wholeCallback();
                halfCallback();
                quarterCallback();
                eighthCallback();
                sixteenthCallback();
            }
            else if (sixteenthCount % 8 == 0)
            {
                halfCallback();
                quarterCallback();
                eighthCallback();
                sixteenthCallback();
            }
            else if (sixteenthCount % 4 == 0)
            {
                quarterCallback();
                eighthCallback();
                sixteenthCallback();
            }
            else if (sixteenthCount % 2 == 0)
            {
                eighthCallback();
                sixteenthCallback();
            }
            else
            {
                sixteenthCallback();
            }

            sixteenthCount += 1;
        }
        if (time > eighthTripletCount * secondsPerEighthTriplet)
        {
            if (eighthTripletCount % 4 == 0)
            {
                halfTripletCallback();
                quarterTripletCallback();
                eighthTripletCallback();
            }
            else if (eighthTripletCount % 2 == 0)
            {
                quarterTripletCallback();
                eighthTripletCallback();
            }
            else
            {
                eighthTripletCallback();
            }

            eighthTripletCount += 1;
        }
    }

    public static void AddHandler(Rhythm.Subdivision subdivision, Action action)
    {
        switch (subdivision)
        {
            case Rhythm.Subdivision.Whole:
                {
                    singleton.wholeCallback += action;
                    break;
                }
            case Rhythm.Subdivision.Half:
                {
                    singleton.halfCallback += action;
                    break;
                }
            case Rhythm.Subdivision.Quarter:
                {
                    singleton.quarterCallback += action;
                    break;
                }
            case Rhythm.Subdivision.Eighth:
                {
                    singleton.eighthCallback += action;
                    break;
                }
            case Rhythm.Subdivision.Sixteenth:
                {
                    singleton.sixteenthCallback += action;
                    break;
                }
            case Rhythm.Subdivision.HalfTriplet:
                {
                    singleton.halfTripletCallback += action;
                    break;
                }
            case Rhythm.Subdivision.QuarterTriplet:
                {
                    singleton.quarterTripletCallback += action;
                    break;
                }
            case Rhythm.Subdivision.EighthTriplet:
                {
                    singleton.eighthTripletCallback += action;
                    break;
                }
        }
    }

    public static void RemoveHandler(Rhythm.Subdivision subdivision, Action action)
    {
        switch (subdivision)
        {
            case Rhythm.Subdivision.Whole:
                {
                    singleton.wholeCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.Half:
                {
                    singleton.halfCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.Quarter:
                {
                    singleton.quarterCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.Eighth:
                {
                    singleton.eighthCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.Sixteenth:
                {
                    singleton.sixteenthCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.HalfTriplet:
                {
                    singleton.halfTripletCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.QuarterTriplet:
                {
                    singleton.quarterTripletCallback -= action;
                    break;
                }
            case Rhythm.Subdivision.EighthTriplet:
                {
                    singleton.eighthTripletCallback -= action;
                    break;
                }
        }
    }
}
