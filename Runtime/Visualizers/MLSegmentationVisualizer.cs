/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class MLSegmentationVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="map">Segmentation map.</param>
        public void Render (Texture2D map) {
            // Display image
            var rawImage = GetComponent<RawImage>();
            var aspectFitter = GetComponent<AspectRatioFitter>();
            rawImage.material = material;
            rawImage.texture = map;
            aspectFitter.aspectRatio = (float)map.width / map.height;
        }
        #endregion


        #region --Operations--

        private Material material;

        void OnEnable () => material = new Material(Shader.Find("Hidden/NatML/MLSegmentationVisualizer"));

        void OnDisable () => Material.Destroy(material);
        #endregion
    }
}