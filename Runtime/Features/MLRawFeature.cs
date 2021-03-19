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
    public sealed unsafe class MLRawFeature : MLFeature, INMLFeature {

        #region --Client API--
        /// <summary>
        /// Feature data.
        /// </summary>
        public readonly void* data;

        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public MLRawFeature (void* data, MLFeatureType type) : base(type) => this.data = data;
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
            Bridge.CreateFeature(data, shape, shape.Length, featureType.dataType.NativeType(), out var result);
            return result;
        }
        #endregion
    }
}