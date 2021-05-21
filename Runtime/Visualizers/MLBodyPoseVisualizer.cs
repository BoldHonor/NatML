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
    public sealed class MLBodyPoseVisualizer : MonoBehaviour { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Render a body pose.
        /// </summary>
        /// <param name="image">Image which body pose is generated from.</param>
        /// <param name="pose">Body pose to render.</param>
        public void Render (Texture image, IMLBodyPose pose) {

        }
        #endregion
    }
}