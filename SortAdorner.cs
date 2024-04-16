

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DTCWaitingList
{
    public class SortAdorner : Adorner
    {
        private static Geometry _ascendingArrowGeometry = Geometry.Parse("M 0,0 L 10,0 L 5,5 Z");
        private static Geometry _descendingArrowGeometry = Geometry.Parse("M 0,5 L 10,5 L 5,0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement adornedElement, ListSortDirection direction)
            : base(adornedElement)
        {
            Direction = direction;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
            (
                AdornedElement.RenderSize.Width - 15,
                (AdornedElement.RenderSize.Height - 5) / 2
            );

            drawingContext.PushTransform(transform);

            Geometry geometry = Direction == ListSortDirection.Ascending ? _ascendingArrowGeometry : _descendingArrowGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}



