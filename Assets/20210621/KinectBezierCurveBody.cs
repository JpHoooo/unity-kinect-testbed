using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;


// FIXME: Head的平滑还没处理好
public class KinectBezierCurveBody : MonoBehaviour
{
    private KinectManager kinectManager;
    [SerializeField] private HumanData[] HumanDatas;
    [SerializeField] private Transform LineRendererGroup;
    [SerializeField] private GameObject lrPrefab;
    private List<BodyLineRenderData> bodyLineRenderDatas = new List<BodyLineRenderData>();

    private List<HeadLineRenderData> headLineRenderDatas = new List<HeadLineRenderData>();
    [Range(1, 20), SerializeField] private float smoothspeed = 10;
    [Range(5, 20), SerializeField] private float scale = 10;

    [SerializeField] private int circlePositionCount = 360;
    [SerializeField] private float radius = 0.1f;
    Quaternion quaternion;


    private void Start()
    {
        kinectManager = KinectManager.Instance;
        EstablishHumanLineRenderer();
    }

    private void Update()
    {
        if (kinectManager.IsInitialized() && HumanDatas.Length > 0)
        {
            DrawHead();
            DrawBody();
        }

    }

    private void DrawHead()
    {
        for (int i = 0; i < headLineRenderDatas.Count; i++)
        {
            HeadLineRenderData headLineRenderData = headLineRenderDatas[i];
            LineRenderer lineRenderer = headLineRenderData.lineRenderer;
            KinectInterop.JointType jointType = headLineRenderData.jointType;
            Vector3 position = headLineRenderData.position;
            ulong userId = kinectManager.GetUserIdByIndex(headLineRenderData.index);

            if (kinectManager.IsUserTracked(userId))
            {
                lineRenderer.gameObject.SetActive(true);
                Vector3 circleCenter = kinectManager.GetJointKinectPosition(userId, jointType, true);

                float angle = 360f / (circlePositionCount - 1);
                lineRenderer.positionCount = circlePositionCount;

                for (int j = 0; j < circlePositionCount; j++)
                {
                    if (j != 0)
                    {
                        quaternion = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z + angle);
                    }

                    Vector3 p = scale * (circleCenter + quaternion * Vector3.down * radius);

                    lineRenderer.SetPosition(j, p);
                }
            }
            else
            {
                lineRenderer.gameObject.SetActive(false);
            }
        }
    }

    private void DrawBody()
    {
        for (int i = 0; i < bodyLineRenderDatas.Count; i++)
        {
            //TODO: Get bodyLineRenderDatas info
            BodyLineRenderData bodyLineRenderData = bodyLineRenderDatas[i];

            LineRenderer lineRenderer = bodyLineRenderData.lineRenderer;
            KinectInterop.JointType[] jointTypes = bodyLineRenderData.jointTypes;
            Vector3[] positions = bodyLineRenderData.positions;
            ulong userId = kinectManager.GetUserIdByIndex(bodyLineRenderData.index);
            if (kinectManager.IsUserTracked(userId))
            {
                lineRenderer.gameObject.SetActive(true);
                //TODO: Use 'kinectManager.cs' to get vec3[]
                for (int j = 0; j < positions.Length; j++)
                    positions[j] = Vector3.Lerp(positions[j], kinectManager.GetJointKinectPosition(userId, jointTypes[j], true) * scale, Time.deltaTime * smoothspeed);

                //TODO: Set LineRenderer
                List<Vector3> poslist = DrawBezierCurve.BezierCurveWithUnlimitPoints(positions, 200);
                lineRenderer.positionCount = poslist.Count;
                lineRenderer.SetPositions(poslist.ToArray());
            }
            else
            {
                lineRenderer.gameObject.SetActive(false);
            }
        }
    }


    private void EstablishHumanLineRenderer()
    {
        for (int i = 0; i < HumanDatas.Length; i++)
        {
            // TODO: Human
            GameObject parent = new GameObject("index:" + HumanDatas[i].index);
            parent.transform.SetParent(LineRendererGroup);

            // Head
            GameObject head = Instantiate(lrPrefab, Vector3.zero, Quaternion.identity, parent.transform);
            head.name = "head";
            HeadLineRenderData headLineRenderData = new HeadLineRenderData
            {
                index = HumanDatas[i].index,
                lineRenderer = head.GetComponent<LineRenderer>(),
                jointType = KinectInterop.JointType.Head
            };
            headLineRenderData.position = new Vector3();
            headLineRenderDatas.Add(headLineRenderData);

            // Body
            for (int j = 0; j < HumanDatas[i].bodyParts.Length; j++)
            {
                GameObject body = Instantiate(lrPrefab, Vector3.zero, Quaternion.identity, parent.transform);
                BodyLineRenderData bodyLineRenderData = new BodyLineRenderData
                {
                    index = HumanDatas[i].index,
                    lineRenderer = body.GetComponent<LineRenderer>(),
                    jointTypes = HumanDatas[i].bodyParts[j],
                };
                bodyLineRenderData.positions = new Vector3[bodyLineRenderData.jointTypes.Length];
                bodyLineRenderDatas.Add(bodyLineRenderData);
            }
        }
    }





    [System.Serializable]
    internal class HumanData
    {
        [SerializeField] internal int index;

        internal KinectInterop.JointType head = KinectInterop.JointType.Head;

        internal KinectInterop.JointType[][] bodyParts = new KinectInterop.JointType[][] {
            armLeftJointsList,
            armRightJointsList,
            legLeftJointsList,
            legRightJointsList,
            spineJointsList
        };
    }

    internal class HeadLineRenderData
    {
        internal int index;
        internal LineRenderer lineRenderer;
        internal KinectInterop.JointType jointType;
        internal Vector3 position;
    }

    internal class BodyLineRenderData
    {
        internal int index;
        internal LineRenderer lineRenderer;
        internal KinectInterop.JointType[] jointTypes;
        internal Vector3[] positions;
    }

    [SerializeField]
    private static KinectInterop.JointType[] armLeftJointsList = new KinectInterop.JointType[]
     {
            KinectInterop.JointType.HandLeft,
            KinectInterop.JointType.WristLeft,
            KinectInterop.JointType.ElbowLeft,
            KinectInterop.JointType.ShoulderLeft
     };
    [SerializeField]
    private static KinectInterop.JointType[] armRightJointsList = new KinectInterop.JointType[]{
            KinectInterop.JointType.HandRight,
            KinectInterop.JointType.WristRight,
            KinectInterop.JointType.ElbowRight,
            KinectInterop.JointType.ShoulderRight
        };
    [SerializeField]
    private static KinectInterop.JointType[] legLeftJointsList = new KinectInterop.JointType[]{
            KinectInterop.JointType.FootLeft,
            KinectInterop.JointType.AnkleLeft,
            KinectInterop.JointType.KneeLeft,
            KinectInterop.JointType.HipLeft
        };
    [SerializeField]
    private static KinectInterop.JointType[] legRightJointsList = new KinectInterop.JointType[]{
            KinectInterop.JointType.FootRight,
            KinectInterop.JointType.AnkleRight,
            KinectInterop.JointType.KneeRight,
            KinectInterop.JointType.HipRight
        };
    [SerializeField]
    private static KinectInterop.JointType[] spineJointsList = new KinectInterop.JointType[]{
            KinectInterop.JointType.Pelvis,
            KinectInterop.JointType.SpineNaval,
            KinectInterop.JointType.SpineChest,
            KinectInterop.JointType.Neck
        };
}
