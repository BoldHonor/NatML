/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System.Runtime.InteropServices;
    using Internal;
    using Types;

    /// <summary>
    /// </summary>
    public class MLArrayFeature<T> : MLFeature, IMLInputFeature where T : unmanaged {

        #region --Client API--
        /// <summary>
        /// Create an array tensor.
        /// </summary>
        public MLArrayFeature (T[] array) : base(new MLTensorType(null, typeof(T), new [] { 1, array.Length })) => this.array = array;

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

        unsafe NMLTensorSpecification IMLInputFeature.Lock () {
            var shape = (type as MLTensorType).shape;
            dataHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            return new NMLTensorSpecification(
                (void*)dataHandle.AddrOfPinnedObject(),
                (int*)shapeHandle.AddrOfPinnedObject(),
                shape.Length,
                typeof(T).NativeType()
            );
        }

        void IMLInputFeature.Unlock() {
            dataHandle.Free();
            shapeHandle.Free();
            dataHandle =
            shapeHandle = default;
        }
        #endregion
    }
}