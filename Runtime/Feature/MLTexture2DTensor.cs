/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;
    using System.Runtime.InteropServices;
    using Internal;
    using Types;
    using UnityEngine;
    using Unity.Collections.LowLevel.Unsafe;

    /// <summary>
    /// </summary>
    public sealed class MLTexture2DFeature : MLFeature, IMLInputFeature {

        #region --Client API--

        public MLTexture2DFeature (
            Texture2D texture,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageType(null, typeof(byte), new [] { 1, 3, texture.height, texture.width })) {
            if (!texture.isReadable)
                throw new ArgumentException(@"Texture must be readable", nameof(texture));
            this.texture = texture;
            this.aspect = aspect;
        }
        #endregion


        #region --Operations--

        private readonly Texture2D texture;
        private readonly MLImageAspect aspect;
        private GCHandle shapeHandle;

        unsafe NMLTensorSpecification IMLInputFeature.Lock () { // INCOMPLETE // Byte type
            var shape = (type as MLTensorType).shape;
            shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            return new NMLTensorSpecification(
                NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(texture.GetRawTextureData<byte>()),
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