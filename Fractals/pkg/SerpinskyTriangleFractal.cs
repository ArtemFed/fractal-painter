namespace Fractals.pkg
{
    /// <summary>
    /// Класс для Треугольника Серпинского.
    /// </summary>
    public class SerpinskyTriangleFractal : Fractal
    {

        /// <summary>
        /// Конструктор Треугольника Серпинского.
        /// </summary>
        /// <param name="steps"></param>
        public SerpinskyTriangleFractal(int steps) : base(steps)
        {

        }


        /// <summary>
        /// Метод для отрисовки Треугольника Серпинского.
        /// </summary>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        public override void DrawFractal(System.Windows.Media.Color startColor, System.Windows.Media.Color endColor)
        {
            // Создаём новое окно со своими параметрами и открываем его.
            ShowWindow(out WindowFractal window, 8, startColor, endColor);
            window.Title = "Треугольник Серпинского";

            // Запускаем на новом окне метод по отрисовке фрактала.
            window.DrawSerpinskyTriangleFractal();
        }
    }

}
