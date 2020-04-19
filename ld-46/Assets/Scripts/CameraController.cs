using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    public float shakeDuration = 0.5f;
    public float amplitude = 0.5f;
    public float frequency = 0.5f;
    private void Start()
    {
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        StartCoroutine(DoWork());
        IEnumerator DoWork()
        {
            var origin = transform.localPosition;
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;
            yield return new WaitForSeconds(shakeDuration);
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
            yield return new WaitForSeconds(0.01f);
            transform.localPosition = origin;
        }
    }
}