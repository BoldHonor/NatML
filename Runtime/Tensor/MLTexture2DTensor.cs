/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;
    using System.Runtime.InteropServices;
    using Feature;
    using Internal;
    using UnityEngine;
    using Unity.Collections.LowLevel.Unsafe;

    /// <summary>
    /// </summary>
    public sealed class MLTexture2DTensor : MLTensor, IMLInputTensor {

        #region --Client API--

        public MLTexture2DTensor (
            Texture2D texture,
            MLImageAspect aspect = MLImageAspect.AspectFill
        ) : base(new MLImageFeature(null, typeof(byte), new [] { 1, 3, texture.height, texture.width })) {
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

        unsafe MLTensorSpecification IMLInputTensor.Lock () { // INCOMPLETE // Byte type
            var shape = (type as MLTensorFeature).shape;
            shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            return new MLTensorSpecification(
                NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(texture.GetRawTextureData<byte>()),
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