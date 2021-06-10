/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Features {

    using System;
    using UnityEngine;
    using Internal;
    using Types;

    /// <summary>
    /// </summary>
    public sealed class MLAudioFeature : MLFeature, IMLFeature {

        #region --Client API--
        /// <summary>
        /// Create an audio feature from an audio clip.
        /// </summary>
        /// <param name="clip">Audio clip.</param>
        /// <param name="duration">Optional duration to extract in seconds. Negative values will use the whole clip.</param>
        public MLAudioFeature (
            AudioClip clip,
            float duration = -1
        ) : this(GetSampleBuffer(clip, duration), clip.frequency, clip.channels) { }

        /// <summary>
        /// Create an audio feature from a sample buffer.
        /// </summary>
        /// <param name="sampleBuffer">Linear PCM sample buffer interleaved by channel.</param>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="channelCount">Channel count.</param>
        public MLAudioFeature (
            float[] sampleBuffer,
            int sampleRate,
            int channelCount
        ) : base(new MLArrayType(null, typeof(float), new [] { 1, sampleBuffer.Length })) {
            this.sampleBuffer = sampleBuffer;
            this.bufferSampleRate = sampleRate;
            this.bufferChannelCount = channelCount;
        }

        /// <summary>
        /// Create an audio feature from a sample buffer.
        /// </summary>
        /// <param name="sampleBuffer">Linear PCM sample buffer interleaved by channel.</param>
        /// <param name="sampleCount">Total sample count.</param>
        /// <param name="sampleRate">Sample rate.</param>
        /// <param name="channelCount">Channel count.</param>
        public unsafe MLAudioFeature (
            float* sampleBuffer,
            int sampleCount,
            int sampleRate,
            int channelCount
        ) : base(new MLArrayType(null, typeof(float), new [] { 1, sampleCount })) {
            this.nativeBuffer = (IntPtr)sampleBuffer;
            this.bufferSampleRate = sampleRate;
            this.bufferChannelCount = channelCount;
        }
        #endregion


        #region --Operations--

        private readonly float[] sampleBuffer;
        private readonly IntPtr nativeBuffer;
        private readonly int bufferSampleRate;
        private readonly int bufferChannelCount;

        unsafe IntPtr IMLFeature.Create (MLFeatureType type) {
            // Check types
            var featureType = type as MLArrayType;
            var bufferType = this.type as MLArrayType;
            if (featureType.dataType != bufferType.dataType)
                throw new ArgumentException($"MLModel expects {featureType.dataType} feature but was given {bufferType.dataType} feature");
            if (featureType.dims != bufferType.dims)
                throw new ArgumentException($"MLModel expects {featureType.dims}D feature but was given {bufferType.dims}D feature");
            // Create feature
            var shape = bufferType.shape;
            if (sampleBuffer != null)
                fixed (void* data = sampleBuffer)
                    return Create(data, shape);
            if (nativeBuffer != IntPtr.Zero)
                return Create((void*)nativeBuffer, shape);
            return IntPtr.Zero;
        }

        private unsafe IntPtr Create (void* data, int[] shape) {
            Bridge.CreateFeature(
                data,
                shape,
                shape.Length,
                type.dataType.NativeType(),
                0,
                out var feature
            );
            return feature;
        }

        private static float[] GetSampleBuffer (AudioClip clip, float duration = -1) {
            var frameCount = duration < 0 ? clip.samples : Mathf.RoundToInt(clip.frequency * duration);
            frameCount = Mathf.Min(frameCount, clip.samples);
            var sampleBuffer = new float[frameCount * clip.channels];
            clip.GetData(sampleBuffer, 0);
            return sampleBuffer;
        }
        #endregion
    }
}