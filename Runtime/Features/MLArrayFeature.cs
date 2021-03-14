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
        /// Backing array.
        /// </summary>
        public readonly T[] array;

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        public MLArrayFeature (T[] array) : this(array, null) { }

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        public MLArrayFeature (T[] array, int[] shape) : base(new MLArrayType(null, typeof(T), shape)) => this.array = array;
        #endregion


        #region --Operations--

        unsafe IntPtr INMLFeature.CreateFeature (MLFeatureType type) {
            // Check types
            var featureType = type as MLArrayType;
            var bufferType = this.type as MLArrayType;
            if (featureType.dataType != bufferType.dataType)
                throw new ArgumentException($"Model expects {featureType.dataType} feature but was given {bufferType.dataType} feature");
            // Create feature
            var result = IntPtr.Zero;
            var shape = bufferType.shape ?? featureType.shape;
            fixed (void* baseAddress = array)
                Bridge.CreateFeature(baseAddress, shape, shape.Length, featureType.dataType.NativeType(), out result);
            return result;
        }
        #endregion
    }
}