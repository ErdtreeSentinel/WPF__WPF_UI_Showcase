using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace WpfApp1;

public partial class MainWindow : FluentWindow
{
    public MainWindow()
    {
        InitializeComponent();

        ApplicationThemeManager.Apply(
            ApplicationTheme.Dark,
            WindowBackdropType.Mica,
            true
        );
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog(RootContentDialogPresenter)
        {
            Title = "Вариант 1: Только кнопка закрытия",
            Content = "Это диалог с единственной кнопкой 'Закрыть'.",
            CloseButtonText = "Закрыть",
            OverridesDefaultStyle = true,
        };

        dialog.CloseButtonAppearance = ControlAppearance.Secondary;
        await dialog.ShowAsync();

        dialog = new ContentDialog(RootContentDialogPresenter)
        {
            Title = "Вариант 2: Кнопки ОК и Отмена",
            Content = "Это диалог с кнопкой подтверждения и кнопкой закрытия.",
            PrimaryButtonText = "ОК",
            CloseButtonText = "Отмена",
            OverridesDefaultStyle = true,
        };

        dialog.PrimaryButtonAppearance = ControlAppearance.Primary;
        dialog.CloseButtonAppearance = ControlAppearance.Secondary;
        var result = await dialog.ShowAsync();

        // Обработка результата нажатия:
        if (result == ContentDialogResult.Primary)
        {
            // Пользователь нажал "ОК"
        }
        else
        {
            // Пользователь нажал "Отмена" или закрыл диалог другим способом
        }

        dialog = new ContentDialog(RootContentDialogPresenter)
        {
            Title = "Вариант 3: Три кнопки",
            Content = "Это диалог с тремя кнопками: 'Сохранить', 'Не сохранять' и 'Отмена'.",
            PrimaryButtonText = "Сохранить",
            SecondaryButtonText = "Не сохранять",
            CloseButtonText = "Отмена",
            OverridesDefaultStyle = true,
        };

        dialog.PrimaryButtonAppearance = ControlAppearance.Primary;
        dialog.SecondaryButtonAppearance = ControlAppearance.Secondary;
        dialog.CloseButtonAppearance = ControlAppearance.Secondary;
        result = await dialog.ShowAsync();

        // Обработка результата:
        if (result == ContentDialogResult.Primary)
        {
            // Нажата кнопка "Сохранить"
        }
        else if (result == ContentDialogResult.Secondary)
        {
            // Нажата кнопка "Не сохранять"
        }
        else
        {
            // Нажата кнопка "Отмена" или диалог закрыт другим способом
        }


        // Создаём набор UI-элементов для содержимого диалога
        var stackPanel = new StackPanel
        {
            Width = 400,
            Margin = new Thickness(10)
        };

        var instructionText = new Wpf.Ui.Controls.TextBlock
        {
            Text = "Введите ваше имя:",
            Margin = new Thickness(0, 0, 0, 10),
            FontSize = 14
        };

        var inputTextBox = new Wpf.Ui.Controls.TextBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(0, 0, 0, 10)
        };

        var rememberCheckBox = new CheckBox
        {
            Content = "Запомнить меня",
            Margin = new Thickness(-10, 0, 0, 10)
        };

        // Добавляем элементы в StackPanel
        stackPanel.Children.Add(instructionText);
        stackPanel.Children.Add(inputTextBox);
        stackPanel.Children.Add(rememberCheckBox);

        // Создаем диалог и задаем UI-элементы как содержимое
        dialog = new ContentDialog(RootContentDialogPresenter)
        {
            Title = "Диалог с UI",
            Content = stackPanel,
            PrimaryButtonText = "ОК",
            CloseButtonText = "Отмена",
            OverridesDefaultStyle = true,
        };

        dialog.PrimaryButtonAppearance = ControlAppearance.Primary;
        dialog.CloseButtonAppearance = ControlAppearance.Secondary;

        // Отображаем диалог
        result = await dialog.ShowAsync();

        // Обработка результата
        if (result == ContentDialogResult.Primary)
        {
            // Здесь можно получить введенное имя, например:
            string enteredName = inputTextBox.Text;
            bool remember = rememberCheckBox.IsChecked ?? false;
            // Дополнительная логика...
        }
    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var mb = new Wpf.Ui.Controls.MessageBox();
        mb.CloseButtonAppearance = ControlAppearance.Primary;
        mb.Content = "Содержимое (Content)";
        mb.Title = "Заголовок (Title)";
        mb.CloseButtonText = "Закрыть";
        mb.SecondaryButtonText = "Вторичная кнопка";
        await mb.ShowDialogAsync();
    }
}

public static class SpacingHelper
{
    public static readonly DependencyProperty SpacingProperty =
        DependencyProperty.RegisterAttached(
            "Spacing",
            typeof(double),
            typeof(SpacingHelper),
            new PropertyMetadata(0.0, OnSpacingChanged));

