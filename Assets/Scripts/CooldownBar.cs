using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBar : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        if(transform.localScale.x > 0){
            transform.localScale = new Vector3(transform.localScale.x - 125f, 1);
        }
    }
    public void startCooldown()
    {
        transform.localScale = new Vector3(1250f, 1);
    }
}
