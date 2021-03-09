/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using Internal;
    using Types;

    /// <summary>
    /// </summary>
    public sealed class MLPixelBufferFeature : MLFeature { // RGBA8888 only.

        #region --Client API--
        /// <summary>
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (
            Texture2D texture,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, texture.height, texture.width })) {
            // Check that texture is readable
            if (!texture.isReadable)
                throw new ArgumentException(@"Texture must be readable", nameof(texture));
            // ...
        }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public MLPixelBufferFeature (
            Color32[] pixelBuffer,
            int width,
            int height,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, height, width })) {
            //this.pixelBuffer = pixelBuffer;
            this.aspect = aspect;
        }

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aspect"></param>
        public unsafe MLPixelBufferFeature (
            void* pixelBuffer,
            int width,
            int height,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, height, width })) {
            //this.pixelBuffer = pixelBuffer;
            this.aspect = aspect;
        }
        #endregion


        #region --Lock--

        private readonly IntPtr pixelBuffer;
        private readonly MLImageAspect aspect;
        #endregion
    }
}