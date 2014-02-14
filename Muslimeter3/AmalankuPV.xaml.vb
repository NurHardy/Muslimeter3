Partial Public Class PivotPage1
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MuslimeterPV_Loaded(sender As Object, e As RoutedEventArgs) Handles MuslimeterPV.Loaded
        Dim textBlock As New TextBlock
        textBlock.Text = "Hello, world!"
        textBlock.FontSize = 64
        textBlock.Margin = New Thickness(50, 100, 0, 0)

        canv_graph.Children.Add(textBlock)
        ''canv_graph.SetLeft(textBlock, 100)
        ''canv_graph.SetTop(textBlock, 150)

    End Sub

    Private Sub ListBox_Loaded(sender As Object, e As RoutedEventArgs)
        CB_subuh.IsEnabled = False

    End Sub
End Class
