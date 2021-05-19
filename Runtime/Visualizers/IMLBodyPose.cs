/*
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Visualizers {

    using UnityEngine;

    /// <summary>
    /// </summary>
    public interface IMLBodyPose {
        Vector3 nose { get; }
        Vector3 leftEye { get; }
        Vector3 rightEye { get; }
        Vector3 leftEar { get; }
        Vector3 rightEar { get; }
        Vector3 leftShoulder { get; }
        Vector3 rightShoulder { get; }
        Vector3 leftElbow { get; }
        Vector3 rightElbow { get; }
        Vector3 leftWrist { get; }
        Vector3 rightWrist { get; }
        Vector3 leftHip { get; }
        Vector3 rightHip { get; }
        Vector3 leftKnee { get; }
        Vector3 rightKnee { get; }
        Vector3 leftAnkle { get; }
        Vector3 rightAnkle { get; }
    }
}