/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Visualizers {

    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class MLSegmentationVisualizer : MonoBehaviour {

        #region --Client API--
        /// <summary>
        /// Render a segmentation map.
        /// </summary>
        /// <param name="map">Segmentation map.</param>
        /// <param name="palette">Color palette for visualization. If `null`, a random palette is generated.</param>
        public void Render (MLSegmentationMap map, Color[] palette = null) {
            // Check
            if (segmentationImage)
                if (segmentationImage.width != map.width || segmentationImage.height != map.height) {
                    segmentationImage.Release();
                    segmentationImage = null;
                }
            // Colorize
            palette = palette ?? CreateRandomPalette(32); // Should be large enough for most things
            segmentationImage = ConvertToImage(map, palette, segmentationImage);
            // Display image
            var rawImage = GetComponent<RawImage>();
            var aspectFitter = GetComponent<AspectRatioFitter>();
            rawImage.texture = segmentationImage;
            aspectFitter.aspectRatio = (float)map.width / map.height;
        }

        /// <summary>
        /// Convert a segmentation map to an image which can be rendered.
        /// </summary>
        /// <param name="map">Segmentation map.</param>
        /// <param name="palette">Color palette for visualizing different classes in the image.</param>
        /// <param name="destination">Destination texture to render into. If `null`, a texture is created.</param>
        /// <returns>Converted RenderTexture.</returns>
        public static unsafe RenderTexture ConvertToImage (MLSegmentationMap map, Color[] palette, RenderTexture destination = null) {
            // Create texture
            if (!destination) {
                destination = new RenderTexture(
                    map.width,
                    map.height,
                    0,
                    RenderTextureFormat.ARGB32,
                    RenderTextureReadWrite.Default
                ) { enableRandomWrite = true };
                destination.Create();
            }
            // Check
            if (map.width != destination.width || map.height != destination.height)
                throw new ArgumentException(@"Destination texture must have the same size as the segmentation map", nameof(destination));
            // Create buffer
            using (
                ComputeBuffer mapBuffer = new ComputeBuffer(map.width * map.height, sizeof(int)),
                paletteBuffer = new ComputeBuffer(palette.Length, sizeof(Vector4))
            ) {
                // Fill buffers
                mapBuffer.SetData(map.data);
                paletteBuffer.SetData(FlattenPalette(palette));
                // Colorize
                colorizer = colorizer ?? (ComputeShader)Resources.Load(@"MLSegmentationColorizer");
                colorizer.SetBuffer(0, "Map", mapBuffer);
                colorizer.SetBuffer(0, "Palette", paletteBuffer);
                colorizer.SetTexture(0, "Result", destination);
                colorizer.Dispatch(0, Mathf.CeilToInt(map.width / 16), Mathf.CeilToInt(map.height / 16), 1);
            }
            // Return
            return destination;
        }

        /// <summary>
        /// Create a random segmentation palette for visualization.
        /// </summary>
        /// <param name="size">Size of the color palette.</param>
        /// <returns>Color palette.</returns>
        public static Color[] CreateRandomPalette (int size) => Enumerable
            .Range(0, size)
            .Select(_ => UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f))
            .ToArray();
        #endregion


        #region --Operations--

        private RenderTexture segmentationImage;
        private static ComputeShader colorizer;

        private static float[] FlattenPalette (Color[] palette) {
            float[] result = new float[palette.Length * 4];
            for (int i = 0; i < palette.Length; ++i) {
                result[4 * i + 0] = palette[i][0];
                result[4 * i + 1] = palette[i][1];
                result[4 * i + 2] = palette[i][2];
                result[4 * i + 3] = palette[i][3];
            }
            return result;
        }
        #endregion
    }
}