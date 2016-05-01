﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    class ZeroIndexLineInPolygonSearcher : ZeroIndexLineSearcher
    {
        internal override uint[] Search(RenderEventArgs e,
            int x, int y, int canvasWidth, int canvasHeight,
            uint lastVertexId, ZeroIndexModernRenderer modernRenderer)
        {
            var zeroIndexBufferPtr = modernRenderer.GetIndexBufferPtr() as ZeroIndexBufferPtr;
            ZeroIndexBufferPtr indexBufferPtr = null;
            // when the temp index buffer could be long, it's no longer needed. (great OpenGL API design)
            using (var buffer = new ZeroIndexBuffer(DrawMode.LineLoop,
                zeroIndexBufferPtr.FirstVertex, zeroIndexBufferPtr.VertexCount))
            {
                indexBufferPtr = buffer.GetBufferPtr() as ZeroIndexBufferPtr;
            }
            modernRenderer.Render4Picking(e, indexBufferPtr);
            uint id = ColorCodedPicking.ReadPixel(x, y, canvasHeight);

            indexBufferPtr.Dispose();

            if (id == zeroIndexBufferPtr.FirstVertex)
            { return new uint[] { (uint)(zeroIndexBufferPtr.FirstVertex + zeroIndexBufferPtr.VertexCount - 1), id, }; }
            else
            { return new uint[] { id - 1, id, }; }
        }
    }
}
