using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    public float shakeDuration = 0.5f;
    public float amplitude = 0.5f;
    public float frequency = 0.5f;

    public float zoomSize;
    public float zoomTime = 2f;
    public Vector3 zoomPos;

    private float originalSize;
    private Vector3 originalPos;
    private Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
        originalSize = mainCam.orthographicSize;
        originalPos = transform.position;
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        StartCoroutine(DoWork());
        IEnumerator DoWork()
        {
            var origin = transform.localPosition;
            var rotation = transform.localRotation;
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;
            yield return new WaitForSeconds(shakeDuration);
            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
            yield return new WaitForSeconds(0.01f);
            transform.localPosition = origin;
            transform.localRotation = rotation;
        }
    }

    public void ZoomOnPlayer()
    {
        vcam.enabled = false;
        transform.parent = FindObjectOfType<Player>().transform;
        Vector3 startPos = transform.localPosition;
        float startTime = Time.unscaledTime;
        float startSize = mainCam.orthographicSize;
        StartCoroutine(Zoom());

        IEnumerator Zoom()
        {
            yield return null;
            transform.DOLocalMove(zoomPos, zoomTime * Time.timeScale);
            while (Time.unscaledTime - startTime <= zoomTime)
            {         
                mainCam.orthographicSize = Mathf.Lerp(startSize, zoomSize, (Time.unscaledTime - startTime) / zoomTime);
                yield return null;
            }
        }
    }

    public void ResetZoom()
    {
        transform.parent = null;
        mainCam.orthographicSize = originalSize;
        transform.position = originalPos;
        vcam.enabled = true;
    }
}