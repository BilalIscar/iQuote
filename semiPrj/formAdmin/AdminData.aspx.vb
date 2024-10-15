Imports AjaxControlToolkit.HTMLEditor.ToolbarButton
Imports SemiApp_bl
Public Class AdminData
    Inherits System.Web.UI.Page

    Private Enum e_ConstGrid
        ModelNum = 0
        ConstName = 1
        ConstValue = 2
        WorkCenterId = 3
        Unit = 4
    End Enum
    Private Enum e_ConstGridQty
        ModelNum = 0
        ConstName = 1
        ConstValue = 2
        WorkCenterId = 3
        Unit = 4
        MaxQty = 5
    End Enum
    Private Enum e_ModelBranchGP
        ModelNum = 0
        BranchNumber = 1
        BranchName = 2
        BranchCode = 3
        BrandeName = 4
        BranchType = 5
        ContinentCode = 6
        GPType = 7
        CustomerType = 8
        GPValue = 9
        MaxQTY = 10

    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                FillConstantGrid()
                FillConstantGridQty()
                FillModelBranchGP
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillConstantGrid()
        Try
            Dim dt As DataTable = clsAdminData.Get_Constants()

            gvConstants.DataSource = dt
            gvConstants.DataBind()

            Dim dt_CONST As DataTable = clsAdminData.Get_Constants()

            gvConstants_CONST.DataSource = dt_CONST
            gvConstants_CONST.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillConstantGridQty()
        Try
            Dim dt As DataTable = clsAdminData.Get_ConstantsQty()

            gvConstants_QTY.DataSource = dt
            gvConstants_QTY.DataBind()

            Dim dt_CONST As DataTable = clsAdminData.Get_ConstantsQty()

            gvConstants_CONST_QTY.DataSource = dt_CONST
            gvConstants_CONST_QTY.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub FillModelBranchGP()
        Try
            Dim dt As DataTable = clsAdminData.Get_ModelBranchGP

            gvModelGP.DataSource = dt
            gvModelGP.DataBind()

            Dim dt_CONST As DataTable = clsAdminData.Get_ModelBranchGP

            gvModelGP_Const.DataSource = dt_CONST
            gvModelGP_Const.DataBind()
        Catch ex As Exception

        End Try
    End Sub


    Private Sub UpdateConstants()
        Try
            Me.Focus()
            Dim i As Integer = 0
            For Each r As GridViewRow In gvConstants.Rows

                Dim viewConstValue As String = CType(r.Cells(e_ConstGrid.ConstValue).FindControl("txtConstValue"), TextBox).Text.ToString.Trim
                Dim viewConstName As String = CType(r.Cells(e_ConstGrid.ConstName).FindControl("txtConstName"), TextBox).Text.ToString.Trim
                Dim viewModelNum As String = CType(r.Cells(e_ConstGrid.ModelNum).FindControl("txtModelNum"), TextBox).Text.ToString
                Dim hideModelNum As String = gvConstants_CONST.Rows(i).Cells(e_ConstGrid.ModelNum).Text.ToString
                Dim hideConstValue As String = gvConstants_CONST.Rows(i).Cells(e_ConstGrid.ConstValue).Text.ToString
                Dim hideConstName As String = gvConstants_CONST.Rows(i).Cells(e_ConstGrid.ConstName).Text.ToString

                If viewModelNum = hideModelNum Then
                    If viewConstValue <> "" AndAlso IsNumeric(viewConstValue) AndAlso viewConstName = hideConstName AndAlso viewConstValue <> hideConstValue Then
                        clsAdminData.UpdateQuotationsConstants(viewModelNum, viewConstValue, viewConstName)
                    End If
                End If
                i += 1
            Next

            FillConstantGrid()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub UpdateConstantsQty()
        Try
            Me.Focus()
            Dim i As Integer = 0
            For Each r As GridViewRow In gvConstants_QTY.Rows

                Dim viewModelNum As String = CType(r.Cells(e_ConstGridQty.ModelNum).FindControl("txtModelNum"), TextBox).Text.ToString
                Dim hideModelNum As String = gvConstants_CONST_QTY.Rows(i).Cells(e_ConstGridQty.ModelNum).Text.ToString

                Dim viewConstValue As String = CType(r.Cells(e_ConstGridQty.ConstValue).FindControl("txtConstValue"), TextBox).Text.ToString.Trim
                Dim viewConstName As String = CType(r.Cells(e_ConstGridQty.ConstName).FindControl("txtConstName"), TextBox).Text.ToString.Trim
                Dim viewMaxQty As String = CType(r.Cells(e_ConstGridQty.ConstName).FindControl("txtMaxQTY"), TextBox).Text.ToString.Trim

                Dim hideConstValue As String = gvConstants_CONST_QTY.Rows(i).Cells(e_ConstGridQty.ConstValue).Text.ToString
                Dim hideConstName As String = gvConstants_CONST_QTY.Rows(i).Cells(e_ConstGridQty.ConstName).Text.ToString
                Dim hideMaxQty As String = gvConstants_CONST_QTY.Rows(i).Cells(e_ConstGridQty.MaxQty).Text.ToString

                If viewConstName = hideConstName AndAlso viewModelNum = hideModelNum Then
                    If (viewConstValue <> "" AndAlso IsNumeric(viewConstValue) AndAlso viewConstValue <> hideConstValue) Or (viewMaxQty <> "" AndAlso IsNumeric(viewMaxQty) AndAlso viewMaxQty <> hideMaxQty) Then
                        clsAdminData.UpdateQuotationsConstantsQTY(viewModelNum, viewConstValue, viewConstName, viewMaxQty)
                    End If
                End If

                i += 1
            Next

            FillConstantGridQty()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UpdateModelBranchGP()
        Try
            Me.Focus()
            Dim i As Integer = 0
            For Each r As GridViewRow In gvModelGP.Rows
                Dim viewModelNum As String = CType(r.Cells(e_ModelBranchGP.ModelNum).FindControl("txtModelNum"), TextBox).Text.ToString
                Dim hideModelNum As String = gvModelGP_Const.Rows(i).Cells(e_ModelBranchGP.ModelNum).Text.ToString
                Dim viewBranchNumber As String = CType(r.Cells(e_ModelBranchGP.BranchNumber).FindControl("txtBranchNumber"), TextBox).Text.ToString
                Dim hideBranchNumber As String = gvModelGP_Const.Rows(i).Cells(e_ModelBranchGP.BranchNumber).Text.ToString
                Dim viewGPType As String = CType(r.Cells(e_ModelBranchGP.GPType).FindControl("txtGPType"), TextBox).Text.ToString
                Dim hideGPType As String = gvModelGP_Const.Rows(i).Cells(e_ModelBranchGP.GPType).Text.ToString
                Dim viewCustomerType As String = CType(r.Cells(e_ModelBranchGP.CustomerType).FindControl("txtCustomerType"), TextBox).Text.ToString
                Dim hideCustomerType As String = gvModelGP_Const.Rows(i).Cells(e_ModelBranchGP.CustomerType).Text.ToString

                Dim viewGPValue As String = CType(r.Cells(e_ModelBranchGP.GPValue).FindControl("txtGPValue"), TextBox).Text.ToString.Trim
                Dim viewMaxQTY As String = CType(r.Cells(e_ModelBranchGP.MaxQTY).FindControl("txtMaxQTY"), TextBox).Text.ToString.Trim
                Dim hideGPValue As String = gvModelGP_Const.Rows(i).Cells(e_ModelBranchGP.GPValue).Text.ToString
                Dim hideMaxQTY As String = gvModelGP_Const.Rows(i).Cells(e_ModelBranchGP.MaxQTY).Text.ToString

                If viewModelNum = hideModelNum AndAlso viewBranchNumber = hideBranchNumber AndAlso viewGPType = hideGPType AndAlso viewCustomerType = hideCustomerType Then
                    If (viewGPValue <> "" AndAlso IsNumeric(viewGPValue) AndAlso viewGPValue <> hideGPValue) Or (viewMaxQTY <> "" AndAlso IsNumeric(viewMaxQTY) AndAlso viewMaxQTY <> hideMaxQTY) Then
                        clsAdminData.UpdateModelBranchGP(viewModelNum, viewBranchNumber, viewGPType, viewCustomerType, viewGPValue, viewMaxQTY)
                    End If
                End If
                i += 1
            Next

            FillModelBranchGP()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnUpdateConstants_Click(sender As Object, e As EventArgs) Handles btnUpdateConstants.Command
        'If Not IsPostBack Then
        UpdateConstants()

        'End If
    End Sub



    Private Sub btnUpdateConstantsQty_Click(sender As Object, e As EventArgs) Handles btnUpdateConstantsQty.Click
        UpdateConstantsQty()
    End Sub

    Private Sub gvConstants_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConstants.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                'CType(e.Row.FindControl("lblPriceID"), Label).Text = priceId


                CType(e.Row.FindControl("txtModelNum"), TextBox).Text = gvConstants.DataSource.rows(e.Row.RowIndex).item("ModelNum")
                CType(e.Row.FindControl("txtConstValue"), TextBox).Text = gvConstants.DataSource.rows(e.Row.RowIndex).item("ConstValue")
                CType(e.Row.FindControl("txtWorkCenterID"), TextBox).Text = gvConstants.DataSource.rows(e.Row.RowIndex).item("WorkCenterID")
                CType(e.Row.FindControl("txtUnit"), TextBox).Text = gvConstants.DataSource.rows(e.Row.RowIndex).item("Unit")
                CType(e.Row.FindControl("txtConstName"), TextBox).Text = gvConstants.DataSource.rows(e.Row.RowIndex).item("ConstName")


            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvConstants_QTY_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvConstants_QTY.RowDataBound
        Try


            If e.Row.RowType = DataControlRowType.DataRow Then
                'CType(e.Row.FindControl("lblPriceID"), Label).Text = priceId

                CType(e.Row.FindControl("txtModelNum"), TextBox).Text = gvConstants_QTY.DataSource.rows(e.Row.RowIndex).item("ModelNum")
                CType(e.Row.FindControl("txtConstName"), TextBox).Text = gvConstants_QTY.DataSource.rows(e.Row.RowIndex).item("ConstName")
                CType(e.Row.FindControl("txtConstValue"), TextBox).Text = gvConstants_QTY.DataSource.rows(e.Row.RowIndex).item("ConstValue")
                CType(e.Row.FindControl("txtWorkCenterID"), TextBox).Text = gvConstants_QTY.DataSource.rows(e.Row.RowIndex).item("WorkCenterID")
                CType(e.Row.FindControl("txtUnit"), TextBox).Text = gvConstants_QTY.DataSource.rows(e.Row.RowIndex).item("Unit")
                CType(e.Row.FindControl("txtMaxQTY"), TextBox).Text = gvConstants_QTY.DataSource.rows(e.Row.RowIndex).item("MaxQTY")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub gvModelGP_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvModelGP.RowDataBound
        Try


            If e.Row.RowType = DataControlRowType.DataRow Then
                'CType(e.Row.FindControl("lblPriceID"), Label).Text = priceId

                CType(e.Row.FindControl("txtModelNum"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("ModelNum").ToString
                CType(e.Row.FindControl("txtBranchNumber"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("BranchNumber").ToString
                CType(e.Row.FindControl("txtBranchName"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("BranchName").ToString
                CType(e.Row.FindControl("txtBranchCode"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("BranchCode").ToString
                CType(e.Row.FindControl("txtBrandeName"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("BrandeName").ToString
                CType(e.Row.FindControl("txtBranchType"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("BranchType").ToString
                CType(e.Row.FindControl("txtContinentCode"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("ContinentCode").ToString
                CType(e.Row.FindControl("txtGPType"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("GPType").ToString
                CType(e.Row.FindControl("txtCustomerType"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("CustomerType").ToString
                CType(e.Row.FindControl("txtGPValue"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("GPValue").ToString
                CType(e.Row.FindControl("txtMaxQTY"), TextBox).Text = gvModelGP.DataSource.rows(e.Row.RowIndex).item("MaxQTY").ToString

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnR_Click(sender As Object, e As EventArgs) Handles btnR.Click
        FillConstantGrid()
        FillConstantGridQty()
        FillModelBranchGP()
    End Sub

    Private Sub btnUpdateModelGP_Click(sender As Object, e As EventArgs) Handles btnUpdateModelGP.Click
        UpdateModelBranchGP()
    End Sub
End Class