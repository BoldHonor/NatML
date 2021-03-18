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
        /// Create an array tensor.
        /// </summary>
        /// <param name="data"></param>
        public MLArrayFeature (T[] data) : this(data, null as int[]) { }

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="shape"></param>
        public MLArrayFeature (T[] data, int[] shape) : this(data, new MLArrayType(null, typeof(T), shape)) { }

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="type"></param>
        public MLArrayFeature (T[] data, MLFeatureType type) : base(type) => this.data = data;
        #endregion


        #region --Operations--

        unsafe IntPtr INMLFeature.CreateFeature (MLFeatureType type) {
            // Check types
            var featureType = type as MLArrayType;
            var bufferType = this.type as MLArrayType;
            if (featureType.dataType != bufferType.dataType)
                throw new ArgumentException($"Model expects {featureType.dataType} feature but was given {bufferType.dataType} feature");
            // Create feature
            var shape = bufferType.shape ?? featureType.shape;
            fixed (void* baseAddress = data) {
                Bridge.CreateFeature(baseAddress, shape, shape.Length, featureType.dataType.NativeType(), out var result);
                return result;
            }
        }
        #endregion
    }
}