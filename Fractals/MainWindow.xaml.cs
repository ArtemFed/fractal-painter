using Fractals.pkg;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fractals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
        /// Угол наклона левой ветви для Пиф. фрактала.
        /// </summary>
        private double fractPifFirstAngle = 45;

        /// <summary>
        /// Угол наклона левой ветви для Пиф. фрактала.
        /// </summary>
        private double fractPifSecondAngle = 45;

        /// <summary>
        /// Текущий фрактал.
        /// </summary>
        private Fractal? CurrentFractal { get; set; }

        /// <summary>
        /// Количество открытых окон.
        /// </summary>
        public static int s_NumberOfOpenWindows = 0;

        /// <summary>
        /// Расстояние между отрезками для Множества Кантора
        /// </summary>
        private int lengthBetwLines = 20;


        /// <summary>
        /// Конструктор основного окна.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ButtonFirstFractal.IsChecked = true;

            StartColorPicker.SelectedBrush = Brushes.Black;
            EndColorPicker.SelectedBrush = Brushes.Black;

            StartColorPicker.SelectedColor = Brushes.Black.Color;
            EndColorPicker.SelectedColor = Brushes.Black.Color;

            CarpetColorPicker.SelectedBrush = Brushes.Orange;
            CarpetColorPicker.SelectedColor = Brushes.Orange.Color;
        }


        /// <summary>
        /// Изменение слайдера со значением глубины рекурсии.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void SliderOfValueOfFractalRec_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NumberOfSteps = (int)SliderOfValueOfFractalRec.Value;
            ValueOfFirstSlider.Content = NumberOfSteps;
        }


        /// <summary>
        /// Изменение слайдера со значением коэффициента изменения длины линии.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void SliderOfValueOfFractalLen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CoefficientOfLength = Math.Round(SliderOfValueOfFractalLen.Value / 100, 2);
            ValueOfSecondSlider.Content = CoefficientOfLength;
        }


        /// <summary>
        /// Изменение слайдера со значением расстояния между прямыми в пятомфрактале.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lengthBetwLines = (int)SliderForLenBetween.Value;
            LabelForLenBetween.Content = (int)SliderForLenBetween.Value;
        }


        /// <summary>
        /// Нажатие главной кнопки - отрисовки фрактала в новом окне.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonFirstFractal.IsChecked == true)
            {
                CurrentFractal = new PifagorFractal(
                    NumberOfSteps, CoefficientOfLength, fractPifFirstAngle, fractPifSecondAngle);
            }
            else if (ButtonSecondFractal.IsChecked == true)
            {
                CurrentFractal = new KohaFractal(NumberOfSteps);
            }
            else if (ButtonThirdFractal.IsChecked == true)
            {
                CurrentFractal = new SerpinskyCarpetFractal(NumberOfSteps, CarpetColorPicker.SelectedColor);
            }
            else if (ButtonFourthFractal.IsChecked == true)
            {
                CurrentFractal = new SerpinskyTriangleFractal(NumberOfSteps);
            }
            else if (ButtonFifthFractal.IsChecked == true)
            {
                CurrentFractal = new CantorArrayFractal(NumberOfSteps, lengthBetwLines);
            }

            if (CurrentFractal != null && ++s_NumberOfOpenWindows <= 5)
            {
                CurrentFractal.DrawFractal(StartColorPicker.SelectedColor, EndColorPicker.SelectedColor);
            }
            else
            {
                --s_NumberOfOpenWindows;
                MessageBox.Show("Вы открыли максимальное количество окон с фракталами!", "ReadMe читать нужно было!");
            }
        }


        /// <summary>
        /// Кнопка для выбора фрактала Пифагора (первого).
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void ButtonFirstFractal_Checked(object sender, RoutedEventArgs e)
        {

            GroupBox.Visibility = Visibility.Visible;

            CanvasThirdRow.Visibility = Visibility.Visible;
            GroupBox2.Visibility = Visibility.Hidden;
            CanvasForColorForSCarpet.Visibility = Visibility.Hidden;

            SliderOfValueOfFractalRec.Maximum = 13;
            SliderOfValueOfFractalRec.Value = 10;
        }


        /// <summary>
        /// Кнопка для выбора фрактала "Кривая Коха" (второго).
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void ButtonSecondFractal_Checked(object sender, RoutedEventArgs e)
        {
            GroupBox.Visibility = Visibility.Hidden;
            CanvasThirdRow.Visibility = Visibility.Hidden;
            GroupBox2.Visibility = Visibility.Hidden;
            CanvasForColorForSCarpet.Visibility = Visibility.Hidden;

            SliderOfValueOfFractalRec.Maximum = 7;
            SliderOfValueOfFractalRec.Value = 4;
        }


        /// <summary>
        /// Кнопка для выбора фрактала "Ковёр Серпинского" (третьего).
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void ButtonThirdFractal_Checked(object sender, RoutedEventArgs e)
        {
            GroupBox.Visibility = Visibility.Hidden;
            CanvasThirdRow.Visibility = Visibility.Hidden;
            GroupBox2.Visibility = Visibility.Hidden;

            CanvasForColorForSCarpet.Visibility = Visibility.Visible;

            SliderOfValueOfFractalRec.Maximum = 5;
            SliderOfValueOfFractalRec.Value = 3;
        }


        /// <summary>
        /// Кнопка для выбора фрактала "Треугольник Серпинского" (четвёртого).
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void ButtonFourthFractal_Checked(object sender, RoutedEventArgs e)
        {
            GroupBox.Visibility = Visibility.Hidden;
            CanvasThirdRow.Visibility = Visibility.Hidden;
            GroupBox2.Visibility = Visibility.Hidden;
            CanvasForColorForSCarpet.Visibility = Visibility.Hidden;

            SliderOfValueOfFractalRec.Maximum = 8;
            SliderOfValueOfFractalRec.Value = 5;
        }


        /// <summary>
        /// Кнопка для выбора фрактала "Множество Кантора" (пятого).
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void ButtonFifthFractal_Checked(object sender, RoutedEventArgs e)
        {
            GroupBox.Visibility = Visibility.Hidden;
            CanvasThirdRow.Visibility = Visibility.Hidden;
            CanvasForColorForSCarpet.Visibility = Visibility.Hidden;

            GroupBox2.Visibility = Visibility.Visible;

            SliderOfValueOfFractalRec.Maximum = 9;
            SliderOfValueOfFractalRec.Value = 5;
        }


        /// <summary>
        /// Изменение угла наклона для ветвей во фрактале Пифагора.
        /// </summary>
        /// <param name="sender">Издатель.</param>
        /// <param name="e">Информация о событии.</param>
        private void ButtonChangeValue_Click(object sender, RoutedEventArgs e)
        {
            string oldValue;
            if (sender.GetHashCode() == ButLeftPlus.GetHashCode())
            {
                oldValue = AngleFirst.Text.ToString();
                if (oldValue != "180")
                {
                    AngleFirst.Text = (int.Parse(oldValue) + 15).ToString();
                    fractPifFirstAngle = double.Parse(oldValue) + 15;
                }
            }
            else if (sender.GetHashCode() == ButLeftMinus.GetHashCode())
            {
                oldValue = AngleFirst.Text.ToString();
                if (oldValue != "0")
                {
                    AngleFirst.Text = (int.Parse(oldValue) - 15).ToString();
                    fractPifFirstAngle = double.Parse(oldValue) - 15;
                }
            }
            else if (sender.GetHashCode() == ButRightPlus.GetHashCode())
            {
                oldValue = AngleSecond.Text.ToString();
                if (oldValue != "180")
                {
                    AngleSecond.Text = (int.Parse(oldValue) + 15).ToString();
                    fractPifSecondAngle = double.Parse(oldValue) + 15;
                }
            }
            else if (sender.GetHashCode() == ButRightMinus.GetHashCode())
            {
                oldValue = AngleSecond.Text.ToString();
                if (oldValue != "0")
                {
                    AngleSecond.Text = (int.Parse(oldValue) - 15).ToString();
                    fractPifSecondAngle = double.Parse(oldValue) - 15;
                }
            }
        }

    }
}
