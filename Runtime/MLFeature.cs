/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using UnityEngine;
    using Features;

    /// <summary>
    /// </summary>
    public abstract class MLFeature {

        #region --Client API--
        /// <summary>
        /// Feature type.
        /// </summary>
        public readonly MLFeatureType type;

        /// <summary>
        /// Implicitly convert a Texture2D to an ML feature.
        /// </summary>
        public static implicit operator MLFeature (Texture2D texture) => new MLImageFeature(texture);

        /// <summary>
        /// Implicitly convert a WebCamTexture to an ML feature.
        /// </summary>
        public static implicit operator MLFeature (WebCamTexture texture) => new MLImageFeature(texture.GetPixels32(), texture.width, texture.height);

        /// <summary>
        /// Implicitly convert a float array an ML feature.
        /// </summary>
        public static implicit operator MLFeature (float[] array) => new MLArrayFeature<float>(array);

        /// <summary>
        /// Implicitly convert a integer array an ML feature.
        /// </summary>
        public static implicit operator MLFeature (int[] array) => new MLArrayFeature<int>(array);
        #endregion



        #region --Operations--

        protected MLFeature (MLFeatureType type) => this.type = type;
        #endregion
    }
}