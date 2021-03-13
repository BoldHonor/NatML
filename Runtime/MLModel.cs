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
        public unsafe MLFeature[] Predict (params MLFeature[] inputs) { // DEPLOY
            // Check input count
            if (inputs.Length != this.inputs.Count)
                throw new ArgumentException(@"Incorrect number of inputs provided", nameof(inputs));
            // Create NML features
            var inputBlitters = inputs.Cast<IBlittableFeature>().Select(i => i.CreateBlitter()).ToArray();
            var inputFeatures = inputBlitters.Select(i => i.feature).ToArray();
            var outputFeatures = new NMLFeature[this.outputs.Count];
            // Run inference
            model.Predict(inputFeatures, outputFeatures);
            var outputs = outputFeatures.Select(o => o.ManagedFeature()).ToArray();
            // Cleanup
            foreach (var blitter in inputBlitters)
                blitter.Dispose();
            foreach (var feature in outputFeatures)
                ((IntPtr)(&feature)).DisposeFeature();
            // Return
            return outputs;
        }

        /// <summary>
        /// Dispose the model and release resources.
        /// </summary>
        public void Dispose () => model.DisposeModel();
        #endregion


        #region --Operations--

        private readonly IntPtr model;

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator () { // DEPLOY
            foreach (var key in (this as IReadOnlyDictionary<string, string>).Keys)
                yield return new KeyValuePair<string, string>(key, this[key]);
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys { // DEPLOY
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
            var validKey = (this as IReadOnlyDictionary<string, string>).Keys.Contains(key);
            value = validKey ? this[key] : default;
            return validKey;
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<KeyValuePair<string, string>>).GetEnumerator();
        #endregion
    }
}