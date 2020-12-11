/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using UnityEngine;
    using Tensor;

    /// <summary>
    /// </summary>
    public abstract class MLTensor {

        #region --Operations--

        internal abstract void GetData (MLFeature specification);

        public static implicit operator MLTensor (Texture2D texture) { // Create MLImageTensor
            return null;
        }

        public static implicit operator MLTensor (float[] array) { // Create MLArrayTensor
            return null;
        }
        #endregion
    }
}