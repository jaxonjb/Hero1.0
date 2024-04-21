using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int maxPlanes = 1;
    private int numberOfPlanes = 0;
    private int enemiesKilled = 0;
    private int numberOfRockets = 0;
    public Text HeroMode = null;
    public Text RocketCountText = null;
    public Text EnemyCount = null;
    public Text EnemyDestroyedCount = null;
    public bool isMouse = true;
    private Vector3[] flagPositions = new Vector3[6];
    private Vector3[] flagOrigins = new Vector3[6];

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            pause();
        }
        if (Input.GetKey(KeyCode.Escape))
        {
#if UNITY_EDITOR
         // Application.Quit() does not work in the editor so
         // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
         UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        if (numberOfPlanes < maxPlanes)
        {
            CameraSupport s = Camera.main.GetComponent<CameraSupport>();
            Assert.IsTrue(s != null);

            GameObject e = Instantiate(Resources.Load("Prefabs/Enemy") as GameObject); // Prefab MUST BE locaed in Resources/Prefab folder!
            Vector3 pos;
            pos.x = (s.GetWorldBound().min.x + Random.value * s.GetWorldBound().size.x) *0.9f;
            pos.y = (s.GetWorldBound().min.y + Random.value * s.GetWorldBound().size.y) * 0.9f;
            pos.z = 0;
            e.transform.localPosition = pos;
            ++numberOfPlanes;
            EnemyCount.text = "On Screen(" + numberOfPlanes + ")";
        }
    }

    public void EnemyDestroyed() 
    {
        --numberOfPlanes;
        ++enemiesKilled;
        EnemyCount.text = "On Screen(" + numberOfPlanes + "(";
        EnemyDestroyedCount.text = "Destroyed(" + enemiesKilled + ")";
    } 
    public void RocketDestroyed()
    {
        --numberOfRockets;
        RocketCountText.text = "Rockets Count: " + numberOfRockets;
    }
    public void RocketCreated()
    {
        ++numberOfRockets;
        RocketCountText.text = "Rockets Count: " + numberOfRockets;
    }
    public void SwitchHeroMode()
    {
        if(isMouse == true)
        {
            HeroMode.text = "Hero Mode: Keyboard";
            isMouse = false;
        }
        else{
            HeroMode.text = "Hero Mode: Mouse";
            isMouse = true;
        }
    }
    private void pause(){
        if(Time.timeScale == 0){
            Time.timeScale = 1;
        }else{
            Time.timeScale = 0;
        }
    }
    public Vector3 RespawnCheckpoint(int flagNum){
        Vector3 newPosition = flagOrigins[flagNum];
        newPosition.x += Random.Range(-15,15);
        newPosition.y += Random.Range(-15,15);
        flagPositions[flagNum] = newPosition;
        return newPosition;
    }
    public void originalPosition(int flagNum, Vector3 flagPos)
    {
        flagOrigins[flagNum] = flagPos;
        flagPositions[flagNum] = flagPos;
    }
    public Vector3 GetFlagPos(int Checkpoint)
    {
        return flagPositions[Checkpoint];
    }
}
