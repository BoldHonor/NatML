/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public abstract class MLModelBase : IDisposable, IEnumerable<string> {

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
        /// Dispose the model and release resources.
        /// </summary>
        public virtual void Dispose () => model.DisposeModel(); // DEPLOY
        #endregion


        #region --Operations--

        private readonly IntPtr model;

        protected internal MLModelBase (IntPtr model) {
            this.model = model;
            this.inputs = new MLFeatureCollection(model, true);
            this.outputs = new MLFeatureCollection(model, false);
        }

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