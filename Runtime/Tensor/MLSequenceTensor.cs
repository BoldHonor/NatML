/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;

    /// <summary>
    /// </summary>
    public sealed class MLSequenceTensor : MLTensor {

        #region --Operations--

        internal MLSequenceTensor () {

        }

        internal override IntPtr LockBuffer (MLFeature specification) {
            return IntPtr.Zero;
        }

        internal override void UnlockBuffer () {

        }
        #endregion
    }
}