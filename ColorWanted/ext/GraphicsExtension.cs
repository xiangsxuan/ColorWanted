﻿using ColorWanted.screenshot;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ColorWanted.ext
{
    public static class GraphicsExtension
    {
        public static void Draw(this Graphics graphics, DrawRecord record)
        {
            var width = record.Width;
            using (var pen = new Pen(record.Color.ToDrawingColor(), width))
            {
                pen.StartCap = LineCap.Round;
                pen.MiterLimit = 0.1f;
                if (record.LineStyle == LineStyles.Dashed)
                {
                    pen.DashStyle = DashStyle.Dash;
                }
                else if (record.LineStyle == LineStyles.Dotted)
                {
                    pen.DashStyle = DashStyle.Dot;
                }
                switch (record.Shape)
                {
                    case DrawShapes.Curve:
                        pen.EndCap = LineCap.Round;
                        graphics.DrawCurve(pen, record.Points.Select(p=>new Point((int)p.X, (int)p.Y)).ToArray());
                        break;
                    case DrawShapes.Ellipse:
                        graphics.DrawEllipse(pen, record.Rect.ToDrawingRectangle());
                        break;
                    case DrawShapes.Line:
                        pen.EndCap = LineCap.Round;
                        graphics.DrawLine(pen, record.Start.ToDrawingPoint(), record.End.ToDrawingPoint());
                        break;
                    case DrawShapes.Rectangle:
                        graphics.DrawRectangle(pen, record.Rect.ToDrawingRectangle());
                        break;
                    case DrawShapes.Text:
                        if (string.IsNullOrWhiteSpace(record.Text))
                        {
                            return;
                        }
                        graphics.DrawString(record.Text, record.TextFont, 
                            new SolidBrush(record.Color.ToDrawingColor()), record.Start.ToDrawingPoint());
                        break;
                    case DrawShapes.Arrow:
                        var cap = new AdjustableArrowCap(5, 5, false);
                        pen.CustomEndCap = cap;
                        graphics.DrawLine(pen, record.Start.ToDrawingPoint(), record.End.ToDrawingPoint());
                        break;
                }
            }
            graphics.Flush();
            GC.Collect();
        }
    }
}
