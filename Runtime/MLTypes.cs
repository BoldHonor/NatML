/* 
*   NatML
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    /// <summary>
    /// </summary>
    public enum MLImageAspect : int {
        /// <summary>
        /// </summary>
        ScaleToFit = 0,
        /// <summary>
        /// Image will be aspect-filled to the input dimensions of the model.
        /// </summary>
        AspectFill = 1 << 12,
    }
}