<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="wucTab.ascx.vb" Inherits="semiPrj.wucTab" %>

<style>
    .rowMnu {
        /*--bs-gutter-x: 1.5rem;*/
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(-1 * var(--bs-gutter-y));
            margin-right: calc(-.5 * var(--bs-gutter-x));
            margin-left: calc(-.5 * var(--bs-gutter-x))
    }

    .col-sm-N {
        flex: 0 0 auto;
        width: 25%
    }

    .btnTabCls {
        text-align: left;
        background-repeat: no-repeat;
        width: 100%;
        height: 60px;
        border-style: none;
        padding-top: 0px;
        margin-top: 0px;
        /*position: relative;*/
        /*margin-left: -4px;
        margin-right: 0px;*/
        vertical-align: top;
        background-size: 100%;
        background-color: #e9eaec;
        margin-left: 0px;
        border-left-style: solid;
        border-left-width: thin;
        border-left-color: lightgray;
    }

    .btnTabClsLast {
        text-align: left;
        background-repeat: no-repeat;
        width: 100%;
        height: 60px;
        border-right-color: #e9eaec;
        border-right-style: none;
        border-left-style: none;
        border-bottom-style: none;
        border-top-style: none;
        /*padding-left: 20px;*/
        padding-top: 0px;
        margin-top: 0px;
        /*position: relative;*/
        /*margin-left: -4px;*/
        /*margin-right: 0px;*/
        vertical-align: top;
        background-color: #e9eaec;
        border-left-style: solid;
        border-left-width: thin;
        border-left-color: lightgray;
    }

    .LabelTitle {
        width: 100%;
        font-size: 18px;
        font-weight: 400 !important;
        border-style: none;
        font-family: Oswald,Calibri,Arial !important;
    }

    .LabelTitleSub {
        font-size: 12px;
        vertical-align: middle;
        text-align: left;
        width: 100%;
        height: 80px;
        /*padding-left: 38px;*/
        color: #1d5095 !important;
        font-weight: 400 !important;
        border-style: solid;
        border-width: 1px;
        border-color: transparent;
        font-family: Roboto,Calibri,Arial !important;
    }


    .btnInsideTab {
        background-repeat: no-repeat;
        background-size: 45px;
        height: 40px;
        background-position-x: 16px;
        border-style: none;
        padding-left: 50px;
        /*background-size:40px;*/
        background-position-x: left;
        margin-left: 6px;
    }

    .cssDisplayCurrentNone {
        display: none;
    }


    .lblProductTypeCss {
        display: block !important;
    }

    @media screen and (max-width: 500px) {

        .lblProductTypeCss {
            display: none !important;
        }
    }





    .DivSmallWidth {
    }

    @media screen and (max-width: 500px) {

        .DivSmallWidth {
            width: 25%
        }
    }
</style>

<script>
    function RunProductType() {

        btn = document.getElementById("ContentPlaceHolderMain_wucTabs_btnRunPT");
        btn.click();
        return true;
    }

    function RunMaterial() {

        btn = document.getElementById("ContentPlaceHolderMain_wucTabs_btnRunMT");
        btn.click();
        return true;
    }

    function RunBuilder() {

        btn = document.getElementById("ContentPlaceHolderMain_wucTabs_btnRunBL");
        btn.click();
        return true;
    }
</script>


<div style="padding-top: 10px;" class="mainLeftRightPadding">
    <div class="rowMnu">
        <div class="col-sm-N DivSmallWidth">
            <button type="button" runat="server" id="btnProductType" class="img-fluid img-thumbnail btnTabCls " style="margin-left: 1px;" onclick="cancelonbeforeunload(); RunProductType()">
                <div runat="server" id="divModel" style="background-image: url('../media/TabImage/icon_ProductType_active.svg'); background-size: 40px; margin-left: 10px" class="btnInsideTab">
                    <div>
                        <asp:Label ID="lblProductType" runat="server" CssClass="LabelTitle lblProductTypeCss"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblProductTypeTEXT" runat="server" CssClass="LabelTitleSub lblProductTypeCss"></asp:Label>
                    </div>
                </div>
            </button>
        </div>


        <div class="col-sm-N DivSmallWidth">
            <button type="button" runat="server" id="btnMaterial" class="img-fluid img-thumbnail btnTabCls " onclick="cancelonbeforeunload(); RunMaterial()">
                <div runat="server" id="divMaterial" style="background-image: url('../media/TabImage/icon_ProductType_active.svg'); background-size: 40px;" class="btnInsideTab">
                    <div>
                        <asp:Label ID="lblMaterial" runat="server" CssClass="LabelTitle lblProductTypeCss"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblMaterialTEXT" runat="server" CssClass="LabelTitleSub lblProductTypeCss"></asp:Label>
                    </div>
                </div>
            </button>
        </div>

        <div class="col-sm-N DivSmallWidth">
            <button type="button" runat="server" id="btnParameters" class="img-fluid img-thumbnail btnTabCls" onclick="cancelonbeforeunload(); RunBuilder()">
                <div runat="server" id="divParameters" style="background-image: url('../media/TabImage/icon_ProductType_active.svg'); background-size: 40px;" class="btnInsideTab">
                    <div>
                        <asp:Label ID="lblParameters" runat="server" CssClass="LabelTitle lblProductTypeCss"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblParametersTEXT" runat="server" Text="" CssClass="LabelTitleSub lblProductTypeCss"></asp:Label>
                    </div>
                </div>
            </button>
        </div>

        <div class="col-sm-N DivSmallWidth">
            <button runat="server" id="btnGetQuotation" class="img-fluid img-thumbnail btnTabClsLast" title="Modify" onclick="cancelonbeforeunload();">
                <div runat="server" id="divQuotations" style="background-image: url('../media/TabImage/icon_ProductType_active.svg'); background-size: 40px;" class="btnInsideTab">
                    <div>
                        <asp:Label ID="lblGetQuotation" runat="server" CssClass="LabelTitle lblProductTypeCss"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblGetQuotationTEXT" runat="server" Text="" CssClass="LabelTitleSub lblProductTypeCss"></asp:Label>
                    </div>
                </div>
            </button>
        </div>
    </div>
</div>

<asp:Button ID="btnRunMT" runat="server" CssClass="cssDisplayCurrentNone" Text="" />
<asp:Button ID="btnRunBL" runat="server" CssClass="cssDisplayCurrentNone" Text="" />
<asp:Button ID="btnRunPT" runat="server" CssClass="cssDisplayCurrentNone" Text="" />