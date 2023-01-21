namespace Fractals.pkg
{
    /// <summary>
    /// Класс Фрактального дерева.
    /// </summary>
    public class PifagorFractal : Fractal
    {

        /// <summary>
        /// Коэффициент изменения длины.
        /// </summary>
        private readonly double coefficientOfLength;

        /// <summary>
        /// Угол для левой ветви.
        /// </summary>
        private readonly double fractPifFirstAngle;

        /// <summary>
        /// Угол для првой ветви.
        /// </summary>
        private readonly double fractPifSecondAngle;


        /// <summary>
        /// Конструктор Фрактального дерева.
        /// </summary>
        /// <param name="steps">Глубина рекурсии.</param>
        /// <param name="lengthCoeff">Коэффициент изменения длины.</param>
        /// <param name="firstAngle">Угол для левой ветви.</param>
        /// <param name="secondAngle">Угол для првой ветви.</param>
        public PifagorFractal(int steps, double lengthCoeff, double firstAngle, double secondAngle) : base(steps)
        {
            coefficientOfLength = lengthCoeff;

            fractPifFirstAngle = firstAngle;

            fractPifSecondAngle = secondAngle;
        }


        /// <summary>
        /// Метод для отрисовки Фрактального дерева.
        /// </summary>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        public override void DrawFractal(System.Windows.Media.Color startColor, System.Windows.Media.Color endColor)
        {
            // Создаём новое окно со своими параметрами и открываем его.
            var window = new WindowFractal(
                numberOfSteps, 13, startColor, endColor, coefficientOfLength, fractPifFirstAngle, fractPifSecondAngle);
            window.Show();
            window.Title = "Фрактальное Дерево";

            // Запускаем на новом окне метод по отрисовке фрактала.
            window.DrawPifagorFractal();
        }
    }
}
