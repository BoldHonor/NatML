/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features.Types {

    using System;

    /// <summary>
    /// Array feature type.
    /// </summary>
    public class MLArrayType : MLFeatureType {

        #region --Client API--
        /// <summary>
        /// Tensor shape.
        /// </summary>
        public readonly int[] shape; // Can be `null`

        /// <summary>
        /// Tensor dimensions.
        /// </summary>
        public int dims => shape?.Length ?? 0; // Mark `readonly` in C# 8
        #endregion


        #region --Operations--

        public MLArrayType (string name, Type type, int[] shape) : base(name, type) => this.shape = shape;

        public override string ToString () => $"{name}: ({string.Join(", ", shape)}) {dataType}";
        #endregion
    }
}