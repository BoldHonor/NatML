/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Predictors {

    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Features;
    using Features.Types;
    using Internal;

    /// <summary>
    /// </summary>
    public class MLFeaturePredictor : IMLPredictor<MLFeature[]> {

        #region --Client API--
        /// <summary>
        /// Create a feature predictor.
        /// </summary>
        /// <param name="model"></param>
        public MLFeaturePredictor (MLModel model) => this.model = model;

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public MLFeature[] Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != model.inputs.Length)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Predict
            var inputFeatures = inputs.Zip(model.inputs, (f, t) => (f as IMLFeature).Create(t)).ToArray();
            var outputFeatures = model.Predict(inputFeatures);
            var outputs = outputFeatures.Select(MarshalFeature).ToArray();
            // Release
            foreach (var feature in inputFeatures)
                feature.ReleaseFeature();
            foreach (var feature in outputFeatures)
                feature.ReleaseFeature();
            return outputs;
        }
        #endregion


        #region --Operations--

        private readonly IMLModel model;

        void IDisposable.Dispose () { }

        private static unsafe MLFeature MarshalFeature (IntPtr feature) { // INCOMPLETE
            // Get feature type
            feature.FeatureType(out var nativeType);
            var type = nativeType.MarshalFeatureType();
            // Marshal
            switch (type.dataType.NativeType()) { // Easier to switch on native type
                case NMLDataType.UInt8: return MarshalArrayFeature<byte>(feature, type);
                case NMLDataType.Int16: return MarshalArrayFeature<short>(feature, type);
                case NMLDataType.Int32: return MarshalArrayFeature<int>(feature, type);
                case NMLDataType.Int64: return MarshalArrayFeature<long>(feature, type);
                case NMLDataType.Float: return MarshalArrayFeature<float>(feature, type);
                case NMLDataType.Double: return MarshalArrayFeature<double>(feature, type);
                case NMLDataType.String:
                case NMLDataType.Sequence:
                case NMLDataType.Dictionary: return null;
                default: return null;
            }
        }

        private static unsafe MLArrayFeature<T> MarshalArrayFeature<T> (IntPtr feature, MLFeatureType type) where T : unmanaged {
            var shape = (type as MLArrayType).shape;
            var elementCount = shape.Aggregate(1, (a, b) => a * b);
            var byteSize = elementCount * Marshal.SizeOf<T>();
            var data = new T[elementCount];
            fixed (T* dstAddress = data)
                Buffer.MemoryCopy((void*)feature.FeatureData(), dstAddress, byteSize, byteSize);
            return new MLArrayFeature<T>(data, type);
        }
        #endregion
    }
}