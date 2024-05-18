using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;

public class main : MonoBehaviour
{
    public GameObject m_tracker;
    private SkeletalTrackingProvider m_skeletalTrackingProvider;
    public BackgroundData m_lastFrameData = new BackgroundData();
    private Device kinect;
    public BackgroundDisplay backgroundDisplay;

    void Start()
    {
        const int TRACKER_ID = 0;
        kinect = Device.Open(TRACKER_ID);
        kinect.StartCameras(new DeviceConfiguration
        {
            ColorFormat = ImageFormat.ColorBGRA32,
            ColorResolution = ColorResolution.R1080p,
            DepthMode = DepthMode.WFOV_Unbinned,
            SynchronizedImagesOnly = true,
            CameraFPS = FPS.FPS15
        });

        m_skeletalTrackingProvider = new SkeletalTrackingProvider(TRACKER_ID, kinect);
        //GetComponent<BackgroundDisplay>().SetKinectDevice(kinect);
        backgroundDisplay.SetKinectDevice(kinect);
    }

    void Update()
    {
        if (m_skeletalTrackingProvider.IsRunning)
        {
            if (m_skeletalTrackingProvider.GetCurrentFrameData(ref m_lastFrameData))
            {
                if (m_lastFrameData.NumOfBodies != 0)
                {
                    m_tracker.GetComponent<TrackerHandler>().updateTracker(m_lastFrameData);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (m_skeletalTrackingProvider != null)
        {
            m_skeletalTrackingProvider.Dispose();
        }

        if (kinect != null)
        {
            kinect.Dispose();
        }
    }
}