/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features {

    using Types;
    using Internal;
    using Internal.Blitters;

    /// <summary>
    /// </summary>
    public sealed unsafe class MLArrayFeature<T> : MLFeature, IBlittableFeature where T : unmanaged {

        #region --Client API--
        /// <summary>
        /// </summary>
        public readonly T[] array;

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        public MLArrayFeature (T[] array) : base(new MLArrayType(null, typeof(T), new [] { 1, array.Length })) => this.array = array;

        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="array"></param>
        public MLArrayFeature (T[] array, int[] shape) : base(new MLArrayType(null, typeof(T), shape)) => this.array = array;
        
        /// <summary>
        /// Create an array tensor.
        /// </summary>
        /// <param name="nativeBuffer"></param>
        public MLArrayFeature (void* nativeBuffer, int[] shape) : base(new MLArrayType(null, typeof(T), shape)) => this.nativeBuffer = nativeBuffer;
        #endregion


        #region --Operations--

        private readonly void* nativeBuffer;

        IFeatureBlitter IBlittableFeature.CreateBlitter () {
            var shape = (type as MLArrayType).shape;
            if (array != null)
                return new ArrayBlitter<T>(array, shape);
            if (nativeBuffer != null)
                return new NativeBlitter(nativeBuffer, shape, typeof(T), 0);
            return null;
        }
        #endregion
    }
}