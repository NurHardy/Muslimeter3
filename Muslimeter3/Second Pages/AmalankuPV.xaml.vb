Imports System.Windows.Media.Imaging

Partial Public Class PivotPage1
    Inherits PhoneApplicationPage

    Private _DBConnectionString As String = "Data Source=isostore:/data_ibadah.sdf"
    Private _db As New dataibadah.dataibadah_context(_DBConnectionString)
    Private _today_row As dataibadah.tb_ibadah_harian
    Private _today_sched As dataibadah.tb_jadwal_sholat

    Enum waktu_sholat
        Isya = 0
        Imsak
        Subuh
        Terbit
        Dhuha
        Dzuhur
        Ashr
        Maghrib
    End Enum

    Private _month_names As String() = {
        "Bulan",
        "Januari",
        "Februari",
        "Maret",
        "April",
        "Mei",
        "Juni",
        "Juli",
        "Agustus",
        "September",
        "Oktober",
        "November",
        "Desember"
    }

    Private _day_names As String() = {
       "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu"
    }

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MuslimeterPV_Loaded(sender As Object, e As RoutedEventArgs) Handles MuslimeterPV.Loaded
        ''======================= DUMMY COMMAND, DON'T GIVE ATTENTION =================
        Dim textBlock As New TextBlock
        textBlock.Text = "Grafik di sini..." + vbCrLf + "[not implemented yet]"
        textBlock.FontSize = 32
        textBlock.Margin = New Thickness(50, 100, 0, 0)

        canv_graph.Children.Add(textBlock)
        ''canv_graph.SetLeft(textBlock, 100)
        ''canv_graph.SetTop(textBlock, 150)
        txt_date.Text = generate_date()

        '// Initialize the WriteableBitmap with size 512x512 and set it as source of an Image control
        Dim writeableBmp As New WriteableBitmap(512, 512)
        canv_img.Source = writeableBmp
        writeableBmp.GetBitmapContext()

        '// Load an image from the calling Assembly's resources only by passing the relative path
        writeableBmp = BitmapFactory.[New](512, 512)

        '// Clear the WriteableBitmap with white color
        writeableBmp.Clear(Colors.White)

        '// Set the pixel at P(10, 13) to black
        writeableBmp.SetPixel(10, 13, Colors.Black)

        '// Get the color of the pixel at P(30, 43)
        'Color color = writeableBmp.GetPixel(30, 43);

        '// Green line from P1(1, 2) to P2(30, 40)
        writeableBmp.DrawLine(1, 2, 30, 40, Colors.Green)

        '// Line from P1(1, 2) to P2(30, 40) using the fastest draw line method with the color as integer
        'int[] pixels = writeableBmp.Pixels;
        'int w = writeableBmp.PixelWidth;
        'int h = writeableBmp.PixelHeight;
        'WriteableBitmapExtensions.DrawLine(pixels, w, h, 1, 2, 30, 40, myIntColor);

        '// Blue anti-aliased line from P1(10, 20) to P2(50, 70)
        writeableBmp.DrawLineAa(10, 20, 50, 70, Colors.Blue)

        '// Black triangle with the points P1(10, 5), P2(20, 40) and P3(30, 10)
        writeableBmp.DrawTriangle(10, 5, 20, 40, 30, 10, Colors.Black)

        '// Red rectangle from the point P1(2, 4) that is 10px wide and 6px high
        writeableBmp.DrawRectangle(2, 4, 12, 10, Colors.Red)

        '// Filled blue ellipse with the center point P1(2, 2) that is 8px wide and 5px high
        writeableBmp.FillEllipseCentered(2, 2, 8, 5, Colors.Blue)

        '// Closed green polyline with P1(10, 5), P2(20, 40), P3(30, 30) and P4(7, 8)
        'int[] p = new int[] { 10, 5, 20, 40, 30, 30, 7, 8, 10, 5 };
        'writeableBmp.DrawPolyline(p, Colors.Green);

        '// Cubic Beziér curve from P1(5, 5) to P4(20, 7) with the control points P2(10, 15) and P3(15, 0)
        writeableBmp.DrawBezier(5, 5, 10, 15, 15, 0, 20, 7, Colors.Purple)

        '// Cardinal spline through the points P1(10, 5), P2(20, 40) and P3(30, 30) with a tension of 0.5
        'int[] pts = new int[] { 10, 5, 20, 40, 30, 30};
        'writeableBmp.DrawCurve(pts, 0.5,  Colors.Yellow);

        '// A filled Cardinal spline through the points P1(10, 5), P2(20, 40) and P3(30, 30) with a tension of 0.5
        ' writeableBmp.FillCurveClosed(pts, 0.5,  Colors.Green);

        '// Blit a bitmap using the additive blend mode at P1(10, 10)
        'writeableBmp.Blit(new Point(10, 10), bitmap, sourceRect, Colors.White, WriteableBitmapExtensions.BlendMode.Additive);

        '// Override all pixels with a function that changes the color based on the coordinate
        'writeableBmp.ForEach((x, y, color) => Color.FromArgb(color.A, (byte)(color.R / 2), (byte)(x * y), 100));

        '// Present the WriteableBitmap!
        writeableBmp.Invalidate()
        canv_img.Source = writeableBmp
        '' ===============================================================

        '' ===== LOAD JADWAL SHOLAT
        Dim _DBConnectionSched As String = "Data Source=/Application Data/static_data.sdf"
        Dim _db_sched As New dataibadah.jadwalsholat_context(_DBConnectionSched)
        Dim _sekarang As Date
        _sekarang = Date.Now()

        Try
            Dim query = From _e In _db_sched.tabel_jadwal
                           Where (_e.f_tanggal = _sekarang.Day) And (_e.f_bulan = _sekarang.Month)
                           Select _e

            _today_sched = query.FirstOrDefault()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ListBox_Loaded(sender As Object, e As RoutedEventArgs)
        Dim _sekarang As Date
        _sekarang = Date.Now()

        Dim query = From _e In _db.tabel_data_harian
                            Where (_e.f_tanggal.Day = _sekarang.Day) And (_e.f_tanggal.Month = _sekarang.Month) And (_e.f_tanggal.Year = _sekarang.Year)
                            Select _e

        If query.Count = 0 Then
            _db.tabel_data_harian.InsertOnSubmit(New dataibadah.tb_ibadah_harian With {.f_tanggal = Date.Now(), .f_status_ibadah = 1L})
            _db.SubmitChanges()
            query = From _e In _db.tabel_data_harian
                            Where (_e.f_tanggal.Day = _sekarang.Day) And (_e.f_tanggal.Month = _sekarang.Month) And (_e.f_tanggal.Year = _sekarang.Year)
                            Select _e
        End If
        _today_row = query.FirstOrDefault()

        CB_subuh.IsChecked = (_today_row.f_status_ibadah And 1)
        CB_Dzuhur.IsChecked = (_today_row.f_status_ibadah And 2)
        CB_Ashar.IsChecked = (_today_row.f_status_ibadah And 4)
        CB_Maghrib.IsChecked = (_today_row.f_status_ibadah And 8)
        CB_Isya.IsChecked = (_today_row.f_status_ibadah And 16)

        CB_subuh.Visibility = Windows.Visibility.Collapsed
        CB_dzuhur.Visibility = Windows.Visibility.Collapsed
        CB_ashar.Visibility = Windows.Visibility.Collapsed
        CB_maghrib.Visibility = Windows.Visibility.Collapsed
        CB_Isya.Visibility = Windows.Visibility.Collapsed

        Dim _m_sekarang As Integer = (_sekarang.Hour * 60) + _sekarang.Minute

        If (_m_sekarang < _today_sched.w_isya) Then CB_Isya.IsEnabled = False
        If (_m_sekarang < _today_sched.w_maghrib) Then CB_maghrib.IsEnabled = False
        If (_m_sekarang < _today_sched.w_ashr) Then CB_ashar.IsEnabled = False
        If (_m_sekarang < _today_sched.w_dzuhur) Then CB_dzuhur.IsEnabled = False
        If (_m_sekarang < _today_sched.w_subuh) Then CB_subuh.IsEnabled = False

        If (_m_sekarang >= _today_sched.w_isya) Then
            CB_Isya.Visibility = Windows.Visibility.Visible
        ElseIf (_m_sekarang >= _today_sched.w_maghrib) Then
            CB_maghrib.Visibility = Windows.Visibility.Visible
        ElseIf (_m_sekarang >= _today_sched.w_ashr) Then
            CB_ashar.Visibility = Windows.Visibility.Visible
        ElseIf (_m_sekarang >= _today_sched.w_dzuhur) Then
            CB_dzuhur.Visibility = Windows.Visibility.Visible
        ElseIf (_m_sekarang >= _today_sched.w_dhuha) Then
            'waktu = "--"
        ElseIf (_m_sekarang >= _today_sched.w_subuh) Then
            CB_subuh.Visibility = Windows.Visibility.Visible
        End If
    End Sub

    Private Sub btnSurprise_Click(sender As Object, e As RoutedEventArgs) Handles btnSurprise.Click
        CB_subuh.Visibility = Windows.Visibility.Visible
        CB_dzuhur.Visibility = Windows.Visibility.Visible
        CB_ashar.Visibility = Windows.Visibility.Visible
        CB_maghrib.Visibility = Windows.Visibility.Visible
        CB_Isya.Visibility = Windows.Visibility.Visible
        btnSurprise.Visibility = Windows.Visibility.Collapsed
    End Sub

    Private Function generate_date(Optional ByVal _date As Date = Nothing)
        Dim __date As Date
        Dim _out_ As String = ""
        If (_date = Nothing) Then __date = Date.Now() Else __date = _date
        _out_ = _day_names(Date.Now.DayOfWeek) + ", " + Date.Now.Day.ToString + " " + _month_names(Date.Now.Month) + " " + Date.Now.Year.ToString
        Return _out_
    End Function

    Private Sub CB_subuh_Click(sender As Object, e As RoutedEventArgs) Handles CB_subuh.Tap
        If (Not CB_subuh.IsEnabled) Then Return
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 1
        _db.SubmitChanges()
        'MessageBox.Show(_today_row.f_status_ibadah.ToString)
    End Sub

    Private Sub CB_dzuhur_Checked(sender As Object, e As RoutedEventArgs) Handles CB_dzuhur.Tap
        If (Not CB_dzuhur.IsEnabled) Then Return
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 2
        _db.SubmitChanges()
        'MessageBox.Show(_today_row.f_status_ibadah.ToString)
    End Sub

    Private Sub CB_ashar_Checked(sender As Object, e As RoutedEventArgs) Handles CB_ashar.Tap
        If (Not CB_ashar.IsEnabled) Then Return
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 4
        _db.SubmitChanges()
        'MessageBox.Show(_today_row.f_status_ibadah.ToString)
    End Sub

    Private Sub CB_maghrib_Checked(sender As Object, e As RoutedEventArgs) Handles CB_maghrib.Tap
        If (Not CB_maghrib.IsEnabled) Then Return
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 8
        _db.SubmitChanges()
        'MessageBox.Show(_today_row.f_status_ibadah.ToString)
    End Sub


    Private Sub CB_Isya_Checked(sender As Object, e As RoutedEventArgs) Handles CB_Isya.Tap
        If (Not CB_Isya.IsEnabled) Then Return
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 16
        _db.SubmitChanges()
        'MessageBox.Show(_today_row.f_status_ibadah.ToString)
    End Sub
End Class
