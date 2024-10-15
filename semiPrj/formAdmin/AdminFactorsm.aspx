<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminFactorsm.aspx.vb" Inherits="semiPrj.AdminFactorsm" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="icon" type="image/ico" href="../media/Icons/IQ.png" />
    <link rel="stylesheet" href="~/App_Themes/LTR/cssSemiGlobal-min.css" />

</head>
  
<style>
    .snippet {
        position: relative;
        background: #fff;
        padding: 2rem 5%;
        margin: 1.5rem 0;
        box-shadow: 0 .4rem .8rem -.1rem rgba(0, 32, 128, .1), 0 0 0 1px #f0f2f7;
        border-radius: .25rem;
    }

    .cssGrid_NPrices {
        vertical-align: top;
        border: none;
        width: 100%;
    }


    @media screen and (max-width: 1300px) {

        .cssGrid_NPrices {
            width: 100%;
            height: 100px;
        }
    }

    .display_non {
        display: none;
    }


    .AdminLabelTitle {
        font-family: 'Roboto',Arial, Script;
        font-size: 14px;
    }

    .colorblue {
        color: blue;
        font-weight: bold
    }

    .colorred {
        color: red;
        font-weight: bold
    }

    .AdminLabelTitleA {
        font-family: 'Roboto',Arial, Script;
        font-size: 16px;
    }

    .AdminLabelTitleB {
        font-family: 'Roboto',Arial, Script;
        font-size: 14px;
        color: blue;
    }

    .AdminButtonTitle {
        font-family: 'Roboto',Arial, Script;
        font-size: 16px;
        background-color: #ffd966;
        /*width: 200px;*/
        text-align: left;
        width: 200px;
        text-align: center;
        height: 30px
    }

    .AdminButtonTitleB {
        font-family: 'Roboto',Arial, Script;
        font-size: 16px;
        background-color: #eeeeb8;
        /*width: 200px;*/
        text-align: left;
        border-style: none;
    }


    .ServerDataLabelcss {
    }

    @media screen and (max-width: 1400px) {

        .ServerDataLabelcss {
            display: none
        }
    }



    .cssGridCell_1Admin {
        font: normal 16px Arial;
        text-align: center;
        vertical-align: middle;
        /*border: solid 1px #D2D2D2;*/
        border-style: none;
        background-color: Transparent;
        width: 78px;
        height: 30px;
    }

    .cssGridCell_ColorAdmin {
        font: normal 14px Arial !important;
        text-align: center;
        vertical-align: middle;
        /*border: solid 1px #D2D2D2;*/
        border: none;
        /*background-color: #FFFF8C;*/
        width: 78px;
        height: 30px;
        color: red;
    }


    .cssGridCell_Color1_1Admin {
        font: normal 14px !important Arial;
        text-align: center;
        vertical-align: middle;
        /*border: solid 1px #D2D2D2;*/
        border: none;
        /*background-color: #FFFF8C;*/
        width: 78px;
        height: 30px;
    }

    .ItemBorderS {
        border-bottom-style: solid;
        border-top-style: none;
        border-left-style: none;
        border-right-style: none;
        border-color: #E2E2E2;
    }

    .cssGrid_NTempItemAdminS {
        font-family: 'Roboto',Arial, Script;
        font-size: 15px;
        color: brown;
        text-align: left;
    }

    .cssGrid_NTempItemAdmin {
        font-family: 'Roboto',Arial, Script;
        font-size: 18px;
        color: brown;
        text-align: left;
    }

    .cssGrid_NTempItem {
        font-family: 'Roboto',Arial, Script;
        font-size: 12px;
        color: brown;
        text-align: left;
    }

    .cssGridHeaderStyleTemp {
        font-family: 'Roboto',Arial, Script;
        font-size: 16px !important;
        background-color: #CEDCEA;
        font-size: 12px;
        color: brown;
        text-align: center;
        width: 1px;
        height: 40px;
    }

    .cssGrid_NTemp {
        font-family: 'Roboto',Arial, Script;
        vertical-align: top;
        border: solid 1px #E2E2E2;
        width: 100%;
    }
</style>




