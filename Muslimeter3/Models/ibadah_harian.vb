Imports System.ComponentModel
Imports System.Data.Linq
Imports System.Data.Linq.Mapping

Namespace dataibadah.Model

    Public Class dataibadah_context
        Inherits DataContext
        ' Pass the connection string to the base class.
        Public Sub New(ByVal connectionString As String)
            MyBase.New(connectionString)
        End Sub

        ' Specify a table for the to-do items.
        Public tabel_data_harian As Table(Of row_ibadah_harian)

        ' Specify a table for the categories.
        ' Public tabel_jenis_ibadah As Table(Of ToDoCategory)
    End Class

    <Table()>
    Public Class row_ibadah_harian
        Implements INotifyPropertyChanged, INotifyPropertyChanging

        ' Define ID: private field, public property, and database column.
        Private _f_id As Integer

        <Column(IsPrimaryKey:=True, IsDbGenerated:=True, DbType:="INT NOT NULL Identity", CanBeNull:=False, AutoSync:=AutoSync.OnInsert)>
        Public Property f_id() As Integer
            Get
                Return _f_id
            End Get
            Set(ByVal value As Integer)
                If _f_id <> value Then
                    NotifyPropertyChanging("f_id")
                    _f_id = value
                    NotifyPropertyChanged("f_id")
                End If
            End Set
        End Property

        ' Define item name: private field, public property, and database column.
        Private _f_tanggal As Date

        <Column()>
        Public Property f_tanggal() As Date
            Get
                Return _f_tanggal
            End Get
            Set(ByVal value As Date)
                If _f_tanggal <> value Then
                    NotifyPropertyChanging("f_tanggal")
                    _f_tanggal = value
                    NotifyPropertyChanged("f_tanggal")
                End If
            End Set
        End Property

        ' Define completion value: private field, public property, and database column.
        Private _f_status_ibadah As UInt64

        <Column()>
        Public Property f_status_ibadah() As UInt64
            Get
                Return _f_status_ibadah
            End Get
            Set(ByVal value As UInt64)
                If _f_status_ibadah <> value Then
                    NotifyPropertyChanging("f_status_ibadah")
                    _f_status_ibadah = value
                    NotifyPropertyChanged("f_status_ibadah")
                End If
            End Set
        End Property

        ' Version column aids update performance.
        <Column(IsVersion:=True)>
        Private _version As Binary


#Region "INotifyPropertyChanged Members"

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        ' Used to notify that a property changed
        Private Sub NotifyPropertyChanged(ByVal propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

#End Region

#Region "INotifyPropertyChanging Members"

        Public Event PropertyChanging As PropertyChangingEventHandler Implements INotifyPropertyChanging.PropertyChanging

        ' Used to notify that a property is about to change
        Private Sub NotifyPropertyChanging(ByVal propertyName As String)
            RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs(propertyName))
        End Sub

#End Region
    End Class

End Namespace
