Imports Microsoft.Phone.Tasks


Partial Public Class Page1
    Inherits PhoneApplicationPage
    Public Sub New()
        InitializeComponent()



    End Sub

    Private Sub hyperlink_Click(sender As Object, e As RoutedEventArgs)
        Dim emailComposeTask As EmailComposeTask = New EmailComposeTask()

        emailComposeTask.Subject = "message subject"
        emailComposeTask.Body = "message body"
        emailComposeTask.To = "wahyudi_eko@if.undip.ac.id"
        'emailComposeTask.Cc = "cc@example.com"
        'emailComposeTask.Bcc = "bcc@example.com"

        emailComposeTask.Show()
    End Sub
End Class
