/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensors {

    using System;
    using UnityEngine;

    public sealed class MLImageTensor : MLTensor { // RGBA8888 only.

        #region --Operations--

        internal MLImageTensor (int width, int height, IntPtr pixelBuffer) {

        }

        internal override void GetData (MLFeature specification) {

        }
        #endregion
    }
}