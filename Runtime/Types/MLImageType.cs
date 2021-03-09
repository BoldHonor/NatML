/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Feature.Types {

    using System;

    /// <summary>
    /// Image feature type.
    /// </summary>
    public sealed class MLImageType : MLTensorType {

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

        public MLImageType (string name, Type type, int[] shape) : base(name, type, shape) { }
        #endregion
    }
}