using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private GameManager gameManager;

    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown npgDropdown;
    [SerializeField] private Button npgModifyButton;

    [SerializeField] private TMP_Dropdown qalsDropdown;
    [SerializeField] private Button qalsModifyButton;

    [SerializeField] private TMP_Dropdown fafDropdown;
    [SerializeField] private Button fafModifyButton;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI confirmationText;

    void Start()
    {
        gameManager = GameManager.Instance;
        confirmationPanel.SetActive(false);

        // Setup the dropdown to the good values
        GameManager.NeufPointsGagnants neufPointsGagnants = gameManager.get_NeufPointsGagnants();
        npgDropdown.SetValueWithoutNotify((int)neufPointsGagnants);
        if (!gameManager.isModifiable(neufPointsGagnants)) { npgModifyButton.interactable = false; }

        GameManager.QuatreALaSuite quatreALaSuite = gameManager.get_QuatreALaSuite();
        qalsDropdown.SetValueWithoutNotify((int)quatreALaSuite);
        if (!gameManager.isModifiable(quatreALaSuite)) { qalsModifyButton.interactable = false; }

        GameManager.FaceAFace faceAFace = gameManager.get_FaceAFace();
        fafDropdown.SetValueWithoutNotify((int)faceAFace);
        if (!gameManager.isModifiable(faceAFace)) { fafModifyButton.interactable = false; }
    }


    #region Setter
    public void set_NpG()
    {
        gameManager.setGame((GameManager.NeufPointsGagnants)npgDropdown.value);
        if (gameManager.isModifiable((GameManager.NeufPointsGagnants)npgDropdown.value)) { npgModifyButton.interactable = true; }
        else { npgModifyButton.interactable = false; }
    }
    public void set_QalS() 
    {
        gameManager.setGame((GameManager.QuatreALaSuite)qalsDropdown.value);
        if (gameManager.isModifiable((GameManager.QuatreALaSuite)qalsDropdown.value)) { qalsModifyButton.interactable = true; }
        else { qalsModifyButton.interactable = false; }
    }
    public void set_FaF() 
    { 
        gameManager.setGame((GameManager.FaceAFace)fafDropdown.value); 
        if (gameManager.isModifiable((GameManager.FaceAFace)fafDropdown.value)) { fafModifyButton.interactable = true; }
        else { fafModifyButton.interactable = false; }
    }
    #endregion

    #region Validation
    public void askForValidation() 
    {
        confirmationPanel.SetActive(true);

        string confirmText = "Game configuration :\n";
        if (gameManager.get_NeufPointsGagnants() != GameManager.NeufPointsGagnants.None) { confirmText += "           - 9 points gagnants : " + gameManager.get_NeufPointsGagnants() + ";\n"; }
        if (gameManager.get_QuatreALaSuite() != GameManager.QuatreALaSuite.None) { confirmText += "           - 4 à la suite : " + gameManager.get_QuatreALaSuite() + ";\n"; }
        if (gameManager.get_FaceAFace() != GameManager.FaceAFace.None) { confirmText += "           - Face à face : " + gameManager.get_FaceAFace() + ";\n"; }

        confirmationText.text = confirmText;
    }
    public void returnToChoice() { confirmationPanel.SetActive(false); }
    public void startGame() { gameManager.nextGame(); }
    #endregion

    #region Change images
    public void changeFirstGameImages() { }
    public void changeSecondGameImages() { }
    public void changeThirdGameImages() { }
    #endregion
}
