/* 
*   NatML Extensions
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class MLDetectionVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="image">Image which detections are made on.</param>
        /// <param name="detections">Detections to render.</param>
        public void Render (Texture image, params (Rect rect, string label)[] detections) {
            // Delete current
            foreach (var rect in currentRects)
                GameObject.Destroy(rect.gameObject);
            currentRects.Clear();
            // Display image
            var rawImage = GetComponent<RawImage>();
            var aspectFitter = GetComponent<AspectRatioFitter>();
            rawImage.texture = image;
            aspectFitter.aspectRatio = (float)image.width / image.height;
            // Render rects
            var imageRect = new Rect(0, 0, image.width, image.height);
            foreach (var detection in detections) {
                var rect = Instantiate(detectionRect, transform);
                rect.gameObject.SetActive(true);
                var normalizedPosition = Rect.PointToNormalized(imageRect, detection.rect.position);
                var normalizedSize = Rect.PointToNormalized(imageRect, detection.rect.size);
                var normalizedRect = new Rect(normalizedPosition, normalizedSize);
                rect.Render(rawImage, normalizedRect, detection.label);
                currentRects.Add(rect);
            }
        }
        #endregion


        #region --Operations--
        [SerializeField] MLDetectionRect detectionRect;
        List<MLDetectionRect> currentRects = new List<MLDetectionRect>();
        #endregion
    }
}