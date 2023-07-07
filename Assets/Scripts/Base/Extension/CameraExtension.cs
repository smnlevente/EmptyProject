using UnityEngine;

public static class CameraExtension
{
    public static float PixelRatio(this Camera camera)
    {
        return (camera.orthographicSize * 2.0f) / (float)camera.pixelHeight;
    }
}
