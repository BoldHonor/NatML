/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// </summary>
    public interface ISegmentationModel : IMLModel {

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Segment<T> (T[] pixelBuffer, int width, int height) where T : struct;

        /// <summary>
        /// </summary>
        /// <param name="nativeBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Segment (IntPtr nativeBuffer, int width, int height);
    }
}