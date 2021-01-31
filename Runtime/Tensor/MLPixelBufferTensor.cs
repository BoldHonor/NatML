/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;
    using System.Runtime.InteropServices;
    using Feature;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLPixelBufferTensor : MLTensor, IMLInputTensor { // RGBA8888 only.

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelBuffer"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferTensor (
            int width,
            int height,
            IntPtr pixelBuffer,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageFeature(null, typeof(byte), new [] { 1, 3, height, width })) {
            this.pixelBuffer = pixelBuffer;
            this.aspect = aspect;
        }
        #endregion


        #region --Lock--

        private readonly IntPtr pixelBuffer;
        private readonly MLImageAspect aspect;
        private GCHandle shapeHandle;

        unsafe MLTensorSpecification IMLInputTensor.Lock () { // INCOMPLETE // Byte type
            var shape = (type as MLTensorFeature).shape;
            shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            return new MLTensorSpecification(
                (void*)pixelBuffer,
                (int*)shapeHandle.AddrOfPinnedObject(),
                shape.Length,
                0,
                MLTensorFlag.PixelBuffer | (MLTensorFlag)aspect
            );
        }

        void IMLInputTensor.Unlock () {
            shapeHandle.Free();
            shapeHandle = default;
        }
        #endregion
    }
}