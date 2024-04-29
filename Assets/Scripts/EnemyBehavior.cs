using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 45f;
    private int currentCheckPoint;
    private GameController heroGameController = null;
    private GameObject player = null;
    private GameObject EnemyCamera;
    private Camera cam;
    private CameraZoom zoom;
    private bool inOrder = true;
    private int hitCount = 0;
    public Sprite newSprite;
    private string State = "Roaming";
    private Quaternion alertRotation;
    private bool reachedLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindFirstObjectByType<GameController>();
        currentCheckPoint = Random.Range(0, 6);
        player = GameObject.FindGameObjectWithTag("Player");
        EnemyCamera = GameObject.FindGameObjectWithTag("Camera");
        cam = EnemyCamera.GetComponent<Camera>();
        zoom = cam.GetComponent<CameraZoom>();
    }

    // Update is called once per frame
    void Update()
    {    
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        Color c = s.color;
        if(State == "Roaming")
        {
            c.r = 255;
            c.g = 255;
            c.b = 255;
            PointAtCheckpoint();
        }else if (State == "Alert") 
        {
            c.r = 255;
            c.g = 0;
            c.b = 0;
            PlaneAlert();
        }else if(State == "Chasing")
        {
            zoom.ZoomIn(transform.position);
            c.r = 255;
            c.g = 0;
            c.b = 0;
            PointAtPlayer();
        }else if(State == "Spinning"){
            c.r = 255;
            c.g = 255;
            c.b = 0;
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
        }
        s.color = c;
    }
    
    private void PointAtCheckpoint()
    {
        inOrder = heroGameController.getOrder();
        Vector3 target = heroGameController.GetFlagPos(currentCheckPoint);
        if(Vector3.Distance(transform.position, target) < 15){
            if(inOrder == false){
                int i = currentCheckPoint;
                while (i == currentCheckPoint){
                    currentCheckPoint = Random.Range(0,6);
                }
            }else{
                if(currentCheckPoint == 5){
                    currentCheckPoint = 0;
                }else{
                    currentCheckPoint += 1;
                }
            }
        }
        Vector3 targetDir = target - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.position += transform.up * speed * Time.deltaTime;
    }
    private void PointAtPlayer()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < 90){
            Vector3 target = player.transform.position;
            Vector3 targetDir = target - transform.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.up * speed * Time.deltaTime;
        }else{
            State = "Alert";
        }
    }
    private void PlaneAlert()
    {
        int angle = 60;
        if(alertRotation.z < 0) angle = -angle;
        if(!reachedLeft){
            Quaternion leftTarget = Quaternion.Euler(alertRotation.x, alertRotation.y, alertRotation.z - angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, leftTarget, rotationSpeed * Time.deltaTime);
            if(transform.rotation == leftTarget){
                reachedLeft = true;
            }
        }else if (reachedLeft == true){
            Quaternion rightTarget = Quaternion.Euler(alertRotation.x, alertRotation.y, alertRotation.z + angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rightTarget, rotationSpeed * Time.deltaTime);
            if(transform.rotation == rightTarget){
                if (Vector3.Distance(transform.position, player.transform.position) < 40){ 
                    State = "Chasing";
                }else {
                    State = "Roaming";
                }
                reachedLeft = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rocket"){
            zoom.reset();
            hitCount += 1;
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            if(hitCount == 1){
                State = "Spinning";
            }else if (hitCount == 2){
                s.sprite = newSprite;
                State = "Egg";
            }else{
                Destroy(transform.gameObject);
                heroGameController.EnemyDestroyed();
            }
            
        }else if (collision.tag == "Player" && State != "Egg"){
            if(State == "Chasing"){
                heroGameController.EnemyDestroyed();
                zoom.reset();
                Destroy(transform.gameObject);
            }else{
                alertRotation = transform.rotation;
                State = "Alert";
            } 
        }
      
    }
    
}

