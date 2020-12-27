/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLVisionModel : MLModelBase { // RGBA8888 only

        #region --Client API--
        /// <summary>
        /// Create a vision ML model.
        /// </summary>
        /// <param name="modelPath">Path to ONNX model.</param>
        public MLVisionModel (string modelPath) : base(Bridge.CreateVisionModel(modelPath)) { }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public unsafe MLTensor Predict<T> (T[] pixelBuffer, int width, int height, MLImageAspect aspect = MLImageAspect.AspectFill) where T : unmanaged {
            fixed (T* baseAddress = pixelBuffer)
                return Predict((IntPtr)baseAddress, width, height, aspect);
        }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLTensor Predict (IntPtr nativeBuffer, int width, int height, MLImageAspect aspect = MLImageAspect.AspectFill) {
            return default;
        }
        #endregion
    }

    /// <summary>
    /// </summary>
    public enum MLImageAspect : int {
        /// <summary>
        /// Image will be aspect-filled to the input dimensions of the model.
        /// </summary>
        AspectFill = 0,
    }
}