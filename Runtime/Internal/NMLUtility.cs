/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Features;

    public static class NMLUtility {

        public static NMLFeatureType NativeType (this Type type) {
            switch (type) {
                case var t when t == typeof(byte): return NMLFeatureType.UInt8;
                case var t when t == typeof(short): return NMLFeatureType.Int16;
                case var t when t == typeof(int): return NMLFeatureType.Int32;
                case var t when t == typeof(long): return NMLFeatureType.Int64;
                case var t when t == typeof(float): return NMLFeatureType.Float;
                case var t when t == typeof(double): return NMLFeatureType.Double;
                case var t when t == typeof(string): return NMLFeatureType.String;
                case var t when t.IsAssignableFrom(typeof(IList)): return NMLFeatureType.Sequence;
                case var t when t.IsAssignableFrom(typeof(IDictionary)): return NMLFeatureType.Dictionary;
                default: return NMLFeatureType.Undefined;
            }
        }

        public static Type ManagedType (this NMLFeatureType type) {
            switch (type) {
                case NMLFeatureType.UInt8: return typeof(byte);
                case NMLFeatureType.Int16: return typeof(short);
                case NMLFeatureType.Int32: return typeof(int);
                case NMLFeatureType.Int64: return typeof(long);
                case NMLFeatureType.Float: return typeof(float);
                case NMLFeatureType.Double: return typeof(double);
                case NMLFeatureType.String: return typeof(string);
                case NMLFeatureType.Sequence: return typeof(IList);
                case NMLFeatureType.Dictionary: return typeof(IDictionary);
                default: return null;
            }
        }

        public static unsafe MLFeature ManagedFeature (this in NMLFeature feature) { // DEPLOY
            switch (feature.type) {
                case NMLFeatureType.UInt8: return feature.CopyFeature<byte>();
                case NMLFeatureType.Int16: return feature.CopyFeature<short>();
                case NMLFeatureType.Int32: return feature.CopyFeature<int>();
                case NMLFeatureType.Int64: return feature.CopyFeature<long>();
                case NMLFeatureType.Float: return feature.CopyFeature<float>();
                case NMLFeatureType.Double: return feature.CopyFeature<double>();
                case NMLFeatureType.String:
                case NMLFeatureType.Sequence:
                case NMLFeatureType.Dictionary: return null;
                default: return null;
            }
        }

        private static unsafe MLArrayFeature<T> CopyFeature<T> (this in NMLFeature feature) where T : unmanaged {
            // Get shape
            var shape = new int[feature.dimensions];
            Marshal.Copy((IntPtr)feature.shape, shape, 0, feature.dimensions);
            // Copy data
            var elementCount = shape.Aggregate(1, (a, b) => a * b);
            var byteSize = elementCount * Marshal.SizeOf<T>();
            var destination = new T[elementCount];
            fixed (T* dstAddress = destination)
                Buffer.MemoryCopy(feature.data, dstAddress, byteSize, byteSize);
            return new MLArrayFeature<T>(destination, shape);
        }
    }
}