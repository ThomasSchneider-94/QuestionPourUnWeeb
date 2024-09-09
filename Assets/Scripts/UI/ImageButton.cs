using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    [SerializeField] private Image image;

    public void setImage(Sprite sprite) { image.sprite = sprite; }
}
