using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuatreALaSuite : MonoBehaviour
{
    [SerializeField] GameObject[] currentPoints = new GameObject[5];
    [SerializeField] GameObject[] maxPoints = new GameObject[5];
    [SerializeField] Timer timer;

    private int currentPoint;
    private int maxPoint;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        currentPoint = 0;
        maxPoint = 0;

        for (int i = 1; i < currentPoints.Length; i++)
        {
            currentPoints[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.getStartTimer())
        {
            if (Input.GetKeyDown(KeyCode.T) || Input.GetMouseButtonDown(0))
            {
                addPoint();
            }
            if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1))
            {
                returnTo0();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                removePoint();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            resetGame();
        }
    }

    // Augment de 1 la série en cours du joueur
    public void addPoint()
    {
        currentPoint++;
        currentPoints[currentPoint].SetActive(true);
        
        if (currentPoint > maxPoint)
        {
            maxPoint = currentPoint;
            maxPoints[maxPoint].GetComponent<Image>().color = new Color(1, 0.5033457f, 0, 1);
        }
    }

    // En cas d'erreur du juge, permet de retirer un point donné
    public void removePoint()
    {
        currentPoints[currentPoint].SetActive(false);
        
        if (currentPoint == maxPoint)
        {
            maxPoints[maxPoint].GetComponent<Image>().color = new Color(0, 0.03045726f, 1, 1);
            maxPoint--;
        }
        currentPoint--;
    }

    // En cas d'erreur du joueur, sa série repart à 0
    public void returnTo0()
    {
        for (int i = 1; i <= currentPoint; i++)
        {
            currentPoints[i].SetActive(false);
        }
        currentPoint = 0;
    }

    // Reset pour le joueur suivant
    public void resetGame()
    {
        SceneManager.LoadScene("QuatreALaSuite");
    }
}
