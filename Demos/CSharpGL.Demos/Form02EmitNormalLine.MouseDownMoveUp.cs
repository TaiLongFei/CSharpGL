﻿using CSharpGL.ModelAdapters;
using CSharpGL.Models;
using GLM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpGL.Demos
{
    public partial class Form02EmitNormalLine : Form
    {

        DragParam dragParam;

        private void glCanvas1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // operate camera
                rotator.SetBounds(this.glCanvas1.Width, this.glCanvas1.Height);
                rotator.MouseDown(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // move vertex
                PickedGeometry pickedGeometry = RunPicking(new RenderEventArgs(
                    this.PickingMode == SelectionMode.DrawMode ? RenderModes.ColorCodedPicking : RenderModes.ColorCodedPickingPoints,
                    this.camera), e.X, e.Y);
                if (pickedGeometry != null)
                {
                    var dragParam = new DragParam(
                        camera.GetProjectionMat4(),
                        camera.GetViewMat4(),
                        new Point(e.X, glCanvas1.Height - e.Y - 1),
                        pickedGeometry.Indexes);
                    this.dragParam = dragParam;
                }
            }
        }

        private void glCanvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // operate camera
                rotator.MouseMove(e.X, e.Y);
                //this.cameraUpdated = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // move vertex
                DragParam dragParam = this.dragParam;
                if (dragParam != null)
                {
                    var current = new Point(e.X, glCanvas1.Height - e.Y - 1);
                    Point differenceOnScreen = new Point(
                        current.X - dragParam.lastMousePositionOnScreen.X,
                        current.Y - dragParam.lastMousePositionOnScreen.Y);
                    dragParam.lastMousePositionOnScreen = current;
                    this.rendererDict[this.selectedModel].MovePositions(
                        differenceOnScreen,
                        dragParam.viewMatrix, dragParam.projectionMatrix,
                        dragParam.viewport,
                        dragParam.pickedIndexes);
                }
            }
            else
            {
                RunPicking(new RenderEventArgs(
                    this.PickingMode == SelectionMode.DrawMode ? RenderModes.ColorCodedPicking : RenderModes.ColorCodedPickingPoints,
                    this.camera), e.X, e.Y);
            }
        }

        private PickedGeometry RunPicking(RenderEventArgs e, int x, int y)
        {
            lock (this.synObj)
            {
                {
                    this.glCanvas1_OpenGLDraw(this.glCanvas1, null);
                    Color c = GL.ReadPixel(x, this.glCanvas1.Height - y - 1);
                    c = Color.FromArgb(255, c);
                    this.lblColor.BackColor = c;
                    this.lblReadColor.Text = string.Format(
                        "Color at {0}: {1}",
                        new Point(x, this.glCanvas1.Height - y - 1), c);
                }
                {
                    IColorCodedPicking pickable = this.rendererDict[this.SelectedModel];
                    pickable.MVP = this.camera.GetProjectionMat4() * this.camera.GetViewMat4();
                    PickedGeometry pickedGeometry = ColorCodedPicking.Pick(
                        e, x, y, this.glCanvas1.Width, this.glCanvas1.Height, pickable);
                    if (pickedGeometry != null)
                    {
                        this.RunPickingBoard.SetContent(pickedGeometry.ToString(
                            camera.GetProjectionMat4(), camera.GetViewMat4()));
                    }
                    else
                    {
                        this.RunPickingBoard.SetContent("picked nothing.");
                    }

                    return pickedGeometry;
                }
            }
        }

        private void glCanvas1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // operate camera
                rotator.MouseUp(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // move vertex
                this.dragParam = null;
            }
        }

        public SelectionMode PickingMode { get; set; }
    }
}
