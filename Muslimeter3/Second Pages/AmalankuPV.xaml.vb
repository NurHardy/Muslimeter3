Imports System.Windows.Media.Imaging

Partial Public Class PivotPage1
    Inherits PhoneApplicationPage

    Structure _ibadah_harian
        Dim _tanggal As Date
        Dim _nilai As Single

    End Structure

    Private _DBConnectionString As String = "Data Source=isostore:/data_ibadah.sdf"
    Private _db As New dataibadah.dataibadah_context(_DBConnectionString)
    Private _today_row As dataibadah.tb_ibadah_harian
    Private _today_sched As dataibadah.tb_jadwal_sholat
    Private _dataibadah(120) As _ibadah_harian

    Private _ndatarec As Single
    Private _datediffday As Single = 7

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

    Private Sub drawGraph()
        graph_canv.Children.Clear()
        Dim _query = From _e In _db.tabel_data_harian
                           Where (_e.f_tanggal >= Date.Now.AddDays(-_datediffday)) And (_e.f_tanggal <= Date.Now)
                           Select _e Order By _e.f_tanggal

        Dim graphbg As New Rectangle
        graphbg.Width = 450
        graphbg.Height = 360
        graphbg.Fill = New SolidColorBrush(Colors.White)
        graph_canv.Children.Add(graphbg)
        _ndatarec = 0
        Dim _datarange, _datacc As Single
        Dim nilai_akh As Integer = 0
        _datarange = Math.Floor(_datediffday / 7)
        For Each _dharian As dataibadah.tb_ibadah_harian In _query
            _datacc = _datacc + 1
            Dim a As Single
            Dim tmpval As Integer = _dharian.f_status_ibadah
            For a = 1 To 11 '' bit-bit amalan
                nilai_akh = nilai_akh + CInt((tmpval And 1))
                tmpval = tmpval >> 1
            Next
            If _datacc >= _datarange Then
                _dataibadah(_ndatarec)._tanggal = _dharian.f_tanggal
                _dataibadah(_ndatarec)._nilai = nilai_akh / _datarange
                _ndatarec = _ndatarec + 1
                nilai_akh = 0
                _datacc = 0
            End If
        Next
        txt_daterange.Text = String.Concat(_dataibadah(0)._tanggal.ToShortDateString, " sampai ", _dataibadah(_ndatarec - 1)._tanggal.ToShortDateString)
        ''======================= DUMMY COMMAND, DON'T GIVE ATTENTION =================


        '// Initialize the WriteableBitmap with size 512x512 and set it as source of an Image control
        '''' Dim writeableBmp As New WriteableBitmap(450, 320)
        ''canv_img.Source = writeableBmp
        ''''writeableBmp.GetBitmapContext()

        '// Load an image from the calling Assembly's resources only by passing the relative path
        ''''writeableBmp = BitmapFactory.[New](450, 320)

        '// Clear the WriteableBitmap with white color
        ''''writeableBmp.Clear(Colors.White)

        Dim c As Integer
        For c = 0 To 11
            Dim Xaxis As New Line
            With Xaxis
                .X1 = 48 : .Y1 = 288 - (c * 24)
                .X2 = 402 : .Y2 = .Y1
                .Stroke = New SolidColorBrush(Colors.Gray)
            End With
            graph_canv.Children.Add(Xaxis)
        Next
        Dim graphLine As New PointCollection
        Dim curGraphPoint As Point
        Dim barWidth As Single = 48
        For c = 0 To (_ndatarec - 1)
            curGraphPoint = New Point((c * barWidth) + 48, 288 - (_dataibadah(c)._nilai * 24))
            graphLine.Add(curGraphPoint)
            If c > 0 Then
                Dim bottomArea As New Polygon
                bottomArea.Points.Add(New Point(((c - 1) * barWidth) + 48, 288))
                bottomArea.Points.Add(New Point(((c - 1) * barWidth) + 48, 288 - (_dataibadah(c - 1)._nilai * 24)))
                bottomArea.Points.Add(curGraphPoint)
                bottomArea.Points.Add(New Point(curGraphPoint.X, 288))
                bottomArea.Fill = New SolidColorBrush(Colors.Blue)
                bottomArea.Opacity = 0.5
                graph_canv.Children.Add(bottomArea)
            End If
            Dim bullet_ As New Ellipse
            bullet_.Width = 16
            bullet_.Height = 16
            bullet_.Margin = New Thickness(curGraphPoint.X - 8, curGraphPoint.Y - 8, 0, 0)
            bullet_.Fill = New SolidColorBrush(Colors.Blue)
            graph_canv.Children.Add(bullet_)
            Dim txtLabel As New TextBlock
            txtLabel.Margin = New Thickness(curGraphPoint.X - 20, curGraphPoint.Y - 40, 0, 0)
            txtLabel.Text = Math.Round(_dataibadah(c)._nilai, 1).ToString
            txtLabel.FontSize = 24
            txtLabel.Foreground = New SolidColorBrush(Colors.Black)
            graph_canv.Children.Add(txtLabel)
            Dim txtLabelDate As New TextBlock
            txtLabelDate.Margin = New Thickness(curGraphPoint.X - 16, 300, 0, 0)
            txtLabelDate.Text = _dataibadah(c)._tanggal.Day
            txtLabelDate.FontSize = 24
            txtLabelDate.Foreground = New SolidColorBrush(Colors.Black)
            graph_canv.Children.Add(txtLabelDate)
            Dim txtLabelDateM As New TextBlock
            txtLabelDateM.Margin = New Thickness(curGraphPoint.X - 8, 330, 0, 0)
            txtLabelDateM.Text = _dataibadah(c)._tanggal.Month
            txtLabelDateM.FontSize = 16
            txtLabelDateM.Foreground = New SolidColorBrush(Colors.Black)
            graph_canv.Children.Add(txtLabelDateM)
        Next
        Dim _graph_line As New Polyline
        _graph_line.Points = graphLine
        _graph_line.Stroke = New SolidColorBrush(Colors.Blue)
        _graph_line.StrokeThickness = 5
        graph_canv.Children.Add(_graph_line)

        '// Present the WriteableBitmap!
        ''''writeableBmp.Invalidate()
        ''canv_img.Source = writeableBmp
        '' ===============================================================
    End Sub

    Private Sub MuslimeterPV_Loaded(sender As Object, e As RoutedEventArgs) Handles MuslimeterPV.Loaded
        'drawGraph()
        'AddHandler lst_graph_range.SelectionChanged, AddressOf ListPicker_SelectionChanged
        Dim _sekarang As Date
        _sekarang = Date.Now()

        '' ===== LOAD JADWAL SHOLAT
        Dim _DBConnectionSched As String = "Data Source=/Application Data/static_data.sdf"
        Dim _db_sched As New dataibadah.jadwalsholat_context(_DBConnectionSched)

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
            Dim cc As Long
            For cc = 0 To 300
                _db.tabel_data_harian.InsertOnSubmit(New dataibadah.tb_ibadah_harian With {.f_tanggal = Date.Now().AddDays(-cc), .f_status_ibadah = (cc Mod 512)})
                _db.SubmitChanges()
            Next
            
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
        CB_rawatib.IsChecked = (_today_row.f_status_ibadah And 32)
        CB_tahajjud.IsChecked = (_today_row.f_status_ibadah And 64)
        CB_senin_kam.IsChecked = (_today_row.f_status_ibadah And 128)
        CB_sedekah.IsChecked = (_today_row.f_status_ibadah And 256)
        CB_silaturahim.IsChecked = (_today_row.f_status_ibadah And 512)

        CB_subuh.Visibility = Windows.Visibility.Collapsed
        CB_dzuhur.Visibility = Windows.Visibility.Collapsed
        CB_ashar.Visibility = Windows.Visibility.Collapsed
        CB_maghrib.Visibility = Windows.Visibility.Collapsed
        CB_Isya.Visibility = Windows.Visibility.Collapsed
        'CB_rawatib.Visibility = Windows.Visibility.Collapsed
        'CB_tahajjud.Visibility = Windows.Visibility.Collapsed
        'CB_sedekah.Visibility = Windows.Visibility.Collapsed
        'CB_silaturahim.Visibility = Windows.Visibility.Collapsed
        If (Date.Now().DayOfWeek = DayOfWeek.Monday) Or (Date.Now().DayOfWeek = DayOfWeek.Thursday) Then
            CB_senin_kam.Visibility = Windows.Visibility.Visible
        Else
            CB_senin_kam.Visibility = Windows.Visibility.Collapsed
        End If

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
        txt_date.Text = generate_date()
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

    Private Sub ListPicker_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If lst_graph_range.SelectedIndex = 0 Then
                _datediffday = 7
            ElseIf lst_graph_range.SelectedIndex = 1 Then
                _datediffday = 30
            ElseIf lst_graph_range.SelectedIndex = 2 Then
                _datediffday = 90
            ElseIf lst_graph_range.SelectedIndex = 3 Then
                _datediffday = 180
            ElseIf lst_graph_range.SelectedIndex = 4 Then
                _datediffday = 365
            End If
            drawGraph()
        Catch ex As Exception
            'MessageBox.Show("Error happens! : " & ex.Message)
        Finally
            'MessageBox.Show("Selchanged over...!")
        End Try

    End Sub

    Private Sub CB_rawatib_Tap(sender As Object, e As Input.GestureEventArgs) Handles CB_rawatib.Tap
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 32
        _db.SubmitChanges()
    End Sub

    Private Sub CB_tahajjud_Tap(sender As Object, e As Input.GestureEventArgs) Handles CB_tahajjud.Tap
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 64
        _db.SubmitChanges()
    End Sub

    Private Sub CB_senin_kam_Tap(sender As Object, e As Input.GestureEventArgs) Handles CB_senin_kam.Tap
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 128
        _db.SubmitChanges()
    End Sub

    Private Sub CB_sedekah_Tap(sender As Object, e As Input.GestureEventArgs) Handles CB_sedekah.Tap
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 256
        _db.SubmitChanges()
    End Sub

    Private Sub CB_silaturahim_Tap(sender As Object, e As Input.GestureEventArgs) Handles CB_silaturahim.Tap
        _today_row.f_status_ibadah = _today_row.f_status_ibadah Xor 512
        _db.SubmitChanges()
    End Sub
End Class
