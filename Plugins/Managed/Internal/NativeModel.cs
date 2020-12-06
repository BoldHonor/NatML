/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {    

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class NativeModel : IMLModel, IClassifierModel {

        #region --Client API--

        public string this [string key] { // DEPLOY
            get {
                var result = new StringBuilder(2048);
                model.MetadataValue(key, result);
                return result.ToString();
            }
        }

        public NativeModel (IntPtr model) => this.model = model;

        public void Dispose () => model.DisposeModel(); // DEPLOY

        public MLFeature[] inputs { // DEPLOY
            get {
                var nameBuffer = new StringBuilder(2048);
                var shapeBuffer = new long[10]; // should be large enough for most things
                var result = Enumerable.Range(0, model.InputFeatureCount()).Select(index => {
                    nameBuffer.Clear();
                    model.InputFeature(index, nameBuffer, out var type, shapeBuffer);
                    return new MLFeature(nameBuffer, type, shapeBuffer);
                });
                return result.ToArray();
            }
        }

        public MLFeature[] outputs { // DEPLOY
            get {
                var nameBuffer = new StringBuilder(2048);
                var shapeBuffer = new long[10]; // should be large enough for most things
                var result = Enumerable.Range(0, model.OutputFeatureCount()).Select(index => {
                    nameBuffer.Clear();
                    model.OutputFeature(index, nameBuffer, out var type, shapeBuffer);
                    return new MLFeature(nameBuffer, type, shapeBuffer);
                });
                return result.ToArray();
            }
        }

        public void Classify<T> (T[] pixelBuffer, int width, int height) where T : struct {
            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            Classify(handle.AddrOfPinnedObject(), width, height);
            handle.Free();
        }

        public void Classify (IntPtr nativeBuffer, int width, int height) { // INCOMPLETE

        }
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