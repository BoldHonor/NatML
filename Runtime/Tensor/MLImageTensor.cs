/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;

    /// <summary>
    /// </summary>
    public sealed class MLImageTensor : MLTensor { // RGBA8888 only.

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelBuffer"></param>
        /// <param name="aspect"></param>
        public MLImageTensor (int width, int height, IntPtr pixelBuffer, MLImageAspect aspect = MLImageAspect.AspectFill) {

        }
        #endregion


        #region --Operations--

        internal override IntPtr LockBuffer (MLFeature specification) {
            return IntPtr.Zero;
        }

        internal override void UnlockBuffer () {

        }
        #endregion
    }
}