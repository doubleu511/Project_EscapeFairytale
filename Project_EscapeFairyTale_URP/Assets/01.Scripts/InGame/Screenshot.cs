using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public static void TakeScreenshot_()
    {
        string path = Application.dataPath;
        path = path.Substring(0, path.LastIndexOf('/'));
        ScreenCapture.CaptureScreenshot(Path.Combine(path, "../saved.png"));
        Debug.Log(Path.Combine(path, "saved.png"));
        //instance.TakeScreenshot(width, height);
    }

    private static Sprite LoadSprite(string path, float pixelPerUnit = 100.0f)
    {
        Texture2D t = LoadTexture(path);
        if (t != null)
        {
            Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), pixelPerUnit);
            // 텍스쳐, 크기, 피봇, 픽셀퍼 유닛
            return s;
        }
        return null;
    }

    public static Texture2D LoadTexture(string path)
    {
        Texture2D t;
        byte[] FileData;

        if (File.Exists(path))
        {
            FileData = File.ReadAllBytes(path);
            //System.io에서 ByteArr로 파일정보를 읽어오고
            t = new Texture2D(2, 2);           // 새로운 비어있는 텍스쳐를 만들어준다. 
            if (t.LoadImage(FileData))           // 이미지 데이터를 로드하여 텍스쳐에 넣는다. 이때 크기는 이미지 크기에 맞춰 자동으로 들어간다.
                return t;                 // 데이터가 읽을 수 있었다면 해당 텍스쳐를 리턴 LoadImage는 성공시 true를 반환
        }
        return null;
    }
}