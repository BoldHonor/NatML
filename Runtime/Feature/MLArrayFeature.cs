/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using Types;
    using Internal;
    using Internal.Blitters;

    /// <summary>
    /// </summary>
    public sealed unsafe class MLArrayFeature<T> : MLFeature, IBlittableFeature where T : unmanaged {

        #region --Client API--
        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        public MLArrayFeature (T[] array) : base(new MLTensorType(null, typeof(T), new [] { 1, array.Length })) => this.array = array;

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        public MLArrayFeature (T[] array, int[] shape) : base(new MLTensorType(null, typeof(T), shape)) => this.array = array;
        
        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="nativeBuffer"></param>
        public MLArrayFeature (void* nativeBuffer, int[] shape) : base(new MLTensorType(null, typeof(T), shape)) => this.nativeBuffer = nativeBuffer;
        #endregion


        #region --Operations--

        private readonly T[] array;
        private readonly void* nativeBuffer;

        IFeatureBlitter IBlittableFeature.CreateBlitter () {
            var shape = (type as MLTensorType).shape;
            if (array != null)
                return new ArrayBlitter<T>(array, shape);
            if (nativeBuffer != null)
                return new NativeBlitter(nativeBuffer, shape, typeof(T), 0);
            return null;
        }
        #endregion
    }
}