/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using UnityEngine;
    using Feature;

    /// <summary>
    /// </summary>
    public abstract class MLFeature {

        #region --Client API--
        /// <summary>
        /// Tensor type information.
        /// </summary>
        public readonly MLFeatureType type;

        /// <summary>
        /// Implicitly convert a Texture2D to a tensor.
        /// </summary>
        public static unsafe implicit operator MLFeature (Texture2D texture) => new MLTexture2DFeature(texture);

        /// <summary>
        /// Implicitly convert a float array a tensor.
        /// </summary>
        public static implicit operator MLFeature (float[] array) => new MLArrayFeature<float>(array);

        /// <summary>
        /// Implicitly convert a integer array a tensor.
        /// </summary>
        public static implicit operator MLFeature (int[] array) => new MLArrayFeature<int>(array);
        #endregion



        #region --Operations--

        protected MLFeature (MLFeatureType type) => this.type = type;
        #endregion
    }
}