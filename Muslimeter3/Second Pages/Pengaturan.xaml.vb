Imports Microsoft.Phone.Scheduler

Partial Public Class SettingPage
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Berhasil")
        Dim Name As String = Guid.NewGuid().ToString()

        Dim _DBConnectionString As String = "Data Source=/Application Data/static_data.sdf"
        Dim _db As New dataibadah.jadwalsholat_context(_DBConnectionString)
        Dim _sekarang As Date
        _sekarang = Date.Now()

        Try
            Dim query = From _e In _db.tabel_jadwal
                           Where (_e.f_tanggal = _sekarang.Day) And (_e.f_bulan = _sekarang.Month)
                           Select _e

            Dim Jadwal_ As dataibadah.tb_jadwal_sholat
            Jadwal_ = query.FirstOrDefault()


            Dim waktu_subuh As String = Math.Floor(Jadwal_.w_subuh / 60).ToString + ":" + (Jadwal_.w_subuh Mod 60).ToString
            Dim waktu_dzuhur As String = Math.Floor(Jadwal_.w_dzuhur / 60).ToString + ":" + (Jadwal_.w_dzuhur Mod 60).ToString
            Dim waktu_ashr As String = Math.Floor(Jadwal_.w_ashr / 60).ToString + ":" + (Jadwal_.w_ashr Mod 60).ToString
            Dim waktu_maghrib As String = Math.Floor(Jadwal_.w_maghrib / 60).ToString + ":" + (Jadwal_.w_maghrib Mod 60).ToString
            Dim waktu_isya As String = Math.Floor(Jadwal_.w_isya / 60).ToString + ":" + (Jadwal_.w_isya Mod 60).ToString

            Dim subuh_exp As String = Math.Floor((Jadwal_.w_subuh + 30) / 60).ToString + ":" + ((Jadwal_.w_subuh + 30) Mod 60).ToString
            Dim dzuhur_exp As String = Math.Floor((Jadwal_.w_dzuhur + 30) / 60).ToString + ":" + ((Jadwal_.w_dzuhur + 30) Mod 60).ToString
            Dim ashr_exp As String = Math.Floor((Jadwal_.w_ashr + 30) / 60).ToString + ":" + ((Jadwal_.w_ashr + 30) Mod 60).ToString
            Dim maghrib_exp As String = Math.Floor((Jadwal_.w_maghrib + 30) / 60).ToString + ":" + ((Jadwal_.w_maghrib + 30) Mod 60).ToString
            Dim isya_exp As String = Math.Floor((Jadwal_.w_isya + 30) / 60).ToString + ":" + ((Jadwal_.w_isya + 30) Mod 60).ToString

            Dim [date] As Date = _sekarang.Date
            Dim menit_sekarang As Integer = (_sekarang.Hour * 60) + _sekarang.Minute
            MessageBox.Show(waktu_subuh)
            MessageBox.Show([date])

            'REMINDER UNTUK SUBUH
            If (menit_sekarang < Jadwal_.w_subuh) And CBool(CB_subuh.IsChecked) Then

                Dim time_begin As Date = CDate(waktu_subuh)
                Dim beginTime As Date = [date] + time_begin.TimeOfDay

                Dim time_exp As Date = CDate(subuh_exp)
                Dim expirationTime As Date = [date] + time_exp.TimeOfDay

                Dim ulang As RecurrenceInterval = RecurrenceInterval.Daily
                'buat remnider baru
                Dim reminder As New Reminder("Subuh")
                reminder.BeginTime = beginTime
                reminder.ExpirationTime = expirationTime
                reminder.RecurrenceType = ulang
                ' Register the reminder with the system.
                ScheduledActionService.Add(reminder)
                ' MessageBox.Show(beginTime)
                ' MessageBox.Show(expirationTime)
            End If
            'REMINDER UNTUK DZUHUR
            If (menit_sekarang < Jadwal_.w_dzuhur) And CBool(CB_dzuhur.IsChecked) Then

                Dim time_begin As Date = CDate(waktu_dzuhur)
                Dim beginTime As Date = [date] + time_begin.TimeOfDay

                Dim time_exp As Date = CDate(dzuhur_exp)
                Dim expirationTime As Date = [date] + time_exp.TimeOfDay

                Dim ulang As RecurrenceInterval = RecurrenceInterval.Daily

                Dim reminder As New Reminder("Dzuhur")
                reminder.BeginTime = beginTime
                reminder.ExpirationTime = expirationTime
                reminder.RecurrenceType = ulang
                ' Register the reminder with the system.
                ScheduledActionService.Add(reminder)
                ' MessageBox.Show(beginTime)
                ' MessageBox.Show(expirationTime)
            End If

            'REMINDER UNTUK ASHR
            If (menit_sekarang < Jadwal_.w_ashr) And CBool(CB_ashar.IsChecked) Then

                Dim time_begin As Date = CDate(waktu_ashr)
                Dim beginTime As Date = [date] + time_begin.TimeOfDay

                Dim time_exp As Date = CDate(ashr_exp)
                Dim expirationTime As Date = [date] + time_exp.TimeOfDay

                Dim ulang As RecurrenceInterval = RecurrenceInterval.Daily

                Dim reminder As New Reminder("Ashr")
                reminder.BeginTime = beginTime
                reminder.ExpirationTime = expirationTime
                reminder.RecurrenceType = ulang
                ' Register the reminder with the system.
                ScheduledActionService.Add(reminder)
                MessageBox.Show(time_begin)
                MessageBox.Show(time_exp)
            End If
            'REMINDER UNTUK MAGHRIB
            If (menit_sekarang < Jadwal_.w_maghrib) And CBool(CB_maghrib.IsChecked) Then

                Dim time_begin As Date = CDate(waktu_maghrib)
                Dim beginTime As Date = [date] + time_begin.TimeOfDay

                Dim time_exp As Date = CDate(maghrib_exp)
                Dim expirationTime As Date = [date] + time_exp.TimeOfDay

                Dim ulang As RecurrenceInterval = RecurrenceInterval.Daily

                Dim reminder As New Reminder("Maghrib")
                reminder.BeginTime = beginTime
                reminder.ExpirationTime = expirationTime
                reminder.RecurrenceType = ulang
                ' Register the reminder with the system.
                ScheduledActionService.Add(reminder)
                ' MessageBox.Show(beginTime)
                ' MessageBox.Show(expirationTime)
            End If
            'REMINDER UNTUK ISYA
            If (menit_sekarang < Jadwal_.w_isya) And CBool(CB_Isya.IsChecked) Then

                Dim time_begin As Date = CDate(waktu_isya)
                Dim beginTime As Date = [date] + time_begin.TimeOfDay

                Dim time_exp As Date = CDate(isya_exp)
                Dim expirationTime As Date = [date] + time_exp.TimeOfDay

                Dim ulang As RecurrenceInterval = RecurrenceInterval.Daily

                Dim reminder As New Reminder("Isya")
                reminder.BeginTime = beginTime
                reminder.ExpirationTime = expirationTime
                reminder.RecurrenceType = ulang
                ' Register the reminder with the system.
                ScheduledActionService.Add(reminder)
                ' MessageBox.Show(beginTime)
                ' MessageBox.Show(expirationTime)
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub buka_pengaturan_pengingat_Click(sender As Object, e As RoutedEventArgs)
        CB_subuh.Visibility = Windows.Visibility.Visible
        CB_dzuhur.Visibility = Windows.Visibility.Visible
        CB_ashar.Visibility = Windows.Visibility.Visible
        CB_maghrib.Visibility = Windows.Visibility.Visible
        CB_Isya.Visibility = Windows.Visibility.Visible
        CB_rawatib.Visibility = Windows.Visibility.Visible
        CB_tahajjud.Visibility = Windows.Visibility.Visible
        CB_senin_kam.Visibility = Windows.Visibility.Visible
        CB_sedekah.Visibility = Windows.Visibility.Visible
        CB_silaturahim.Visibility = Windows.Visibility.Visible
        buka_pengaturan_pengingat.Visibility = Windows.Visibility.Collapsed
        tutup_pengaturan_pengingat.Visibility = Windows.Visibility.Visible
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub tutup_pengaturan_pengingat_Click(sender As Object, e As RoutedEventArgs)
        CB_subuh.Visibility = Windows.Visibility.Collapsed
        CB_dzuhur.Visibility = Windows.Visibility.Collapsed
        CB_ashar.Visibility = Windows.Visibility.Collapsed
        CB_maghrib.Visibility = Windows.Visibility.Collapsed
        CB_Isya.Visibility = Windows.Visibility.Collapsed
        CB_rawatib.Visibility = Windows.Visibility.Collapsed
        CB_tahajjud.Visibility = Windows.Visibility.Collapsed
        CB_senin_kam.Visibility = Windows.Visibility.Collapsed
        CB_sedekah.Visibility = Windows.Visibility.Collapsed
        CB_silaturahim.Visibility = Windows.Visibility.Collapsed
        tutup_pengaturan_pengingat.Visibility = Windows.Visibility.Collapsed
        buka_pengaturan_pengingat.Visibility = Windows.Visibility.Visible
    End Sub
End Class
