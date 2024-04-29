using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    public Transform offScreen;
    public Transform target;
    public Vector3 offset;
    public Text EnemyCam;
    public void ZoomIn(Vector3 target2)
    {
        EnemyCam.text = "Enemy Cam: Active";
        Camera c = GetComponent<Camera>();
        float currentZoom = Vector3.Distance(target.position, target2);
        Debug.Log("Distance from enemy to player:  " + currentZoom);
        Vector3 pos = (target.position + target2)/2;
        pos.z = -1;
        Debug.Log(pos);
        transform.position = pos;
        c.orthographicSize = currentZoom;
        
    }

    public void reset()
    {
        EnemyCam.text = "Enemy Cam: Inactive";
        Camera c = GetComponent<Camera>();
        Vector3 desiredPosition = offScreen.position + offset;
        c.transform.position = desiredPosition;
        c.orthographicSize = 15f;
    }
}