Imports System.IO
Imports System.Windows.Resources
Imports System.IO.IsolatedStorage

Partial Public Class PengaturanPage
    Inherits PhoneApplicationPage

    Private Sub SaveFilesToIsoStore()

        '//These files must match what is included in the application package,
        ' //or BinaryStream.Dispose below will throw an exception.
        Dim files() As String = {
            "Application Data/dzikir_data/index.html",
            "Application Data/dzikir_data/dzikir.html",
            "Application Data/dzikir_data/doa.html",
            "Application Data/dzikir_data/bacaan_dzikir.html",
            "Application Data/dzikir_data/keutamaan_amal.html",
            "Application Data/dzikir_data/keutamaan_dzikir.html",
            "Application Data/dzikir_data/style.css",
            "Application Data/dzikir_data/jquery.min.js"
        }

        Dim isoStore As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()

        If (False = isoStore.FileExists(files(0))) Then
            For Each f As String In files
                Dim sr As StreamResourceInfo = Application.GetResourceStream(New Uri(f, UriKind.Relative))
                Using br As New BinaryReader(sr.Stream)
                    Dim data() As Byte = br.ReadBytes(sr.Stream.Length)
                    SaveToIsoStore(f, data)
                End Using
            Next
        End If
    End Sub

    Private Sub SaveToIsoStore(ByVal fileName As String, ByVal data() As Byte)
        Dim strBaseDir As String = String.Empty
        Dim delimStr As String = "/"
        Dim delimiter() As Char = delimStr.ToCharArray()
        Dim dirsPath() As String = fileName.Split(delimiter)

        '//Get the IsoStore.
        Dim isoStore As IsolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication()
        ' //Re-create the directory structure.
        Dim i As Integer
        For i = 0 To dirsPath.Length - 2
            strBaseDir = System.IO.Path.Combine(strBaseDir, dirsPath(i))
            isoStore.CreateDirectory(strBaseDir)
        Next
        ' //Remove the existing file.
        If (isoStore.FileExists(fileName)) Then
            isoStore.DeleteFile(fileName)
        End If

        '//Write the file.
        Using bw As New BinaryWriter(isoStore.CreateFile(fileName))
            bw.Write(data)
            bw.Close()
        End Using
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub PhoneApplicationPage_Loaded(sender As Object, e As RoutedEventArgs)
        'MessageBox.Show("WebBrowser init!")

        'dzikir_box.Navigate(New Uri(, UriKind.Absolute))
        'Try
        SaveFilesToIsoStore()
        dzikir_box.Navigate(New Uri("Application Data/dzikir_data/index.html", UriKind.Relative))
        'Catch ex As Exception
        'MessageBox.Show(ex.Message)
        'End Try

    End Sub
End Class
