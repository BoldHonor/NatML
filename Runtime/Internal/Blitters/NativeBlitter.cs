/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal.Blitters {

    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// </summary>
    public unsafe readonly struct NativeBlitter : IFeatureBlitter {

        #region --Client API--
        public NMLFeature feature => new NMLFeature {
            data = data,
            shape = (int*)shapeHandle.AddrOfPinnedObject(),
            dimensions = dimensions,
            dataType = type,
            flags = flags
        };

        public NativeBlitter (void* data, int[] shape, Type type, NMLFeatureFlag flags) {
            this.data = data;
            this.shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            this.dimensions = shape.Length;
            this.type = type.NativeType();
            this.flags = flags;
        }

        public void Dispose () => shapeHandle.Free();
        #endregion


        #region --Operations--
        private readonly void* data;
        private readonly GCHandle shapeHandle;
        private readonly int dimensions;
        private readonly NMLDataType type;
        private readonly NMLFeatureFlag flags;
        #endregion
    }
}