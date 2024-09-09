using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FaF_Image : FaceAFace
{
    [Header("---------- Face à Face - Image ---------")]
    [SerializeField] private InputActionReference nextAction;

    [Header("Image to Guess")]
    [SerializeField] private TextAsset imagesInformationJSon;
    [SerializeField] private TextMeshProUGUI description;

    [Header("Image Capture")]
    [SerializeField] protected SpriteRenderer imageToCapture;
    [SerializeField] protected Camera captureCamera;
    protected Vector3 viewPoint;

    [Header("Other")]
    [SerializeField] private Sprite finalImage;
    [SerializeField] protected Button showAnswerButton;
    [SerializeField] protected Button returnMenuButton;
    [SerializeField] protected float cameraFOV = 64f;
    [SerializeField] private string file;

    // List of images informations
    public struct ImageInfo
    {
        public String name;
        public String description;
        public Sprite sprite;
        public Vector2 startPoint;
        public int pixelSize;

        public String toString() { return "name:" + name + " start: " + startPoint + " pixelSize: " + pixelSize; }
    }
    private List<ImageInfo> images;
    protected ImageInfo currentImage;

    // Time gestion
    protected float globalTime;

    // For opening the json
    [Serializable]  public class ImageInformation { 
        public string imageName; 
        public string description; 
        public float start_x;
        public float start_y;
        public int pixelSize;
    }
    [Serializable] public class ImagesInformation { 
        public ImageInformation[] imagesInformation; 
    }


#region Enable / Disable
    public override void OnEnable()
    {
        base.OnEnable();
        nextAction.action.started += next;
        enableActions();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        nextAction.action.started -= next;
        disableActions();
    }

    public override void enableActions()
    {
        base.enableActions();
        nextAction.action.Enable();
    }
    public override void disableActions()
    {
        base.disableActions();
        nextAction.action.Disable();
    }
    #endregion Enable / Disable

#region Initialize
    public override void Start()
    {
        returnMenuButton.gameObject.SetActive(false);

        // Camera FOV
        captureCamera.fieldOfView = cameraFOV;

        // Lecture des images à ajouter
        ImagesInformation imagesAndDescription = JsonUtility.FromJson<ImagesInformation>(imagesInformationJSon.text);

        // Initialisation des listes
        images = new List<ImageInfo>();

        // Disable button
        showAnswerButton.gameObject.SetActive(false);

        foreach (ImageInformation imageInformation in imagesAndDescription.imagesInformation)
        {
            ImageInfo image = initializeImageInfo(imageInformation);

            images.Add(image);
        }

        base.Start();
    }
    public virtual ImageInfo initializeImageInfo(ImageInformation imageInformation)
    {
        ImageInfo image = new ImageInfo();
        image.name = imageInformation.imageName;
        image.description = imageInformation.description;
        image.sprite = Resources.Load<Sprite>(file + "/" + image.name);

        return image;
    }

    public override void resetGame()
    {
        // Reset image Sprite
        imageToCapture.sprite = null;

        currentImage = images[UnityEngine.Random.Range(0, images.Count)];

        // Set Description
        description.gameObject.SetActive(true);
        description.text = currentImage.description;

        // Time
        globalTime = 0f;

        float verticalFOV = cameraFOV * Mathf.Deg2Rad;
        float horizontalFOV = Camera.VerticalToHorizontalFieldOfView(cameraFOV, 16 / (float)9) * Mathf.Deg2Rad;

        // Place Camera
        
        initializeTravelPoints(currentImage.sprite, verticalFOV, horizontalFOV);
        showFirstImage();
        placeCamera();
        base.resetGame();
    }

    public virtual void initializeTravelPoints(Sprite sprite,  float verticalFOV, float horizontalFOV)
    {
        findViewPoint(sprite, verticalFOV, horizontalFOV);
    }
    public void findViewPoint(Sprite sprite, float verticalFOV, float horizontalFOV)
    {
        // Find the view point of camera
        float end_xz = (sprite.bounds.size.x / 2) / (float)Math.Tan(horizontalFOV / 2);
        float end_yz = (sprite.bounds.size.y / 2) / (float)Math.Tan(verticalFOV / 2);

        viewPoint = new Vector3(0, 0, -Mathf.Max(end_xz, end_yz));
    }

    public virtual void placeCamera()
    {
        captureCamera.transform.localPosition = viewPoint;
    }

    public virtual void showFirstImage()
    {
        imageToCapture.sprite = currentImage.sprite;
    }
#endregion Initialize
    public override void startGame(InputAction.CallbackContext context)
    {
        if (pointToPlayer[currentPoint] == 0) { return; }

        base.startGame(context);
        StartCoroutine(Modification());
    }

#region Coroutines
    public virtual IEnumerator Modification() {
        while (globalTime < gameDuration)
        {
            globalTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion Coroutines

    #region Answer
    public void showAnswer()
    {
        nextButton.gameObject.SetActive(true);
        nextAction.action.started -= next;

        undoModifications();

        showAnswerButton.gameObject.SetActive(false);
    }

    public override void next()
    {
        images.Remove(currentImage);
        if (images.Count == 0) { showFinalImage(); }
        else { resetGame(); }
    }
    public void next(InputAction.CallbackContext context) { next(); }

    #endregion Answer

    public virtual void undoModifications() { }

    public virtual void showFinalImage()
    {
        imageToCapture.sprite = finalImage;

        float verticalFOV = cameraFOV * Mathf.Deg2Rad;
        float horizontalFOV = Camera.VerticalToHorizontalFieldOfView(cameraFOV, 16 / (float)9) * Mathf.Deg2Rad;
        findViewPoint(finalImage, verticalFOV, horizontalFOV);
        captureCamera.transform.localPosition = viewPoint;

        returnMenuButton.gameObject.SetActive(true);
    }

    public override void answer(InputAction.CallbackContext context)
    {
        base.answer(context);
        if (!answerState)
        {
            showAnswerButton.gameObject.SetActive(false);
            StartCoroutine(Modification());
        }
        else
        {
            showAnswerButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
    }

    public void returnToMenu() { SceneManager.LoadScene("Title Screen"); }
}