    public static void SetSpacing(DependencyObject element, double value)
    {
        element.SetValue(SpacingProperty, value);
    }

    public static double GetSpacing(DependencyObject element)
    {
        return (double)element.GetValue(SpacingProperty);
    }

    private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Panel panel)
        {
            panel.Loaded += (s, args) => UpdateChildrenMargins(panel);
        }
    }

    private static void UpdateChildrenMargins(Panel panel)
    {
        double spacing = GetSpacing(panel);

        if (panel is StackPanel stackPanel)
        {
            for (int i = 0; i < panel.Children.Count; i++)
            {
                if (panel.Children[i] is FrameworkElement child)
                {
                    if (stackPanel.Orientation == Orientation.Horizontal)
                    {
                        child.Margin = (i < panel.Children.Count - 1)
                            ? new Thickness(0, 0, spacing, 0)
                            : new Thickness(0);
                    }
                    else
                    {
                        child.Margin = (i < panel.Children.Count - 1)
                            ? new Thickness(0, 0, 0, spacing)
                            : new Thickness(0);
                    }
                }
            }
        }
    }
}

public static class GridSpacingHelper
{
    public static readonly DependencyProperty RowSpacingProperty =
        DependencyProperty.RegisterAttached(
            "RowSpacing",
            typeof(double),
            typeof(GridSpacingHelper),
            new PropertyMetadata(0.0, OnSpacingChanged));

    public static void SetRowSpacing(DependencyObject element, double value)
    {
        element.SetValue(RowSpacingProperty, value);
    }

    public static double GetRowSpacing(DependencyObject element)
    {
        return (double)element.GetValue(RowSpacingProperty);
    }

    public static readonly DependencyProperty ColumnSpacingProperty =
        DependencyProperty.RegisterAttached(
            "ColumnSpacing",
            typeof(double),
            typeof(GridSpacingHelper),
            new PropertyMetadata(0.0, OnSpacingChanged));

    public static void SetColumnSpacing(DependencyObject element, double value)
    {
        element.SetValue(ColumnSpacingProperty, value);
    }

    public static double GetColumnSpacing(DependencyObject element)
    {
        return (double)element.GetValue(ColumnSpacingProperty);
    }

    private static void OnSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Grid grid)
        {
            grid.Loaded += (s, args) => UpdateChildrenMargins(grid);
            grid.LayoutUpdated += (s, args) => UpdateChildrenMargins(grid);
        }
    }

    private static void UpdateChildrenMargins(Grid grid)
    {
        double rowSpacing = GetRowSpacing(grid);
        double columnSpacing = GetColumnSpacing(grid);

        foreach (UIElement element in grid.Children)
        {
            if (element is FrameworkElement child)
            {
                int col = Grid.GetColumn(child);
                int row = Grid.GetRow(child);

                Thickness margin = new Thickness();

                if (row > 0)
                {
                    margin.Top = rowSpacing;
                }
                if (col > 0)
                {
                    margin.Left = columnSpacing;
                }

                child.Margin = margin;
            }
        }
    }
}


public static class ButtonXamlDisplayBehavior
{
    public static readonly DependencyProperty ShowXamlOnClickProperty =
        DependencyProperty.RegisterAttached(
            "ShowXamlOnClick",
            typeof(bool),
            typeof(ButtonXamlDisplayBehavior),
            new PropertyMetadata(false, OnShowXamlOnClickChanged));

    public static bool GetShowXamlOnClick(UIElement element)
    {
        return (bool)element.GetValue(ShowXamlOnClickProperty);
    }

    public static void SetShowXamlOnClick(UIElement element, bool value)
    {
        element.SetValue(ShowXamlOnClickProperty, value);
    }

    private static void OnShowXamlOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Wpf.Ui.Controls.Button button)
        {
            if ((bool)e.NewValue)
            {
                button.Click += Button_Click;
            }
            else
            {
                button.Click -= Button_Click;
            }
        }
    }

    private static void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Wpf.Ui.Controls.Button button)
        {
            try
            {
                string appearanceValue = "";
                PropertyInfo appearanceProp = button.GetType().GetProperty("Appearance");
                if (appearanceProp != null)
                {
                    object value = appearanceProp.GetValue(button);
                    appearanceValue = value?.ToString() ?? "";
                }

                string contentValue = button.Content?.ToString() ?? "";

                string minWidth = button.MinWidth.ToString();
                string margin = button.Margin.ToString();

                string xamlSnippet = $"<ui:Button MinWidth=\"{minWidth}\"\nMargin=\"{margin}\"\n" +
                                     $"Appearance=\"{appearanceValue}\"\nContent=\"{contentValue}\" />";

                System.Windows.MessageBox.Show(xamlSnippet, "XAML код кнопки", System.Windows.MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при сериализации: {ex.Message}", "Ошибка", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}