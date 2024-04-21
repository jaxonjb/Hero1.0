using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    private GameController heroGameController = null;
    public int flagNum;
    
    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindFirstObjectByType<GameController>();
        heroGameController.originalPosition(flagNum, transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rocket")
        {
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            Color c = s.color;
            const float delta = 0.20f;
            c.r -= delta;
            c.a -= delta;
            s.color = c;
            Debug.Log("Checkpoint: Color = " + c);
            if(c.a <= 0.3f)
            {
                Vector3 newPos = heroGameController.RespawnCheckpoint(flagNum);
                c.r = 1f;
                c.a = 1f;
                s.color = c;
                transform.position = newPos;
            }
        }
    }
}
