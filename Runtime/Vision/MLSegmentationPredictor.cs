/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using System.Runtime.InteropServices;
    using Internal;
    using Types;
    using Visualizers;

    /// <summary>
    /// </summary>
    public sealed class MLSegmentationPredictor : IMLPredictor<MLSegmentationMap> {

        #region --Client API--
        /// <summary>
        /// Create a segmentation predictor.
        /// </summary>
        /// <param name="model">Segmentation ML model.</param>
        public MLSegmentationPredictor (MLModel model) => this.model = model;
        
        /// <summary>
        /// </summary>
        /// <param name="inputs">Input feature.</param>
        /// <returns></returns>
        public unsafe MLSegmentationMap Predict (params MLFeature[] inputs) {
            // Check
            if (inputs.Length != 1)
                throw new ArgumentException(@"Segmentation predictor expects a single feature", nameof(inputs));
            // Check type
            var input = inputs[0];
            if (!(input.type is MLArrayType))
                throw new ArgumentException(@"Segmentation predictor expects an an array or image feature", nameof(inputs));
            // Predict
            var inputType = model.inputs[0];
            var inputFeature = (input as IMLFeature).Create(inputType);
            var outputFeature = model.Predict(inputFeature)[0];
            inputFeature.ReleaseFeature();
            // Get output type
            outputFeature.FeatureType(out var type);
            var outputType = type.MarshalFeatureType() as MLImageType;
            type.ReleaseFeatureType();
            // Marshal
            var mapData = new int[outputType.width * outputType.height];
            Marshal.Copy(outputFeature.FeatureData(), mapData, 0, mapData.Length);
            outputFeature.ReleaseFeature();
            // Return
            var result = new MLSegmentationMap(outputType.width, outputType.height, mapData);
            return result;
        }
        #endregion


        #region --Operations--

        private readonly IMLModel model;

        void IDisposable.Dispose () { }
        #endregion
    }
}