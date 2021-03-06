﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VolumeRendering.Raycast
{
    public partial class FormMain
    {
        private PickedGeometry pickedGeometry;
        private Point lastMousePosition;

        private void glCanvas1_MouseDown(object sender, MouseEventArgs e)
        {
            this.lastMousePosition = e.Location;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //// operate camera
                //rotator.SetBounds(this.glCanvas1.Width, this.glCanvas1.Height);
                //rotator.MouseDown(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //// move vertex
                //if (pickedGeometry != null)
                //{
                //    IGLCanvas canvas = this.winGLCanvas1;
                //    var viewport = new vec4(0, 0, canvas.Width, canvas.Height);
                //    var lastWindowSpacePos = new vec3(e.X, this.winGLCanvas1.Height - e.Y - 1, pickedGeometry.PickedPosition.z);
                //    mat4 projectionMat = this.scene.Camera.GetProjectionMatrix();
                //    mat4 viewMat = this.scene.Camera.GetViewMatrix();
                //    mat4 modelMat = (pickedGeometry.FromObject as PickableNode).GetModelMatrix();
                //    var lastModelSpacePos = glm.unProject(lastWindowSpacePos, viewMat * modelMat, projectionMat, viewport);

                //    var dragParam = new DragParam(
                //        lastModelSpacePos,
                //        this.scene.Camera.GetProjectionMatrix(),
                //        this.scene.Camera.GetViewMatrix(),
                //        viewport,
                //        new ivec2(e.X, this.winGLCanvas1.Height - e.Y - 1));
                //    dragParam.pickedVertexIds.AddRange(pickedGeometry.VertexIds);
                //    this.dragParam = dragParam;
                //}
            }
        }

        private void glCanvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastMousePosition == e.Location) { return; }

            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    //// operate camera
            //    //rotator.MouseMove(e.X, e.Y);
            //}
            //else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    // move vertex
            //    DragParam dragParam = this.dragParam;
            //    if (dragParam != null && this.pickedGeometry != null)
            //    {
            //        var node = this.pickedGeometry.FromObject as PickableNode;
            //        var currentWindowSpacePos = new vec3(e.X, this.winGLCanvas1.Height - e.Y - 1, this.pickedGeometry.PickedPosition.z);
            //        var currentModelSpacePos = glm.unProject(currentWindowSpacePos, dragParam.viewMat * node.GetModelMatrix(), dragParam.projectionMat, dragParam.viewport);
            //        var modelSpacePositionDiff = currentModelSpacePos - dragParam.lastModelSpacePos;
            //        dragParam.lastModelSpacePos = currentModelSpacePos;
            //        IList<vec3> newPositions = node.MovePositions(
            //              modelSpacePositionDiff,
            //              dragParam.pickedVertexIds);

            //        this.UpdateHightlight(newPositions);
            //    }
            //}
            //else
            {
                int x = e.X;
                int y = this.winGLCanvas1.Height - e.Y - 1;
                PickedGeometry pickedGeometry = this.pickingAction.Pick(x, y, PickingGeometryTypes.Triangle | PickingGeometryTypes.Quad, this.winGLCanvas1.Width, this.winGLCanvas1.Height);
                this.pickedGeometry = pickedGeometry;
                this.UpdateHightlight(pickedGeometry);

                if (pickedGeometry != null)
                {
                    this.winGLCanvas1.Invalidate();
                }
            }

            this.lastMousePosition = e.Location;

            //this.winGLCanvas1.Invalidate();// redraw the scene.
        }

        private void glCanvas1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //// operate camera
                //rotator.MouseUp(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // move vertex
            }

            this.lastMousePosition = e.Location;
        }

        private void UpdateHightlight(IList<vec3> newPositions)
        {
            switch (this.pickedGeometry.Type)
            {
                case GeometryType.Point:
                    throw new NotImplementedException();
                case GeometryType.Line:
                    throw new NotImplementedException();
                case GeometryType.Triangle:
                    triangleTip.Vertex0 = newPositions[0];
                    triangleTip.Vertex1 = newPositions[1];
                    triangleTip.Vertex2 = newPositions[2];
                    break;
                case GeometryType.Quad:
                    quadTip.Vertex0 = newPositions[0];
                    quadTip.Vertex1 = newPositions[1];
                    quadTip.Vertex2 = newPositions[2];
                    quadTip.Vertex3 = newPositions[3];
                    break;
                case GeometryType.Polygon:
                    throw new NotImplementedException();
                default:
                    throw new NotDealWithNewEnumItemException(typeof(GeometryType));
            }
        }

        private void UpdateHightlight(PickedGeometry picked)
        {
            if (picked != null)
            {
                switch (picked.Type)
                {
                    case GeometryType.Point:
                        throw new NotImplementedException();
                    case GeometryType.Line:
                        throw new NotImplementedException();
                    case GeometryType.Triangle:
                        triangleTip.Vertex0 = picked.Positions[0];
                        triangleTip.Vertex1 = picked.Positions[1];
                        triangleTip.Vertex2 = picked.Positions[2];
                        triangleTip.Parent = picked.FromObject as SceneNodeBase;
                        quadTip.Parent = null;
                        break;
                    case GeometryType.Quad:
                        quadTip.Vertex0 = picked.Positions[0];
                        quadTip.Vertex1 = picked.Positions[1];
                        quadTip.Vertex2 = picked.Positions[2];
                        quadTip.Vertex3 = picked.Positions[3];
                        quadTip.Parent = picked.FromObject as SceneNodeBase;
                        triangleTip.Parent = null;
                        break;
                    case GeometryType.Polygon:
                        throw new NotImplementedException();
                    default:
                        throw new NotDealWithNewEnumItemException(typeof(GeometryType));
                }
            }
            else
            {
                triangleTip.Parent = null;
                quadTip.Parent = null;
            }
        }
    }
}
