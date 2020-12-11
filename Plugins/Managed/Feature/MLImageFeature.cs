/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// ML image feature.
    /// </summary>
    public class MLImageFeature : MLFeature {

        #region --Client API--
        /// <summary>
        /// Image width.
        /// </summary>
        public int width => shape[3];

        /// <summary>
        /// Image height.
        /// </summary>
        public int height => shape[2];

        /// <summary>
        /// Image channels.
        /// </summary>
        public int channels => shape[1];
        #endregion


        #region --Operations--

        private readonly int[] shape;

        internal MLImageFeature (string name, Type type, int[] shape) : base(name, type) => this.shape = shape;

        public override string ToString () => $"{name}: ({width}, {height}, {channels}) {type}";

        public static implicit operator MLTensorFeature (MLImageFeature feature) => new MLTensorFeature(feature.name, feature.type, feature.shape);
        #endregion
    }
}