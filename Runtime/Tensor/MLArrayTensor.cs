/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Tensor {

    using System;
    using System.Runtime.InteropServices;
    using Feature;
    using Internal;

    /// <summary>
    /// </summary>
    public class MLArrayTensor<T> : MLTensor, IMLInputTensor where T : unmanaged {

        #region --Client API--
        /// <summary>
        /// Create an array tensor.
        /// </summary>
        public MLArrayTensor (T[] array) : base(new MLTensorFeature(null, typeof(T), new [] { 1, array.Length })) => this.array = array;

        /// <summary>
        /// Copy tensor data to an array.
        /// </summary>
        /// <param name="destination">Destination array.</param>
        public void CopyTo (T[] destination) {

        }
        #endregion


        #region --Operations--

        private readonly T[] array;
        private GCHandle dataHandle;
        private GCHandle shapeHandle;

        unsafe MLTensorSpecification IMLInputTensor.Lock () {
            var shape = (type as MLTensorFeature).shape;
            dataHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            return new MLTensorSpecification(
                (void*)dataHandle.AddrOfPinnedObject(),
                (int*)shapeHandle.AddrOfPinnedObject(),
                shape.Length,
                typeof(T).NativeType()
            );
        }

        void IMLInputTensor.Unlock() {
            dataHandle.Free();
            shapeHandle.Free();
            dataHandle =
            shapeHandle = default;
        }
        #endregion
    }
}