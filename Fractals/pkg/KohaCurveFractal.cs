namespace Fractals.pkg
{
    /// <summary>
    /// Класс для "Кривой Коха".
    /// </summary>
    public class KohaFractal : Fractal
    {

        /// <summary>
        /// Конструктор для Кривой Коха.
        /// </summary>
        /// <param name="steps">Глубина рекурсии.</param>
        public KohaFractal(int steps) : base(steps)
        {

        }


        /// <summary>
        /// Метод для отрисовки Кривой Коха.
        /// </summary>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        public override void DrawFractal(System.Windows.Media.Color startColor, System.Windows.Media.Color endColor)
        {
            // Создаём новое окно со своими параметрами и открываем его.
            ShowWindow(out WindowFractal window, 7, startColor, endColor);
            window.Title = "Кривая Коха";

            // Запускаем на новом окне метод по отрисовке фрактала.
            window.DrawKohaFractal();
        }
    }
}
