/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;
    using UnityEngine;
    using Unity.Collections.LowLevel.Unsafe;
    using Types;
    using Internal;
    using Internal.Blitters;

    /// <summary>
    /// </summary>
    public sealed unsafe class MLPixelBufferFeature : MLFeature, IBlittableFeature { // RGBA8888 only

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (Texture2D texture, MLImageAspect aspect = 0) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, texture.height, texture.width })) {
            if (!texture.isReadable)
                throw new ArgumentException(@"Texture must be readable", nameof(texture));
            this.nativeBuffer = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(texture.GetRawTextureData<byte>());
        }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (byte[] pixelBuffer, int width, int height, MLImageAspect aspect = 0) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, height, width })) {
            this.pixelBuffer = pixelBuffer;
            this.aspect = aspect;
        }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (Color32[] pixelBuffer, int width, int height, MLImageAspect aspect = 0) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, height, width })) {
            this.colorBuffer = pixelBuffer;
            this.aspect = aspect;
        }

        /// <summary>
        /// </summary>
        /// <param name="nativeBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (void* nativeBuffer, int width, int height, MLImageAspect aspect = 0) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, height, width })) {
            this.nativeBuffer = nativeBuffer;
            this.aspect = aspect;
        }
        #endregion


        #region --Operations--
        public readonly byte[] pixelBuffer;
        public readonly Color32[] colorBuffer;
        private readonly void* nativeBuffer;
        private readonly MLImageAspect aspect;

        IFeatureBlitter IBlittableFeature.CreateBlitter () {
            var shape = (type as MLTensorType).shape;
            var dataType = (type as MLTensorType).dataType;
            var flags = NMLFeatureFlag.PixelBuffer | (NMLFeatureFlag)aspect;
            if (pixelBuffer != null)
                return new ArrayBlitter<byte>(pixelBuffer, shape, flags);
            if (colorBuffer != null)
                return new ArrayBlitter<Color32>(colorBuffer, shape, flags);
            if (nativeBuffer != null)
                return new NativeBlitter(nativeBuffer, shape, dataType, flags);
            return null;
        }
        #endregion
    }
}