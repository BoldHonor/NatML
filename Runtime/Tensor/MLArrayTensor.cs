/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;

    /// <summary>
    /// </summary>
    public abstract class MLArrayTensor : MLTensor {

        #region --Client API--
        /// <summary>
        /// Feature type.
        /// This will typically be a numeric type.
        /// </summary>
        public readonly Type type;

        /// <summary>
        /// Tensor shape.
        /// </summary>
        public readonly int[] shape;

        /// <summary>
        /// Copy tensor data to an array.
        /// </summary>
        /// <param name="destination">Destination array.</param>
        public abstract void CopyTo<T> (T[] destination) where T : unmanaged;
        #endregion


        #region --Operations--

        internal MLArrayTensor (Type type, int[] shape) {
            this.type = type;
            this.shape = shape;
        }
        #endregion
    }
}