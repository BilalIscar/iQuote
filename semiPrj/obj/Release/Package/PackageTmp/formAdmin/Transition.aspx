<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="Transition.aspx.vb" Inherits="semiPrj.Transition" EnableEventValidation="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .cssGridParams {
            font-family: Arial;
            font-size: 12px;
            vertical-align: top;
            border: none 0px #E2E2E2;
        }

        .ButtonControlcss {
            text-align: center;
            background-color: #cef5f6;
            border-radius: 4px;
            /*text-decoration:underline;*/
        }



        .GridHeaderStyle {
            font-family: Arial;
            font-size: 14px;
            vertical-align: central;
            background-color: #f5951c;
            color: white;
            font-weight: bold;
            border-radius: 6px;
        }

        .cssBoundFieldDisplayNone {
            display: none;
            visibility: hidden;
        }

        .display_non {
            display: none;
        }


        .Label_A_16_Bold {
            border: none 0px black;
            font: bold 15px Arial;
            text-align: left;
            white-space: nowrap;
        }

        .lbbor {
            border-style: solid;
            border-width: 2px;
            border-color: lightgray;
            width: 200px;
            margin: 1px;
            background-color: #f6f1f1
        }

        .lbborEN {
            border-style: solid;
            border-width: 2px;
            border-color: lightgray;
            width: 200px;
            margin: 1px;
            background-color: white
        }

        .lbborTitle {
            border-style: none;
            border-width: 1px;
            border-color: lightgray;
            width: 200px;
            padding-bottom: 10px;
            background-color: white;
            font-weight: bold;
        }

        .rdX_non {
            display: none;
            width: 20px;
            border: none
        }

        .padL {
            padding-left: 10px;
            text-align: left
        }

        .verM {
            vertical-align: middle
        }
    </style>

    <script>
        function ShowFpanel() {
     
            document.getElementById("pnlRd").style.display = 'block';
        }
    </script>
