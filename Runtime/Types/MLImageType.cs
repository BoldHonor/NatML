/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features.Types {

    using System;

    /// <summary>
    /// Image feature type.
    /// </summary>
    public sealed class MLImageType : MLArrayType {

        #region --Client API--
        /// <summary>
        /// Image width.
        /// </summary>
        public int width => shape[interleaved ? 2 : 3];

        /// <summary>
        /// Image height.
        /// </summary>
        public int height => shape[interleaved ? 1 : 2];

        /// <summary>
        /// Image channels.
        /// </summary>
        public int channels => shape[interleaved ? 3 : 1];
        #endregion


        #region --Operations--

        private readonly bool interleaved;

        public MLImageType (int width, int height) : this(width, height, typeof(byte)) { }

        public MLImageType (int width, int height, Type type) : this(null, type, new [] { 1, 3, height, width }) { }

        public MLImageType (string name, Type type, int[] shape) : base(name, type, shape) => interleaved = shape[3] == 3;
        #endregion
    }
}