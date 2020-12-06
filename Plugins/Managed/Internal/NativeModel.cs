/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {    

    using System;
    using System.Collections;
    using System.Collections.Generic;
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

        public MLFeature[] inputs { // INCOMPLETE
            get {
                return default;
            }
        }

        public MLFeature[] outputs { // INCOMPLETE
            get {
                return default;
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
            var keys = IntPtr.Zero;
            try {
                model.MetadataKeys(out keys, out var keyCount);
                for (var i = 0; i < keyCount; i++) {
                    var key = Marshal.ReadIntPtr(keys, i * IntPtr.Size);
                    yield return Marshal.PtrToStringAuto(key);
                }
            } finally {
                Marshal.FreeHGlobal(keys);
            }
        }

        IEnumerator IEnumerable.GetEnumerator () => (this as IEnumerable<string>).GetEnumerator();
        #endregion
    }
}