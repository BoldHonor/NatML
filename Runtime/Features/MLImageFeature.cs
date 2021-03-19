/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features {

    using System;
    using UnityEngine;
    using Internal;
    using Types;
    using Vision;

    /// <summary>
    /// </summary>
    public sealed unsafe class MLImageFeature : MLFeature, INMLFeature { // RGBA8888 only

        #region --Client API--
        /// <summary>
        /// Normalization mean.
        /// </summary>
        public Vector3 mean = Vector3.zero;

        /// <summary>
        /// Normalization standard deviation.
        /// </summary>
        public Vector3 std = Vector3.one;

        /// <summary>
        /// </summary>
        public MLAspectMode aspectMode = 0;

        /// <summary>
        /// </summary>
        /// <param name="texture"></param>
        public MLImageFeature (Texture2D texture) : this(texture.GetPixels32(), texture.width, texture.height) { }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MLImageFeature (Color32[] pixelBuffer, int width, int height) : base(new MLImageType(width, height)) => this.colorBuffer = pixelBuffer;

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MLImageFeature (byte[] pixelBuffer, int width, int height) : base(new MLImageType(width, height)) => this.pixelBuffer = pixelBuffer;

        /// <summary>
        /// </summary>
        /// <param name="nativeBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MLImageFeature (void* nativeBuffer, int width, int height) : base(new MLImageType(width, height)) => this.nativeBuffer = nativeBuffer;
        #endregion


        #region --Operations--

        private readonly byte[] pixelBuffer;
        private readonly Color32[] colorBuffer;
        private readonly void* nativeBuffer;

        unsafe IntPtr INMLFeature.CreateFeature (MLFeatureType type) {
            if (pixelBuffer != null)
                fixed (void* data = pixelBuffer)
                    return CreateFeature(type, data);
            if (colorBuffer != null)
                fixed (void* data = colorBuffer)
                    return CreateFeature(type, data);
            if (nativeBuffer != null)
                return CreateFeature(type, nativeBuffer);
            return IntPtr.Zero;
        }

        private unsafe IntPtr CreateFeature (MLFeatureType type, void* data) {
            var featureType = type as MLArrayType;
            var bufferType = this.type as MLImageType;
            Bridge.CreateFeatureFromPixelBuffer(
                data,
                bufferType.width,
                bufferType.height,
                featureType.shape,
                featureType.dataType.NativeType(),
                aspectMode,
                new [] { mean.x, mean.y, mean.z },
                new [] { std.x, std.y, std.z },
                out var result
            );
            return result;
        }
        #endregion
    }
}