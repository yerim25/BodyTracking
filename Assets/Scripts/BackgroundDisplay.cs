using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.Sensor;

public class BackgroundDisplay : MonoBehaviour
{
    public RawImage rawImage;
    private Device kinect;
    private Texture2D kinectTexture;

    public void SetKinectDevice(Device device)
    {
        kinect = device;
        kinectTexture = new Texture2D(1920, 1080, TextureFormat.BGRA32, false);
        rawImage.texture = kinectTexture;
    }

    void Update()
    {
        if (kinect != null)
        {
            using (Capture capture = kinect.GetCapture())
            {
                UpdateTextureWithColorImage(capture);
            }
        }
    }

    void UpdateTextureWithColorImage(Capture capture)
    {
        using (Microsoft.Azure.Kinect.Sensor.Image colorImage = capture.Color)
        {
            var colorImageData = colorImage.Memory.Span;
            var colorData = colorImageData.ToArray();

            int width = colorImage.WidthPixels;
            int height = colorImage.HeightPixels;
            int stride = width * 4;

            for (int y = 0; y < height / 2; y++)
            {
                int topIndex = y * stride;
                int bottomIndex = (height - 1 - y) * stride;

                for (int x = 0; x < stride; x++)
                {
                    byte temp = colorData[topIndex + x];
                    colorData[topIndex + x] = colorData[bottomIndex + x];
                    colorData[bottomIndex + x] = temp;
                }
            }

            kinectTexture.LoadRawTextureData(colorData);
            kinectTexture.Apply();
        }
    }
}