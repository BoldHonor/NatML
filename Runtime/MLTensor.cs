/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using UnityEngine;
    using Unity.Collections.LowLevel.Unsafe;
    using Tensor;

    /// <summary>
    /// </summary>
    public abstract class MLTensor {

        #region --Operations--

        internal abstract IntPtr LockBuffer (MLFeature specification);

        internal abstract void UnlockBuffer ();
        #endregion


        #region --Conversions--

        public static unsafe implicit operator MLTensor (Texture2D texture) { // Create MLImageTensor
            void* baseAddress = texture.GetRawTextureData<byte>().GetUnsafeReadOnlyPtr();
            return new MLImageTensor(texture.width, texture.height, (IntPtr)baseAddress);
        }

        public static implicit operator MLTensor (WebCamTexture webCamTexture) { // Create MLImageTensor
            return default;
        }

        public static implicit operator MLTensor (float[] array) => default; // INCOMPLETE

        public static implicit operator MLTensor (int[] array) => default; // INCOMPLETE

        public static implicit operator MLTensor (string value) => null; // INCOMPLETE
        #endregion
    }
}