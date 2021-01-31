/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Internal;
    using Tensor;

    /// <summary>
    /// </summary>
    public class MLModel : IDisposable, IEnumerable<string> {

        #region --Client API--
        /// <summary>
        /// Model inputs.
        /// </summary>
        public readonly IReadOnlyList<MLFeature> inputs;

        /// <summary>
        /// Model outputs.
        /// </summary>
        public readonly IReadOnlyList<MLFeature> outputs;

        /// <summary>
        /// Get a value in the model's metadata dictionary.
        /// </summary>
        /// <param name="key">Metadata key.</param>
        public string this [string key] { // DEPLOY
            get {
                var result = new StringBuilder(2048);
                model.MetadataValue(key, result);
                return result.ToString();
            }
        }

        /// <summary>
        /// Create an ML model.
        /// </summary>
        /// <param name="modelPath">Path to ONNX model.</param>
        public MLModel (string modelPath) {
            this.model = Bridge.CreateModel(modelPath);
            this.inputs = new MLFeatureCollection(model, true);
            this.outputs = new MLFeatureCollection(model, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        public unsafe MLTensor[] Predict (params MLTensor[] inputs) {
            // Check input count
            if (inputs.Length != this.inputs.Count)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Check prediction input
            foreach (var input in inputs)
                if (!(input is IMLInputTensor))
                    throw new ArgumentException($"Feature '{input.type.name}' ccannot be used as prediction input");
            // Run inference
            var inputSpecs = inputs.Select(input => (input as IMLInputTensor).Lock()).ToArray();
            var outputSpecs = new MLTensorSpecification[this.outputs.Count];
            model.Predict(inputSpecs, outputSpecs);
            Array.ForEach(inputs, input => (input as IMLInputTensor).Unlock());
            // Create output tensors

            /*
            // Run inference
            var rawInputs = inputs.Zip(this.inputs, (input, feature) => input.LockBuffer(feature)).ToArray();
            var rawOutputs = new IntPtr[this.outputs.Count];
            model.Predict(rawInputs, rawOutputs);
            foreach (var input in inputs)
                input.UnlockBuffer();
            // Create output tensors
            var result = new List<MLTensor>();
            foreach (var rawOutput in rawOutputs) {
                MLTensor tensor;
                var shape = new long[10]; // should be big enough for most things
                rawOutput.TensorFeature(out var nativeType, out int dims, shape);
                var type = MLFeatureCollection.TypeForNativeType(nativeType);
                if (type == typeof(IList))
                    tensor = new MLSequenceTensor();
                else if (type == typeof(IDictionary))
                    tensor = new MLDictionaryTensor();
                else
                    tensor = new MLNativeArrayTensor(rawOutput, type, shape.Cast<int>().ToArray());
                result.Add(tensor);
            }
            return result.ToArray();
            */
            return default;
        }

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public void Dispose () => model.DisposeModel(); // DEPLOY
        #endregion


        #region --Operations--

        private readonly IntPtr model;

        IEnumerator<string> IEnumerable<string>.GetEnumerator () { // DEPLOY
            var count = model.MetadataKeyCount();
            var buffer = new StringBuilder(2048);
            for (var i = 0; i < count; i++) {
                model.MetadataKey(i, buffer);
                yield return buffer.ToString();
                buffer.Clear();
            }
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<string>).GetEnumerator();
        #endregion
    }
}