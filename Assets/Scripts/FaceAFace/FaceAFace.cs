using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FaceAFace : MiniGame
{
    [Header("---------- Face à face ----------")]
    [SerializeField] private float pointOffset = 50;

    [Header("Actions")]
    [SerializeField] private InputActionReference chooseWhoStartAction;
    [SerializeField] private InputActionReference startGameAction;
    [SerializeField] private InputActionReference answerAction;

    [Header("Point Objects")]
    [SerializeField] private List<PointObject> points = new List<PointObject>();
    [SerializeField] private List<float> pointTimeFraction = new List<float>() { 0.375f, 0.25f, 0.25f, 0.125f };
    protected List<int> pointToPlayer; // -1 -> Left; 1 -> Right

    [Header("Next")]
    [SerializeField] protected Button nextButton;

    [Header("Indication Arrows")]
    [SerializeField] private GameObject indicationArrows;
    private List<Image> indications = new List<Image>();
    [SerializeField] private float arrowsIndicationTime = 1f;
    [SerializeField] private float minTransparency = 0.2f;
    [SerializeField] private float maxTransparency = 0.6f;

    // Bools
    private bool chooseState;
    protected bool answerState;    

    protected int currentPoint;
    private float pointTime;

#region Enable/Disable
    public override void OnEnable()
    {
        base.OnEnable();
        chooseWhoStartAction.action.started += chooseWhoStart;
        startGameAction.action.started += startGame;
        answerAction.action.started += answer;
    }
    public override void OnDisable()
    {
        base.OnDisable();

        chooseWhoStartAction.action.started -= chooseWhoStart;
        startGameAction.action.started -= startGame;
        answerAction.action.started -= answer;
        
        disableActions();
    }

    // Manage Actions
    public override void enableActions()
    {
        base.enableActions();
        if (chooseState)
        {
            chooseWhoStartAction.action.Enable();
            startGameAction.action.Enable();
        }
        else
        {
            answerAction.action.Enable();
        }
    }
    public override void disableActions()
    {
        base.disableActions();

        chooseWhoStartAction.action.Disable();
        startGameAction.action.Disable();
        answerAction.action.Disable();

    }
#endregion Enable/Disable

    // Prepare the variables for the game
    public virtual void Start()
    {
        checkErrors();

        Image[] tmp_indications = indicationArrows.GetComponentsInChildren<Image>();
        foreach (Image indication in tmp_indications) { indications.Add(indication); }

        pointToPlayer = new List<int>(points.Count);
        for (int i = 0; i < points.Count; i++) { pointToPlayer.Add(0); }

        resetGame();
    }

    public void checkErrors()
    {
        if (points.Count != pointTimeFraction.Count) { Debug.LogError("List points and pointTimeFraction are not of the same length"); }
        if (pointTimeFraction.Sum() != 1) { Debug.LogError("pointTimeFraction sum must be 1"); }
    }
    public override void resetGame()
    {
        StopAllCoroutines();

        // Reset states
        chooseState = true;
        answerState = false;

        // Reset actions
        disableActions();
        enableActions();

        // Reset points position
        foreach (PointObject point in points)
        {
            point.transform.localPosition = new Vector2(0f, point.transform.localPosition.y);
            point.setFillAmount(1f);
        }

        // Next Button
        nextButton.gameObject.SetActive(false);

        // Time
        pointTime = 0;

        // Choose who start
        indicationArrows.SetActive(true);
        currentPoint = points.Count - 1;
        pointToPlayer[currentPoint] = 0;
        StartCoroutine(IndicationArrowsBlink(-1));
    }

    // Indications
    public IEnumerator IndicationArrowsBlink(int modificator)
    {
        float time = 0f;
        float startValue;
        float endValue;
        if (modificator < 0) { startValue = maxTransparency; endValue = minTransparency; }
        else { startValue = minTransparency; endValue = maxTransparency; }

        while (time < arrowsIndicationTime && chooseState)
        {
            time += Time.deltaTime;            

            float a = Mathf.Lerp(startValue, endValue, time / arrowsIndicationTime);
            foreach (Image indication in indications) { Color tmp_Color = indication.color; tmp_Color.a = a; indication.color = tmp_Color; }
            yield return null;
        }

        if (chooseState) { StartCoroutine(IndicationArrowsBlink(-modificator)); }
    }

    // Choose who will start the game
    public void chooseWhoStart(InputAction.CallbackContext context)
    {
        int input = (int)context.ReadValue<Vector2>().x;

        if (pointToPlayer[currentPoint] != input)
        {
            changeAllPlayer(currentPoint, input);
            for (int i = 0; i < points.Count; i++) { points[i].transform.localPosition = new Vector2(pointToPlayer[i] * pointOffset, points[i].transform.localPosition.y); }
        }
    }

    public void changeAllPlayer(int point, int player)
    {
        pointToPlayer[point] = player;
        if (point <= 0) { return; }
        else { changeAllPlayer(point - 1, -player); }
    }

    public virtual void startGame(InputAction.CallbackContext context)
    {
        if (pointToPlayer[currentPoint]  == 0) { return; }

        chooseState = false;
        indicationArrows.SetActive(false);

        // Enable/Disable actions
        disableActions();
        enableActions();

        StartCoroutine(DecreasePointTimer());
    }

    public IEnumerator DecreasePointTimer()
    {
        float pointDuration = gameDuration * pointTimeFraction[currentPoint];

        while (pointTime < pointDuration)
        {
            pointTime += Time.deltaTime;
            points[currentPoint].setFillAmount(Mathf.Lerp(1f, 0f, pointTime / pointDuration));
            yield return null;
        }

        points[currentPoint].setFillAmount(0);
        if (currentPoint != 0)
        {
            pointTime = 0f;
            currentPoint--;
            StartCoroutine(DecreasePointTimer());
        }
    }

    public virtual void answer(InputAction.CallbackContext context)
    {
        answerState = !answerState;

        if (answerState)
        {
            StopAllCoroutines();
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            pointToPlayer[currentPoint] = -pointToPlayer[currentPoint];
            points[currentPoint].transform.localPosition = new Vector2(pointToPlayer[currentPoint] * pointOffset, points[currentPoint].transform.localPosition.y);
            int i = currentPoint - 1;

            while (i >= 0 && pointToPlayer[i] != pointToPlayer[currentPoint])
            {
                pointToPlayer[i] = pointToPlayer[currentPoint];
                points[i].transform.localPosition = new Vector2(pointToPlayer[currentPoint] * pointOffset, points[i].transform.localPosition.y);
                i--;
            }

            nextButton.gameObject.SetActive(false);
            answerState = false;
            StartCoroutine(DecreasePointTimer());
        }
    }
    public virtual void next() { resetGame(); }
}
