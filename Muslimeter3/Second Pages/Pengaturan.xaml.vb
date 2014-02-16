Partial Public Class SettingPage
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub
	
	Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
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


            Dim imsak As String = Math.Floor(Jadwal_.w_imsak / 60).ToString + ":" + (Jadwal_.w_imsak Mod 60).ToString
            Dim subuh As String = Math.Floor(Jadwal_.w_subuh / 60).ToString + ":" + (Jadwal_.w_subuh Mod 60).ToString
            Dim dzuhur As String = Math.Floor(Jadwal_.w_dzuhur / 60).ToString + ":" + (Jadwal_.w_dzuhur Mod 60).ToString
            Dim ashr As String = Math.Floor(Jadwal_.w_ashr / 60).ToString + ":" + (Jadwal_.w_ashr Mod 60).ToString
            Dim maghrib As String = Math.Floor(Jadwal_.w_maghrib / 60).ToString + ":" + (Jadwal_.w_maghrib Mod 60).ToString
            Dim isya As String = Math.Floor(Jadwal_.w_isya / 60).ToString + ":" + (Jadwal_.w_isya Mod 60).ToString

            Dim imsak_exp As String = Math.Floor((Jadwal_.w_imsak + 30) / 60).ToString + ":" + ((Jadwal_.w_imsak + 30) Mod 60).ToString

            Dim waktu_imsak As Date = CDate(imsak)
            Dim waktu_subuh As Date = CDate(subuh)
            Dim waktu_dzuhur As Date = CDate(dzuhur)
            Dim waktu_ashr As Date = CDate(ashr)
            Dim waktu_maghrib As Date = CDate(maghrib)
            Dim waktu_isya As Date = CDate(isya)

            Dim ulang As RecurrenceInterval = RecurrenceInterval.Daily

            Dim [date] As Date = Date.Now()

            Dim time As Date = waktu_imsak
            Dim beginTime As Date = [date] + time.TimeOfDay

            Dim time_exp As Date = CDate(imsak_exp)
            Dim expirationTime As Date = [date] + time_exp.TimeOfDay

            Dim reminder As New Reminder(Name)
            reminder.BeginTime = beginTime
            reminder.ExpirationTime = expirationTime
            reminder.RecurrenceType = ulang

            ' Register the reminder with the system.
            ScheduledActionService.Add(reminder)


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
