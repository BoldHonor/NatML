/*
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Vision;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class MLBodyPoseVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// Render a body pose.
        /// </summary>
        /// <param name="image">Image which body pose is generated from.</param>
        /// <param name="pose">Body pose to render.</param>
        /// <param name="confidenceThreshold">Keypoints with confidence lower than this value are not rendered.</param>
        public void Render (Texture image, MLBodyPose pose, float confidenceThreshold = 0f) {
            // Delete current
            foreach (var point in currentPoints)
                GameObject.Destroy(point.gameObject);
            currentPoints.Clear();
            // Display image
            var imageTransform = transform as RectTransform;
            var rawImage = GetComponent<RawImage>();
            var aspectFitter = GetComponent<AspectRatioFitter>();
            rawImage.texture = image;
            aspectFitter.aspectRatio = (float)image.width / image.height;
            // Check
            if (pose == null)
                return;
            // Render keypoints
            foreach (var point in GetKeypoints(pose)) {
                // Check confidence
                if (point.z < confidenceThreshold)
                    continue;
                // Instantiate
                var anchor = Instantiate(keypoint, transform);
                anchor.gameObject.SetActive(true);
                // Position
                anchor.anchorMin = 0.5f * Vector2.one;
                anchor.anchorMax = 0.5f * Vector2.one;
                anchor.pivot = 0.5f * Vector2.one;
                anchor.anchoredPosition = Rect.NormalizedToPoint(imageTransform.rect, point);
                // Add
                currentPoints.Add(anchor);
            }
        }

        /// <summary>
        /// Get the keypoints in a body pose.
        /// </summary>
        /// <param name="pose">Body pose to extract keypoints from.</param>
        /// <returns>Enumrable of body pose keypoints.</returns>
        public static IEnumerable<Vector3> GetKeypoints (MLBodyPose pose) {
            yield return pose.nose;
            yield return pose.leftEye;
            yield return pose.rightEye;
            yield return pose.leftEar;
            yield return pose.rightEar;
            yield return pose.leftShoulder;
            yield return pose.rightShoulder;
            yield return pose.leftElbow;
            yield return pose.rightElbow;
            yield return pose.leftWrist;
            yield return pose.rightWrist;
            yield return pose.leftHip;
            yield return pose.rightHip;
            yield return pose.leftKnee;
            yield return pose.rightKnee;
            yield return pose.leftAnkle;
            yield return pose.rightAnkle;
        }
        #endregion


        #region --Operations--
        [SerializeField] RectTransform keypoint;
        List<RectTransform> currentPoints = new List<RectTransform>();
        #endregion
    }
}