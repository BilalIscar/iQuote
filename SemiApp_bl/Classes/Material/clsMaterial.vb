Imports System.Web
Imports IscarDal

Public Class clsMaterial
    Public Shared Function GetMainMaterial(_languageID As String) As DataTable
        Try

            'Dim dtF As New DataTable
            'dtF.Columns.Add("CategoryName")
            'dtF.Columns.Add("CategoryColor")
            'dtF.Columns.Add("GroupIdentification")
            'dtF.Columns.Add("CategoryOrder")
            'dtF.Columns.Add("Category")
            'dtF.Columns.Add("ForColor")

            'dtF.Rows.Add()
            'dtF.Rows(dtF.Rows.Count - 1).Item("CategoryName") = "Steel"
            'dtF.Rows(dtF.Rows.Count - 1).Item("CategoryColor") = "Steel"
            'dtF.Rows(dtF.Rows.Count - 1).Item("GroupIdentification") = "Steel"
            'dtF.Rows(dtF.Rows.Count - 1).Item("CategoryOrder") = "Steel"
            'dtF.Rows(dtF.Rows.Count - 1).Item("Category") = "Steel"
            'dtF.Rows(dtF.Rows.Count - 1).Item("ForColor") = "Steel"
            Dim CacheDependencyFileName As String = ""
            Dim dt As DataTable = CacheManager.GetDataTableGlob(CacheManager.Keys.glb_ModelParametersCode, "glb_MainMaterialsList", "", "CategoryOrder", "*", CacheDependencyFileName, _languageID.ToString)
            'If dt Is Nothing Then
            '    Dim modelNum As String = clsQuatation.ACTIVE_ModelNumber
            '    Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            '    Dim Params As New SqlParams
            '    If IsNumeric(modelNum) Then
            '        Params.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, modelNum))
            '    End If
            '    Dim dtt As DataTable
            '    If CacheDependencyFileName <> "" Then
            '        dtt = oSql.ExecuteSPReturnDT("USP_GetMainMaterialsList", Params)
            '        HttpContext.Current.Cache.Insert(CacheManager.Keys.glb_ModelParametersCode, dtt, New Caching.CacheDependency(CacheDependencyFileName))
            '    End If

            '    Return dtt

            'Else
            '    Return dt
            'End If


            Return dt


        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetMaterial(GroupName As String, LanguageID As Int16) As DataTable
        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams

            Params.Add(New SqlParam("@GroupName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, GroupName))
            Params.Add(New SqlParam("@LanguageID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, LanguageID))

            Return oSql.ExecuteSPReturnDT("USP_GetSelectedMaterialsList", Params)

        Catch ex As Exception
            Throw
        End Try
    End Function

    'Public Shared Function GetMaterial(CategoryName As String, LanguageID As Int16) As DataTable
    '    Try

    '        Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
    '        Dim Params As New SqlParams

    '        Params.Add(New SqlParam("@CategoryName", SqlParam.ParamType.ptNVarchar, SqlParam.ParamDirection.pdInput, CategoryName))
    '        Params.Add(New SqlParam("@LanguageID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, LanguageID))

    '        Return oSql.ExecuteSPReturnDT("USP_GetSelectedMaterialsList", Params)

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Function

    Public Shared Function GetSelectedMaterial(sID As String) As DataTable
        Try

            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim Params As New SqlParams
            If IsNumeric(sID) Then
                Params.Add(New SqlParam("@ID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CInt(sID)))
            Else
                Params.Add(New SqlParam("@ID", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, 0))
            End If
            Return oSql.ExecuteSPReturnDT("USP_GetSelectedMaterial", Params)

        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
