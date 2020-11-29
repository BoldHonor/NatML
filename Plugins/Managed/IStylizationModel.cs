/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    using System;

    /// <summary>
    /// </summary>
    public interface IStylizationModel : IMLModel {

        /// <summary>
        /// </summary>
        /// <param name="pixelBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Stylize<T> (T[] pixelBuffer, int width, int height) where T : struct;

        /// <summary>
        /// </summary>
        /// <param name="nativeBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Stylize (IntPtr nativeBuffer, int width, int height);
    }
}