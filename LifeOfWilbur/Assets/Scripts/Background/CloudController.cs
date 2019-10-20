using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller used on an empty game object to spawn some cool clouds for arctic ambience
/// </summary>
public class CloudController : MonoBehaviour
{
    /// <summary>
    /// Direction of cloud movement (magnitude is meaningless)
    /// </summary
    public Vector3 _windDirection = Vector3.right;
    
    /// <summary>
    /// Axis on which cloud spawn position may vary (magnitude is meaningless)
    /// </summary>
    public Vector3 _cloudSpawnAxis = Vector3.up;

    /// <summary>
    /// Standard deviation of cloud spawn positions.
    /// </summary>
    public float _cloudSpawnStdDev = 1.0f;

    /// <summary>
    /// Mean speed of clouds spawned
    /// </summary>
    public float _meanCloudSpeed;

    /// <summary>
    /// Standard deviation of cloud speed
    /// </summary>
    public float _cloudSpeedStdDevSec;

    /// <summary>
    /// Mean time between clouds
    /// </summary>
    public float _meanCloudSpawnTimeSec;

    /// <summary>
    /// Standard deviation of time between clouds
    /// </summary>
    public float _cloudSpawnTimeStdDevSec;

    /// <summary>
    /// How far clouds should travel before being despawned
    /// </summary>
    public float _cloudTravelDistance;
    
    /// <summary>
    /// Game object to spawn as a cloud
    /// </summary>
    public GameObject _cloudObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    /// <summary>
    /// Coroutine called to spawn clouds, called on component start
    /// </summary>
    private IEnumerator SpawnClouds()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(Mathf.Clamp(RandomGaussian(_meanCloudSpawnTimeSec, _cloudSpawnTimeStdDevSec), 0, float.PositiveInfinity));

            // Create a cloud
            var cloud = Instantiate(_cloudObject);
            cloud.transform.parent = transform;
            cloud.transform.localPosition = _cloudSpawnAxis * RandomGaussian(0, _cloudSpawnStdDev);
            StartCoroutine(MoveCloud(cloud, RandomGaussian(_meanCloudSpeed, _cloudSpeedStdDevSec)));
        }
    }

    private IEnumerator MoveCloud(GameObject cloud, float speed)
    {
        // why does Unity abuse IEnumerator for this????

        while(cloud.transform.localPosition.magnitude <= _cloudTravelDistance)
        {
            cloud.transform.localPosition += _windDirection * Time.deltaTime * speed;
            // wait a frame (why is yielding null the convention for this???)
            yield return null;
        }

        Destroy(cloud);
    }

    /// <summary>
    /// Sample a normal distribution N(mu, sigma).
    /// </summary>
    /// <param name="mu">Mean parameter for the normal distribution</param>
    /// <param name="sigma">Standard deviation parameter for the normal distribution</param>
    /// <returns>Sampled number from input distribution</returns>
    private float RandomGaussian(float mu, float sigma)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Cos(2 * Mathf.PI * u2);
        return z * sigma + mu;
    }
}
