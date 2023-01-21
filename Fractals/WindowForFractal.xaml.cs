using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Fractals
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WindowFractal : Window
    {

        /// <summary>
        /// Глубина фрактала.
        /// </summary>
        private int NumberOfSteps { get; set; }

        /// <summary>
        /// Коэффициент изменения длины для Пиф. фрактала.
        /// </summary>
        private double CoefficientOfLength { get; set; }

        /// <summary>
        /// Коэффициент значения оси ординат для первой точки для Пиф. фрактала.
        /// </summary>
        private double CoefficientOfYStart { get; set; } = 35;

        /// <summary>
        /// Угол наклона левой ветви для Пиф. фрактала.
        /// </summary>
        private double FractPifFirstAngle { get; set; }

        /// <summary>
        /// Угол наклона левой ветви для Пиф. фрактала.
        /// </summary>
        private double FractPifSecondAngle { get; set; }

        /// <summary>
        /// Изначальная длина линии для Пиф. фрактала
        /// </summary>
        private int LineLength { get; set; }

        /// <summary>
        /// Номер рисуемого фрактала.
        /// </summary>
        private int FractalNum { get; set; }

        /// <summary>
        /// Первый цвет.
        /// </summary>
        private Color colorStart = Brushes.Black.Color;

        /// <summary>
        /// Последний цвет.
        /// </summary>
        private Color colorEnd = Brushes.Black.Color;

        /// <summary>
        /// Основной цвет ковра.
        /// </summary>
        private static Color s_carpetColor = Brushes.Orange.Color;

        /// <summary>
        /// Набор цветов для градиента.
        /// </summary>
        private List<SolidColorBrush> colorsList = new();


        /// <summary>
        /// Флаг: Это первый запуск окна? true, false
        /// [0] - для слайдера.
        /// [1] - для кнопки скрина.
        /// [2] - для события изменения размера окна.
        /// [3] - для слайдера (ещё раз).
        /// </summary>
        private readonly bool[] flagOnStart = { true, true, true, true };

        /// <summary>
        /// Флаг для запуска второго потока.
        /// </summary>
        private bool redrawFractalReqested = false;

        /// <summary>
        /// Настройка масштабирования.
        /// </summary>
        private readonly ScaleTransform scaleTransform = new();


        /// <summary>
        /// Основной канструктор окны.
        /// </summary>
        /// <param name="numberOfSteps">Глубина фрактала.</param>
        /// <param name="maxDepth">Максимальная доступная глубина (для слайдера)</param>
        /// <param name="coefficientOfLength">Коэффициент изменения длины для Пиф. фрактала.</param>
        /// <param name="fractPifFirstAngle">Угол наклона левой ветви для Пиф. фрактала.</param>
        /// <param name="fractPifSecondAngle">Угол наклона левой ветви для Пиф. фрактала.</param>
        /// <param name="lineLength">Изначальная длина линии для Пиф. фрактала</param>
        public WindowFractal(int numberOfSteps, int maxDepth, Color startColor, Color endColor,
            double coefficientOfLength = 1.5, double fractPifFirstAngle = 45, double fractPifSecondAngle = 45)
        {
            InitializeComponent();

            NumberOfSteps = numberOfSteps;
            CoefficientOfLength = coefficientOfLength;
            FractPifFirstAngle = fractPifFirstAngle;
            FractPifSecondAngle = fractPifSecondAngle;

            LabelForSliderDepthValue.Content = numberOfSteps;
            SliderForDepth.Value = numberOfSteps;

            SliderForDepth.Maximum = maxDepth;

            colorStart = startColor;
            colorEnd = endColor;

            fractalWindow.MinWidth = SystemParameters.PrimaryScreenWidth / 2;
            fractalWindow.MinHeight = SystemParameters.PrimaryScreenHeight / 2;

            MainCanvas.Width = fractalWindow.MinWidth;
            MainCanvas.Height = fractalWindow.MinHeight;

            MainCanvas.LayoutTransform = scaleTransform;

        }


        /// <summary>
        /// Событие для приближения изображения.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void MainCanvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            // Масштабирование.
            if (e.Delta > 0)
            {
                scaleTransform.ScaleX *= 1.1;
            }

            if (e.Delta < 0)
            {
                scaleTransform.ScaleX /= 1.1;
            }

            scaleTransform.ScaleY = scaleTransform.ScaleX;
            // Ограничиваю минимальное и максимальное приближение.
            if (scaleTransform.ScaleX < 1 || scaleTransform.ScaleY < 1)
            {
                scaleTransform.ScaleX = 1;
                scaleTransform.ScaleY = 1;
            }
            else if (scaleTransform.ScaleX > 5 || scaleTransform.ScaleY > 5)
            {
                scaleTransform.ScaleX = 5;
                scaleTransform.ScaleY = 5;
            }

            LabelValueZoom.Content = Math.Round(scaleTransform.ScaleX, 1);
        }

        /// <summary>
        /// Составить градиент (набор цветов).
        /// </summary>
        /// <param name="colorStart">Первый цвет.</param>
        /// <param name="colorEnd">Второй цвет.</param>
        private void GetColors(int size, Color colorStart, Color colorEnd)
        {
            byte rMin = colorStart.R, gMin = colorStart.G, bMin = colorStart.B;
            byte rMax = colorEnd.R, gMax = colorEnd.G, bMax = colorEnd.B;

            colorsList = new List<SolidColorBrush>();
            SolidColorBrush brushColor = new();
            brushColor.Color = colorStart;
            colorsList.Add(brushColor);

            // Вычисляем все промежуточные значения Red, Green и Blue для всех цветов.
            for (int i = 0; i < size; i++)
            {
                byte rAverage = (byte)(rMin + ((rMax - rMin) * i / size));
                byte gAverage = (byte)(gMin + ((gMax - gMin) * i / size));
                byte bAverage = (byte)(bMin + ((bMax - bMin) * i / size));

                brushColor = new();
                brushColor.Color = Color.FromRgb(rAverage, gAverage, bAverage);

                colorsList.Add(brushColor);
            }

            colorsList.Reverse();
        }


        /// <summary>
        /// Нарисовать фрактал Пифагора (Первый)
        /// </summary>
        public void DrawPifagorFractal()
        {
            GetColors(NumberOfSteps, colorStart, colorEnd);
            FractalNum = 1;
            LineLength = (int)(MainCanvas.RenderSize.Height / (CoefficientOfLength * 2));

            double yMin = 0, yMax = MainCanvas.RenderSize.Height;

            Recursion(NumberOfSteps, MainCanvas.RenderSize.Width / 2,
                MainCanvas.RenderSize.Height - MainCanvas.RenderSize.Height / CoefficientOfYStart, LineLength, 0);

            // Рекурсивный метод для рисования фрактального дерева.
            void Recursion(int depth, double x, double y, double length, double angle)
            {
                double x1 = x + length * Math.Sin(angle * Math.PI / 180),
                    y1 = y - length * Math.Cos(angle * Math.PI / 180);

                // Ищем минимальное и максимальное значение точки,
                // чтобы перерисовать фрактал, если он вышел за размер окна.
                if (y1 < yMin)
                { yMin = y1; }
                else if (y1 > yMax)
                { yMax = y1; }

                Line line = makeLine(new double[] { x, y, x1, y1, (depth + 1) / 3 });

                if (depth > 0 && depth <= colorsList.Count)
                { line.Stroke = colorsList[depth - 1]; }

                MainCanvas.Children.Add(line);
                if (depth > 1 && length > 1)
                {
                    Recursion(depth - 1, x1, y1, length / CoefficientOfLength, angle + FractPifFirstAngle);
                    Recursion(depth - 1, x1, y1, length / CoefficientOfLength, angle - FractPifSecondAngle);
                }
            }
            ReDrawSmaller(yMin, yMax);
        }


        /// <summary>
        /// Метод для перерисовки фрактала, если он не поместился в окно сверху или снизу.
        /// </summary>
        /// <param name="yMin">Минимальное значение по оси ординат у фрактала</param>
        /// <param name="yMax">Максимальное значение по оси ординат у фрактала</param>
        private void ReDrawSmaller(double yMin, double yMax)
        {
            if (yMin < 0)
            {

                MainCanvas.Children.Clear();
                CoefficientOfLength += 0.05;
                DrawPifagorFractal();
                return;

            }
            else if (yMax > MainCanvas.RenderSize.Height)
            {

                MainCanvas.Children.Clear();
                CoefficientOfYStart -= 5;
                DrawPifagorFractal();
                return;

            }

        }


        /// <summary>
        /// Нарисовать фрактал "Кривая Коха" (Второй).
        /// </summary>
        public void DrawKohaFractal()
        {
            FractalNum = 2;
            int depth = NumberOfSteps - 1;
            List<Line> lines = new();
            //        /-C-\
            // A___B_/     \_D___E
            double ax = MainCanvas.RenderSize.Width * 0.05, ay = MainCanvas.RenderSize.Height / 3.0 * 2.0,
               ex = MainCanvas.RenderSize.Width - MainCanvas.RenderSize.Width * 0.05,
               ey = MainCanvas.RenderSize.Height / 3.0 * 2.0;
            if (depth < 0)
            {
                lines.Add(makeLine(new double[] { ax, ay, ex, ey, 6 }));
            }
            else
            {
                Recursion(depth, ax, ay, ex, ey);
                // Хвостовая рекурсивная функция рисования кривой Коха
                void Recursion(int depth, double ax, double ay, double ex, double ey)
                {
                    // Вычисляем координаты точек, как на рисунке выше ( 0.5 - cos60, 0.866 - sin60 )
                    double bx = ax + (ex - ax) / 3.0, by = ay + (ey - ay) / 3.0,
                        dx = ax + (ex - ax) * 2.0 / 3.0, dy = ay + (ey - ay) * 2.0 / 3.0,
                        cx = bx + (dx - bx) * 0.5 + 0.866 * (dy - by),
                        cy = by - (dx - bx) * 0.866 + 0.5 * (dy - by);
                    if (depth == 0)
                    {
                        lines.Add(makeLine(new double[] { ax, ay, bx, by, 12 / (NumberOfSteps + 1) }));
                        lines.Add(makeLine(new double[] { bx, by, cx, cy, 12 / (NumberOfSteps + 1) }));
                        lines.Add(makeLine(new double[] { cx, cy, dx, dy, 12 / (NumberOfSteps + 1) }));
                        lines.Add(makeLine(new double[] { dx, dy, ex, ey, 12 / (NumberOfSteps + 1) }));
                        return;
                    }
                    Recursion(depth - 1, ax, ay, bx, by);
                    Recursion(depth - 1, bx, by, cx, cy);
                    Recursion(depth - 1, cx, cy, dx, dy);
                    Recursion(depth - 1, dx, dy, ex, ey);
                }
            }
            DisplayAndPaint(lines);
        }


        /// <summary>
        /// Метод для отрисовки и покраски фрактала.
        /// </summary>
        /// <param name="lines">Лист с прямыми.</param>
        private void DisplayAndPaint(List<Line> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                MainCanvas.Children.Add(lines[i]);
            }

            GetColors(MainCanvas.Children.Count, colorStart, colorEnd);
            colorsList.Reverse();

            for (int i = 0; i < MainCanvas.Children.Count; i++)
            {
                Line line = (Line)MainCanvas.Children[i];
                line.Stroke = colorsList[i];
            }
        }


        /// <summary>
        /// Нарисовать фрактал "Ковёр Серпинского" (Третий).
        /// </summary>
        /// <param name="mainColor">Основной цвет ковра (фон)</param>
        public void DrawSerpinskyCarpetFractal(Color mainColor)
        {
            s_carpetColor = mainColor;
            GetColors(NumberOfSteps, colorStart, colorEnd);
            FractalNum = 3;
            List<MyRectangle> myRectangles = new();
            myRectangles.Add(makeRectangle(MainCanvas.RenderSize.Height - 50, MainCanvas.RenderSize.Height - 50,
                MainCanvas.RenderSize.Width / 2.0 - (MainCanvas.RenderSize.Height - 50) / 2.0, 25));
            Recursion(NumberOfSteps, MainCanvas.RenderSize.Height - 50, MainCanvas.RenderSize.Height - 50,
                MainCanvas.RenderSize.Width / 2.0 - (MainCanvas.RenderSize.Height - 50) / 2.0, 25);

            // Рекурсивная функция для отрисовки фрактала "Ковёр Серпинского".
            void Recursion(int depth, double rectangleWidth, double rectangleHeight, double xLeft, double yTop)
            {
                if (depth == 0)
                {
                    return;
                }
                else
                {
                    double x0 = xLeft, y0 = yTop, width = rectangleWidth / 3.0, height = rectangleHeight / 3.0;
                    double x1 = x0 + width, y1 = y0 + height, x2 = x0 + width * 2.0, y2 = y0 + height * 2.0;
                    // Рекурсивно вызываем отрисовку восьми квадратов и Рисуем центральный квадрат, закрашивая его.
                    Recursion(depth - 1, width, height, x0, y0);
                    Recursion(depth - 1, width, height, x1, y0);
                    Recursion(depth - 1, width, height, x2, y0);
                    Recursion(depth - 1, width, height, x0, y1);
                    Recursion(depth - 1, width, height, x2, y1);
                    Recursion(depth - 1, width, height, x0, y2);
                    Recursion(depth - 1, width, height, x1, y2);
                    Recursion(depth - 1, width, height, x2, y2);
                    RectangleGradient(ref myRectangles, depth, x1, y1, width, height);
                }
            }
            for (int i = 0; i < myRectangles.Count; i++)
            {
                MainCanvas.Children.Add(myRectangles[i].rectangle);
                Canvas.SetTop(myRectangles[i].rectangle, myRectangles[i].yPos);
                Canvas.SetLeft(myRectangles[i].rectangle, myRectangles[i].xPos);
            }
        }


        /// <summary>
        /// Метод для создания градиента на Ковре Серпинского.
        /// </summary>
        /// <param name="list">Лист с квадратами.</param>
        /// <param name="depth">Текущая шлубина рекурсии.</param>
        /// <param name="x">Положения относительно абсцисс.</param>
        /// <param name="y">Положения относительно ординат.</param>
        /// <param name="width">Ширина квадрата.</param>
        /// <param name="height">Высота квадрата.</param>
        private void RectangleGradient(ref List<MyRectangle> list, int depth, double x, double y, double width, double height)
        {
            int index = depth > 0 ? depth - 1 : 0;

            MyRectangle rectangleNew = makeRectangle(width, height, x, y);

            rectangleNew.rectangle.Stroke = colorsList[index];
            rectangleNew.rectangle.Fill = colorsList[index];

            list.Add(rectangleNew);
        }


        /// <summary>
        /// Нарисовать фрактал "Треугольник Серпинского" (Четвёртый).
        /// </summary>
        public void DrawSerpinskyTriangleFractal()
        {
            FractalNum = 4;
            double y1 = MainCanvas.RenderSize.Height / 30,
                y2 = MainCanvas.RenderSize.Height - y1,
                x1 = MainCanvas.RenderSize.Width / 2 - (MainCanvas.RenderSize.Height - y1 * 2) * Math.Tan(Math.PI / 6),
                x2 = MainCanvas.RenderSize.Width / 2 + (MainCanvas.RenderSize.Height - y1 * 2) * Math.Tan(Math.PI / 6);

            List<Line> lines = new List<Line>();
            Recursion(NumberOfSteps, MainCanvas.RenderSize.Width / 2, y1, x1, y2, x2, y2);

            // Хвостовая рекурсивная функция для отрисовки фрактала "Треугольник Серпинского".
            void Recursion(int depth, double topPointX, double topPointY,
                double leftPointX, double leftPointY, double rightPointX, double rightPointY)
            {
                if (depth == 0)
                {
                    // Рисуем три стороны по точкам.
                    lines.Add(makeLine(
                        new double[] { topPointX, topPointY, leftPointX, leftPointY, 10 / (NumberOfSteps + 1) }));
                    lines.Add(makeLine(
                        new double[] { topPointX, topPointY, rightPointX, rightPointY, 10 / (NumberOfSteps + 1) }));
                    lines.Add(makeLine(
                        new double[] { rightPointX, rightPointY, leftPointX, leftPointY, 10 / (NumberOfSteps + 1) }));
                }
                else
                {
                    // Вычисляем координаты середин сторон для построения треугольников.
                    double leftMidX = (topPointX + leftPointX) / 2.0, leftMidY = (topPointY + leftPointY) / 2.0,
                        rightMidX = (topPointX + rightPointX) / 2.0, rightMidY = (topPointY + rightPointY) / 2.0,
                        bottomMidX = (leftPointX + rightPointX) / 2.0, bottomMidY = (leftPointY + rightPointY) / 2.0;

                    Recursion(depth - 1, topPointX, topPointY, leftMidX, leftMidY, rightMidX, rightMidY);
                    Recursion(depth - 1, leftMidX, leftMidY, leftPointX, leftPointY, bottomMidX, bottomMidY);
                    Recursion(depth - 1, rightMidX, rightMidY, bottomMidX, bottomMidY, rightPointX, rightPointY);
                }
            }
            DisplayAndPaint(lines);
        }


        /// <summary>
        /// Нарисовать фрактал "Множество Кантора" (Пятый).
        /// </summary>
        public void DrawCantorArrayFractal()
        {
            GetColors(NumberOfSteps, colorStart, colorEnd);

            FractalNum = 5;

            double yStep = CoefficientOfLength;

            int depth = NumberOfSteps;

            Recursion(depth, 50, MainCanvas.RenderSize.Width - 50, 50 + yStep);
            // Рекурсивная функция для отрисовки фрактала "Множество Кантора".
            void Recursion(int depth, double xStart, double xEnd, double yCur)
            {
                // *--*--*--*
                // xS-x1-x2-xE

                Line line = makeLine(new double[] { xStart, yCur, xEnd, yCur, 8 });

                int index = depth > 0 ? depth - 1 : 0;

                line.Stroke = colorsList[index];

                MainCanvas.Children.Add(line);

                double x1 = xStart + (xEnd - xStart) / 3, x2 = xStart + (xEnd - xStart) / 3 * 2;

                if (depth == 0)
                {
                    return;
                }

                Recursion(depth - 1, xStart, x1, yCur + yStep);
                Recursion(depth - 1, x2, xEnd, yCur + yStep);
            }
        }


        /// <summary>
        /// Делегат для создания Прямой.
        /// </summary>
        private readonly Func<double[], Line> makeLine = delegate (double[] sizese)
        {
            Line line = new();

            line.Stroke = Brushes.Black;

            line.X1 = sizese[0];
            line.Y1 = sizese[1];

            line.X2 = sizese[2];
            line.Y2 = sizese[3];

            line.StrokeThickness = sizese[4];

            return line;

        };


        /// <summary>
        /// Делегат для создания Квадрата с координатами его расположения на canvas'е.
        /// </summary>
        private readonly Func<double, double, double, double, MyRectangle>
        makeRectangle = delegate (double width, double height, double xPos, double yPos)
        {
            Rectangle rectangle = new();

            SolidColorBrush colorBrush = new();

            colorBrush.Color = s_carpetColor;
            rectangle.Stroke = colorBrush;
            rectangle.Fill = colorBrush;

            rectangle.Width = width;
            rectangle.Height = height;

            MyRectangle myRectangle = new(rectangle, xPos, yPos);

            return myRectangle;

        };


        /// <summary>
        /// Событие на изменение размера окна.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs? e = null)
        {
            MainCanvas.Width = scroll.RenderSize.Width;
            MainCanvas.Height = scroll.RenderSize.Height;
            if (!redrawFractalReqested)
            {
                redrawFractalReqested = true;
                // "Безопасно" запускаем перерисовку в новом поток.
                Dispatcher.BeginInvoke(new Action(ReDrawFractal), DispatcherPriority.ApplicationIdle);
            }
        }


        /// <summary>
        /// Метод для перерисовки текущего фрактала (из-за изменения окна или слайдера)
        /// </summary>
        private void ReDrawFractal()
        {
            redrawFractalReqested = false;

            if (flagOnStart[2])
            {
                flagOnStart[2] = false;
                return;
            }

            MainCanvas.Children.Clear();

            switch (FractalNum)
            {
                case 1:
                    DrawPifagorFractal();
                    break;
                case 2:
                    DrawKohaFractal();
                    break;
                case 3:
                    DrawSerpinskyCarpetFractal(s_carpetColor);
                    break;
                case 4:
                    DrawSerpinskyTriangleFractal();
                    break;
                case 5:
                    DrawCantorArrayFractal();
                    break;
            }

        }


        /// <summary>
        /// Реакция на изменение глубины фрактала.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Так надо) Просто для оптимизации.
            if (flagOnStart[0])
            {
                flagOnStart[0] = false;
                return;
            }
            if (flagOnStart[3])
            {
                flagOnStart[3] = false;
                return;
            }

            LabelForSliderDepthValue.Content = (int)SliderForDepth.Value;
            NumberOfSteps = (int)SliderForDepth.Value;

            if (!redrawFractalReqested)
            {
                redrawFractalReqested = true;
                // "Безопасно" запускаем перерисовку в новом поток.
                Dispatcher.BeginInvoke(new Action(ReDrawFractal), DispatcherPriority.ApplicationIdle);
            }
        }


        /// <summary>
        /// Сохранение фотографии экрана.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RenderTargetBitmap renderTargetBitmap = new((int)MainCanvas.RenderSize.Width,
                    (int)MainCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);

                renderTargetBitmap.Render(MainCanvas);
                PngBitmapEncoder pngImage = new();
                pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                SaveFileDialog saveFileDialog = new();
                saveFileDialog.Filter = "PNG file (*.png)|*.png|JPG file (*.JPG)|*.JPG";

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (Stream fileStream = File.Create(saveFileDialog.FileName))
                    {
                        pngImage.Save(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} Произошла ошибка при сохранении скриншота!", "Ошибка!");
            }
        }


        /// <summary>
        /// Событие - Закрытие окна, чтобы следить за их количеством.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            --MainWindow.s_NumberOfOpenWindows;
        }

    }


    /// <summary>
    /// Дополненный класс с квадратами и полями его координат расположения на canvas.
    /// </summary>
    internal struct MyRectangle
    {
        /// <summary>
        /// Квадрат
        /// </summary>
        public Rectangle rectangle;

        /// <summary>
        /// Позиция по оси абсцисс.
        /// </summary>
        public double xPos;

        /// <summary>
        /// Позиция по оси ординат.
        /// </summary>
        public double yPos;


        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="rectangle">Квадрат.</param>
        /// <param name="xPos">Позиция по оси абсцисс.</param>
        /// <param name="yPos">Позиция по оси ординат.</param>
        public MyRectangle(Rectangle rectangle, double xPos, double yPos)
        {
            this.rectangle = rectangle;

            this.xPos = xPos;
            this.yPos = yPos;
        }
    }
}
