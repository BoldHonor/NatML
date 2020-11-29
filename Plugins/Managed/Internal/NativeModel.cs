/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {    

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public sealed class NativeModel : IMLModel {

        #region --Client API--

        public string this [string key] { // DEPLOY
            get {
                model.MetadataValue(key, out var value);
                var result = Marshal.PtrToStringAuto(value);
                Marshal.FreeHGlobal(value); // Make sure to use platform default allocator
                return result;
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