<script>

         function ShowOnlyFormula(DivToHide) {
             document.getElementById("divFormula").style.visibility = "hidden";
             document.getElementById("divFormula").style.height = "0px";

             document.getElementById("divGridGen").style.visibility = "hidden";
             document.getElementById("divGridGen").style.height = "0px";

             document.getElementById("divGridPrices").style.visibility = "hidden";
             document.getElementById("divGridPrices").style.height = "0px";

             document.getElementById("divGridParams").style.visibility = "hidden";
             document.getElementById("divGridParams").style.height = "0px";

             document.getElementById("btnShowFormula").style.fontWeight = "";
             document.getElementById("btnShowGen").style.fontWeight = "";
             document.getElementById("btnShowPrices").style.fontWeight = "";
             document.getElementById("btnShowParameters").style.fontWeight = "";
             document.getElementById("btnShowFormula").style.backgroundColor = "#ffd966";
             document.getElementById("btnShowGen").style.backgroundColor = "#ffd966";
             document.getElementById("btnShowPrices").style.backgroundColor = "#ffd966";
             document.getElementById("btnShowParameters").style.backgroundColor = "#ffd966";

             if (DivToHide == "divFormula") {
                 document.getElementById("divFormula").style.visibility = "visible";
                 document.getElementById("divFormula").style.height = "100%";
                 document.getElementById("btnShowFormula").style.fontWeight = "bold";
                 document.getElementById("btnShowFormula").style.backgroundColor = "#efd8d8";
             }
             else
                 if (DivToHide == "divGridGen") {
                     document.getElementById("divGridGen").style.visibility = "visible";
                     document.getElementById("divGridGen").style.height = "100%";
                     document.getElementById("btnShowGen").style.fontWeight = "bold";
                     document.getElementById("btnShowGen").style.backgroundColor = "#efd8d8";
                 }
                 else
                     if (DivToHide == "divGridPrices") {
                         document.getElementById("divGridPrices").style.visibility = "visible";
                         document.getElementById("divGridPrices").style.height = "100%";
                         document.getElementById("btnShowPrices").style.fontWeight = "bold";
                         document.getElementById("btnShowPrices").style.backgroundColor = "#efd8d8";
                     }
                     else
                         if (DivToHide == "divGridParams") {
                             document.getElementById("divGridParams").style.visibility = "visible";
                             document.getElementById("divGridParams").style.height = "100%";
                             document.getElementById("btnShowParameters").style.fontWeight = "bold";
                             document.getElementById("btnShowParameters").style.backgroundColor = "#efd8d8";
                         }
         }
