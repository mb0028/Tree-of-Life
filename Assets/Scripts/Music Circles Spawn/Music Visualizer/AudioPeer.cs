using UnityEngine;

/// <summary>
/// use this to acsses Audio Visualizer Components.
/// values: [audioBand - audioBandBuffer]
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{

    public AudioSource audioSource;
    float[] samples = new float[512];
    float[] freeqBand = new float[8];
    float[] bandBuffer = new float[8];

    float[] bufferDecrease = new float[8];

    float[] freeqBandHighest = new float[8];


    /// <summary>
    /// use this to get an NON-smooth visualizer between 0 and 1
    /// </summary>
    public static float[] audioBand = new float[8];

    /// <summary>
    /// use this to get an smooth visualizer between 0 and 1
    /// example: transform.localscale = AudioPeer.audioBandBuffer[Band]
    /// Band is 0-7 number that defines buffer hrz band
    /// </summary>
    public static float[] audioBandBuffer = new float[8];


    void Update()
    {
        if (!MusicCircleSpawner.Instance.IsStarted)
            return;

        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freeqBand[i] > freeqBandHighest[i])
                freeqBandHighest[i] = freeqBand[i];
            
            audioBand[i] = freeqBand[i] / freeqBandHighest[i];
            audioBandBuffer[i] = bandBuffer[i] / freeqBandHighest[i];
        }
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (freeqBand[g] > bandBuffer[g])
            {
                bandBuffer[g] = freeqBand[g];
                bufferDecrease[g] = 0.005f;
            }

            if (freeqBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            freeqBand[i] = average * 10;
        }
    }

}
