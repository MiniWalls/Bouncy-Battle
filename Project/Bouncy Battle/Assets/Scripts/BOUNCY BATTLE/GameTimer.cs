using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public bool Timer = false;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        Timer = true;
        time = 240;
    }

    public void StopTimer()
    {
        Timer = false;
        //Times up
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);

    }

    // Update is called once per frame
    void Update()
    {
        if(Timer)
        {
            time -= Time.deltaTime;
            if(time <= 0f)
            {
                StopTimer();
            }
        }
    }
}
