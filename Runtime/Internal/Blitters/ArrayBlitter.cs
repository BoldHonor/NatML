/* 
*   NatML
*   Copyright (c) 2021 Yusuf Olokoba.
*/

namespace NatSuite.ML.Internal.Blitters {

    using System.Runtime.InteropServices;

    /// <summary>
    /// </summary>
    public unsafe readonly struct ArrayBlitter<T> : IFeatureBlitter where T : unmanaged {

        #region --Client API--
        public NMLFeature feature => new NMLFeature {
            data = (void*)dataHandle.AddrOfPinnedObject(),
            shape = (int*)shapeHandle.AddrOfPinnedObject(),
            dimensions = dimensions,
            type = typeof(T).NativeType(),
            flags = flags
        };

        public ArrayBlitter (T[] array, int[] shape, NMLFeatureFlag flags = 0) {
            this.dataHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            this.shapeHandle = GCHandle.Alloc(shape, GCHandleType.Pinned);
            this.dimensions = shape.Length;
            this.flags = flags;
        }

        public void Dispose () {
            dataHandle.Free();
            shapeHandle.Free();
        }
        #endregion


        #region --Operations--

        private readonly GCHandle dataHandle;
        private readonly GCHandle shapeHandle;
        private readonly int dimensions;
        private readonly NMLFeatureFlag flags;
        #endregion
    }
}