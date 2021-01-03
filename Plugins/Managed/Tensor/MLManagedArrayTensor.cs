/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;
    using System.Runtime.InteropServices;

    internal sealed class MLManagedArrayTensor : MLArrayTensor {

        #region --Client API--
        /// <summary>
        /// Copy tensor data to an array.
        /// </summary>
        /// <param name="destination">Destination array.</param>
        public override void CopyTo<T> (T[] destination) { // INCOMPLETE

        }
        #endregion


        #region --Operations--

        internal MLManagedArrayTensor (int[] data) : base(typeof(int), new [] { data.Length }) {

        }

        internal MLManagedArrayTensor (float[] data) : base(typeof(float), new [] { data.Length }) {

        }

        internal override IntPtr LockBuffer (MLFeature specification) {
            return IntPtr.Zero;
        }

        internal override void UnlockBuffer () {

        }
        #endregion
    }
}