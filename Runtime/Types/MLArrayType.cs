/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Types {

    using System;

    /// <summary>
    /// ML array feature type.
    /// </summary>
    public class MLArrayType : MLFeatureType {

        #region --Client API--
        /// <summary>
        /// Array shape.
        /// </summary>
        public readonly int[] shape; // Can be `null`

        /// <summary>
        /// Array dimensions.
        /// </summary>
        public int dims => shape?.Length ?? 0; // Mark `readonly` in C# 8

        /// <summary>
        /// Create an ML array feature type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="shape"></param>
        public MLArrayType (string name, Type type, int[] shape) : base(name, type) => this.shape = shape;
        #endregion


        #region --Operations--

        public override string ToString () => $"{name}: ({string.Join(", ", shape)}) {dataType}";
        #endregion
    }
}