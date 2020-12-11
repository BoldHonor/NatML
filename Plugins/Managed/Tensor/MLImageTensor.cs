/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;
    using UnityEngine;

    public sealed class MLImageTensor : MLTensor { // Only accept RGBA32 data.

        #region --Client API--
        /// <summary>
        /// </summary>
        public MLImageTensor (int width, int height, IntPtr pixelBuffer) {

        }

        /// <summary>
        /// </summary>
        public void Normalize (int[] mean, int[] stdDev) {

        }
        #endregion


        #region --Operations--

        internal override void GetData (MLFeature specification) {

        }

        public static implicit operator MLImageTensor (Texture2D texture) {
            return null;
        }
        #endregion
    }
}