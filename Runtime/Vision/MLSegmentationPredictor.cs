/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    using System;
    using Unity.Collections.LowLevel.Unsafe;
    using Internal;
    using Types;

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
            var (width, height) = (outputType.width, outputType.height);
            var mapData = new int[width * height];
            var srcBuffer = (int*)outputFeature.FeatureData();
            fixed (int* dstBuffer = mapData)
                for (var i = 0; i < height; ++i)
                    UnsafeUtility.MemCpy(&dstBuffer[i * width], &srcBuffer[(height - i - 1) * width], width * sizeof(int));
            outputFeature.ReleaseFeature();
            // Return
            var result = new MLSegmentationMap(width, height, mapData);
            return result;
        }
        #endregion


        #region --Operations--

        private readonly IMLModel model;

        void IDisposable.Dispose () { }
        #endregion
    }
}