</script>
<body onload="ShowOnlyFormula('divGridGen')">

    <form id="form1" runat="server" >
         <div style="padding-top: 20px; width: 100%" id="MdIv">


        <div style="padding: 20px 20px 20px 20px; width: 98%; background-color: lightgray;">
            <input id="btnShowGen" type="button" value="Quotation Information" class="AdminButtonTitle" onclick="ShowOnlyFormula('divGridGen')" />
            <input id="btnShowFormula" type="button" value="Formula" class="AdminButtonTitle" onclick="ShowOnlyFormula('divFormula')" />
            <input id="btnShowParameters" type="button" value="Quotation Parameters" class="AdminButtonTitle" onclick="ShowOnlyFormula('divGridParams')" />
            <input id="btnShowPrices" type="button" value="Quotation Prices" class="AdminButtonTitle" onclick="ShowOnlyFormula('divGridPrices')" />
            <asp:Label ID="ServerData" runat="server" CssClass="ServerDataLabelcss"></asp:Label>
            <asp:Label ID="lblItemCat_a" runat="server" Text=" ---- ItemNoCat : " CssClass="ServerDataLabelcss"></asp:Label>
            <asp:Label ID="lblItemNoCat" runat="server"></asp:Label>
            ---- <a href="../forms/CatalogTableDetails.aspx" target="_blank">Show Catalog table</a>
        </div>


        <div id="divFormula" style="padding-left: 20px;">
            <div>
                <table border="0">
                    <tr>
                        <td style="width: 340px;">
                            <asp:Label ID="lblf" runat="server" Text="NET PRICE Formula: ACCORDING TFR Price :" CssClass="AdminLabelTitle colorblue"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblFormulaTFR" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>

                        <td>
                            <asp:Label runat="server" ID="lblFormulaTFR_B" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleB"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="Label1" runat="server" Text="Branch TFR Formula: According TFR Price :" CssClass="AdminLabelTitle colorblue"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblFormulaTFRBRANCH" runat="server" Text="" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>

                        <td>
                            <asp:Label ID="lblFormulaTFRBRANCH_B" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="Label2" runat="server" Text="NET PRICE Formula: According MKT Price:" CssClass="AdminLabelTitle colorred"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblFormulaMKT" runat="server" Text="" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>

                        <td>
                            <asp:Label ID="lblFormulaMKT_B" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="Label3" runat="server" Text="Branch MKT Formula: According MKT Price" CssClass="AdminLabelTitle colorred"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblFormulaMKTBRANCH" runat="server" Text="" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>

                        <td>
                            <asp:Label ID="lblFormulaMKTBRANCH_B" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label>
                        </td>
                    </tr>


                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td style="width: 200px;">
                            <asp:Label ID="Label24" runat="server" Text="Cost Formula:" CssClass="AdminLabelTitle"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblCostFormula" Text="lblCostFormula" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>



                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="Label4" runat="server" Text="Constants Formula:" CssClass="AdminLabelTitle"></asp:Label><br />
                            <input type="button" value="Table: 9. MNT_QtyParametersFactors" class="AdminButtonTitleB" id="btmFactorParamsQTYss" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblConsFormula" Text="lblConsFormula" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>

                        <td>
                            <asp:Label ID="lblConsFormula_B" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="Label7" runat="server" Text="GP Formula:" CssClass="AdminLabelTitle"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblGP" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>

                        <td></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">
                            <asp:Label ID="Label16" runat="server" Text="Description Formula" CssClass="AdminLabelTitle"></asp:Label></td>
                        <td>
                            <asp:Label runat="server" ID="lblDesc" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                        </td>
                    </tr>
                </table>

            </div>

            <br />
            &nbsp;
            <table style="width: 100%;">
                <tr>
                    <td style="vertical-align: top; padding-right: 20px; width: 300px;">
                        <div style="width: 300px">
                            <asp:GridView runat="server" ID="gvFactorsV" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true" Width="300px">
                                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />
                                <Columns>
                                    <asp:BoundField DataField="dc_FactorName" HeaderText="Factor Name" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="dc_FactorValue" HeaderText="Factor Value" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td style="vertical-align: top; padding-right: 20px; " colspan="2">
                        <div>
                            <input type="button" value="Table 13:glb_ModelParametersCoden" class="AdminButtonTitleB" id="btmModelParams" />
                        </div>
                        <div style="width: 1000px">
                            <asp:GridView runat="server" ID="gvModelParametersCode" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true" Width="1000px">
                                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />
                                <Columns>
                                    <asp:BoundField DataField="ModelNum" HeaderText="Model Num" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="ParamCode" HeaderText="Param Code" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="ParamManipulation" HeaderText="Param Manipulation" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Operator" HeaderText="Operator" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="SerachValue" HeaderText="Operator" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />

                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                    <td style="vertical-align: top; padding-right: 20px">
                        <div>
                            <input type="button" value="Factor Params" class="AdminButtonTitleB" id="btmFactorParams" />
                        </div>
                        <div>
                            <asp:GridView runat="server" ID="gvCon" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true" Width="200px">
                                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />
                                <Columns>
                                    <asp:BoundField DataField="paramcode" HeaderText="Param Code" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="pVal" HeaderText="Value" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                </Columns>
                            </asp:GridView>

                        </div>
                    </td>


                </tr>

                <tr>
                    <td colspan="4">&nbsp;<br />

                        <div>
                            <input type="button" value="Table: 9. MNT_QtyParametersFactors" class="AdminButtonTitleB" id="btmFactorParamsQTY" />
                        </div>
                        <div>
                            <asp:GridView runat="server" ID="gvQtyfactors" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true">
                                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />
                                <Columns>
                                    <%--<asp:BoundField DataField="ModelNum" HeaderText="MN" HeaderStyle-Width="50px" />--%>
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <%--<asp:BoundField DataField="OrderSeq" HeaderText="OS" HeaderStyle-Width="50px" />--%>

                                    <asp:BoundField DataField="ModelNum" HeaderText="ModelNum" ItemStyle-CssClass="cssGrid_NTempItem" />
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" ItemStyle-CssClass="cssGrid_NTempItem" />
                                    <asp:BoundField DataField="QtyString" HeaderText="QtyString" ItemStyle-CssClass="cssGrid_NTempItem" />
                                    <asp:BoundField DataField="OrderSeq" HeaderText="Seq" ItemStyle-CssClass="cssGrid_NTempItem" />
                                    <asp:BoundField DataField="FactorValue" HeaderText="Factor Value" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="FactorType" HeaderText="Factor Type" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Cond1" HeaderText="Condition 1" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Param1" HeaderText="Condition 1" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Opr1" HeaderText="Opr 1" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Val1" HeaderText="Val 1" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Cond2" HeaderText="Cond 2" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Param2" HeaderText="Param 2" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Opr2" HeaderText="Opr 2" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Val2" HeaderText="Val 2" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Cond3" HeaderText="Cond 3" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Param3" HeaderText="Param 3" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Opr3" HeaderText="Opr 3" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Val3" HeaderText="Val 3" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Cond4" HeaderText="Cond 4" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Param4" HeaderText="Param 4" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Opr4" HeaderText="Opr 4" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="Val4" HeaderText="Val 4" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="SP_SETUP" HeaderText="SP SETUP" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                    <asp:BoundField DataField="STN_SETUP" HeaderText="STN SETUP" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                   

                                    <asp:BoundField DataField="Condition2" HeaderText="Condition2" ItemStyle-CssClass="cssGrid_NTempItem" />
 <asp:BoundField DataField="FormulaFactorValue" HeaderText="FormulaFactorValue" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>

                </tr>
            </table>
            &nbsp;<br />
            <div>
                <input type="button" value="Table: 8. MNT_ParametersFactors" class="AdminButtonTitleB" id="btmFactorParamsQTYx" />
            </div>
            <asp:GridView runat="server" ID="gvFactors" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true" Width="90%">
                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />
                <Columns>
                    <asp:BoundField DataField="ModelNum" HeaderText="MN" HeaderStyle-Width="50px" />
                    <asp:BoundField DataField="ParamCode" HeaderText="Param Code" />
                    <asp:BoundField DataField="MinValue" HeaderText="Min Value" />
                    <asp:BoundField DataField="MaxValue" HeaderText="Max Value" />
                    <asp:BoundField DataField="OrderSeq" HeaderText="Order Seq" />
                    <asp:BoundField DataField="FactorParam" HeaderText="Factor Param" />
                    <asp:BoundField DataField="FactorValue" HeaderText="Factor Value" />
                    <asp:BoundField DataField="FactorType" HeaderText="Factor Type" />
                    <asp:BoundField DataField="Condition1" HeaderText="Condition1" />
                    <asp:BoundField DataField="Condition2" HeaderText="Condition2" />
                    <asp:BoundField DataField="Condition3" HeaderText="Condition3" />
                    <asp:BoundField DataField="Manipulation" HeaderText="Manipulation" />
                    <asp:BoundField DataField="Scale" HeaderText="Scale" />
                </Columns>
            </asp:GridView>
        </div>

        <br />
        <div id="divGridGen" style="padding-left: 20px;">
            <asp:GridView runat="server" ID="gvGen" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true" Width="600px">
                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="cssGrid_NTempItemAdmin" />
                    <asp:BoundField DataField="Val" HeaderText="Value" ItemStyle-CssClass="cssGrid_NTempItemAdmin" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="cssGrid_NTempItemAdmin" />
                </Columns>
            </asp:GridView>
        </div>






        <div id="divGridPrices" style="padding-left: 20px;">
            <table>
                <tr>
                    <td style="vertical-align: top">
                        <asp:Panel ID="pnl1" runat="server" ScrollBars="Both" Width="900px">
                            <input type="button" value="Quotation Prices" class="AdminButtonTitleB" id="bt9p" /><br />
                            <asp:GridView runat="server" ID="gvPrices" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true">
                                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontFamilyRoboto" Font-Size="16px" />
                                <Columns>
