using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShootCircle.Windows
{
    /// <summary>
    /// Логика взаимодействия для FirstLvL.xaml
    /// </summary>
    public partial class FirstLvL : Window
    {
        private const double MoveStep = 5.0;
        private double angle = 0;
        private double angle1 = 0;
        private DispatcherTimer moveTimer;

        public FirstLvL()
        {
            InitializeComponent();
            this.PreviewKeyDown += FirstLvL_PreviewKeyDown;

            moveTimer = new DispatcherTimer();
            moveTimer.Interval = TimeSpan.FromMilliseconds(10); // Задаем интервал таймера
            moveTimer.Tick += MoveTimer_Tick; // Обработчик события тика таймера
            moveTimer.Start(); // Запускаем таймер

            PlayWavFilesAsync();
        }

        public async Task PlayWavFilesAsync()
        {
            string folderPath = "Music";
            string[] files = Directory.GetFiles(folderPath, "*.wav");

            while (true)
            {
                foreach (string file in files)
                {
                    using (SoundPlayer player = new SoundPlayer(file))
                    {
                        await Task.Run(() => player.PlaySync());
                    }
                }
            }
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            bool isWPressed1 = Keyboard.IsKeyDown(Key.Up);
            bool isAPressed1 = Keyboard.IsKeyDown(Key.Left);
            bool isSPressed1 = Keyboard.IsKeyDown(Key.Down);
            bool isDPressed1 = Keyboard.IsKeyDown(Key.Right);

            double currentTop1 = Canvas.GetTop(ellipse2);
            double currentLeft1 = Canvas.GetLeft(ellipse2);
            double ellipseWidth1 = ellipse2.ActualWidth;
            double ellipseHeight1 = ellipse2.ActualHeight;

            // Перемещение ellipse1 в зависимости от нажатых клавиш
            if (isWPressed1 && currentTop1 > 0)
            {
                Canvas.SetTop(ellipse2, currentTop1 - MoveStep);
            }
            else if (isSPressed1 && (currentTop1 + ellipseHeight1) < canvas.ActualHeight)
            {
                Canvas.SetTop(ellipse2, currentTop1 + MoveStep);
            }

            if (isAPressed1 && currentLeft1 > 0)
            {
                Canvas.SetLeft(ellipse2, currentLeft1 - MoveStep);
            }
            else if (isDPressed1 && (currentLeft1 + ellipseWidth1) < canvas.ActualWidth)
            {
                Canvas.SetLeft(ellipse2, currentLeft1 + MoveStep);
            }

            // Проверка столкновения с каждым Border на Canvas
            foreach (var child in canvas.Children)
            {
                if (child is Border border)
                {
                    // Получаем координаты и размеры текущего Border
                    double borderLeft = Canvas.GetLeft(border);
                    double borderTop = Canvas.GetTop(border);
                    double borderWidth = border.ActualWidth;
                    double borderHeight = border.ActualHeight;

                    // Проверяем наложение областей между ellipse1 и Border
                    if (currentLeft1 < (borderLeft + borderWidth) &&
                        (currentLeft1 + ellipseWidth1) > borderLeft &&
                        currentTop1 < (borderTop + borderHeight) &&
                        (currentTop1 + ellipseHeight1) > borderTop)
                    {
                        // Определяем сторону столкновения
                        double horizontalIntersection = Math.Min(currentLeft1 + ellipseWidth1 - borderLeft, borderLeft + borderWidth - currentLeft1);
                        double verticalIntersection = Math.Min(currentTop1 + ellipseHeight1 - borderTop, borderTop + borderHeight - currentTop1);

                        if (horizontalIntersection < verticalIntersection)
                        {
                            // Столкновение по горизонтали
                            if (currentLeft1 + ellipseWidth1 / 2 < borderLeft + borderWidth / 2)
                            {
                                // Слева от Border
                                Canvas.SetLeft(ellipse2, currentLeft1 - MoveStep); // Отталкиваем влево
                            }
                            else
                            {
                                // Справа от Border
                                Canvas.SetLeft(ellipse2, currentLeft1 + MoveStep); // Отталкиваем вправо
                            }
                        }
                        else
                        {
                            // Столкновение по вертикали
                            if (currentTop1 + ellipseHeight1 / 2 < borderTop + borderHeight / 2)
                            {
                                // Сверху от Border
                                Canvas.SetTop(ellipse2, currentTop1 - MoveStep); // Отталкиваем вверх
                            }
                            else
                            {
                                // Снизу от Border
                                Canvas.SetTop(ellipse2, currentTop1 + MoveStep); // Отталкиваем вниз
                            }
                        }
                    }
                }
            }

            bool isWPressed = Keyboard.IsKeyDown(Key.W);
            bool isAPressed = Keyboard.IsKeyDown(Key.A);
            bool isSPressed = Keyboard.IsKeyDown(Key.S);
            bool isDPressed = Keyboard.IsKeyDown(Key.D);

            double currentTop = Canvas.GetTop(ellipse1);
            double currentLeft = Canvas.GetLeft(ellipse1);
            double ellipseWidth = ellipse1.ActualWidth;
            double ellipseHeight = ellipse1.ActualHeight;

            // Перемещение ellipse1 в зависимости от нажатых клавиш
            if (isWPressed && currentTop > 0)
            {
                Canvas.SetTop(ellipse1, currentTop - MoveStep);
            }
            else if (isSPressed && (currentTop + ellipseHeight) < canvas.ActualHeight)
            {
                Canvas.SetTop(ellipse1, currentTop + MoveStep);
            }

            if (isAPressed && currentLeft > 0)
            {
                Canvas.SetLeft(ellipse1, currentLeft - MoveStep);
            }
            else if (isDPressed && (currentLeft + ellipseWidth) < canvas.ActualWidth)
            {
                Canvas.SetLeft(ellipse1, currentLeft + MoveStep);
            }

            // Проверка столкновения с каждым Border на Canvas
            foreach (var child in canvas.Children)
            {
                if (child is Border border)
                {
                    // Получаем координаты и размеры текущего Border
                    double borderLeft = Canvas.GetLeft(border);
                    double borderTop = Canvas.GetTop(border);
                    double borderWidth = border.ActualWidth;
                    double borderHeight = border.ActualHeight;

                    // Проверяем наложение областей между ellipse1 и Border
                    if (currentLeft < (borderLeft + borderWidth) &&
                        (currentLeft + ellipseWidth) > borderLeft &&
                        currentTop < (borderTop + borderHeight) &&
                        (currentTop + ellipseHeight) > borderTop)
                    {
                        // Определяем сторону столкновения
                        double horizontalIntersection = Math.Min(currentLeft + ellipseWidth - borderLeft, borderLeft + borderWidth - currentLeft);
                        double verticalIntersection = Math.Min(currentTop + ellipseHeight - borderTop, borderTop + borderHeight - currentTop);

                        if (horizontalIntersection < verticalIntersection)
                        {
                            // Столкновение по горизонтали
                            if (currentLeft + ellipseWidth / 2 < borderLeft + borderWidth / 2)
                            {
                                // Слева от Border
                                Canvas.SetLeft(ellipse1, currentLeft - MoveStep); // Отталкиваем влево
                            }
                            else
                            {
                                // Справа от Border
                                Canvas.SetLeft(ellipse1, currentLeft + MoveStep); // Отталкиваем вправо
                            }
                        }
                        else
                        {
                            // Столкновение по вертикали
                            if (currentTop + ellipseHeight / 2 < borderTop + borderHeight / 2)
                            {
                                // Сверху от Border
                                Canvas.SetTop(ellipse1, currentTop - MoveStep); // Отталкиваем вверх
                            }
                            else
                            {
                                // Снизу от Border
                                Canvas.SetTop(ellipse1, currentTop + MoveStep); // Отталкиваем вниз
                            }
                        }
                    }
                }
            }

            // Расчет позиции для greenEllipse1 вокруг ellipse2
            double radius1 = 30; // Радиус окружности, на которой двигается greenEllipse
            double centerX1 = Canvas.GetLeft(ellipse2) + ellipse2.Width / 2; // Центр ellipse1 по X
            double centerY1 = Canvas.GetTop(ellipse2) + ellipse2.Height / 2; // Центр ellipse1 по Y

            // Вычисляем новую позицию для greenEllipse
            double greenEllipseX1 = centerX1 + radius1 * Math.Cos(angle1);
            double greenEllipseY1 = centerY1 + radius1 * Math.Sin(angle1);

            // Устанавливаем новые координаты для greenEllipse
            Canvas.SetLeft(greenEllipse1, greenEllipseX1 - greenEllipse.Width / 2);
            Canvas.SetTop(greenEllipse1, greenEllipseY1 - greenEllipse.Height / 2);

            // Увеличиваем угол для следующего тика таймера (для движения по часовой стрелке)
            double angularSpeed1 = 0.05; // Скорость вращения (угловая скорость)
            angle1 += angularSpeed1;

            // Расчет позиции для greenEllipse вокруг ellipse1
            double radius = 30; // Радиус окружности, на которой двигается greenEllipse
            double centerX = Canvas.GetLeft(ellipse1) + ellipse1.Width / 2; // Центр ellipse1 по X
            double centerY = Canvas.GetTop(ellipse1) + ellipse1.Height / 2; // Центр ellipse1 по Y

            // Вычисляем новую позицию для greenEllipse
            double greenEllipseX = centerX + radius * Math.Cos(angle);
            double greenEllipseY = centerY + radius * Math.Sin(angle);

            // Устанавливаем новые координаты для greenEllipse
            Canvas.SetLeft(greenEllipse, greenEllipseX - greenEllipse.Width / 2);
            Canvas.SetTop(greenEllipse, greenEllipseY - greenEllipse.Height / 2);

            // Увеличиваем угол для следующего тика таймера (для движения по часовой стрелке)
            double angularSpeed = 0.05; // Скорость вращения (угловая скорость)
            angle += angularSpeed;
        }

        private bool canShoot = true; // Флаг, показывающий, можно ли выпускать новый шарик
        private DateTime lastShotTime; // Время последнего выстрела
        private bool canShoot1 = true; // Флаг, показывающий, можно ли выпускать новый шарик
        private DateTime lastShotTime1; // Время последнего выстрела
        private bool gameClosed = false;
        private bool delBall = false;
        private bool delBall1 = false;

        private void FirstLvL_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
                case Key.RightShift:
                    if (canShoot1 && (DateTime.Now - lastShotTime1).TotalSeconds >= 0.5)
                    {
                        // Создание и настройка нового черного шарика
                        Ellipse blackBall = new Ellipse();
                        blackBall.Width = 20;
                        blackBall.Height = 20;
                        blackBall.Fill = Brushes.Black;
                        delBall1 = false;

                        // Установка начальной позиции рядом с greenEllipse
                        double centerX = Canvas.GetLeft(greenEllipse1) + greenEllipse1.Width / 2;
                        double centerY = Canvas.GetTop(greenEllipse1) + greenEllipse1.Height / 2;
                        Canvas.SetLeft(blackBall, centerX);
                        Canvas.SetTop(blackBall, centerY);
                        canvas.Children.Add(blackBall);

                        // Начальное направление движения
                        double moveSpeed = 16.0; // Скорость движения черного шарика
                        double angleRadians = angle; // Используем текущий угол greenEllipse для направления движения
                        double dx = moveSpeed * Math.Cos(angleRadians);
                        double dy = moveSpeed * Math.Sin(angleRadians);

                        // Таймер для перемещения черного шарика
                        DispatcherTimer bulletTimer = new DispatcherTimer();
                        bulletTimer.Interval = TimeSpan.FromMilliseconds(10);

                        DateTime bulletStartTime = DateTime.Now;

                        bulletTimer.Tick += (s, args) =>
                        {
                            // Перемещение шарика
                            Canvas.SetLeft(blackBall, Canvas.GetLeft(blackBall) + dx);
                            Canvas.SetTop(blackBall, Canvas.GetTop(blackBall) + dy);

                            // Проверка столкновений с ellipse1
                            double eballLeft = Canvas.GetLeft(blackBall);
                            double eballTop = Canvas.GetTop(blackBall);
                            double eballWidth = blackBall.Width;
                            double eballHeight = blackBall.Height;

                            double ellipseLeft = Canvas.GetLeft(ellipse1);
                            double ellipseTop = Canvas.GetTop(ellipse1);
                            double ellipseWidth = ellipse1.Width;
                            double ellipseHeight = ellipse1.Height;

                            // Проверка столкновения шарика с ellipse1
                            if (eballLeft < (ellipseLeft + ellipseWidth) &&
                                (eballLeft + eballWidth) > ellipseLeft &&
                                eballTop < (ellipseTop + ellipseHeight) &&
                                (eballTop + eballHeight) > ellipseTop)
                            {
                                if (!delBall1)
                                {
                                    // Если шарик столкнулся с ellipse1 и игра не закрыта
                                    if (!gameClosed && (DateTime.Now - bulletStartTime).TotalMilliseconds >= 100)
                                    {
                                        // Закрываем игру и устанавливаем флаг gameClosed в true
                                        gameClosed = true;
                                        canvas.Children.Remove(blackBall);
                                        MessageBox.Show("Синий проиграл!");
                                        MainWindow mainWindow1 = new MainWindow();
                                        mainWindow1.Show();
                                        this.Close();
                                    }
                                } 
                            }

                            // Проверка столкновений с ellipse2
                            double eeballLeft = Canvas.GetLeft(blackBall);
                            double eeballTop = Canvas.GetTop(blackBall);
                            double eeballWidth = blackBall.Width;
                            double eeballHeight = blackBall.Height;

                            double eellipseLeft = Canvas.GetLeft(ellipse2);
                            double eellipseTop = Canvas.GetTop(ellipse2);
                            double eellipseWidth = ellipse2.Width;
                            double eellipseHeight = ellipse2.Height;

                            // Проверка столкновения шарика с ellipse1
                            if (eeballLeft < (eellipseLeft + eellipseWidth) &&
                                (eeballLeft + eeballWidth) > eellipseLeft &&
                                eeballTop < (eellipseTop + eellipseHeight) &&
                                (eeballTop + eeballHeight) > eellipseTop)
                            {
                                if (!delBall1)
                                {
                                    // Если шарик столкнулся с ellipse1 и игра не закрыта
                                    if (!gameClosed && (DateTime.Now - bulletStartTime).TotalMilliseconds >= 100)
                                    {
                                        // Закрываем игру и устанавливаем флаг gameClosed в true
                                        gameClosed = true;
                                        canvas.Children.Remove(blackBall);
                                        MessageBox.Show("Красный проиграл!");
                                        MainWindow mainWindow1 = new MainWindow();
                                        mainWindow1.Show();
                                        this.Close();
                                    }
                                }       
                            }

                            // Проверка столкновений с границами Canvas и объектами Border
                            double ballLeft = Canvas.GetLeft(blackBall);
                            double ballTop = Canvas.GetTop(blackBall);
                            double ballWidth = blackBall.Width;
                            double ballHeight = blackBall.Height;

                            // Отражение от границ Canvas
                            if (ballLeft < 0 || ballLeft + ballWidth > canvas.ActualWidth)
                            {
                                dx = -dx; // Изменяем горизонтальное направление
                            }
                            if (ballTop < 0 || ballTop + ballHeight > canvas.ActualHeight)
                            {
                                dy = -dy; // Изменяем вертикальное направление
                            }

                            // Проверка столкновений с объектами Border
                            foreach (var child in canvas.Children)
                            {
                                if (child is Border border)
                                {
                                    double borderLeft = Canvas.GetLeft(border);
                                    double borderTop = Canvas.GetTop(border);
                                    double borderWidth = border.ActualWidth;
                                    double borderHeight = border.ActualHeight;

                                    // Проверка столкновений с Border
                                    if (ballLeft < (borderLeft + borderWidth) &&
                                        (ballLeft + ballWidth) > borderLeft &&
                                        ballTop < (borderTop + borderHeight) &&
                                        (ballTop + ballHeight) > borderTop)
                                    {
                                        canvas.Children.Remove(blackBall);
                                        delBall1 = true;
                                        break;
                                    }
                                }
                            }

                            if ((DateTime.Now - bulletStartTime).TotalMilliseconds >= 1500)
                            {
                                canvas.Children.Remove(blackBall);
                                bulletTimer.Stop(); // Останавливаем таймер
                            }
                        };
                        bulletTimer.Start();

                        // Обновляем время последнего выстрела и устанавливаем флаг "не готово к выстрелу"
                        lastShotTime1 = DateTime.Now;
                        canShoot1 = false;

                        // Запускаем таймер для перезарядки (3 секунды)
                        DispatcherTimer reloadTimer = new DispatcherTimer();
                        reloadTimer.Interval = TimeSpan.FromSeconds(0.5);
                        reloadTimer.Tick += (s, args) =>
                        {
                            canShoot1 = true; // Устанавливаем флаг "готово к выстрелу" после перезарядки
                            reloadTimer.Stop(); // Останавливаем таймер перезарядки
                        };
                        reloadTimer.Start();
                    }
                    break;
                case Key.LeftShift:
                    if (canShoot && (DateTime.Now - lastShotTime).TotalSeconds >= 0.5)
                    {
                        // Создание и настройка нового черного шарика
                        Ellipse blackBall = new Ellipse();
                        blackBall.Width = 20;
                        blackBall.Height = 20;
                        blackBall.Fill = Brushes.Black;
                        delBall = false;

                        // Установка начальной позиции рядом с greenEllipse
                        double centerX = Canvas.GetLeft(greenEllipse) + greenEllipse.Width / 2;
                        double centerY = Canvas.GetTop(greenEllipse) + greenEllipse.Height / 2;
                        Canvas.SetLeft(blackBall, centerX);
                        Canvas.SetTop(blackBall, centerY);
                        canvas.Children.Add(blackBall);

                        // Начальное направление движения
                        double moveSpeed = 16.0; // Скорость движения черного шарика
                        double angleRadians = angle; // Используем текущий угол greenEllipse для направления движения
                        double dx = moveSpeed * Math.Cos(angleRadians);
                        double dy = moveSpeed * Math.Sin(angleRadians);

                        // Таймер для перемещения черного шарика
                        DispatcherTimer bulletTimer = new DispatcherTimer();
                        bulletTimer.Interval = TimeSpan.FromMilliseconds(10);

                        DateTime bulletStartTime = DateTime.Now;

                        bulletTimer.Tick += (s, args) =>
                        {
                            // Перемещение шарика
                            Canvas.SetLeft(blackBall, Canvas.GetLeft(blackBall) + dx);
                            Canvas.SetTop(blackBall, Canvas.GetTop(blackBall) + dy);

                            // Проверка столкновений с ellipse1
                            double eballLeft = Canvas.GetLeft(blackBall);
                            double eballTop = Canvas.GetTop(blackBall);
                            double eballWidth = blackBall.Width;
                            double eballHeight = blackBall.Height;

                            double ellipseLeft = Canvas.GetLeft(ellipse1);
                            double ellipseTop = Canvas.GetTop(ellipse1);
                            double ellipseWidth = ellipse1.Width;
                            double ellipseHeight = ellipse1.Height;

                            // Проверка столкновения шарика с ellipse1
                            if (eballLeft < (ellipseLeft + ellipseWidth) &&
                                (eballLeft + eballWidth) > ellipseLeft &&
                                eballTop < (ellipseTop + ellipseHeight) &&
                                (eballTop + eballHeight) > ellipseTop)
                            {
                                if (!delBall)
                                {
                                    // Если шарик столкнулся с ellipse1 и игра не закрыта
                                    if (!gameClosed && (DateTime.Now - bulletStartTime).TotalMilliseconds >= 100)
                                    {
                                        // Закрываем игру и устанавливаем флаг gameClosed в true
                                        gameClosed = true;
                                        canvas.Children.Remove(blackBall);
                                        MessageBox.Show("Синий проиграл!");
                                        MainWindow mainWindow1 = new MainWindow();
                                        mainWindow1.Show();
                                        this.Close();
                                    }
                                }  
                            }

                            // Проверка столкновений с ellipse2
                            double eeballLeft = Canvas.GetLeft(blackBall);
                            double eeballTop = Canvas.GetTop(blackBall);
                            double eeballWidth = blackBall.Width;
                            double eeballHeight = blackBall.Height;

                            double eellipseLeft = Canvas.GetLeft(ellipse2);
                            double eellipseTop = Canvas.GetTop(ellipse2);
                            double eellipseWidth = ellipse2.Width;
                            double eellipseHeight = ellipse2.Height;

                            // Проверка столкновения шарика с ellipse1
                            if (eeballLeft < (eellipseLeft + eellipseWidth) &&
                                (eeballLeft + eeballWidth) > eellipseLeft &&
                                eeballTop < (eellipseTop + eellipseHeight) &&
                                (eeballTop + eeballHeight) > eellipseTop)
                            {
                                if (!delBall)
                                {
                                    // Если шарик столкнулся с ellipse1 и игра не закрыта
                                    if (!gameClosed && (DateTime.Now - bulletStartTime).TotalMilliseconds >= 100)
                                    {
                                        // Закрываем игру и устанавливаем флаг gameClosed в true
                                        gameClosed = true;
                                        canvas.Children.Remove(blackBall);
                                        MessageBox.Show("Красный проиграл!");
                                        MainWindow mainWindow1 = new MainWindow();
                                        mainWindow1.Show();
                                        this.Close();
                                    }
                                }
                            }

                            // Проверка столкновений с границами Canvas и объектами Border
                            double ballLeft = Canvas.GetLeft(blackBall);
                            double ballTop = Canvas.GetTop(blackBall);
                            double ballWidth = blackBall.Width;
                            double ballHeight = blackBall.Height;

                            // Отражение от границ Canvas
                            if (ballLeft < 0 || ballLeft + ballWidth > canvas.ActualWidth)
                            {
                                dx = -dx; // Изменяем горизонтальное направление
                            }
                            if (ballTop < 0 || ballTop + ballHeight > canvas.ActualHeight)
                            {
                                dy = -dy; // Изменяем вертикальное направление
                            }

                            // Проверка столкновений с объектами Border
                            foreach (var child in canvas.Children)
                            {
                                if (child is Border border)
                                {
                                    double borderLeft = Canvas.GetLeft(border);
                                    double borderTop = Canvas.GetTop(border);
                                    double borderWidth = border.ActualWidth;
                                    double borderHeight = border.ActualHeight;

                                    // Проверка столкновений с Border
                                    if (ballLeft < (borderLeft + borderWidth) &&
                                        (ballLeft + ballWidth) > borderLeft &&
                                        ballTop < (borderTop + borderHeight) &&
                                        (ballTop + ballHeight) > borderTop)
                                    {
                                        canvas.Children.Remove(blackBall);
                                        delBall = true;
                                        break;
                                    }
                                }
                            }

                            if ((DateTime.Now - bulletStartTime).TotalMilliseconds >= 1500)
                            {
                                canvas.Children.Remove(blackBall);
                                bulletTimer.Stop(); // Останавливаем таймер
                            }
                        };
                        bulletTimer.Start();

                        // Обновляем время последнего выстрела и устанавливаем флаг "не готово к выстрелу"
                        lastShotTime = DateTime.Now;
                        canShoot = false;

                        // Запускаем таймер для перезарядки (3 секунды)
                        DispatcherTimer reloadTimer = new DispatcherTimer();
                        reloadTimer.Interval = TimeSpan.FromSeconds(0.5);
                        reloadTimer.Tick += (s, args) =>
                        {
                            canShoot = true; // Устанавливаем флаг "готово к выстрелу" после перезарядки
                            reloadTimer.Stop(); // Останавливаем таймер перезарядки
                        };
                        reloadTimer.Start();
                    }
                    break;
            }
        }
    }

}
