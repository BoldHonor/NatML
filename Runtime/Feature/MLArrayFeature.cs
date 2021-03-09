/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System.Runtime.InteropServices;
    using Types;

    /// <summary>
    /// </summary>
    public class MLArrayFeature<T> : MLFeature where T : unmanaged {

        #region --Client API--
        /// <summary>
        /// Create an array tensor.
        /// </summary>
        public MLArrayFeature (T[] array) : base(new MLTensorType(null, typeof(T), new [] { 1, array.Length })) => this.array = array;
        #endregion


        #region --Operations--

        private readonly T[] array;
        private GCHandle dataHandle;
        #endregion
    }
}