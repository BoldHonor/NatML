/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;

    /// <summary>
    /// ML tensor feature.
    /// </summary>
    public class MLTensorFeature : MLFeature {

        #region --Client API--
        /// <summary>
        /// Tensor shape.
        /// </summary>
        public readonly int[] shape;

        /// <summary>
        /// Tensor dimensions.
        /// </summary>
        public int dimensions => shape?.Length ?? 0; // Mark `readonly` in C# 8
        #endregion


        #region --Operations--

        internal MLTensorFeature (string name, Type type, int[] shape) : base(name, type) => this.shape = shape;

        public override string ToString () => $"{name}: {string.Join("x", shape)} {type}";
        #endregion
    }
}