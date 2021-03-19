/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal {

    using System;
    using System.Collections;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using Features;
    using Features.Types;

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

        public static unsafe MLFeature MarshalFeature (this in IntPtr feature) { // INCOMPLETE // OPTIMIZE
            // Get feature type
            feature.FeatureType(out var nativeType);
            var type = nativeType.MarshalFeatureType();
            // Marshal
            switch (type.dataType.NativeType()) { // Easier to switch on native type
                case NMLDataType.UInt8: return feature.MarshalArrayFeature<byte>(type);
                case NMLDataType.Int16: return feature.MarshalArrayFeature<short>(type);
                case NMLDataType.Int32: return feature.MarshalArrayFeature<int>(type);
                case NMLDataType.Int64: return feature.MarshalArrayFeature<long>(type);
                case NMLDataType.Float: return feature.MarshalArrayFeature<float>(type);
                case NMLDataType.Double: return feature.MarshalArrayFeature<double>(type);
                case NMLDataType.String:
                case NMLDataType.Sequence:
                case NMLDataType.Dictionary: return null;
                default: return null;
            }
        }

        public static MLFeatureType MarshalFeatureType (this in IntPtr nativeType) { // INCOMPLETE // Nested types
            // Get dtype
            var dtype = nativeType.FeatureTypeDataType();
            if (dtype == NMLDataType.Undefined)
                return null;
            // Get name
            var nameBuffer = new StringBuilder(2048);
            nativeType.FeatureTypeName(nameBuffer);
            var name = nameBuffer.ToString();
            // Get shape
            var shape = new int[nativeType.FeatureTypeDimensions()];
            nativeType.FeatureTypeShape(shape, shape.Length);
            // Return
            nativeType.ReleaseFeatureType();
            switch (dtype) {
                case NMLDataType.Sequence:
                    return null;
                case NMLDataType.Dictionary:
                    return null;
                case var _ when shape.Length == 4:
                    return new MLImageType(name, dtype.ManagedType(), shape);
                default:
                    return new MLArrayType(name, dtype.ManagedType(), shape);
            }
        }

        private static unsafe MLArrayFeature<T> MarshalArrayFeature<T> (this in IntPtr feature, MLFeatureType type) where T : unmanaged {
            var shape = (type as MLArrayType).shape;
            var elementCount = shape.Aggregate(1, (a, b) => a * b);
            var byteSize = elementCount * Marshal.SizeOf<T>();
            var data = new T[elementCount];
            fixed (T* dstAddress = data)
                Buffer.MemoryCopy((void*)feature.FeatureData(), dstAddress, byteSize, byteSize);
            feature.ReleaseFeature();
            return new MLArrayFeature<T>(data, type);
        }
    }
}