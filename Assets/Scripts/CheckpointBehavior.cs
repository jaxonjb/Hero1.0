using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    private GameController heroGameController = null;
    public Camera checkpointCamera;
    public int flagNum;
    private bool isHidden = false;
    private float hit = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindFirstObjectByType<GameController>();
        heroGameController.originalPosition(flagNum, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(isHidden != heroGameController.GetCheckpointHidden())
        {
            isHidden = heroGameController.GetCheckpointHidden();
            ToggleHide(isHidden);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rocket" && isHidden == false)
        {
            //Rocket
            Destroy(collision.gameObject);
            heroGameController.RocketDestroyed();

            //Checkpoint
            hit += 1f;
            Vector3 pos = transform.position;
            pos.z = -1f;
            checkpointCamera.transform.position = pos;
            CameraShake cameraShake = checkpointCamera.GetComponent<CameraShake>();
            StartCoroutine(cameraShake.Shake(hit, hit));
            SpriteRenderer s = GetComponent<SpriteRenderer>();
            Color c = s.color;
            const float delta = 0.20f;
            c.r -= delta;
            c.a -= delta;
            s.color = c;
            if(c.a <= 0.3f)
            {
                Vector3 newPos = heroGameController.RespawnCheckpoint(flagNum);
                c.r = 1f;
                c.a = 1f;
                s.color = c;
                hit = 0;
                transform.position = newPos;
            }
        }
    }
    private void ToggleHide(bool isHidden)
    {
        if(isHidden == true){
            gameObject.GetComponent<Renderer>().enabled = false;
        }else{
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
