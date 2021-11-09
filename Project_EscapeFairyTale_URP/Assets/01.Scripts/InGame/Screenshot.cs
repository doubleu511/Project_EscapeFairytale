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
            // �ؽ���, ũ��, �Ǻ�, �ȼ��� ����
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
            //System.io���� ByteArr�� ���������� �о����
            t = new Texture2D(2, 2);           // ���ο� ����ִ� �ؽ��ĸ� ������ش�. 
            if (t.LoadImage(FileData))           // �̹��� �����͸� �ε��Ͽ� �ؽ��Ŀ� �ִ´�. �̶� ũ��� �̹��� ũ�⿡ ���� �ڵ����� ����.
                return t;                 // �����Ͱ� ���� �� �־��ٸ� �ش� �ؽ��ĸ� ���� LoadImage�� ������ true�� ��ȯ
        }
        return null;
    }
}