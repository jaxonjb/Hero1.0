using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 45f;
    private int currentCheckPoint;
    private GameController heroGameController = null;
    private int time = 0;
    private bool inOrder = true;
    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindFirstObjectByType<GameController>();
        currentCheckPoint = Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        inOrder = heroGameController.getOrder();
        Vector3 target = heroGameController.GetFlagPos(currentCheckPoint);
        Debug.Log("Distance: " + Vector3.Distance(transform.position, target));
        if(Vector3.Distance(transform.position, target) < 15){
            time = 0;
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
        transform.position += transform.up * speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 targetDir = target - transform.position;
        Debug.Log("target: " + target);
        PointAtCheckpoint(currentCheckPoint, targetDir);
        time++;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rocket"){
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            Color c = s.color;
            const float delta = 0.20f;
            c.r -= delta;
            c.a -= delta;
            s.color = c;
            //Debug.Log("Plane: Color = " + c);
            if(c.a <= 0.3f)
            {
                Destroy(transform.gameObject);
                heroGameController.EnemyDestroyed();
            }
        }
    }
    private void PointAtCheckpoint(int cp, Vector3 targetDir)
    {
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
}

