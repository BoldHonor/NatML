/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature.Types {

    using System;

    /// <summary>
    /// Tensor feature type.
    /// </summary>
    public class MLTensorType : MLFeatureType {

        #region --Client API--
        /// <summary>
        /// Tensor shape.
        /// </summary>
        public readonly int[] shape; // INCOMPLETE // read only list??

        /// <summary>
        /// Tensor dimensions.
        /// </summary>
        public int dimensions => shape?.Length ?? 0; // Mark `readonly` in C# 8
        #endregion


        #region --Operations--

        internal MLTensorType (string name, Type type, int[] shape) : base(name, type) => this.shape = shape;

        public override string ToString () => $"{name}: ({string.Join(", ", shape)}) {type}";
        #endregion
    }
}