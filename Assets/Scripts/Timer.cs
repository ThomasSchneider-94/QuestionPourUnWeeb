using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeRemaining = 40;
    [SerializeField] TMPro.TextMeshProUGUI tmpro;

    private bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        tmpro.text = Mathf.FloorToInt(timeRemaining % 60).ToString();
        startTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            startTimer = true;
        }


        if (startTimer)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                tmpro.text = Mathf.FloorToInt(timeRemaining % 60).ToString();
            }
        }
    }

    public bool getStartTimer()
    {
        return startTimer;
    }



}
