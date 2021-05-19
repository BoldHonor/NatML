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
    public sealed class MLBodyPoseVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="image">Image which detections are made on.</param>
        /// <param name="pose">Body pose to render.</param>
        public void Render (Texture image, IMLBodyPose pose) {

        }
        #endregion
    }
}