</head>
<body>
    <form id="formTr" runat="server" class="padL">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <table border="0" style="width: 100%">
            <tr>

                <td style="width: 50%; vertical-align: top">
                    <table class="maincontent">
                        <tr>
                            <td style="vertical-align: top;">
                                <table style="width: 500px;" border="0">
                                    <tr>
                                        <td>

                                            <asp:Panel ID="PanelToHide4" runat="server">
                                                <table style="border: 2px solid #f2bd81; width: 400px; vertical-align: central; padding-left: 10px;">
                                                    <tr>
                                                        <td>MM&nbsp;
                        <asp:RadioButton runat="server" ID="rb_Hmm" GroupName="genderMMINCH" Checked="true" AutoPostBack="true" />
                                                        </td>
                                                        <td style="width: 50px;"></td>
                                                        <td>Inch&nbsp;
                        <asp:RadioButton runat="server" ID="rb_HInch" GroupName="genderMMINCH" AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>

                                            <asp:Panel ID="paneltoHide3C" runat="server" CssClass="">

                                                <table style="border: 2px solid #f2bd81; width: 400px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label Text="MM/Inch" runat="server" CssClass="Label_A_16_Bold" Visible="false"></asp:Label>
                                                            <asp:RadioButton runat="server" ID="rbMMmain" GroupName="genderNNINCH" Checked="true" AutoPostBack="true" Text="MM" CssClass="Label_A_16_Bold" />
                                                            &nbsp;
                    <asp:RadioButton runat="server" ID="rbInchmain" GroupName="genderNNINCH" AutoPostBack="true" Text="Inch" CssClass="Label_A_16_Bold" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="border: 2px solid #f2bd81; width: 400px">
                                                <tr>
                                                    <td>Model</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFamily_MOD" runat="server" AutoPostBack="true" Width="180"></asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>lang</td>
                                                    <td>
                                                        <asp:TextBox ID="lang_MOD" runat="server" Text="en"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>vers</td>
                                                    <td>
                                                        <asp:TextBox ID="vers_MOD" runat="server" Text="M"></asp:TextBox><br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>family</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlModel_MOD" runat="server" AutoPostBack="true" Width="180"></asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>tool</td>
                                                    <td>
                                                        <asp:TextBox ID="sTool_MOD" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>item No.</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlModel_Items_MOD" runat="server" AutoPostBack="true"></asp:DropDownList><br />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="StartModification" runat="server" Text="Modification" Font-Bold="true" Font-Names="Arial" Font-Size="14px" ForeColor="DarkBlue" BackColor="#e9e9e9" />
                                                        <asp:Button ID="QuotationListMod" runat="server" Text="Configuratur" Font-Bold="true" Font-Names="Arial" Font-Size="14px" ForeColor="DarkBlue" BackColor="#e9e9e9" />

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top; vertical-align: top">
                                <asp:Panel ID="paneltoHide1" runat="server">

                                    <table style="border: 2px solid #f2bd81; width: 400px">
                                        <tr>
                                            <td>lang</td>
                                            <td>
                                                <asp:TextBox ID="lang" runat="server" Text="en"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>vers</td>
                                            <td>
                                                <asp:TextBox ID="vers" runat="server" Text="M"></asp:TextBox><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Family Picture</td>
                                            <td>
                                                <asp:TextBox ID="txtFamilyPic" runat="server" Text=""></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>tool</td>
                                            <td>
                                                <asp:TextBox ID="sTool" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>item No.</td>
                                            <td>
                                                <asp:TextBox ID="itemNumbe" runat="server" Text=""></asp:TextBox><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="startConfig" runat="server" Text="Configuratur" Font-Bold="true" Font-Names="Arial" Font-Size="14px" ForeColor="DarkBlue" BackColor="#e9e9e9" />&nbsp;&nbsp;
                                            </td>
                                        </tr>

                                    </table>

                                </asp:Panel>

                                <asp:Panel ID="paneltoHide2" runat="server">
                                    <br />
                                    <table style="border: 2px solid #f2bd81; width: 400px">


                                        <tr>
                                            <td style="width: 136px">Calc Mode</td>
                                            <td>
                                                <asp:DropDownList ID="ddlCalcMode" runat="server" AutoPostBack="true" Width="100px"></asp:DropDownList></td>
                                        </tr>

                                    </table>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="paneltoHide3" runat="server" CssClass="padL">

                                    <table style="border: 2px solid #f2bd81;">

                                        <tr style="height: 4px">
                                            <td colspan="5" style="height: 4px;"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlRd" runat="server" CssClass="" Width="800px">
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <table style="border: 0px solid #f2bd81;">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnConnectToGal" runat="server" Text="Connect" CssClass="ButtonControlcss" BackColor="DarkBlue" ForeColor="White" /></td>
                                                        <td><font color="blue">User email address</font></td>
                                                        <td>

                                                            <asp:TextBox ID="txtMailAddress" runat="server" Text="@iscar.co.il"></asp:TextBox>&nbsp;
                    <asp:TextBox Width="400px" BorderStyle="None" ForeColor="Red" Font-Bold="true" ID="txtMailAddressNote" runat="server" Text="Error update user data in GAL"></asp:TextBox>
                                                            <asp:Panel runat="server" ID="pnlRado">

                                                                <asp:RadioButton ID="rbEmailMickiv" runat="server" GroupName="rbgrp" Text="Micki" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbEmailNataly" runat="server" GroupName="rbgrp" Text="Nataly" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbEmailTamar" runat="server" GroupName="rbgrp" Text="Tamar" AutoPostBack="true" />
                                                                <asp:RadioButton ID="rbEmailBilal" runat="server" GroupName="rbgrp" Text="Bilal" AutoPostBack="true" />
                                                            </asp:Panel>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel Width="100%" Height="300px" runat="server" ScrollBars="Horizontal">
                                    <asp:GridView runat="server" CssClass="cssGridParams" ID="dgvItems" AutoGenerateColumns="False" ShowHeader="true" HeaderStyle-CssClass="GridHeaderStyle" Width="100%">
                                        <AlternatingRowStyle BackColor="#ffe5ca" />
                                        <Columns>
                                            <asp:BoundField DataField="GISEQ" HeaderText="GISEQ" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="GISC" HeaderText="GISC" ItemStyle-HorizontalAlign="Center" />
                                            <asp:ButtonField ButtonType="Button" CommandName="GICAT" DataTextField="GICAT" HeaderText="GICAT" ControlStyle-CssClass="ButtonControlcss" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="GIFNUM" HeaderText="GIFNUM" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="GIDSCO" HeaderText="GIDSCO" />
                                            <asp:BoundField DataField="GIIC" HeaderText="GIIC" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="GIPRNM" HeaderText="GIPRNM" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>


                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                            <td style="vertical-align: top;">
                                <table style="border: 2px solid #f2bd81; margin-left: 10px">
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <asp:Image ID="imgLine" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                    <br />

                </td>

            </tr>
        </table>


    </form>
</body>
</html>
