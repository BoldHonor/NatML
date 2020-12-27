/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using UnityEngine;
    using Tensors;

    /// <summary>
    /// </summary>
    public abstract class MLTensor {

        #region --Client API--
        
        #endregion

        #region --Operations--

        internal abstract void GetData (MLFeature specification);

        public static implicit operator MLTensor (Texture2D texture) { // Create MLImageTensor
            return default;
        }

        public static implicit operator MLTensor (WebCamTexture webCamTexture) { // Create MLImageTensor
            return default;
        }

        public static implicit operator MLTensor (float[] array) { // Create MLArrayTensor
            return default;
        }

        public static implicit operator MLTensor (int[] array) { // Create MLArrayTensor
            return default;
        }
        #endregion
    }
}