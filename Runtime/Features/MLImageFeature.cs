/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features {

    using System;
    using UnityEngine;
    using Unity.Collections.LowLevel.Unsafe;
    using Internal;
    using Types;
    using Vision;

    /// <summary>
    /// </summary>
    public sealed unsafe class MLImageFeature : MLFeature, INMLFeature { // RGBA8888 only

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="aspect"></param>
        public MLImageFeature (Texture2D texture, MLAspectMode aspect = 0) : base(new MLImageType(
            default,
            typeof(byte),
            new [] { 1, 3, texture.height, texture.width }
        )) {
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
        public MLImageFeature (byte[] pixelBuffer, int width, int height, MLAspectMode aspect = 0) : base(new MLImageType(
            default,
            typeof(byte),
            new [] { 1, 3, height, width }
        )) {
            this.pixelBuffer = pixelBuffer;
            this.aspect = aspect;
        }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLImageFeature (Color32[] pixelBuffer, int width, int height, MLAspectMode aspect = 0) : base(new MLImageType(
            default,
            typeof(byte),
            new [] { 1, 3, height, width }
        )) {
            this.colorBuffer = pixelBuffer;
            this.aspect = aspect;
        }

        /// <summary>
        /// </summary>
        /// <param name="nativeBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLImageFeature (void* nativeBuffer, int width, int height, MLAspectMode aspect = 0) : base(new MLImageType(
            default,
            typeof(byte),
            new [] { 1, 3, height, width }
        )) {
            this.nativeBuffer = nativeBuffer;
            this.aspect = aspect;
        }
        #endregion


        #region --Operations--

        private readonly byte[] pixelBuffer;
        private readonly Color32[] colorBuffer;
        private readonly void* nativeBuffer;
        private readonly MLAspectMode aspect;

        unsafe IntPtr INMLFeature.CreateFeature (MLFeatureType type) { // CHECK // Revisit this, very redundant
            var result = IntPtr.Zero;
            var featureType = type as MLArrayType;
            var bufferType = this.type as MLImageType;
            if (pixelBuffer != null)
                fixed (void* baseAddress = pixelBuffer)
                    Bridge.CreateFeatureFromPixelBuffer(
                        baseAddress,
                        bufferType.width,
                        bufferType.height,
                        featureType.shape,
                        featureType.dataType.NativeType(),
                        aspect,
                        out result
                    );
            if (colorBuffer != null)
                fixed (void* baseAddress = colorBuffer)
                    Bridge.CreateFeatureFromPixelBuffer(
                        baseAddress,
                        bufferType.width,
                        bufferType.height,
                        featureType.shape,
                        featureType.dataType.NativeType(),
                        aspect,
                        out result
                    );
            if (nativeBuffer != null)
                Bridge.CreateFeatureFromPixelBuffer(
                    nativeBuffer,
                    bufferType.width,
                    bufferType.height,
                    featureType.shape,
                    featureType.dataType.NativeType(),
                    aspect,
                    out result
                );
            return result;
        }
        #endregion
    }
}