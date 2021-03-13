/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML {

    /// <summary>
    /// Image aspect mode for performing inference on images.
    /// </summary>
    public enum MLImageAspect : int { // CHECK // Must match `NatML.h`
        /// <summary>
        // Image will be scaled to fit the input dimensions of the model.
        /// </summary>
        ScaleToFit = 0,
        /// <summary>
        /// Image will be aspect-filled to the input dimensions of the model.
        /// </summary>
        AspectFill = 1 << 8,
    }
}