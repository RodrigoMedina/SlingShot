﻿Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Imports System.Data.SQLite


Public Class GHSQLite_Command
    Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQLite Command", "LiteCommand", "Sends a command to a SQLite database file (*.s3db)", "Slingshot!", "RDBMS Connection")
  End Sub

  'Exposure parameter (line dividers)
  Public Overrides ReadOnly Property Exposure As Grasshopper.Kernel.GH_Exposure
    Get
      Return GH_Exposure.quarternary
    End Get
  End Property

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{b3a921bf-e5cf-4579-bcac-4f39e402aa4c}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHSQLite_Command
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddBooleanParameter("Create Toggle", "CToggle", "Set to 'True' to create the *.s3db database file.", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Directory Path", "Directory", "The directory for the SQLite database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Database", "Database", "The name of the database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Command", "Command", "A SQLite Command.", GH_ParamAccess.item)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Prints error or success streams.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try

      Dim ctoggle As Boolean = False
      Dim path As String = Nothing
      Dim database As String = Nothing
      Dim command As String = Nothing

      DA.GetData(Of Boolean)(0, ctoggle)
      DA.GetData(Of String)(1, path)
      DA.GetData(Of String)(2, database)
      DA.GetData(Of String)(3, command)

      If ctoggle = True Then
        Dim filepath As String = path + "\" + database + ".s3db"

        'Connect to SQLite
        Dim SQLConnect As New SQLite.SQLiteConnection()
        Dim SQLCommand As SQLiteCommand

        SQLConnect.ConnectionString = "Data Source=" & filepath
        SQLConnect.Open()
        SQLCommand = SQLConnect.CreateCommand
        SQLCommand.CommandText = command
        SQLCommand.ExecuteNonQuery()
        SQLCommand.Dispose()
        SQLConnect.Close()

        DA.SetData(0, "Database command executed!")
      End If

    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class