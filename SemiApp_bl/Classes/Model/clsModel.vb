Imports IscarDal

Public Class clsModel
    Public Enum e_Model
        'End_Mill_Configurato = 3000
        SCEndMillsChatterFree = 2517
        SolidEndmillChatter = 2935
    End Enum



    Public Shared Function GetModelDetails(ByVal ModelNum As Integer) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, ModelNum))

            dt = oSql.ExecuteSPReturnDT("USP_GetModelDetails", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function SetNewGPNUM_ISONumbetr(dtModel As DataTable) As DataTable
        Try
            For Each r As DataRow In dtModel.Rows
                If r.Item("GPNUM_ISO_Alternate") <> "0" Then
                    r.Item("GPNUM_ISO") = r.Item("GPNUM_ISO_Alternate")
                End If
            Next
            Return dtModel
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetModelParameters(ByVal ModelNum As Integer, ChangeLabel As Boolean) As DataTable
        Dim dtModel As DataTable
        dtModel = CacheManager.GetDataTable(CacheManager.Keys.Parameters, "glb_TToolGeometricParameter", " ModelNum=" & ModelNum & " ", "TabIndex", "*")
        If Not dtModel Is Nothing AndAlso dtModel.Rows.Count > 0 Then
            dtModel = SetNewGPNUM_ISONumbetr(dtModel)

        End If
        Try
            Dim dtCat As DataTable = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation._dtParamsFromCatalog, "")

            If ChangeLabel Then
                If Not dtModel Is Nothing AndAlso Not dtCat Is Nothing Then
                    For Each rM As DataRow In dtModel.Rows
                        For Each rC As DataRow In dtCat.Rows
                            If rM.Item("GPNUM_ISO").ToString.Trim = rC.Item("GPNUM_ISO").ToString.Trim AndAlso rC.Item("GPNUM_ISO").ToString.Trim <> "" Then
                                'rM.Item("Label") = rC.Item("GPDSC_ISO").ToString.Trim & " (" & rC.Item("GIPRGP_ISO").ToString.Trim & ")"
                                'rM.Item("Label") = rM.Item("Label") & " (" & rC.Item("GIPRGP_ISO").ToString.Trim & ")"
                                If Not rM.Item("Label").ToString.Contains("(") Then
                                    rM.Item("Label") = rM.Item("Label") & " (" & rM.Item("CostName").ToString.Trim & ")"
                                End If


                                Exit For
                            End If
                        Next
                    Next
                End If

            End If
        Catch ex As Exception

        End Try


        Return dtModel

    End Function

    Public Shared Function GetModelRules(ByVal ModelNum As Integer) As DataTable
        Dim dt As DataTable
        dt = CacheManager.GetDataTable(CacheManager.Keys.Rules, "glb_TToolGeometricRules", " ModelNum=" & ModelNum & " AND (Operation <> 'visible' OR Operation IS NULL) ", "OrderForHelp", "*")
        Return dt
    End Function

    Public Shared Function GetModelRelations(ByVal ModelNum As Integer, ByVal CurrentParameterIndex As Integer) As DataTable
        'Dim dt As DataTable
        'dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & " AND RelationRuleNum=" & CurrentParameterIndex & " AND (BranchNum= " & StateManager.GetValue(StateManager.Keys.BranchNumber) & " OR BranchNum=0)", "LabelNum", "*")
        'If dt.Rows.Count = 0 Then
        '    dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & "AND RelationRuleNum=" & CurrentParameterIndex & " ", "LabelNum", "*")
        'Else
        '    Dim i As Int16 = 1
        'End If
        'Return dt

        Dim dt As DataTable
        'dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & " AND RelationRuleNum=" & CurrentParameterIndex & " AND BranchNum= " & StateManager.GetValue(StateManager.Keys.s_BranchNumber), "LabelNum", "*")
        'dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & " AND RelationRuleNum=" & CurrentParameterIndex & " AND BranchNum= 0", "LabelNum", "*")

        dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & " AND BranchNum= 0", "LabelNum", "*")

        If dt.Rows.Count = 0 Then
            'dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & "AND RelationRuleNum=" & CurrentParameterIndex & " AND BranchNum=0", "LabelNum", "*")
            dt = CacheManager.GetDataTable(CacheManager.Keys.Relations, "glb_TRelationsBetweenRulels", " ModelNum=" & ModelNum & "AND BranchNum=0", "LabelNum", "*")
        End If
        Return dt


    End Function



    Public Shared Function GetParamsArrayByVal(UniqId As String, OpenType As Integer, QuotationNo As Integer, BranchCode As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@ModelNum", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, UniqId))
            oParams.Add(New SqlParam("@OpenType", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, OpenType))
            oParams.Add(New SqlParam("@QuotationNo", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, QuotationNo))
            oParams.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptChar, SqlParam.ParamDirection.pdInput, BranchCode))

            dt = oSql.ExecuteSPReturnDT("USP_GetParamsArrayNew", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetModelList_ModelsFormFamily(MainFamily As String, ModelType As String, ToolType As String) As DataTable
        Try 'ModelType As String, MainModel As Integer
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            'oParams.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelType))
            'oParams.Add(New SqlParam("@MainModel", SqlParam.ParamType.ptInt, SqlParam.ParamDirection.pdInput, MainModel))

            oParams.Add(New SqlParam("@MainFamily", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, MainFamily))
            oParams.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelType))
            oParams.Add(New SqlParam("@ToolType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ToolType))

            'dt = oSql.ExecuteSPReturnDT("USP_GetModelList", oParams)
            dt = oSql.ExecuteSPReturnDT("USP_GetMainFamilyModelList", oParams)


            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetFamilylist(ModelType As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelType))

            dt = oSql.ExecuteSPReturnDT("USP_GetFamilylist", oParams)

            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetModellist(ModelType As String, Scale As String, ModelName As String) As DataTable
        Try
            Dim oSql As New SqlDal(clsBranch.GetSiteConnectionString)
            Dim oParams As New SqlParams()
            Dim dt As DataTable

            oParams.Add(New SqlParam("@ModelType", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelType))
            oParams.Add(New SqlParam("@Scale", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Scale))
            oParams.Add(New SqlParam("@ModelName", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, ModelName))

            dt = oSql.ExecuteSPReturnDT("USP_GetModels_1", oParams)


            Return dt
        Catch ex As Exception
            Throw
        End Try
    End Function
End Class
