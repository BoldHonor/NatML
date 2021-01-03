/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;

    /// <summary>
    /// </summary>
    public sealed class MLDictionaryTensor : MLTensor {

        #region --Operations--

        internal MLDictionaryTensor () {

        }

        internal override IntPtr LockBuffer (MLFeature specification) {
            return IntPtr.Zero;
        }

        internal override void UnlockBuffer () {

        }
        #endregion
    }
}