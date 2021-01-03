/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature {

    using System;

    /// <summary>
    /// ML image feature.
    /// </summary>
    public sealed class MLImageFeature : MLTensorFeature {

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

        internal MLImageFeature (string name, Type type, int[] shape) : base(name, type, shape) { }

        public override string ToString () => $"{name}: ({width}, {height}, {channels}) {type}";
        #endregion
    }
}