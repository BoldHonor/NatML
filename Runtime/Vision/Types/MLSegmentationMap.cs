/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Vision {

    /// <summary>
    /// Segmentation map.
    /// Each pixel in the map contains an integer label indicating what class the pixel belongs to.
    /// </summary>
    public sealed class MLSegmentationMap {
        
        #region --Client API--
        /// <summary>
        /// Map width.
        /// </summary>
        public readonly int width;

        /// <summary>
        /// Map height.
        /// </summary>
        public readonly int height;

        /// <summary>
        /// Map data.
        /// </summary>
        public readonly int[] data;

        /// <summary>
        /// Get the label at a pixel position.
        /// </summary>
        public int this [int x, int y] => data[x + y * width];
        #endregion


        #region --Operations--

        public MLSegmentationMap (int width, int height, int[] data) {
            this.width = width;
            this.height = height;
            this.data = data;
        }
        #endregion
    }
}