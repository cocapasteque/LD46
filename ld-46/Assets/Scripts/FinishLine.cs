using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public string unlockingLevel;
    public GameObject effectPrefab;
    public Transform[] effectOrigin;
    public AudioClip FinishClip;

    private AudioSource audioSource;
    private AudioClip originalClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalClip = audioSource.clip;
    }

    public void PlayOriginalClip()
    {
        audioSource.Stop();
        audioSource.clip = originalClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Camera.main.GetComponent<CameraController>().ZoomOnPlayer();
        if (other.CompareTag("Player"))
        {
            other.enabled = false;
            audioSource.Stop();
            audioSource.clip = FinishClip;
            audioSource.loop = false;
            audioSource.Play();
            foreach (var origin in effectOrigin)
            {
                var effect = Instantiate(effectPrefab, origin);
                effect.transform.localPosition = Vector3.zero;
                Destroy(effect, 5);
            }

            // Unlocking level
            var levelsString = PlayerPrefs.GetString("unlocked_levels", "['Tutorial','Level 1']");
            Debug.Log(levelsString);
            var array = JsonConvert.DeserializeObject<string[]>(levelsString);
            var list = array.ToList();
            
            if (!list.Contains(unlockingLevel))
            {
                list.Add(unlockingLevel);
            }

            levelsString = JsonConvert.SerializeObject(list.ToArray());
            Debug.Log(levelsString);
            PlayerPrefs.SetString("unlocked_levels", levelsString);
            GameManager.Instance.ReachedFinishLine();
            PlayerPrefs.Save();
        }
    }
}