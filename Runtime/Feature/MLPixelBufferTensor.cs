/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;
    using System.Runtime.InteropServices;
    using Internal;
    using Types;

    /// <summary>
    /// </summary>
    public sealed class MLPixelBufferFeature : MLFeature, IMLInputFeature { // RGBA8888 only.

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelBuffer"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (
            int width,
            int height,
            IntPtr pixelBuffer,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, height, width })) {
            this.pixelBuffer = pixelBuffer;
            this.aspect = aspect;
        }
        #endregion


        #region --Lock--

        private readonly IntPtr pixelBuffer;
        private readonly MLImageAspect aspect;
        private GCHandle shapeHandle;

        unsafe NMLTensorSpecification IMLInputFeature.Lock () { // INCOMPLETE // Byte type
            var shape = (type as MLTensorType).shape;
            shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            return new NMLTensorSpecification(
                (void*)pixelBuffer,
                (int*)shapeHandle.AddrOfPinnedObject(),
                shape.Length,
                0,
                NMLTensorFlag.PixelBuffer | (NMLTensorFlag)aspect
            );
        }

        void IMLInputFeature.Unlock () {
            shapeHandle.Free();
            shapeHandle = default;
        }
        #endregion
    }
}