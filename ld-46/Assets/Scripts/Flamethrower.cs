using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public Transform target;
    public Transform rotationOrigin;

    private AudioClip clip;
    public AudioSource Audio;

    public bool trackTarget;

    [Tooltip("Shots per minute")]
    public float fireRate;
    public float fireDuration;
    public float firstFireDelay;

    public List<ParticleSystem> ParticleSystems;

    [Tooltip("Offset: x,y | Size: z,w")]
    public List<Vector4> ColliderSizes;

    public float GrowDuration;
    public float ShrinkDuration;

    public BoxCollider2D Coll;

    private void Start()
    {
        clip = Audio.clip;
        foreach(var particle in ParticleSystems)
        {
            particle.Stop(); 
            var main = particle.main;
            main.duration = fireDuration;
        }
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        if (trackTarget)
            LookAtTarget();
    }

    void LookAtTarget()
    {
        float rad = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x);
        // Get Angle in Degrees
        float deg = (180 / Mathf.PI) * rad;
        // Rotate Object
        rotationOrigin.rotation = Quaternion.Euler(0, 0, deg);
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(firstFireDelay);
        while (true)
        {
            foreach (var particle in ParticleSystems)
            {
                particle.Play();
                StartCoroutine(GrowCollider());
            }
            Audio.pitch = clip.length / fireDuration;
            Audio.Play();
            yield return new WaitForSeconds(fireDuration);
            StartCoroutine(ShrinkCollider());
            yield return new WaitForSeconds(60f/fireRate);
        }
    }

    private IEnumerator GrowCollider()
    {
        float startTime = Time.time;
        Vector2 fromOffset = new Vector2(ColliderSizes[0].x, ColliderSizes[0].y);
        Vector2 fromSize = new Vector2(ColliderSizes[0].z, ColliderSizes[0].w);

        Vector2 toOffset = new Vector2(ColliderSizes[1].x, ColliderSizes[1].y);
        Vector2 toSize = new Vector2(ColliderSizes[1].z, ColliderSizes[1].w);

        while (Time.time - startTime <= GrowDuration)
        {
            Coll.offset = Vector2.Lerp(fromOffset, toOffset, (Time.time - startTime) / GrowDuration);
            Coll.size = Vector2.Lerp(fromSize, toSize, (Time.time - startTime) / GrowDuration);
            yield return null;
        }        
    }

    private IEnumerator ShrinkCollider()
    {
        float startTime = Time.time;
        Vector2 fromOffset = new Vector2(ColliderSizes[1].x, ColliderSizes[1].y);
        Vector2 fromSize = new Vector2(ColliderSizes[1].z, ColliderSizes[1].w);

        Vector2 toOffset = new Vector2(ColliderSizes[2].x, ColliderSizes[2].y);
        Vector2 toSize = new Vector2(ColliderSizes[2].z, ColliderSizes[2].w);

        while (Time.time - startTime <= GrowDuration)
        {
            Coll.offset = Vector2.Lerp(fromOffset, toOffset, (Time.time - startTime) / GrowDuration);
            Coll.size = Vector2.Lerp(fromSize, toSize, (Time.time - startTime) / GrowDuration);
            yield return null;
        }
        Coll.offset = new Vector2(ColliderSizes[0].x, ColliderSizes[0].y);
        Coll.size = new Vector2(ColliderSizes[0].z, ColliderSizes[0].w);
    }
}
