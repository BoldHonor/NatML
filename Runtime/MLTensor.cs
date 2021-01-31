/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using UnityEngine;
    using Unity.Collections.LowLevel.Unsafe;
    using Tensor;
    using Internal;

    /// <summary>
    /// </summary>
    public abstract class MLTensor {

        #region --Client API--
        /// <summary>
        /// Tensor type information.
        /// </summary>
        public readonly MLFeature type;

        /// <summary>
        /// Implicitly convert a Texture2D to a tensor.
        /// </summary>
        public static unsafe implicit operator MLTensor (Texture2D texture) => new MLTexture2DTensor(texture);

        /// <summary>
        /// Implicitly convert a float array a tensor.
        /// </summary>
        public static implicit operator MLTensor (float[] array) => new MLArrayTensor<float>(array);

        /// <summary>
        /// Implicitly convert a integer array a tensor.
        /// </summary>
        public static implicit operator MLTensor (int[] array) => new MLArrayTensor<int>(array);
        #endregion



        #region --Operations--

        protected MLTensor (MLFeature type) => this.type = type;
        #endregion
    }
}