using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using Point = System.Windows.Point;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using System.Windows.Input;
using System.Reflection.Metadata;

namespace FinanceSummary.Models
{
    public class SliceOfPie
    {
        public double starting_angle { get; set; }
        public double angle_width { get; set; }
        public double radius { get; set; }
        public Brush colour { get; set; }

        public PieShape pieShape { get; set; }

        public TextBlock percentage_block { get; set; }

        public void Render(Canvas canvas, double left, double top)
        {
            //Path path = new Path();
            //p
            //path.StartFigure();
            //path.AddArc(100, 100, 2, 2, 20, 30);
            //path.AddLine(100, 100, 20, 20);
            //path.CloseFigure();
            //canvas.Children.Add(path);
            PieShape pie = new PieShape();
            

        }

    }

    public class PieShape: UIElement

    {
        public double starting_angle { get; set; }
        public double angle_width { get; set; }
        public double radius { get; set; }
        public string percentage { get; set; }

        public string detailed_text { get; set; }

        public bool Selected { get; set; } = false;

        private bool IsMouseOver;

        private Point TextPoint;
        private Point DetailedtextPoint; 

        public SolidColorBrush colour { get; set; }
        protected Geometry DefiningGeometry
        {
            get
            {
                double angle_w = angle_width;
                double angle = starting_angle;
                double used_radius = radius;

                if (angle_width > 355)
                {
                    angle_w = 355;
                }
                if (angle_width < 5)
                {
                    angle_w = 5;
                    angle = starting_angle + angle_width - 5;
                }
                if (IsMouseOver || Selected)
                {
                    if (angle_w < 350)
                    {
                        angle_w = angle_w + 10;
                        angle = angle - 5;
                    }
                    used_radius *= 1.2;
                }
 
                Point p1 = new Point(0, 0);
                Point p2 = new Point((int)(used_radius * Math.Cos(angle * Math.PI/180)), (int)(used_radius * Math.Sin(angle * Math.PI / 180)));
                Point p3 =  new Point((int)(used_radius * Math.Cos((angle + angle_w) * Math.PI / 180)), (int)(used_radius * Math.Sin((angle + angle_w) * Math.PI / 180)));
                
                TextPoint = new Point((int)(used_radius/3 * Math.Cos((angle + angle_w/2) * Math.PI / 180)), (int)(used_radius/3 * Math.Sin((angle + angle_w/2) * Math.PI / 180)));

                List<PathSegment> shapesegments = new();
                shapesegments.Add(new LineSegment(p2, true));

                shapesegments.Add(new ArcSegment(p3, new(used_radius, used_radius), angle_w * Math.PI / 180, angle_w > 180, SweepDirection.Clockwise, true));
                shapesegments.Add(new LineSegment(p1, true));

                List<PathFigure> figures = new List<PathFigure>();
                PathFigure pf = new PathFigure(p1, shapesegments, true);

                figures.Add(pf);

                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);

                return g;
            }   
        }




        protected override void OnRender(DrawingContext drawingContext)
        {
            // Customize the rendering of your shape using the DrawingContext
            // You can draw lines, curves, etc. here based on your requirements.
            // For simplicity, we'll just fill the shape with a solid color.
            SolidColorBrush brush = (SolidColorBrush)colour;
            drawingContext.DrawGeometry(brush, null, DefiningGeometry);
            Typeface tp = new Typeface("Times New Roman");
            FormattedText txt = new FormattedText(percentage, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tp, 10, Brushes.White);
            drawingContext.DrawText(txt, TextPoint);

            if (IsMouseOver || Selected)
            {
                DetailedtextPoint = new Point((int)(radius * 1.5 * Math.Cos((starting_angle + angle_width / 2) * Math.PI / 180)), (int)(radius * 1.5 * Math.Sin((starting_angle + angle_width / 2) * Math.PI / 180)));
                FormattedText d_text = new FormattedText(detailed_text, System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tp, 15, Brushes.White);
                drawingContext.DrawText(d_text, DetailedtextPoint);
            }
            //EllipseGeometry ellipseGeometry = new EllipseGeometry(new Rect(0, 0, 100, 50));

            // Fill the geometry with a solid color (e.g., blue)
            //SolidColorBrush brush = new SolidColorBrush(Colors.Blue);

            // Draw the geometry
            //drawingContext.DrawGeometry(brush, null, ellipseGeometry);



        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            IsMouseOver = true;
            Panel.SetZIndex(this, 2);
            InvalidateVisual();


        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Panel.SetZIndex(this, 1);
            IsMouseOver = false;
            InvalidateVisual();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Selected = !Selected;
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            InvalidateVisual();
        }
    }
}
