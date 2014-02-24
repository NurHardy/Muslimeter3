Imports System.Windows.Media.Imaging

Partial Public Class PivotPage1
    Inherits PhoneApplicationPage

    Private _DBConnectionString As String = "Data Source=isostore:/data_ibadah.sdf"
    Private _db As New dataibadah.dataibadah_context(_DBConnectionString)
    Private _today_row As dataibadah.tb_ibadah_harian
    Private _today_sched As dataibadah.tb_jadwal_sholat
    Private _dataibadah(8) As Integer
    Private _ndatarec As Single

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
         Dim _sekarang As Date
        _sekarang = Date.Now()

        Dim _query = From _e In _db.tabel_data_harian
                            Where (_e.f_tanggal >= Date.Now.AddDays(-8)) And (_e.f_tanggal <= Date.Now) And (_e.f_tanggal.Year = _sekarang.Year)
                            Select _e Order By _e.f_tanggal

        Dim graphbg As New Rectangle
        graphbg.Width = 450
        graphbg.Height = 320
        graphbg.Fill = New SolidColorBrush(Colors.White)
        graph_canv.Children.Add(graphbg)

        For Each _dharian As dataibadah.tb_ibadah_harian In _query
            _ndatarec = _ndatarec + 1
            Dim a As Single
            Dim nilai_akh As Integer = 0
            Dim tmpval As Integer = _dharian.f_status_ibadah
            For a = 1 To 5 '' Sholat Fardhu
                nilai_akh = nilai_akh + CInt((tmpval And 1))
                tmpval = tmpval >> 1
            Next
            _dataibadah(_ndatarec) = nilai_akh
        Next

        ''======================= DUMMY COMMAND, DON'T GIVE ATTENTION =================
        txt_date.Text = generate_date()

        '// Initialize the WriteableBitmap with size 512x512 and set it as source of an Image control
        Dim writeableBmp As New WriteableBitmap(450, 320)
        canv_img.Source = writeableBmp
        writeableBmp.GetBitmapContext()

        '// Load an image from the calling Assembly's resources only by passing the relative path
        writeableBmp = BitmapFactory.[New](450, 320)

        '// Clear the WriteableBitmap with white color
        writeableBmp.Clear(Colors.White)

        Dim c As Integer
        Dim graphLine As New PointCollection
        For c = 1 To _ndatarec
            graphLine.Add(New Point(c * 64, (_dataibadah(c) * 65)))
            If c > 1 Then
                Dim bottomArea As New Polygon
                bottomArea.Points.Add(New Point((c - 1) * 64, 320))
                bottomArea.Points.Add(New Point((c - 1) * 64, _dataibadah(c - 1) * 65))
                bottomArea.Points.Add(New Point(c * 64, _dataibadah(c) * 65))
                bottomArea.Points.Add(New Point(c * 64, 320))
                bottomArea.Fill = New SolidColorBrush(Colors.Blue)
                bottomArea.Opacity = 0.5
                graph_canv.Children.Add(bottomArea)
            End If
            Dim bullet_ As New Ellipse
            bullet_.Width = 16
            bullet_.Height = 16
            ''bullet_.
        Next
        Dim _graph_line As New Polyline
        _graph_line.Points = graphLine
        _graph_line.Stroke = New SolidColorBrush(Colors.Blue)
        _graph_line.StrokeThickness = 5
        graph_canv.Children.Add(_graph_line)

        '// Present the WriteableBitmap!
        writeableBmp.Invalidate()
        canv_img.Source = writeableBmp
        '' ===============================================================

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
