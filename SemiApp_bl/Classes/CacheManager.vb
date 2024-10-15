Imports System.Configuration
Imports System.Web
Imports IscarDal

Public Class CacheManager

#Region "Enum's"
    Public Enum Keys
        Parameters
        Rules
        Relations
        BranchServer
        BranchDeatils
        Constants
        GP
        CGP
        glb_ModelParametersCode
        LanguageConstants
        LanguageLabels
    End Enum

    Public Enum DalTypes
        OleDb
        Sql
    End Enum
#End Region


    Public Shared Function GetDataTable(ByVal Key As Keys, ByVal Table As String, ByVal Where As String, ByVal OrderBy As String, ByVal Fields As String) As DataTable
        Dim i As Integer = Key.GetHashCode
        Dim Dal As IDal
        'Dim DalName As String
        Dim dt As DataTable
        'Dim Fields As String
        Dim Library As String = ""
        'Dim OrderBy As String
        'Dim Table As String
        Dim TimeOut As Integer = CInt(ConfigurationManager.AppSettings("CacheGeneralTimeOut"))
        Dim CacheDependencyFileName As String = Nothing
        Try
            CacheDependencyFileName = HttpContext.Current.Server.MapPath("~") & ConfigurationManager.AppSettings("CacheDependencyFileName")
        Catch ex As Exception
        End Try

        Try
            Dal = New SqlDal(clsBranch.GetSiteConnectionString)
            Dim BranchCode As String
            Try
                BranchCode = StateManager.GetValue(StateManager.Keys.s_BranchCode)
            Catch ex As Exception
                BranchCode = "IS"
                'No Action
            End Try
            Dim CacheIsActive As Boolean = False
            If BranchCode = "IS" Then
                CacheIsActive = CType(ConfigurationManager.AppSettings("CacheIsActiveIscar"), Boolean)
            Else
                CacheIsActive = CType(ConfigurationManager.AppSettings("CacheIsActiveBranch"), Boolean)
            End If


            'If CType(ConfigurationManager.AppSettings("CacheIsActive"), Boolean) Then
            If CacheIsActive Then
                'dt = CType(HttpContext.Current.Application.Get(General.GetBranchCacheKeyName(Key)), DataTable)
                dt = CType(HttpContext.Current.Cache.Get(Key & Library & Where & OrderBy), DataTable)

                ''''----Cache Check
                If IsNothing(dt) Then

                    dt = clUtils.GetList(Dal, Table, Library, Where, OrderBy, Fields)

                    ''HttpContext.Current.Trace.Warn("Key=" & General.GetBranchCacheKeyName(Key) & "; TimeOut=" & TimeOut)
                    ''HttpContext.Current.Application.Lock()
                    ''HttpContext.Current.Application.Add(General.GetBranchCacheKeyName(Key), dt)
                    ''HttpContext.Current.Application.UnLock()

                    HttpContext.Current.Cache.Insert(Key & Library & Where & OrderBy, dt, New Caching.CacheDependency(CacheDependencyFileName)) ', DateTime.Now.AddMinutes(1440), TimeSpan.Zero)
                    'GeneralException.WriteEventErrors("GetDataTableGlob - " & (Key & Library & Where & OrderBy).ToString)
                    ''HttpContext.Current.Cache.Insert(General.GetBranchCacheKeyName(Key), dt, Nothing, DateTime.Now.MaxValue, TimeSpan.FromHours(24))
                End If
                ''''----
            Else
                dt = clUtils.GetList(Dal, Table, Library, Where, OrderBy, Fields)
            End If

            Return dt
        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "GetDataTable failed")
        Finally
            Dal = Nothing
        End Try
    End Function

    Public Shared Function GetDataTableGlob(ByVal Key As Keys, ByVal Table As String, ByVal Where As String, ByVal OrderBy As String, ByVal Fields As String, ByRef CacheDependencyFileName As String, LanguageID As String) As DataTable
        Dim i As Integer = Key.GetHashCode
        Dim Dal As IDal
        'Dim dt As DataTable
        Dim Library As String = ""
        Dim TimeOut As Integer = CInt(ConfigurationManager.AppSettings("CacheGeneralTimeOut"))
        Try
            CacheDependencyFileName = HttpContext.Current.Server.MapPath("~") & ConfigurationManager.AppSettings("CacheDependencyFileName")
        Catch ex As Exception
            CacheDependencyFileName = ""
        End Try

        Try

            'dt = CType(HttpContext.Current.Cache.Get(Key & Library & Where & OrderBy), DataTable)
            'Return dt
            'If dt Is Nothing AndAlso dt.Rows.Count < 6 Then
            Dim modelNum As String = clsQuatation.ACTIVE_ModelNumber
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            If IsNumeric(modelNum) Then
                Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, modelNum))
                Params.Add(New SqlParam("@LanguageID", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, LanguageID))
            End If
            Dim dtt As DataTable
            If CacheDependencyFileName <> "" Then
                dtt = oSql.ExecuteSPReturnDT("USP_GetMainMaterialsList", Params)
                Try
                    If dtt Is Nothing OrElse dtt.Rows.Count = 0 Then
                        Dim Params1 As New SqlParams
                        If IsNumeric(modelNum) Then
                            Params1.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, modelNum))
                            Params1.Add(New SqlParam("@LanguageID", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, 1))
                        End If
                        dtt = oSql.ExecuteSPReturnDT("USP_GetMainMaterialsList", Params1)
                    End If
                Catch ex As Exception

                End Try

                HttpContext.Current.Cache.Insert(Key & Library & Where & OrderBy, dtt, New Caching.CacheDependency(CacheDependencyFileName))
                'GeneralException.WriteEventErrors("GetDataTableGlob - " & (Key & Library & Where & OrderBy).ToString)
            End If
            Return dtt
            'Else
            '    Return dt
            'End If


            'If IsNothing(dt) Then
            '    HttpContext.Current.Cache.Insert(Table, dt, New Caching.CacheDependency(CacheDependencyFileName))
            'End If

            'Return dt
        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "GetDataTable failed")
        Finally
            Dal = Nothing
        End Try
    End Function

    Public Shared Function GetDataTableForImports(ByVal Key As Keys, ByVal Table As String, ByVal Where As String, ByVal OrderBy As String, ByVal Fields As String) As DataTable
        Dim Dal As IDal
        Dim dt As DataTable
        Dim Library As String = ""
        Try
            Dal = New SqlDal(clsBranch.GetSiteConnectionString)
            dt = clUtils.GetList(Dal, Table, Library, Where, OrderBy, Fields)
            Return dt
        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "GetDataTable failed")
        Finally
            Dal = Nothing
        End Try
    End Function
End Class
