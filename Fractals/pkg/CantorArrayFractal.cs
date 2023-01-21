namespace Fractals.pkg
{
    /// <summary>
    /// Класс для Множества Кантора.
    /// </summary>
    public class CantorArrayFractal : Fractal
    {
        /// <summary>
        /// Длина между линиями из Множества Кантора.
        /// </summary>
        private readonly int lengthBetwLines;


        /// <summary>
        /// Конструктор Множества Кантора.
        /// </summary>
        /// <param name="steps">Глубина рекурсии.</param>
        /// <param name="lengthBetwLines">Длина между линиями из Множества Кантора.</param>
        public CantorArrayFractal(int steps, int lengthBetwLines) : base(steps)
        {
            this.lengthBetwLines = lengthBetwLines;
        }


        /// <summary>
        /// Метод для отрисовки Множества Кантора.
        /// </summary>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        public override void DrawFractal(System.Windows.Media.Color startColor, System.Windows.Media.Color endColor)
        {
            // Создаём новое окно со своими параметрами и открываем его.
            var window = new WindowFractal(numberOfSteps, 9, startColor, endColor, lengthBetwLines);
            window.Title = "Множество Кантора";
            window.Show();

            // Запускаем на новом окне метод по отрисовке фрактала.
            window.DrawCantorArrayFractal();
        }
    }
}