<asp:BoundField DataField="price_id" HeaderText="price_id" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                      <asp:BoundField DataField="btnprice" HeaderText="btnprice" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                    <asp:TemplateField ItemStyle-CssClass="ItemBorderS" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtQuantity" CssClass="cssGridCell_1Admin" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="ItemBorderS" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Net Price">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtNetPrice" CssClass="cssGridCell_Color1_1Admin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="ItemBorderS" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtTotal" CssClass="cssGridCell_Color1_1Admin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Cost" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtCostPrice" CssClass="cssGridCell_ColorAdmin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="GP">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtGP" CssClass="cssGridCell_ColorAdmin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Delivery Weeks">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtDeliveryWeeks" CssClass="cssGridCell_Color1_1Admin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TFR Price" ItemStyle-CssClass="ItemBorderS" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtTFRPrice" CssClass="cssGridCell_Color1_1Admin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="btnAddToCart" HeaderText="btnprice" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                    <asp:BoundField DataField="btnDELETE" HeaderText="btnprice" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                    <asp:BoundField DataField="OrderedQuantity" HeaderText="btnprice" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />

                                    <asp:TemplateField HeaderText="QTYFct-USD" ItemStyle-CssClass="ItemBorderS" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtQTYFct" CssClass="cssGridCell_Color1_1Admin" ReadOnly="true" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            &nbsp;


                            
                        </asp:Panel>
                        <br />


                        <asp:Label runat="server" ID="lblTFRMKT" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>

                        <br />
                        &nbsp;
                    </td>
                    <td style="vertical-align: top">
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" Width="800px">
                            <input type="button" value="Quotation Information" class="AdminButtonTitleB" id="bt9p2" /><br />
                            <asp:GridView runat="server" ID="gvGen1" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true" Width="780px">
                                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" Width="200px" />
                                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" Width="200px" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="cssGrid_NTempItemAdmin" HeaderStyle-Width="200px" ItemStyle-Width="250px" />
                                    <asp:BoundField DataField="Val" HeaderText="Value" ItemStyle-CssClass="cssGrid_NTempItemAdmin" HeaderStyle-Width="200px" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="cssGrid_NTempItemAdmin" HeaderStyle-Width="200px" ItemStyle-Width="400px" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">

                        <asp:Label runat="server" ID="lblFormulaConval" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label><br />
                        <asp:Label runat="server" ID="lblFormulaPriceNet" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label><br />
                        <br />
                        <asp:Label runat="server" ID="lblFormulaPriceNetVal" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label><br />
                        <asp:Label runat="server" ID="lblFormulaCon" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label><br />
                        <table border="0">
                            <tr>
                                <td style="width: 340px;">
                                    <asp:Label ID="lblfV" runat="server" Text="NET PRICE Formula: ACCORDING TFR Price :" CssClass="AdminLabelTitle colorblue"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" ID="lblFormulaTFRV" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>

                                <td>
                                    <asp:Label runat="server" ID="lblFormulaTFR_BV" Text="" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleB"></asp:Label></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 200px;">
                                    <asp:Label ID="Label1V" runat="server" Text="Branch TFR Formula: According TFR Price :" CssClass="AdminLabelTitle colorblue"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFormulaTFRBRANCHV" runat="server" Text="" CssClass="AdminLabelTitleA"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>

                                <td>
                                    <asp:Label ID="lblFormulaTFRBRANCH_BV" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px;">
                                    <asp:Label ID="Label2V" runat="server" Text="NET PRICE Formula: According MKT Price:" CssClass="AdminLabelTitle colorred"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblFormulaMKTV" runat="server" Text="" CssClass="AdminLabelTitleA"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>

                                <td>
                                    <asp:Label ID="lblFormulaMKT_BV" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px;">
                                    <asp:Label ID="Label3V" runat="server" Text="Branch MKT Formula: According MKT Price" CssClass="AdminLabelTitle colorred"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblFormulaMKTBRANCHV" runat="server" Text="" CssClass="AdminLabelTitleA"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>

                                <td>
                                    <asp:Label ID="lblFormulaMKTBRANCH_BV" runat="server" Text="" CssClass="AdminLabelTitleB"></asp:Label>
                                </td>
                            </tr>


                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td style="width: 200px;">
                                    <asp:Label ID="Label24V" runat="server" Text="Cost Formula:" CssClass="AdminLabelTitle"></asp:Label></td>
                                <td>
                                    <asp:Label runat="server" ID="lblCostFormulaV" Text="lblCostFormula" Width="100%" EnableViewState="true" CssClass="AdminLabelTitleA"></asp:Label>
                                </td>
                            </tr>



                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                                </td>
                            </tr>



                            <tr>
                                <td></td>

                                <td></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <div style="height: 1px; width: 100%; border-top-color: lightgray; border-top-style: solid; border-width: thin; padding-top: 8px; padding-bottom: 8px;"></div>
                                </td>
                            </tr>

                        </table>

                    </td>
                </tr>
            </table>



        </div>




        <div id="divGridParams" style="width: 90%; padding-left: 20px;">
            <asp:GridView runat="server" ID="gvParams" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both" ShowHeaderWhenEmpty="true">
                <HeaderStyle CssClass="cssGridHeaderStyleTemp" Wrap="true" BorderColor="#F2F2F2" />
                <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto FontFamilyRoboto" />

                <Columns>
                    <asp:BoundField DataField="TabIndex" HeaderText="TabIndex" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="Label" HeaderText="Label" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="Visible" HeaderText="Visible" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="Formula" HeaderText="Formula" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="Order" HeaderText="Order" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="FormatFormula" HeaderText="Format Formula" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="Measure" HeaderText="Measure" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="StringValue" HeaderText="String Value" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="DrawingField" HeaderText="Drawing Field" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="CostName" HeaderText="Cost Name" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="GPNUM_ISO" HeaderText="GPNUM_ISO" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />
                    <asp:BoundField DataField="DescValue" HeaderText="DescValue" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />

                    <asp:BoundField DataField="PropertyImage" HeaderText="PropertyImage" ItemStyle-CssClass="cssGrid_NTempItemAdminS" />

                </Columns>
            </asp:GridView>

        </div>


             </div>

         <script>
             try {

                 document.getElementById("MdIv").style.visibility = "hidden";
                 var ss = 's';
                 checkCookie()
                 function getCookie(cname) {
                     let name = cname + "=";
                     let ca = document.cookie.split(';');
                     for (let i = 0; i < ca.length; i++) {
                         let c = ca[i];
                         while (c.charAt(0) == ' ') {
                             c = c.substring(1);
                         }
                         if (c.indexOf(name) == 0) {
                             return c.substring(name.length, c.length);
                         }
                     }
                     return "";
                 }
                 function checkCookie() {

                     let urlLink = '';

                     if (window.location.href.includes('localhost'))
                         urlLink = "http://localhost:60377/"
                     else
                         if (window.location.href.includes('iquote.ssl.imc-companies'))
                             urlLink = "https://iquote.ssl.imc-companies.com/"
                         else
                             if (window.location.href.includes('dmstest'))
                                 urlLink = "http://dmstest/iQuote/"
                     let etoedfrku = getCookie("etoedfrku");
                     let HkhtrycdFg = getCookie("HkhtrycdFg");
                     if (etoedfrku != "") {
                         if (etoedfrku != HkhtrycdFg) {
                             window.open(urlLink, "_self");
                             ss = 'a'
                         }
                     }
                 }
             }
             catch (error) {
                 window.open("https://iquote.ssl.imc-companies.com/", "_self");
                 ss = 'a';
             }

             if (ss == 's') {
                 document.getElementById("MdIv").style.visibility = "visible";
             }
         </script>



    </form>
</body>
</html>
