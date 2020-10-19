/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;
    using Unity.Collections;

    /// <summary>
    /// </summary>
    public interface ISegmentationModel : IMLModel {

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Predict<T> (T[] pixelBuffer, int width, int height) where T : struct;

        /// <summary>
        /// </summary>
        /// <param name="nativeArray"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Predict<T> (NativeArray<T> nativeArray, int width, int height) where T : struct;

        /// <summary>
        /// </summary>
        /// <param name="nativeBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Predict (IntPtr nativeBuffer, int width, int height);
    }
}