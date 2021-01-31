/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;
    using Internal;

    internal sealed class MLNativeArrayTensor : MLArrayTensor {

        #region --Client API--
        /// <summary>
        /// Copy tensor data to an array.
        /// </summary>
        /// <param name="destination">Destination array.</param>
        public override unsafe void CopyTo<T> (T[] destination) {
            fixed (T* dstPtr = destination)
                tensor.TensorCopy((IntPtr)dstPtr);
        }
        #endregion


        #region --Operations--

        private readonly IntPtr tensor;

        internal MLNativeArrayTensor (IntPtr tensor, Type type, int[] shape) : base(type, shape) => this.tensor = tensor;

        internal override IntPtr LockBuffer (MLFeature specification) {
            return IntPtr.Zero;
        }

        internal override void UnlockBuffer () {

        }
        #endregion
    }
}