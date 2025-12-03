using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace prolab_2
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameLoop;
        DispatcherTimer spawnTimer;
        DispatcherTimer buttonTimer;

        List<Enemy> enemies = new List<Enemy>();
        List<Tower> towers = new List<Tower>();

        List<Point> PathFromRight = new List<Point>();
        List<Point> PathFromTop = new List<Point>();

        double PlayerHealth = 100;
        int PlayerMoney = 2000;

        bool waveCheck = false;
        bool firstTick = false;
        int currentWave = 0;

        int rightSpawned = 0;
        int topSpawned = 0;
        int monsterSpawned = 0;

        int waveCount = 0;
        int wave2EachCount = 5;

        int wave1SpawnCount = 0;


        public MainWindow()
        {
            InitializeComponent();
            SetupPaths();
            UpdateUI();

            gameLoop = new DispatcherTimer();
            gameLoop.Interval = TimeSpan.FromMilliseconds(16);
            gameLoop.Tick += GameLoop_Tick;

            spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = TimeSpan.FromSeconds(1.5);
            spawnTimer.Tick += SpawnTimer_Tick;

            buttonTimer = new DispatcherTimer();
            buttonTimer.Interval = TimeSpan.FromSeconds(5);
            buttonTimer.Tick += ButtonTimer_Tick;

            Logger.OnLogReceived += AddLogToScreen;
            Logger.Log("Oyun başlatıldı. Düşmanlar bekleniyor...");
        }

        private void SetupPaths()
        {
            PathFromRight.Add(new Point(960, 120));  //960,120 900,120 800,130 700,250 630,300 590,330 400, 380 300, 400 220, 420 80, 480
            PathFromRight.Add(new Point(900, 120));
            PathFromRight.Add(new Point(800, 130));
            PathFromRight.Add(new Point(700, 250));
            PathFromRight.Add(new Point(630, 300));
            PathFromRight.Add(new Point(590, 330));
            PathFromRight.Add(new Point(400, 380));
            PathFromRight.Add(new Point(300, 400));
            PathFromRight.Add(new Point(220, 420));
            PathFromRight.Add(new Point(80, 480));

            PathFromTop.Add(new Point(460, 25));    //460,25 510,200 540, 300 490, 350 400, 380 300, 400 220, 420 80, 480
            PathFromTop.Add(new Point(510, 200));
            PathFromTop.Add(new Point(540, 300));
            PathFromTop.Add(new Point(490, 350));
            PathFromTop.Add(new Point(400, 380));
            PathFromTop.Add(new Point(300, 400));
            PathFromTop.Add(new Point(220, 420));
            PathFromTop.Add(new Point(80, 480));
        }
        private void btnEnemy_Click(object sender, RoutedEventArgs e)
        {
            btnEnemy.Visibility = Visibility.Hidden;
            btnEnemy2.Visibility = Visibility.Hidden;
            UpdateUI();
            if (currentWave == 0)
            {
                Logger.Log("1. Dalga Başlatıldı");
                Logger.Log("5 Goblin"); 
                if (!waveCheck)
                {
                    currentWave = 1;
                }
                else
                {
                    currentWave = 2;
                }

                rightSpawned = 0;
                topSpawned = 0;
                spawnTimer.Start();
                UpdateUI();
            }

            if (!firstTick)
            {
                Logger.Log("2. Dalga Başlatıldı");
                Logger.Log("5. Knight");
                Logger.Log("5 Goblin");
                Logger.Log("4 Monster");
                buttonTimer.Start();
                UpdateUI();
            }

            if (!gameLoop.IsEnabled)
            {
                gameLoop.Start();
                UpdateUI();
            }
        }
        private void btnEnemy2_Click(object sender, RoutedEventArgs e)
        {
            btnEnemy.Visibility = Visibility.Hidden;
            btnEnemy2.Visibility = Visibility.Hidden;
            Logger.Log("2. Dalga Başlatıldı");
            Logger.Log("5. Knight");
            Logger.Log("5 Goblin");
            Logger.Log("4 Monster");
            btnEnemy_Click(sender, e);
            UpdateUI();
            if (currentWave == 0)
            {
                currentWave = 2;
                rightSpawned = 0;
                topSpawned = 0;
                spawnTimer.Start();
                UpdateUI();
            }

            if (!gameLoop.IsEnabled)
            {
                gameLoop.Start();
                UpdateUI();
            }
        }
        private void ButtonTimer_Tick(object sender, EventArgs e)
        {
            firstTick = true;
            buttonTimer.Stop();

            btnEnemy.Visibility = Visibility.Visible;
            btnEnemy2.Visibility = Visibility.Visible;
        }
        private void SpawnTimer_Tick(object sender, EventArgs e)
        {
            if (currentWave == 1)
            {
                waveCount = 1;
                if (wave1SpawnCount < 6)
                    SpawnEnemy(PathFromRight, "Goblin");
                wave1SpawnCount++;

                if (wave1SpawnCount >= 5)
                {
                    spawnTimer.Stop();
                    currentWave = 0;
                    waveCheck = true;
                    Logger.Log("Dalga 1 gönderimi tamamlandı.");
                }
            }
            else if (currentWave == 2)
            {
                waveCount = 2;
                if (topSpawned < 5)
                {
                    SpawnEnemy(PathFromTop, "Knight");
                    topSpawned++;
                }
                else if (rightSpawned < 5)
                {
                    SpawnEnemy(PathFromRight, "Goblin");
                    rightSpawned++;
                }
                else if (monsterSpawned < 3)
                {
                    SpawnEnemy(PathFromRight, "Monster");
                    monsterSpawned++;
                }
                else
                {
                    spawnTimer.Stop();
                    currentWave = 0;
                    Logger.Log("Dalga 2 gönderimi tamamlandı.");
                }
            }
        }
        private void SpawnEnemy(List<Point> path, string enemyType)
        {
            Enemy newEnemy;
            Point startPoint = path[0];
            string id = "E" + (enemies.Count + 1);

            if (enemyType == "Goblin")
                newEnemy = new Goblin(id, startPoint.X, startPoint.Y);
            else if (enemyType == "Knight")
                newEnemy = new Knight(id, startPoint.X, startPoint.Y);
            else if (enemyType == "Monster")
                newEnemy = new FlyingMonster(id, (int)startPoint.X, (int)startPoint.Y);
            else
                return;

            newEnemy.AssignedPath = path;

            enemies.Add(newEnemy);
            GameCanvas.Children.Add(newEnemy.Appearance);
        }
        private void GameLoop_Tick(object sender, EventArgs e)
        {
            double deltaTime = gameLoop.Interval.TotalSeconds;

            // --- KULELERİN GÜNCELLENMESİ ---
            foreach (var tower in towers)
            {
                tower.Update(deltaTime);
                var target = tower.GetTarget(enemies);
                if (target != null) tower.Attack(target, enemies);
            }
            foreach (var tower in towers)
            {
                var target = tower.GetTarget(enemies);
                if (target != null)
                {
                    bool enemyLeft = target.Location.X < tower.X;
                    bool enemyRight = target.Location.X > tower.X;
                    tower.UpdateTowerImage(enemyLeft, enemyRight);
                }
                else
                {
                    tower.UpdateTowerImage(false, false);
                }
            }

            // --- DÜŞMANLARIN GÜNCELLENMESİ ---
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];

                // Hareket ettir
                bool reachedEnd = enemy.Move(enemy.AssignedPath, deltaTime);

                // Görseli güncelle
                enemy.Update(deltaTime);

                // ÖLÜM KONTROLÜ
                if (enemy.CurrentHealth <= 0)
                {
                    PlayerMoney += enemy.Bounty;
                    GameCanvas.Children.Remove(enemy.Appearance);
                    enemies.RemoveAt(i);
                    UpdateUI();
                    Logger.Log($"{enemy.Name} öldü. {enemy.Bounty} Kemik kazandırdı.");
                    continue; // Düşman öldü
                }

                // SONA ULAŞMA KONTROLÜ
                if (reachedEnd)
                {
                    PlayerHealth -= enemy.DamageToPlayer;
                    if (enemy.Name == "Knight")
                        Logger.Log($"{enemy.Name} hedefe ulaştı. 10 can azalttı");
                    else
                        Logger.Log($"{enemy.Name} hedefe ulaştı. 5 can azalttı");
                    GameCanvas.Children.Remove(enemy.Appearance);
                    enemies.RemoveAt(i);
                    UpdateUI();

                    if (PlayerHealth <= 0)
                    {
                        gameLoop.Stop();
                        spawnTimer.Stop();
                        MessageBox.Show("OYUN BİTTİ!\nKAYBETTİN");
                        return;
                    }
                }
            }

            if (enemies.Count == 0 && !spawnTimer.IsEnabled && rightSpawned >= wave2EachCount && topSpawned >= wave2EachCount)
            {
                gameLoop.Stop();
                MessageBox.Show("OYUN BİTTİ!\nKAZANDIN!");
            }

        }
        private void tower_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null || cmb.SelectedItem == null) return;

            ComboBoxItem item = cmb.SelectedItem as ComboBoxItem;
            if (item == null) return;

            string selectedTower = item.Content.ToString();
            double x = Canvas.GetLeft(cmb) + 20;
            double y = Canvas.GetTop(cmb) + 20;
            string id = "T" + towers.Count;

            Tower newTower = null;

            if (selectedTower == "ArcherTower") 
            { 
                newTower = new ArcherTower(id, x, y);

            }
            else if (selectedTower == "IceTower")
            {
                newTower = new IceTower(id, x, y);
            }
            else if (selectedTower == "CannonTower")
            {
                newTower = new CannonTower(id, x, y);
            }
            else if (selectedTower == "SorcererTower")
            {
                newTower = new SorcererTower(id, x, y);
            }
            if (newTower != null)
            {
                if (PlayerMoney >= newTower.Cost)
                {
                    PlayerMoney -= newTower.Cost;
                    towers.Add(newTower);
                    GameCanvas.Children.Add(newTower.Appearance);
                    newTower.UpdateVisual();
                    UpdateUI();
                    cmb.Visibility = Visibility.Hidden;
                    Logger.Log($"{newTower.Name} ({x}, {y}) konumuna inşa edildi.");
                }
                else
                {
                    MessageBox.Show("Yetersiz Para!");
                }
            }
            cmb.SelectedItem = null;
        }
        private void UpdateUI()
        {
            if (HealthText != null) HealthText.Text = $"Can: {Math.Round(PlayerHealth)} | Para: {PlayerMoney} | Dalga: {currentWave}";
            double barWidth = (PlayerHealth / 100.0) * 150;

            // Genişlik eksiye düşerse hata vermesin
            if (barWidth < 0) barWidth = 0;
            PlayerHealthRect.Width = barWidth;
        }
        private void AddLogToScreen(string message)
        {
            // Dispatcher kullanıyoruz çünkü Logger farklı bir thread'den çağrılabilir.
            Dispatcher.Invoke(() =>
            {
                // Listeye ekle
                GameLogList.Items.Add(message);

                // Otomatik aşağı kaydır (Her zaman en son mesajı göster)
                if (GameLogList.Items.Count > 0)
                {
                    GameLogList.ScrollIntoView(GameLogList.Items[GameLogList.Items.Count - 1]);
                }
            });
        }
        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            BtnStartGame.Visibility = Visibility.Collapsed;

            double doorWidth = IntroLayer.ActualWidth / 2;

            // --- SOL KAPI ANİMASYONU (Sola Kayan) ---
            DoubleAnimation animLeft = new DoubleAnimation();
            animLeft.To = -doorWidth; // Eksi yöne (Sola) git
            animLeft.Duration = TimeSpan.FromSeconds(2); // 2 saniye sürsün
                                                         // Easing: Kapı yavaşça hızlanıp yavaşça dursun
            animLeft.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };

            // --- SAĞ KAPI ANİMASYONU (Sağa Kayan) ---
            DoubleAnimation animRight = new DoubleAnimation();
            animRight.To = doorWidth;
            animRight.Duration = TimeSpan.FromSeconds(2);
            animRight.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };

            TranslateTransform leftTrans = LeftDoor.RenderTransform as TranslateTransform;
            TranslateTransform rightTrans = RightDoor.RenderTransform as TranslateTransform;

            // (Hata önlemi: Eğer XAML'da transform yoksa burada oluştururuz)
            if (leftTrans == null) { leftTrans = new TranslateTransform(); LeftDoor.RenderTransform = leftTrans; }
            if (rightTrans == null) { rightTrans = new TranslateTransform(); RightDoor.RenderTransform = rightTrans; }

            leftTrans.BeginAnimation(TranslateTransform.XProperty, animLeft);
            rightTrans.BeginAnimation(TranslateTransform.XProperty, animRight);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2); // Animasyon süresi kadar bekle
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                IntroLayer.Visibility = Visibility.Collapsed; // Giriş ekranını tamamen kaldır
                Logger.Log("Kapılar açıldı, oyun sahnesi hazır.");
            };
            timer.Start();
        }
        private void BtnExitGame_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();

            IntroLayer.Visibility = Visibility.Visible;
            BtnStartGame.Visibility = Visibility.Collapsed;

            DoubleAnimation closeAnim = new DoubleAnimation();
            closeAnim.To = 0; 
            closeAnim.Duration = TimeSpan.FromSeconds(2);
            closeAnim.EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut };
            TranslateTransform leftTrans = LeftDoor.RenderTransform as TranslateTransform;
            TranslateTransform rightTrans = RightDoor.RenderTransform as TranslateTransform;
            if (leftTrans != null) leftTrans.BeginAnimation(TranslateTransform.XProperty, closeAnim);
            if (rightTrans != null) rightTrans.BeginAnimation(TranslateTransform.XProperty, closeAnim);

            DispatcherTimer closeTimer = new DispatcherTimer();
            closeTimer.Interval = TimeSpan.FromSeconds(2);
            closeTimer.Tick += (s, args) =>
            {
                closeTimer.Stop();
                BtnStartGame.Visibility = Visibility.Visible;
                Logger.Log("Oyun bitirildi, ana menüye dönüldü.");
            };
            closeTimer.Start();
        }
        private void ResetGame()
        {
            gameLoop.Stop();
            spawnTimer.Stop();
            buttonTimer.Stop();

            foreach (var enemy in enemies)
            {
                GameCanvas.Children.Remove(enemy.Appearance);
            }
            enemies.Clear();

            foreach (var tower in towers)
            {
                GameCanvas.Children.Remove(tower.Appearance);
            }
            towers.Clear();

            PlayerHealth = 100;
            PlayerMoney = 2000;

            currentWave = 0;
            waveCheck = false;
            firstTick = false;

            rightSpawned = 0;
            topSpawned = 0;
            monsterSpawned = 0;

            wave1SpawnCount = 0;
            waveCount = 0;

            btnEnemy.Visibility = Visibility.Visible;

            UpdateUI();

            Logger.Log("Yeni oyun başlatıldı");
        }


    }
}