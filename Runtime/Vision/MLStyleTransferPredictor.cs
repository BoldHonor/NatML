/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using UnityEngine;
    using Internal;

    /// <summary>
    /// </summary>
    public sealed class MLStyleTransferPredictor : IMLPredictor<Texture2D> { // INCOMPLETE

        #region --Client API--
        /// <summary>
        /// Create a style transfer predictor.
        /// </summary>
        /// <param name="model">Style transfer ML model.</param>
        public MLStyleTransferPredictor (MLModel model) {
            this.model = model;
        }

        /// <summary>
        /// </summary>
        /// <param name="inputs">Input feature.</param>
        /// <returns>Texture containing translated image.</returns>
        public unsafe Texture2D Predict (params MLFeature[] inputs) {
            return default;
        }

        /// <summary>
        /// </summary>
        /// <param name="input">Input feature.</param>
        /// <param name="destination">Destination texture.</param>
        /// <returns>Destination texture containing translated image.</returns>
        public unsafe Texture2D Predict (MLFeature input, Texture2D destination) {
            return default;
        }
        #endregion


        #region --Operations--

        private readonly IMLModel model;
        
        void IDisposable.Dispose () { } // Nop
        #endregion
    }
}