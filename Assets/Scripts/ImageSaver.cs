using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
    
public class ImageSaver : MonoBehaviour
{
    public int FileCounter = 0;

    void CamCapture()
    {
        Camera Cam = GetComponent<Camera>();
    
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;
    
        Cam.Render();
    
        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;
    
        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
    
        File.WriteAllBytes(Application.dataPath + "/images/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }
}
