using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions; 


public class RocketBehavior : MonoBehaviour
{
    private GameController heroGameController = null;
    public const float rocketSpeed = 40f;

    // Start is called before the first frame update
    void Start()
    {
        heroGameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * (rocketSpeed * Time.smoothDeltaTime);
        CameraSupport s = Camera.main.GetComponent<CameraSupport>();
        Assert.IsTrue(s != null);
        Bounds worldBorder = s.GetWorldBound();
        if(!worldBorder.Contains(transform.position)){
            heroGameController.RocketDestroyed();
            Destroy(transform.gameObject);  // kills self
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Enemy")
        {
            Debug.Log("Here x RocketBehavior: OnTriggerEnter2D");
            heroGameController.RocketDestroyed();
            Destroy(transform.gameObject);  // kills self
        }
    }
}
