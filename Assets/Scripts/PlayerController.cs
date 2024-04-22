using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text Touched = null;
    public float speed = 10f;
    public float playerRotateSpeed = 90.0f;
    public bool mouseController = true;
    public float cooldown = 0.2f;
    private float time = 0f;


    
    // Start is called before the first frame update
    private int PlanesTouched = 0;

    private GameController heroGameController = null;
    private CooldownBar cooldownBar;
    void Start()
    {
        heroGameController = FindFirstObjectByType<GameController>();
        cooldownBar = FindFirstObjectByType<CooldownBar>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //toggles mouse/keyboard control with M. Starts as mouse control
        if (Input.GetKeyDown(KeyCode.M))
        {
            mouseController = !mouseController;
            heroGameController.SwitchHeroMode();
        }
        Vector3 pos = transform.position;
        
        if(mouseController)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
        }
        else
        {
            if(Input.GetKey(KeyCode.W))
            {
                pos += ((speed * Time.smoothDeltaTime) * transform.up);
            }
            if(Input.GetKey(KeyCode.S))
            {
                pos -= ((speed * Time.smoothDeltaTime) * transform.up);
            }
            if(Input.GetKey(KeyCode.D))
            {
                transform.Rotate(transform.forward, -playerRotateSpeed * Time.smoothDeltaTime);
            }
            if(Input.GetKey(KeyCode.A))
            {
                transform.Rotate(transform.forward, playerRotateSpeed * Time.smoothDeltaTime);
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (time > cooldown)
            {
                cooldownBar.startCooldown();
                GameObject r = Instantiate(Resources.Load("Prefabs/Rocket") as GameObject);
                heroGameController.RocketCreated();
                r.transform.localPosition = transform.localPosition;
                r.transform.rotation = transform.rotation;
                Debug.Log("Launch Rockets: " + r.transform.localPosition);
                time = 0f;
            }
        }
        transform.position = pos; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy"){
            //Debug.Log("Here x Plane: OnTriggerEnter2D");
            PlanesTouched = PlanesTouched + 1;
            Touched.text = "Touched(" + PlanesTouched + ")";
            Destroy(collision.gameObject);
            heroGameController.EnemyDestroyed();
        }
    }
}
