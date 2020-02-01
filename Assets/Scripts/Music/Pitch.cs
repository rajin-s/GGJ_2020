using UnityEngine;

public static class Pitch
{
    public enum NoteName
    {
        C,
        DFlat,
        D,
        EFlat,
        E,
        F,
        GFlat,
        G,
        AFlat,
        A,
        BFlat,
        B,
    }

    public static float GetRootPitchScale(NoteName note, int octave, double referencePitch = 1.0)
    {
        // Base pitch is at octave 4
        double octaveScale = ((double)(1 << octave)) / 16.0;

        double basePitch;
        switch (note)
        {
            case NoteName.C:
                {
                    basePitch = 261.63;
                    break;
                }
            case NoteName.DFlat:
                {
                    basePitch = 277.18;
                    break;
                }
            case NoteName.D:
                {
                    basePitch = 293.66;
                    break;
                }
            case NoteName.EFlat:
                {
                    basePitch = 311.13;
                    break;
                }
            case NoteName.E:
                {
                    basePitch = 329.63;
                    break;
                }
            case NoteName.F:
                {
                    basePitch = 349.23;
                    break;
                }
            case NoteName.GFlat:
                {
                    basePitch = 369.99;
                    break;
                }
            case NoteName.G:
                {
                    basePitch = 392.00;
                    break;
                }
            case NoteName.AFlat:
                {
                    basePitch = 415.30;
                    break;
                }
            case NoteName.A:
                {
                    basePitch = 440.00;
                    break;
                }
            case NoteName.BFlat:
                {
                    basePitch = 466.16;
                    break;
                }
            case NoteName.B:
                {
                    basePitch = 493.88;
                    break;
                }
            default:
                {
                    basePitch = 0;
                    break;
                }
        }

        return (float)(basePitch * octaveScale / referencePitch);
    }

    public static float GetChromaticRatio(int steps)
    {
        double octaveRatio = 1.0;

        while (steps >= 12)
        {
            octaveRatio *= 2.0;
            steps -= 12;
        }

        double ratio;
        switch (steps)
        {
            case 1:
                {
                    ratio = 256.0 / 243.0;
                    break;
                }
            case 2:
                {
                    ratio = 9.0 / 8.0;
                    break;
                }
            case 3:
                {
                    ratio = 32.0 / 27.0;
                    break;
                }
            case 4:
                {
                    ratio = 5.0 / 4.0;
                    break;
                }
            case 5:
                {
                    ratio = 4.0 / 3.0;
                    break;
                }
            case 6:
                {
                    ratio = 1024.0 / 729.0;
                    break;
                }
            case 7:
                {
                    ratio = 3.0 / 2.0;
                    break;
                }
            case 8:
                {
                    ratio = 128.0 / 81.0;
                    break;
                }
            case 9:
                {
                    ratio = 5.0 / 3.0;
                    break;
                }
            case 10:
                {
                    ratio = 16.0 / 9.0;
                    break;
                }
            case 11:
                {
                    ratio = 15.0 / 8.0;
                    break;
                }
            default:
                {
                    ratio = 1.0;
                    break;
                }
        }

        return (float)(ratio * octaveRatio);
    }
}