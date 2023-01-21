namespace Fractals.pkg
{
    /// <summary>
    /// Абстрактный класс, общий для всех фракталов.
    /// </summary>
    public abstract class Fractal
    {

        /// <summary>
        /// Глубина рекурсии. 
        /// </summary>
        private protected int numberOfSteps { get; private set; }


        /// <summary>
        /// Главный конструктор.
        /// </summary>
        /// <param name="steps">Глубина рекурсии.</param>
        public Fractal(int steps)
        {
            numberOfSteps = steps;
        }


        /// <summary>
        /// Отрисовка фрактала.
        /// </summary>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        public abstract void DrawFractal(System.Windows.Media.Color startColor, System.Windows.Media.Color endColor);


        /// <summary>
        /// Открыть дополнительное окно для рисования фрактала.
        /// </summary>
        /// <param name="window">Окно.</param>
        /// <param name="maxDepth">Максимальная глубина.</param>
        /// <param name="startColor">Начальный цвет градиента.</param>
        /// <param name="endColor">Конечный цвет градиента.</param>
        private protected void ShowWindow(out WindowFractal window, int maxDepth,
            System.Windows.Media.Color startColor, System.Windows.Media.Color endColor)
        {
            window = new WindowFractal(numberOfSteps, maxDepth, startColor, endColor);
            window.Show();
        }

    }
}
