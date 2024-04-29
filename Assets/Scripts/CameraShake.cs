using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShake : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public Text CheckpointCam;


    public IEnumerator Shake (float duration, float magnitude)
    {
        CheckpointCam.text = "CheckPoint Cam: Active";
        Camera c = GetComponent<Camera>();
        Vector3 originalPos = c.transform.position;

        float elapsed = 0.0f;
        if (magnitude < 4f){
            while (elapsed < duration)
            {
                float x = Random.Range(-0.1f, 0.1f) * magnitude;
                float y = Random.Range(-0.1f, 0.1f) * magnitude;

                c.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }
        }
        CheckpointCam.text = "CheckPoint Cam: Inactive";
        Vector3 desiredPosition = target.position + offset;
        c.transform.position = desiredPosition;   
        
    }
}