<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainQuotationDataReport.aspx.vb" Inherits="semiPrj.MainQuotationDataReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="scm"></asp:ScriptManager>
        <div>
            <asp:Button runat="server" ID="btnShow" Text="Show Report" Visible="false" />
            <asp:TextBox ID="txtQuotationNumber" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtBranchCode" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtAS400_Line" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtBranchNum" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtAS400Year" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtLanguageId" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtReportType" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtStorageFolder" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtExpirationDate" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtQuotationDate" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtDateCultureStartWith" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtNumberSuparator" runat="server" Visible="false"></asp:TextBox>
  <asp:TextBox ID="txtselectedLanguage" runat="server" Visible="false"></asp:TextBox>
            

            <rsweb:ReportViewer ID="ReportViewer1" runat="server">
                <LocalReport ReportPath="Reports\MainQuotationDataReport.rdlc" EnableExternalImages="True">
                    <DataSources>

                        <%--<rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ds_Main_Data" />--%>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="ds_MainQuotation_Data" />
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" Name="ds_FormulaData" />
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource4" Name="ds_QuotationPrices" />
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource5" Name="ds_QuotationParams" />
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource6" Name="ds_QuotationParamsR" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>



            <%--            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ds_MainQutDataTableAdapters.USP_ReportMainDataTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtBranchCode" Name="BranchCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtLanguageId" Name="LanguageId" PropertyName="Text" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>--%>


            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ds_MainQutDataTableAdapters.USP_ReportQuotationMainDataTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtBranchCode" Name="BranchCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtQuotationNumber" Name="QuotationNum" PropertyName="Text" Type="Int32" />
                    <asp:ControlParameter ControlID="txtselectedLanguage" Name="SelectedBranchCode" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ds_MainQutDataTableAdapters.USP_ReportFormulaTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtBranchCode" Name="BranchCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtQuotationNumber" Name="QuotationNum" PropertyName="Text" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ds_MainQutDataTableAdapters.USP_ReportQuotationPricesTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtBranchCode" Name="BranchCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtQuotationNumber" Name="QuotationNum" PropertyName="Text" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource ID="ObjectDataSource5" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ds_MainQutDataTableAdapters.USP_ReportQuotationParamsTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtBranchCode" Name="BranchCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtQuotationNumber" Name="QuotationNum" PropertyName="Text" Type="Int32" />
                    <asp:ControlParameter ControlID="txtLanguageId" Name="LANGUAGE_ID" PropertyName="Text" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource ID="ObjectDataSource6" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="ds_MainQutDataTableAdapters.USP_ReportQuotationParamsRTableAdapter">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtBranchCode" Name="BranchCode" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtQuotationNumber" Name="QuotationNum" PropertyName="Text" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </div>
    </form>
</body>
</html>
