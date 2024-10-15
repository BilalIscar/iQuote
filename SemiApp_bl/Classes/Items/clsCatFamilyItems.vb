Imports IscarDal

Public Class clsCatFamilyItems

    Public Shared Function GetCatFamilyItems_AS400(ByVal GIFNUM As Integer, GFSTYP As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@GIFNUM", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, GIFNUM))
            oParams.Add(New SqlParam("@GFSTYP", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, GFSTYP))
            'oParams.Add(New SqlParam("@DC", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, DC))
            'oParams.Add(New SqlParam("@GPNUM_REG", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, GPNUM_REG))


            'dt = oSql.ExecuteSPReturnDT("USP_GetCatFamilyItems_AS400", oParams)
            'USP_GetCatFamilyItems_AS400_2

            dt = oSql.ExecuteSPReturnDT("USP_GetCatFamilyItems_Local", oParams)

            'If dt Is Nothing Then
            '    dt = oSql.ExecuteSPReturnDT("USP_GetCatFamilyItems_Local", oParams)
            'End If

            'If Not dt Is Nothing AndAlso dt.Rows.Count = 0 Then
            '    dt = oSql.ExecuteSPReturnDT("USP_GetCatFamilyItems_Local", oParams)
            'End If

            Return dt

            'Dim dterror As DataTable
            'dterror.Columns.Add("GISEQ")
            'dterror.Columns.Add("GISC")
            'dterror.Columns.Add("GICAT")
            'dterror.Columns.Add("GIFNUM")
            'dterror.Columns.Add("GIDSCO")
            'dterror.Columns.Add("GIIC")
            'dterror.Columns.Add("GIPRNM")
            'dterror.Columns.Add("GIPRNI")
            'dterror.Columns.Add("GIFNUM1")
            'Return dterror

        Catch ex As Exception


            Throw
        End Try
    End Function
End Class
