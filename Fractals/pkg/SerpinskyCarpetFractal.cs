namespace Fractals.pkg
{
    /// <summary>
    /// Класс для Ковра Серпинского.
    /// </summary>
    public class SerpinskyCarpetFractal : Fractal
    {

        /// <summary>
        /// Цвет фона ковра.
        /// </summary>
        private System.Windows.Media.Color mainColor;


        /// <summary>
        /// Конструктор Ковра Серпинского.
        /// </summary>
        /// <param name="steps">Глубина рекурсии.</param>
        /// <param name="color">Цвет фона ковра.</param>
        public SerpinskyCarpetFractal(int steps, System.Windows.Media.Color color) : base(steps)
        {
            mainColor = color;
        }

        /// <summary>
        /// Метод для отрисовки Ковра Серпинского.
        /// </summary>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        public override void DrawFractal(System.Windows.Media.Color startColor, System.Windows.Media.Color endColor)
        {
            // Создаём новое окно со своими параметрами и открываем его.
            ShowWindow(out WindowFractal window, 5, startColor, endColor);
            window.Title = "Ковёр Серпинского";

            // Запускаем на новом окне метод по отрисовке фрактала.
            window.DrawSerpinskyCarpetFractal(mainColor);
        }
    }
}
