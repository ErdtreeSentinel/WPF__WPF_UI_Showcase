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

    private void DestroyUSAButton_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.MessageBox.Show("jfldsjlfdsjl");
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog(RootContentDialogPresenter)
        {
            Title = "Диалоговое окно",
            Content = "Это содержимое диалога, который появляется поверх всего.",
            CloseButtonText = "Закрыть",
            OverridesDefaultStyle = true,
        };


        dialog.CloseButtonAppearance = ControlAppearance.Secondary;
        await dialog.ShowAsync();

        var s = new Wpf.Ui.Controls.MessageBox();

        s.CloseButtonAppearance = ControlAppearance.Primary;
        s.Content = "ОАДВОАДВОДАВОДА";
        s.Title = "fsdfsdf";
        s.CloseButtonText = "Закройся";
        await s.ShowDialogAsync();
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
            // Подписываемся на события, чтобы обновлять отступы при загрузке и изменении разметки грида
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

                // Устанавливаем отступ только для тех элементов, которые не находятся
                // в первой строке или первом столбце – таким образом отступы появляются ТОЛЬКО между элементами.
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