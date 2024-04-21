using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 45f;
    private int currentCheckPoint;
    private GameController heroGameController = null;
    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindFirstObjectByType<GameController>();
        currentCheckPoint = Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = heroGameController.GetFlagPos(currentCheckPoint);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 targetDir = target - transform.position;
        if (Vector3.Dot(transform.up, targetDir) < 0.99f)
        {
            // Plane needs to adjust its orientation towards the checkpoint
            PointAtCheckpoint(currentCheckPoint, targetDir);
        }
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
            Debug.Log("Plane: Color = " + c);
            if(c.a <= 0.3f)
            {
                Destroy(transform.gameObject);
                heroGameController.EnemyDestroyed();
            }
        }
        if(collision.tag == "Checkpoint")
        {
            if(currentCheckPoint == 5){
                currentCheckPoint = 0;
            }else{
                currentCheckPoint += 1;
            }
        }
    }
    private void PointAtCheckpoint(int cp, Vector3 targetDir)
    {
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Debug.Log("ANGLE: " + angle);
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = targetRotation;//Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
