/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Visualizers {

    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    using Vision;

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
        public void Render (MLSegmentationMap map, Color[] palette = null, int width = 0, int height = 0) {
            // Create image
            image?.Release();
            var frameSize = width > 0 && height > 0 ? (width, height) : (map.width, map.height);
            image = new RenderTexture(frameSize.width, frameSize.height, 0);
            // Render
            palette = palette ?? CreateRandomPalette(32); // Should be large enough for most things
            Render(map, palette, image);
            // Display image
            var rawImage = GetComponent<RawImage>();
            var aspectFitter = GetComponent<AspectRatioFitter>();
            rawImage.texture = image;
            aspectFitter.aspectRatio = (float)frameSize.width / frameSize.height;
        }

        /// <summary>
        /// Render a segmentation map to a texture.
        /// </summary>
        /// <param name="map">Segmentation map.</param>
        /// <param name="palette">Color palette for visualizing different classes in the image.</param>
        /// <param name="destination">Destination texture.</param>
        public static unsafe void Render (MLSegmentationMap map, Color[] palette, RenderTexture destination) {
            // Check map
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            // Check palette
            if (palette == null)
                throw new ArgumentNullException(nameof(palette));
            // Check texture
            if (!destination)
                throw new ArgumentNullException(nameof(destination));
            // Create buffer
            using (
                ComputeBuffer mapBuffer = new ComputeBuffer(map.width * map.height, sizeof(int)),
                paletteBuffer = new ComputeBuffer(palette.Length, sizeof(Vector4))
            ) {
                // Upload
                mapBuffer.SetData(map.data);
                paletteBuffer.SetData(FlattenPalette(palette));
                // Create temporary                
                var descriptor = new RenderTextureDescriptor(map.width, map.height, RenderTextureFormat.ARGB32, 0) {
                    enableRandomWrite = true
                };
                var tempBuffer = RenderTexture.GetTemporary(descriptor);
                tempBuffer.Create();
                // Render
                renderer =renderer ?? (ComputeShader)Resources.Load(@"MLSegmentationRenderer");
                renderer.SetBuffer(0, "Map", mapBuffer);
                renderer.SetBuffer(0, "Palette", paletteBuffer);
                renderer.SetTexture(0, "Result", tempBuffer);
                renderer.GetKernelThreadGroupSizes(0, out var gx, out var gy, out var _);
                renderer.Dispatch(0, Mathf.CeilToInt((float)map.width / gx), Mathf.CeilToInt((float)map.height / gy), 1);
                // Blit to destination
                Graphics.Blit(tempBuffer, destination);
                RenderTexture.ReleaseTemporary(tempBuffer);
            }
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

        private RenderTexture image;
        private static new ComputeShader renderer;

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