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

        public static NMLDataType NativeType (this Type type) {
            switch (type) {
                case var t when t == typeof(byte): return NMLDataType.UInt8;
                case var t when t == typeof(short): return NMLDataType.Int16;
                case var t when t == typeof(int): return NMLDataType.Int32;
                case var t when t == typeof(long): return NMLDataType.Int64;
                case var t when t == typeof(float): return NMLDataType.Float;
                case var t when t == typeof(double): return NMLDataType.Double;
                case var t when t == typeof(string): return NMLDataType.String;
                case var t when t.IsAssignableFrom(typeof(IList)): return NMLDataType.Sequence;
                case var t when t.IsAssignableFrom(typeof(IDictionary)): return NMLDataType.Dictionary;
                default: return NMLDataType.Undefined;
            }
        }

        public static Type ManagedType (this NMLDataType type) {
            switch (type) {
                case NMLDataType.UInt8: return typeof(byte);
                case NMLDataType.Int16: return typeof(short);
                case NMLDataType.Int32: return typeof(int);
                case NMLDataType.Int64: return typeof(long);
                case NMLDataType.Float: return typeof(float);
                case NMLDataType.Double: return typeof(double);
                case NMLDataType.String: return typeof(string);
                case NMLDataType.Sequence: return typeof(IList);
                case NMLDataType.Dictionary: return typeof(IDictionary);
                default: return null;
            }
        }

        public static unsafe MLFeature ManagedFeature (this in NMLFeature feature) { // DEPLOY
            switch (feature.dataType) {
                case NMLDataType.UInt8: return feature.CopyFeature<byte>();
                case NMLDataType.Int16: return feature.CopyFeature<short>();
                case NMLDataType.Int32: return feature.CopyFeature<int>();
                case NMLDataType.Int64: return feature.CopyFeature<long>();
                case NMLDataType.Float: return feature.CopyFeature<float>();
                case NMLDataType.Double: return feature.CopyFeature<double>();
                case NMLDataType.String:
                case NMLDataType.Sequence:
                case NMLDataType.Dictionary: return null;
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