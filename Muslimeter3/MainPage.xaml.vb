Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' Constructor
    Public Sub New()
        InitializeComponent()

        ' Set the data context of the listbox control to the sample data
        DataContext = App.ViewModel
    End Sub

    ' Load data for the ViewModel Items
    Private Sub MainPage_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles Me.Loaded
        If Not App.ViewModel.IsDataLoaded Then
            App.ViewModel.LoadData()
        End If
    End Sub

    Private Sub ListBox_Loaded(sender As Object, e As RoutedEventArgs)
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

            Dim outStr As String = ""
            outStr = Date.Now.ToLongDateString + ": " + vbCrLf
            outStr = outStr + "> Imsak : " + Math.Floor(Jadwal_.w_imsak / 60).ToString + ":" + (Jadwal_.w_imsak Mod 60).ToString + vbCrLf
            outStr = outStr + "> Subuh : " + Math.Floor(Jadwal_.w_subuh / 60).ToString + ":" + (Jadwal_.w_subuh Mod 60).ToString + vbCrLf
            outStr = outStr + "> Terbit : " + Math.Floor(Jadwal_.w_terbit / 60).ToString + ":" + (Jadwal_.w_terbit Mod 60).ToString + vbCrLf
            outStr = outStr + "> Dhuha : " + Math.Floor(Jadwal_.w_dhuha / 60).ToString + ":" + (Jadwal_.w_dhuha Mod 60).ToString + vbCrLf
            outStr = outStr + "> Dzuhur : " + Math.Floor(Jadwal_.w_dzuhur / 60).ToString + ":" + (Jadwal_.w_dzuhur Mod 60).ToString + vbCrLf
            outStr = outStr + "> Ashr : " + Math.Floor(Jadwal_.w_ashr / 60).ToString + ":" + (Jadwal_.w_ashr Mod 60).ToString + vbCrLf
            outStr = outStr + "> Maghrib : " + Math.Floor(Jadwal_.w_maghrib / 60).ToString + ":" + (Jadwal_.w_maghrib Mod 60).ToString + vbCrLf
            outStr = outStr + "> Isya : " + Math.Floor(Jadwal_.w_isya / 60).ToString + ":" + (Jadwal_.w_isya Mod 60).ToString + vbCrLf


            txt_jadwal_sholat.Content = outStr


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
