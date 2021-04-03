/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Features;
    using Features.Types;
    using Internal;

    /// <summary>
    /// ML model.
    /// </summary>
    public sealed class MLModel : MLModel<MLFeature[]> {

        #region --Client API--
        /// <summary>
        /// Create an ML model.
        /// </summary>
        /// <param name="path">Path to ONNX model.</param>
        public MLModel (string path) : base(path) { }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public override MLFeature[] Predict (params MLFeature[] inputs) {
            // Check input count
            if (inputs.Length != this.inputs.Length)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Predict
            var inputFeatures = inputs.Select((f, i) => f.CreateNMLFeature(this.inputs[i])).ToArray();
            var outputFeatures = Predict(inputFeatures);
            var outputs = outputFeatures.Select(MarshalFeature).ToArray();
            // Release
            foreach (var feature in inputFeatures)
                feature.ReleaseFeature();
            foreach (var feature in outputFeatures)
                feature.ReleaseFeature();
            // Return
            return outputs;
        }
        #endregion


        #region --Operations--

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