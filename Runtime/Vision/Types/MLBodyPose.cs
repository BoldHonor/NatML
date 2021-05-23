/*
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using UnityEngine;

    /// <summary>
    /// Detected body pose.
    /// The xy coordinates are the normalized position of the keypoint, in range [0, 1].
    /// The z coordinate is the confidence score of the keypoint, in range [0, 1].
    /// </summary>
    public sealed class MLBodyPose { // Once we have C# 9, we'll use `init` properties
        public Vector3 nose;
        public Vector3 leftEye;
        public Vector3 rightEye;
        public Vector3 leftEar;
        public Vector3 rightEar;
        public Vector3 leftShoulder;
        public Vector3 rightShoulder;
        public Vector3 leftElbow;
        public Vector3 rightElbow;
        public Vector3 leftWrist;
        public Vector3 rightWrist;
        public Vector3 leftHip;
        public Vector3 rightHip;
        public Vector3 leftKnee;
        public Vector3 rightKnee;
        public Vector3 leftAnkle;
        public Vector3 rightAnkle;
    }
}