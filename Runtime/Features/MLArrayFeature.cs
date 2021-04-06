/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features {

    using System;
    using Types;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLArrayFeature<T> : MLFeature, INMLFeature where T : unmanaged {

        #region --Client API--
        /// <summary>
        /// Feature data.
        /// </summary>
        public readonly T[] data;

        /// <summary>
        /// Create an array feature.
        /// </summary>
        /// <param name="data"></param>
        public MLArrayFeature (T[] data) : this(data, null as int[]) { }
        
        /// <summary>
        /// Create an array feature.
        /// </summary>
        /// <param name="data"></param>
        public unsafe MLArrayFeature (T* data) : this(data, null as int[]) { }

        /// <summary>
        /// Create an array feature.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="shape"></param>
        public MLArrayFeature (T[] data, int[] shape) : this(data, new MLArrayType(null, typeof(T), shape)) { }
        
        /// <summary>
        /// Create an array feature.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="shape"></param>
        public unsafe MLArrayFeature (T* data, int[] shape) : this(data, new MLArrayType(null, typeof(T), shape)) { }

        /// <summary>
        /// Create an array feature.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public MLArrayFeature (T[] data, MLFeatureType type) : base(type) => this.data = data;

        /// <summary>
        /// Create an array feature.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public unsafe MLArrayFeature (T* data, MLFeatureType type) : base(type) => this.nativeBuffer = (IntPtr)data;
        #endregion


        #region --Operations--

        private readonly IntPtr nativeBuffer;

        unsafe IntPtr INMLFeature.CreateNativeFeature (MLFeatureType type) {
            // Check types
            var featureType = type as MLArrayType;
            var bufferType = this.type as MLArrayType;
            if (featureType.dataType != bufferType.dataType)
                throw new ArgumentException($"Model expects {featureType.dataType} feature but was given {bufferType.dataType} feature");
            // Create feature
            var shape = bufferType.shape ?? featureType.shape;
            var result = IntPtr.Zero;
            if (nativeBuffer != IntPtr.Zero)
                Bridge.CreateFeature((void*)nativeBuffer, shape, shape.Length, featureType.dataType.NativeType(), out result);
            else
                fixed (void* baseAddress = data)
                    Bridge.CreateFeature(baseAddress, shape, shape.Length, featureType.dataType.NativeType(), out result);
            return result;
        }
        #endregion
    }
}