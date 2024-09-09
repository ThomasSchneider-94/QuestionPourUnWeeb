using System;
using UnityEngine;
using System.Collections;

public class FaF_Zoom : FaF_Image
{
    // Camera travel
    private Vector3 cameraStartPoint;
    private Vector3 cameraMiddlePoint;
    private float firstSegmentTime;
    private float secondSegmentTime;

#region Initialize
    public override ImageInfo initializeImageInfo(ImageInformation imageInformation)
    {
        ImageInfo image = base.initializeImageInfo(imageInformation);

        image.startPoint = new Vector2(imageInformation.start_x, imageInformation.start_y );
        return image;
    }
    public override void resetGame()
    {
        base.resetGame();
        initializeTravelTimes();
    }
    public override void placeCamera()
    {
        captureCamera.transform.localPosition = cameraStartPoint;
    }
    public override void initializeTravelPoints(Sprite sprite, float verticalFOV, float horizontalFOV)
    {
        // Find the starting point of the camera
        float start_x = currentImage.startPoint.x * sprite.bounds.size.x / 2;
        float start_y = currentImage.startPoint.y * sprite.bounds.size.y / 2;
        float start_z = -captureCamera.nearClipPlane;

        cameraStartPoint = new Vector3(start_x, start_y, start_z);

        // Find middle point of camera (merci Thalès)
        float end_xz = (sprite.bounds.size.x / 2) / (float)Math.Tan(horizontalFOV / 2);
        float end_yz = (sprite.bounds.size.y / 2) / (float)Math.Tan(verticalFOV / 2);

        float middle_xz = (1 - Mathf.Abs(currentImage.startPoint.x)) * end_xz;
        float middle_yz = (1 - Mathf.Abs(currentImage.startPoint.y)) * end_yz;
        /* Explication de la formule
         * 
         * Thalès : middlePoint_z / endPoint_z = ((image.size()/2) - start_x) / (image.size()/2)
         *          middlePoint_z = endPoint_z * (image.size()/2)(1 - imagesStartPoint[currentImage]) / (image.size()/2)
         *          middlePoint_z = endPoint_z * (1 - imagesStartPoint[currentImage])
         */
        cameraMiddlePoint = new Vector3(start_x, start_y, -Mathf.Min(middle_xz, middle_yz));

        // Find the ending point
        base.initializeTravelPoints(sprite, verticalFOV, horizontalFOV);
    }
    public void initializeTravelTimes()
    {
        // Calculate time for travel each portion
        float firstDistance = Vector3.Distance(cameraStartPoint, cameraMiddlePoint);
        float secondDistance = Vector3.Distance(cameraMiddlePoint, viewPoint);

        firstSegmentTime = gameDuration * (firstDistance / (firstDistance + secondDistance));
        secondSegmentTime = gameDuration * (secondDistance / (firstDistance + secondDistance));
    }
    #endregion Initialize

#region Coroutines    
    public override IEnumerator Modification()
    {
        // First place, dezoom from startPoint to middlePoint
        while (globalTime < firstSegmentTime)
        {
            globalTime += Time.deltaTime;
            captureCamera.transform.localPosition = Vector3.Lerp(cameraStartPoint, cameraMiddlePoint, globalTime / firstSegmentTime);
            yield return null;
        }

        // Then, dezoom from middlePoint to endPoint
        while (globalTime - firstSegmentTime < +secondSegmentTime)
        {
            globalTime += Time.deltaTime;
            captureCamera.transform.localPosition = Vector3.Lerp(cameraMiddlePoint, viewPoint, (globalTime - firstSegmentTime) / secondSegmentTime);
            yield return null;
        }
    }

    public override void undoModifications() 
    { 
        captureCamera.transform.localPosition = viewPoint;
    }
#endregion Coroutines
}
