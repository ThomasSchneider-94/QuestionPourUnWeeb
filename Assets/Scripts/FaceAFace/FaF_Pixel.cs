using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class FaF_Pixel : FaF_Image
{
    private Material pixelizedMaterial;

    #region Initialize

    public override void Start()
    {
        pixelizedMaterial = imageToCapture.material;
        base.Start();        
    }

    public override ImageInfo initializeImageInfo(ImageInformation imageInformation)
    {
        ImageInfo image = base.initializeImageInfo(imageInformation);
        image.pixelSize = imageInformation.pixelSize;
        return image;
    }

    public override void showFirstImage()
    {
        if (pixelizedMaterial == null ) { Debug.Log("Ici");return; }
        pixelizedMaterial.SetFloat("_PixelSize", currentImage.pixelSize);
        pixelizedMaterial.SetTexture("_MainTex", currentImage.sprite.texture);
        imageToCapture.sprite = currentImage.sprite;  
    }
    #endregion Initialize



    public override IEnumerator Modification()
    {
        while (globalTime < gameDuration)
        {
            globalTime += Time.deltaTime;
            pixelizedMaterial.SetFloat("_PixelSize", Mathf.Lerp(currentImage.pixelSize, 1, globalTime / gameDuration));
            yield return null;
        }
    }

    public override void undoModifications() 
    { 
        pixelizedMaterial.SetFloat("_PixelSize", 1);
    }
}
