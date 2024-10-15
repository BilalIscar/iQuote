<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminData.aspx.vb" Inherits="semiPrj.AdminData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" href="~/App_Themes/LTR/cssSemiGlobal-min.css" />


    <style>
        .GridHeaderStyle {
            font-family: Arial;
            font-size: 14px;
            vertical-align: central;
            background-color: #bfd5f4;
            color: white;
            font-weight: bold;
            border-radius: 6px;
        }

        .headSt {
            color: black;
            font-weight: bold;
            border-style: none;
            height: 25px;
            text-align: center;
            font-weight: 700;
            background-color:lightgray;
        }

        .itmSt {
            border-bottom-style: solid !important;
            border-top-style: none;
            border-color: #e2e2e2;
            text-align: center;
        }

        .txtSt {
            text-align: center;
            vertical-align: middle;
            border-style: none;
            background-color: Transparent;
            width: 78px;
            height: 30px;
            font-family: Roboto-Regular !important,Calibri,Arial,Script;
            font-size: 14px
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <table style="width: 98%; padding-left:20px; border-style:none; border-color:gray; border-width:thin;">
            <tr>
                <td colspan="3" style="padding-top:10px;padding-bottom:10px; text-align:center;">
                    <span id="lblTitle" class="AlertTitleStyle FontSizeRoboto25">IQUOTE - Constant/Model GP Admin</span>
                </td>
            </tr>
            <tr>
                <td>

             
                        <asp:GridView runat="server" ID="gvConstants" AutoGenerateColumns="False" CssClass="cssGrid_NPrices GridHeaderStyle" GridLines="Both" Width="30%">
                            <HeaderStyle CssClass="cssGridHeaderStyle FontSizeRoboto FontFamilyRoboto divBorderSolidColored " VerticalAlign="Middle" HorizontalAlign="Center" BorderColor="#e3e3e3" Wrap="true" Height="28px" />
                            <RowStyle CssClass="divBorderSolidColored cssGridRowStylePrice FontSizeRoboto FontFamilyRoboto" BorderStyle="Solid" Height="30px" />
                            <AlternatingRowStyle BackColor="White" />
                            <SortedAscendingCellStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="ModelNum">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtModelNum" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Const Name">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtConstName" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Const Value">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtConstValue" CssClass="txtSt" Enabled="true" AutoCompleteType="None" BorderStyle="Solid" Height="25px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Work CenterID">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtWorkCenterID" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Unit">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtUnit" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="display: none">
                            <asp:GridView runat="server" ID="gvConstants_CONST" AutoGenerateColumns="true" CssClass="cssGrid_NPrices" GridLines="Both" Width="30%">
                            </asp:GridView>
                        </div>
                        <div>
                            <br />
                            <asp:Button runat="server" CssClass="headSt" Text="Update Constants" ID="btnUpdateConstants" Width="200px" BackColor="#000066" ForeColor="White" />
                        </div>
                        <br />


                  
                </td>
                <td style="vertical-align: top">
               
                        <asp:GridView runat="server" ID="gvConstants_QTY" AutoGenerateColumns="False" CssClass="cssGrid_NPrices GridHeaderStyle" GridLines="Both" Width="30%">
                            <HeaderStyle CssClass="cssGridHeaderStyle FontSizeRoboto FontFamilyRoboto divBorderSolidColored " VerticalAlign="Middle" HorizontalAlign="Center" BorderColor="#e3e3e3" Wrap="true" Height="28px" />
                            <RowStyle CssClass="divBorderSolidColored cssGridRowStylePrice FontSizeRoboto FontFamilyRoboto" BorderStyle="Solid" Height="30px" />
                            <AlternatingRowStyle BackColor="White" />
                            <SortedAscendingCellStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="ModelNum">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtModelNum" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Const Name">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtConstName" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Const Value">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtConstValue" CssClass="txtSt" Enabled="true" AutoCompleteType="None" BorderStyle="Solid" Height="25px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Work CenterID">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtWorkCenterID" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Unit">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtUnit" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="MaxQTY">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtMaxQTY" CssClass="txtSt" Enabled="true" AutoCompleteType="None" BorderStyle="Solid" Height="25px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="display: none">
                            <asp:GridView runat="server" ID="gvConstants_CONST_QTY" AutoGenerateColumns="true" CssClass="cssGrid_NPrices" GridLines="Both" Width="30%">
                            </asp:GridView>
                        </div>
                        <div>
                            <br />
                            <asp:Button runat="server" CssClass="headSt" Text="Update Constants Quantity" ID="btnUpdateConstantsQty" Width="200px" BackColor="#000066" ForeColor="White" />
                        </div>
                        <br />


                  
                </td>





                <td style="vertical-align: top">
                  
                        <asp:GridView runat="server" ID="gvModelGP" AutoGenerateColumns="False" CssClass="cssGrid_NPrices GridHeaderStyle" GridLines="Both" Width="30%">
                            <HeaderStyle CssClass="cssGridHeaderStyle FontSizeRoboto FontFamilyRoboto divBorderSolidColored " VerticalAlign="Middle" HorizontalAlign="Center" BorderColor="#e3e3e3" Wrap="true" Height="28px" />
                            <RowStyle CssClass="divBorderSolidColored cssGridRowStylePrice FontSizeRoboto FontFamilyRoboto" BorderStyle="Solid" Height="30px" />
                            <AlternatingRowStyle BackColor="White" />
                            <SortedAscendingCellStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Model Number">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtModelNum" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Branch Number">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBranchNumber" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Branch Name">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBranchName" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Branch Code">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBranchCode" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Brande Name">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBrandeName" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Branch Type">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtBranchType" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Continent Code">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtContinentCode" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="GP Type">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtGPType" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="Customer Type">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtCustomerType" CssClass="txtSt" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="GP Value">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtGPValue" CssClass="txtSt" Enabled="true" AutoCompleteType="None" BorderStyle="Solid" Height="25px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-CssClass="itmSt" HeaderStyle-CssClass="headSt" HeaderText="MaxQTY">
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" ID="txtMaxQTY" CssClass="txtSt" Enabled="true" AutoCompleteType="None" BorderStyle="Solid" Height="25px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="display: none">
                            <asp:GridView runat="server" ID="gvModelGP_Const" AutoGenerateColumns="true" CssClass="cssGrid_NPrices" GridLines="Both" Width="30%">
                            </asp:GridView>
                        </div>
                        <div>
                            <br />
                            <asp:Button runat="server" CssClass="headSt" Text="Update Model Branch GP" ID="btnUpdateModelGP" Width="200px" BackColor="#000066" ForeColor="White" />
                        </div>
                        <br />


                   
                </td>


            </tr>
        </table>
                            <asp:Button runat="server" CssClass="headSt" Text="Refresh" ID="btnR" Width="200px" Visible="false" />

    </form>
</body>
</html>
