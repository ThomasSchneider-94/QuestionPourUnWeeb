using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instance statique du GameManager
    public static GameManager Instance { get; private set; }

#region Jeux
    public enum NeufPointsGagnants
    {
        None = 0,
    }
    public enum QuatreALaSuite
    {
        None = 0,
        Classic = 1,
    }    
    public enum FaceAFace
    {
        None = 0,
        Classic = 1,
        Zoom = 2,
        Pixel = 3,
    }
    #endregion Jeux
    [Header("Jeux")]
    [SerializeField] private NeufPointsGagnants neufPointsGagnants;
    [SerializeField] private QuatreALaSuite quatreALaSuite;
    [SerializeField] private FaceAFace faceAFace;

    [Header("Modifiable")]
    [SerializeField] private List<NeufPointsGagnants> neufPointsGagnantsModifiable;
    [SerializeField] private List<QuatreALaSuite> quatreALaSuitesModifiable;
    [SerializeField] private List<FaceAFace> faceAFacesModifiable;

    private int currentGame;

    private void Awake()
    {
        // Assurez-vous qu'il n'y a qu'une seule instance du GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Détruisez les doublons
        }
        DontDestroyOnLoad(gameObject);

        currentGame = 0;
    }

    // Passe à la partie suivante dans le jeu
    public void nextGame()
    {
        currentGame++;
        if (currentGame == 0)
        {
            switch (neufPointsGagnants)
            {
                case NeufPointsGagnants.None: currentGame++; break;
            }
        }
        if (currentGame == 1)
        {
            switch (quatreALaSuite)
            {
                case QuatreALaSuite.None: currentGame++; break;
                case QuatreALaSuite.Classic: SceneManager.LoadScene("QuatreALaSuite"); break;
            }
        }
        if (currentGame == 2)
        {
            switch (faceAFace)
            {
                case FaceAFace.None: currentGame++; break;
                case FaceAFace.Classic: SceneManager.LoadScene("FaceAFace"); break;
                case FaceAFace.Zoom: SceneManager.LoadScene("FaF - Zoom"); break;
                case FaceAFace.Pixel: SceneManager.LoadScene("FaF - Pixel"); break;
            }
        }
        if (currentGame == 4) { SceneManager.LoadScene("EndPanel"); }
    }


    #region Set / Get

    // Set the different games
    public void setGame(NeufPointsGagnants game) { neufPointsGagnants = game; }
    public void setGame(QuatreALaSuite game) { quatreALaSuite = game; }
    public void setGame(FaceAFace game) { faceAFace = game; }

    // Get the different games
    public NeufPointsGagnants get_NeufPointsGagnants() { return neufPointsGagnants; }
    public QuatreALaSuite get_QuatreALaSuite() { return quatreALaSuite; }
    public FaceAFace get_FaceAFace() { return faceAFace; }

    // To know if the game has modifiable data
    public bool isModifiable(NeufPointsGagnants jeu) { return neufPointsGagnantsModifiable.Contains(jeu); }
    public bool isModifiable(QuatreALaSuite jeu) { return quatreALaSuitesModifiable.Contains(jeu); }
    public bool isModifiable(FaceAFace jeu) { return faceAFacesModifiable.Contains(jeu); }

    #endregion Set / Get

}
