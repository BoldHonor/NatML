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

    /// <summary>
    /// </summary>
    public class MLModel : IDisposable, IReadOnlyDictionary<string, string> {

        #region --Client API--
        /// <summary>
        /// Model inputs.
        /// </summary>
        public readonly IReadOnlyList<MLFeatureType> inputs;

        /// <summary>
        /// Model outputs.
        /// </summary>
        public readonly IReadOnlyList<MLFeatureType> outputs;

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
            this.inputs = new MLInputFeatureMap(model);
            this.outputs = new MLOutputFeatureMap(model);
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs"></param>
        public unsafe MLFeature[] Predict (params MLFeature[] inputs) {
            // Check input count
            if (inputs.Length != this.inputs.Count)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Create native input features

            // Run inference

            // Create managed output features

            
            // Run inference
            /*
            var inputSpecs = inputs.Select(input => (input as IMLInputFeature).Lock()).ToArray();
            var outputSpecs = new NMLTensorSpecification[this.outputs.Count];
            model.Predict(inputSpecs, outputSpecs);
            Array.ForEach(inputs, input => (input as IMLInputFeature).Unlock());
            */
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

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator () { // DEPLOY
            foreach (var key in (this as IReadOnlyDictionary<string, string>).Keys)
                yield return new KeyValuePair<string, string>(key, this[key]);
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys {
            get {
                var count = model.MetadataKeyCount();
                var buffer = new StringBuilder(2048);
                for (var i = 0; i < count; i++) {
                    buffer.Clear();
                    model.MetadataKey(i, buffer);
                    yield return buffer.ToString();
                }
            }
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Values {
            get {
                foreach (var key in (this as IReadOnlyDictionary<string, string>).Keys)
                    yield return this[key];
            }
        }

        int IReadOnlyCollection<KeyValuePair<string, string>>.Count => model.MetadataKeyCount();

        bool IReadOnlyDictionary<string, string>.ContainsKey (string key) => (this as IReadOnlyDictionary<string, string>).Keys.Contains(key);

        bool IReadOnlyDictionary<string, string>.TryGetValue (string key, out string value) {
            if ((this as IReadOnlyDictionary<string, string>).Keys.Contains(key)) {
                value = this[key];
                return true;
            } else {
                value = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<KeyValuePair<string, string>>).GetEnumerator();
        #endregion
    }
}