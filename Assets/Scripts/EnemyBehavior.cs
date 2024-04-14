using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private GameController heroGameController = null;
    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

}
