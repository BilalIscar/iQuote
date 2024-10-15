<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masters/SemiSTDMaster.Master" CodeBehind="QuotationsList.aspx.vb" Inherits="semiPrj.QuotationsList" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <%@ Register Src="~/uc/wucTab.ascx" TagName="wuc_Tabs" TagPrefix="wuc_Tabs" %>

    <script>
        function RenewConfirm() {
            $.confirm({
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">Renew Quotation<div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto18" style="width:100%">Quotation has expired.<br>Would you like to renew the quotation?</div>',
                buttons: {
                    Cancel: function () {
                        return true;
                    },
                    Yes: {
                        text: 'Renew',
                        btnClass: 'btn-blueblue',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $("#ContentPlaceHolderMain_btnRenewQuotation").click();
                        }
                    }
                }
            });
        }

        function EnterValueForSearch(txtId, btnId) {
            var txt;
            var btn;
            txt = document.getElementById(txtId);
            btn = document.getElementById(btnId);
            if (window.event.keyCode == 13) {
                btn.click();
                return false;
            }
            return true;
        }

        function DuplicateConfirm() {
            var dupTitle = document.getElementById("ContentPlaceHolderMain_hfduplicateTitle").value;
            var dupMsg = document.getElementById("ContentPlaceHolderMain_hfduplicateMessage").value;
            var dupDupBut = document.getElementById("ContentPlaceHolderMain_hfduplicateButton").value;
            var dupDupButCancel = document.getElementById("ContentPlaceHolderMain_hfduplicateButtonCancel").value;

            $.confirm({
                useBootstrap: false,
                title: '<div class="AlertTitleStyle">' + dupTitle + '<div>',
                content: '<div class="AlertBodyStyle">' + dupMsg + '</div>',
                buttons: {
                    Yes: {
                        text: dupDupBut,
                        btnClass: 'AlertOkButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $("#ContentPlaceHolderMain_btnDuplicateQuotation").click();
                        }
                    },
                    Cancel: {
                        text: dupDupButCancel,
                        btnClass: 'AlertCancelButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {

                        }
                    }

                }
            });
        }

        function TransactionExist(qutno, qd) {
            var cTsxt = '<asp:Label ID="lblAlertPriceCh2" runat="server"></asp:Label>';
            cTsxt = cTsxt.replace("dateRed", String(qd));
            cTsxt = cTsxt.replace("qutBlue", String(qutno));

            $.confirm({
                useBootstrap: false,
                boxWidth: '380px',
                title: '',
                //content: '<div style="margin-left: 15px; margin-right: 15px;"><br/><div><img src="../media/Images/bluebell.png" style="width: 100px;"></div><div class="AlertBodyStyle">Price changes were made to quotation <b>' + String(qutno) + '</b><div>on ' + String(qd) + '.</div></div><div class="AlertBodyStyle" style="padding-top:10px;">Please make sure to view the <b>new quotation report</b> under "Quotation Documents".</div><div class= "AlertBodyStyle" > You can also email the revised quotation by clicking on the "Submit Quotation"/ "Email Quotation Details" button.</div ></div>',
                content: cTsxt,
                //content: content.replace("redDate", qd),
                buttons: {
                    Yes: {
                        text: '<asp:Label ID="lblVVqut" runat="server"></asp:Label>',
                        btnClass: 'AlertOkButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $("#ContentPlaceHolderMain_btnRedirectPrices").click();
                        }
                    }

                }
            });
        }
        function TransactionExistReadOnly(qutno, qd) {
            if (!qd.includes('greybell.png')) {
                $.confirm({
                    useBootstrap: false,
                    title: '',
                    content: '<br/><div><img src="../media/Images/bluebell.png" style="width: 100px;"></div><div class="AlertBodyStyle"><asp:Label ID="lblAlertPriceCh1" runat="server"></asp:Label> <b>' + String(qutno) + '</b></div>',
                    buttons: {
                        Yes: {
                            text: 'Close',
                            btnClass: 'AlertOkButtonStyle',
                            keys: ['enter', 'shift'],
                            action: function () {

                            }
                        }
                    }
                });
                return false;
            }
            else {
                return false;
            }
        }


        function bChangeAddColor() {
            if (document.getElementById("ContentPlaceHolderMain_txtConnect").value == "") {
                document.getElementById("ContentPlaceHolderMain_AddtoMyQuotations").style.backgroundColor = "#757677";
            }
            else {
                document.getElementById("ContentPlaceHolderMain_AddtoMyQuotations").style.backgroundColor = "#12498a";
            }
        }
    </script>

    <style>
        @media (min-width:768px) {
            .col-md-3L {
                flex: 0 0 auto;
                width: 25%
            }

            .col-md-33L {
                flex: 0 0 auto;
                width: 20%
            }

            .col-md-4L {
                flex: 0 0 auto;
                width: 33.33333333%
            }
        }



        .rowL {
            /*--bs-gutter-x: 1.5rem;*/
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(-1 * var(--bs-gutter-y));
            margin-right: calc(-.5 * var(--bs-gutter-x));
            margin-left: calc(-.5 * var(--bs-gutter-x))
        }

        .rowLcss {
            border-left: thin solid #e9eaec;
            border-right: thin solid #e9eaec;
            padding-top: 20px;
            padding-bottom: 16px;
        }

        @media screen and (max-width: 500px) {
            .rowLcss {
                display: unset
            }
        }
        /*            .rowL > * {
                flex-shrink: 0;
                width: 100%;
                max-width: 100%;
                padding-right: calc(var(--bs-gutter-x) * .5);
                padding-left: calc(var(--bs-gutter-x) * .5);
                margin-top: var(--bs-gutter-y)
            }*/
        @media (min-width:576px) {
            .col-smL {
                flex: 1 0 0%
            }

            .col-sm-12L {
                flex: 0 0 auto;
                width: 96%
            }
        }


        @media screen and (max-width: 1024px) {
            .searchdiv {
                width: 150px !important
            }
        }

        .searchdText {
            width: 85%;
        }

        @media screen and (max-width: 1024px) {
            .searchdText {
                width: 70% !important
            }
        }






        .myDivLeft {
            height: 100%;
            width: 1720px;
            display: inline-block;
            vertical-align: top;
        }

        /* The block of code below tells the browsers what to do on a screen that has a width of 320px or less */

        @media screen and (max-width: 1000px) {

            .myDivLeft {
                height: 100%;
                width: 1000px;
                display: inline-block;
                vertical-align: top;
                /*border-style:solid;*/
                margin-left: 40px;
            }
        }

        .GridListDiv {
            border-style: solid;
            display: block;
            margin-left: 20px;
            margin-right: 20px;
            height: 550px;
            margin-bottom: 40px;
            overflow-y: auto;
            overflow-x: hidden;
            width: 100%;
            height: 100%
        }

        @media screen and (max-width: 500px) {
            .GridListDiv {
                padding-left: 1px;
                width: 90% !important
            }
        }

        .searchdiv {
            z-index: 999;
            border: solid;
            border-color: lightgray;
            border-radius: 14px;
            width: 300px;
            height: 20px;
            padding-left: 10px;
            height: 32px;
            border-width: thin;
        }

        .searchdivsmall {
            z-index: 999;
            border: solid;
            border-color: lightgray;
            border-radius: 14px;
            width: 210px;
            height: 20px;
            padding-left: 10px;
            height: 32px;
            border-width: thin;
        }

        .searchdivCombo {
            z-index: 999;
            border: solid;
            border-color: lightgray;
            border-radius: 14px;
            width: 210px;
            height: 20px;
            padding-left: 10px;
            height: 32px;
            border-width: thin;
        }

        .SearchIcon {
            padding-left: 30px;
            padding-right: 20px;
            /*padding-top: 20px;*/
            /*padding-bottom: 20px;*/
            margin-bottom: 4px;
            display: flex;
        }

        .SearchIcoSub {
            margin-left: 20px;
            margin-right: 20px;
            padding-top: 8px;
        }

        @media screen and (max-width: 1000px) {

            .SearchIcoSub {
                margin-left: 1px;
                margin-right: 1px;
            }
        }

        .child-div {
            text-align: center;
            margin: auto 10px;
        }

        .GridLabelcls {
        }

        .GridIconcls {
            height: 18px;
            width: 18px;
            margin-top: 6px;
        }

        .PCss td {
            font-size: 18px !important;
            padding-right: 8px !important;
        }

        .imgRver {
            width: 30px;
            vertical-align: middle;
            padding-right: 5px;
        }

        .fontForMobRes {
        }

        @media screen and (max-width: 540px) {

            .fontForMobRes {
                font-size: 11px !important;
            }
        }




        .ColumnA {
            /*margin: auto;*/
            /*position: absolute;*/
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
            /*border-style:solid*/
        }

        .ColumnB {
            width: 100px;
            border-style: solid;
            border-color: black;
            word-wrap: break-word
        }

        .ResoHidecss {
            background-color: #e9eaec;
            height: 45px;
            display: block;
        }

        @media screen and (max-width: 1600px) {

            .ResoHidecss {
                display: none
            }
        }

        .ItemHidecss {
            /*display: block*/
        }

        @media screen and (max-width: 1600px) {

            .ItemHidecss {
                display: none
            }
        }

        .listveheadertext {
            text-decoration: none;
            font-weight: bold;
            color: #354993;
            /*background-color: #e9eaec;*/
        }

        .listveheadertextTable {
            height: 45px;
        }


        .prevnextcssNumCurrent {
            color: #354993;
        }

        .prevnextcssNButCurrent {
            color: #354993;
        }

        .prevnextcss {
            color: lightgray
        }

        .HeadeP {
            padding: 10px;
        }

        .rowSccLv {
            padding-left: 10px;
            border-left: thin solid #e9eaec;
            border-right: thin solid #e9eaec;
            border-bottom: solid;
            border-bottom-width: thin;
            border-bottom-color: lightgray;
            padding-top: 5px;
            padding-bottom: 3px;
        }

        .containerMedia {
            max-height: 710px;
        }

        @media screen and (max-width: 1030px) {

            .containerMedia {
                max-height: 410px;
            }
        }

        @media screen and (max-width: 770px) {

            .containerMedia {
                max-height: 710px;
            }
        }

        @media screen and (max-width: 740px) {

            .containerMedia {
                max-height: 210px;
            }
        }

        @media screen and (max-width: 500px) {

            .containerMedia {
                max-height: 510px;
            }
        }

        .lvImageFlag {
            width: 26px;
            margin-right: 70px;
            margin-left: 50px
        }
    </style>



    <script>
        cancelonbeforeunload();

    </script>



    <asp:UpdatePanel ID="upPnlAll_QutList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div style="display: none;">
                <asp:Label ID="lblToDuplicateBC" runat="server" Style="display: none;"></asp:Label>
                <asp:Label ID="lblToDuplicateQutNo" runat="server" Style="display: none;"></asp:Label>
                <asp:Label ID="lblToRenewBC" runat="server" Style="display: none;"></asp:Label>
                <asp:Label ID="lblToRenewQutNo" runat="server" Style="display: none;"></asp:Label>
                <asp:Label ID="lblDeleteBranchCode" runat="server" Style="display: none;"></asp:Label>
                <asp:Label ID="lblDeleteQuotationNo" runat="server" Style="display: none;"></asp:Label>
                <asp:Button ID="btnDuplicateQuotation" runat="server" Text="DuplicateQuotation" Style="display: none;" />
                <asp:Button ID="btnRenewQuotation" runat="server" Text="RenewQuotation" Style="display: none;" />
                <asp:Label ID="lblFolderPath" runat="server" Style="display: none;"></asp:Label>
                <asp:Label ID="selecteddisplayType" runat="server" Style="display: none;"></asp:Label>
                <asp:Button ID="btnRedirectPrices" runat="server" />
                <asp:HiddenField ID="hfRepoprtsNames" runat="server" />

            </div>

            <div class="GlobDef_MLAll BorderRightLeft">
                <wuc_Tabs:wuc_Tabs ID="wucTabs" runat="server" />
                <div class="mainLeftRightPadding">
                    <div class="rowL" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; padding-top: 0px;">
                        <div class="col-sm-12L" style="padding-right: 20px; padding-top: 10px; padding-left: 30px;">
                            <asp:Label runat="server" Text="My Quotations" CssClass="FontFamlyTitle FontSize" ID="lblmql"></asp:Label>
                        </div>
                    </div>
                    <div class="rowL rowLcss">
                        <div class="col-md-33L SearchIcon" style="padding-right: 20px;">
                            <div class="searchdiv divBorderSolid">
                                <asp:TextBox ID="txtSearchAll" runat="server" placeholder="Search Any." CssClass="searchdText BorderNone FontFamilyRoboto FontSizeRoboto FloatRLeft" Height="29px" EnableViewState="true"></asp:TextBox>
                                <asp:ImageButton ID="btnSearchAll" runat="server" ImageUrl="../media/Icons/search29.png" />
                            </div>
                        </div>
                        <div class="col-md-3L SearchIcon" style="padding-right: 20px;">
                            <div class="SearchIcoSub">

                                <asp:CheckBox runat="server" ID="chkSearchOrderd" Text=" Orderd" TextAlign="Right" CssClass="ChaeckBoxSearch FontFamilyRoboto FontSizeRoboto" EnableViewState="true" AutoPostBack="true" /></td>
                            </div>
                            <div class="SearchIcoSub">

                                <asp:CheckBox runat="server" ID="chkSearchExpired" Text=" Expired " TextAlign="Right" CssClass="ChaeckBoxSearch  FontFamilyRoboto FontSizeRoboto" AutoPostBack="true" EnableViewState="true" /></td>
                            </div>
                            <div class="SearchIcoSub">

                                <asp:CheckBox Checked="false" runat="server" ID="chkSearchSubmit" Text=" Submitted " TextAlign="Right" CssClass="ChaeckBoxSearch  FontFamilyRoboto FontSizeRoboto" EnableViewState="true" AutoPostBack="true" /></td>
                            </div>
                        </div>
                        <div class="col-md-4L SearchIcon" style="padding-right: 20px">
                            <div style="border-style: none; align-self: end; display: contents">
                                <asp:TextBox ID="txtConnect" runat="server" placeholder="Insert temporary quotation serial RFQ number" CssClass="FontFamilyRoboto FontSizeRoboto floatrightCss MyAcB" Width="325px" Height="29px" EnableViewState="true" onkeyup="bChangeAddColor()"></asp:TextBox>
                                <asp:Button CssClass="MyAc" ID="AddtoMyQuotations" BackColor="#757677" runat="server" Text="Add to My Quotations" Width="176px" />
                            </div>
                        </div>
                        <div class="col-md-33L SearchIcon ItemHidecss" style="padding-right: 20px;">
                            <div class="searchdivCombo divBorderSolid " runat="server" id="divBS" visible="false" style="float: right">
                                <asp:DropDownList ID="ddlBranch" runat="server" placeholder="Branch" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto FloatRight" Width="94%" Height="29px" EnableViewState="true" AutoPostBack="true" Visible="false"></asp:DropDownList>
                            </div>
                            <div class="searchdivCombo divBorderSolid " runat="server" id="divBC" visible="false" style="float: right; margin-left: auto">
                                <asp:DropDownList ID="ddlCustomer" runat="server" placeholder="Customer" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto " Height="29px" EnableViewState="true" Width="94%" AutoPostBack="true" Visible="false"></asp:DropDownList>
                            </div>

                        </div>
                    </div>
                    <div class="rowL" style="border-left: thin solid #e9eaec; border-bottom: thin solid #e9eaec; border-right: thin solid #e9eaec; padding-top: 0px;">
                        <div class="col-sm-12L" style="padding-right: 20px; padding-top: 0px;">

                            <div class="GlobDef" style="width: 100%">
                                <div style="width: 100%; height: auto; padding-left: 20px; padding-right: 20px;">
                                </div>

                                <div class="GridListDiv divBorderSolidColored" id="DivListForScroll">

                                    <asp:ListView ID="lvQuotationListA" runat="server" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="OnPagePropertiesChanging">


                                        <LayoutTemplate>
                                            <header class="ResoHidecss">
                                                <table cellpadding="0" cellspacing="0" class="listveheadertextTable" style="margin-left: 10px">
                                                    <tr>
                                                        <div id="divlvSELECTH" class="ColumnA" style="max-width: 20px; min-width: 20px;">
                                                            <th style="max-width: 20px; min-width: 20px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA">&nbsp;
                                                            </th>
                                                        </div>
                                                        <th id="pnliQuoteNO" runat="server" width="100px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA">iQuote Number
                                                            <asp:ImageButton ID="imgbtnlvSORTAA" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTAA" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                        <th runat="server" id="pnliQuoteNOAs400" style="max-width: 216px; min-width: 216px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA">Quotation Number
                                                            <asp:ImageButton ID="imgbtnlvSORTA" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTA" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                        <th id="pnlMC" runat="server" width="20px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA"></th>
                                                        <th id="pnlItemDesc" style="max-width: 200px; min-width: 200px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto">Item Description
                                                            <asp:ImageButton ID="imgbtnlvSORTB" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTB" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                        <th id="pnlqutDated" runat="server" style="max-width: 150px; min-width: 150px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto">Quotation Date
                                                            <asp:ImageButton ID="imgbtnlvSORTC" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTC" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                        <th id="pnlqutDatee" runat="server" style="max-width: 130px; min-width: 130px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ItemHidecss">Expiry Date
                                                            <asp:ImageButton ID="imgbtnlvSORTD" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTD" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                        <th style="max-width: 30px; min-width: 30px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto"></th>
                                                        <th id="pnlqutDatlu" style="max-width: 120px; min-width: 120px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto">Last Update
                                                            <asp:ImageButton ID="imgbtnlvSORTE" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTE" CssClass="VerticalAlignMiddle" />
                                                        </th>
                                                        <th id="pnlRepLAng" style="max-width: 120px; min-width: 120px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto">Report Language
                                                        </th>
                                                        <th id="pnlColStatus" style="max-width: 125px; min-width: 125px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto">Status
                                                            <asp:ImageButton ID="imgbtnlvSORTF" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTF" CssClass="VerticalAlignSub" />
                                                        </th>

                                                        <th id="pnlBC" runat="server" width="20px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA"></th>

                                                        <th id="pnlItemNu" style="max-width: 110px; min-width: 110px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ItemHidecss">Item Number
                                                            <asp:ImageButton ID="imgbtnlvSORTG" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTI" CssClass="VerticalAlignSub" />
                                                        </th>
                                                        <th style="max-width: 85px; min-width: 85px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto"></th>
                                                        <th style="max-width: 85px; min-width: 85px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto"></th>

                                                        <th runat="server" id="divLoggedEmail" style="max-width: 100px; min-width: 100px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA">LoggedEmail
                                                            <asp:ImageButton ID="imgbtnlvSORTK" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTK" CssClass="VerticalAlignSub" />
                                                        </th>
                                                        <th></th>

                                                        <th id="pnliCusNO" runat="server" width="100px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA">Customer No
                                                            <asp:ImageButton ID="imgbtnlvSORTAcustNo" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTACNO" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                        <th></th>
                                                        <th id="pnliCusNA" runat="server" width="100px" class="listveheadertext FontSizeRoboto14 FontFamilyRoboto ColumnA">Customer Name
                                                            <asp:ImageButton ID="imgbtnlvSORTAcustNa" runat="server" ImageUrl="../media/icons/none.svg" CommandName="SORTACNA" CssClass="VerticalAlignMiddle ColumnA" />
                                                        </th>
                                                    </tr>
                                                </table>
                                            </header>
                                            <table cellpadding="0" cellspacing="0">
                                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                                                <tr>
                                                    <td colspan="10">
                                                        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lvQuotationListA" PageSize="5">
                                                            <Fields>
                                                                <asp:NextPreviousPagerField ButtonCssClass="prevnextcss FontSizeRoboto18 FontFamilyOswald" ButtonType="Link" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowNextPageButton="false" FirstPageText="<label id=lblFFirst>First</label>" PreviousPageText="<label id=lblFPrevious>Previous</label>" />
                                                                <asp:NumericPagerField CurrentPageLabelCssClass="prevnextcssNumCurrent FontSizeRoboto18 FontFamilyOswald" NumericButtonCssClass="prevnextcssNButCurrent" ButtonType="Link" />
                                                                <asp:NextPreviousPagerField ButtonCssClass="prevnextcss FontSizeRoboto18 FontFamilyOswald" ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="false" ShowPreviousPageButton="false" NextPageText="<label id=lblFNext>Next</label>" />
                                                            </Fields>
                                                        </asp:DataPager>
                                                    </td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>

                                        <GroupTemplate>
                                            <tr style="margin-left: 10px">
                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                            </tr>
                                        </GroupTemplate>

                                        <ItemTemplate>

                                            <div class="">

                                                <div class="rowL rowSccLv" onmouseover="this.style.background='#f7f7f7';" onmouseout="this.style.background='white'">

                                                    <div id="divlvSELECT" class="ColumnA" style="max-width: 20px; min-width: 20px;">
                                                        <asp:ImageButton CssClass="VerticalAlignSub" ID="imgbtnlvSelect" runat="server" ImageUrl="../media/Icons/ListArrow.png" CommandName="SELECTA" CommandArgument='<%#DataBinder.Eval(Container, "DataItemIndex")%>' />
                                                    </div>

                                                    <div runat="server" id="divQN" class="ColumnA" style="max-width: 100px; min-width: 100px; display: none">
                                                        <asp:Label ID="lblvlQuotationNum" runat="server" Text='<%# Eval("QuotationNum") %>'> CssClass="VerticalAlignSub"</asp:Label>
                                                    </div>
                                                    <div runat="server" id="divQNA400" class="ColumnA" style="max-width: 216px; min-width: 216px;">
                                                        <asp:Label ID="lblvlAS400Number" runat="server" Text='<%# Eval("AS400Number") %>' CssClass="VerticalAlignSub"></asp:Label>
                                                    </div>
                                                    <div runat="server" id="lblvlMC" class="ColumnA" style="max-width: 20px; min-width: 20px; display: none">
                                                        <asp:Label ID="lblvlOpenTypeDesp" runat="server" Text='<%# Eval("OpenTypeDesp").ToString().Substring(0, 1) %>'></asp:Label>
                                                    </div>
                                                    <div class="ColumnA" style="max-width: 200px; min-width: 200px;">
                                                        <asp:Label ID="lblvlSemiToolDescription" runat="server" Text='<%# Eval("SemiToolDescription") %>' CssClass="VerticalAlignSub"></asp:Label>
                                                    </div>
                                                    <div runat="server" id="divTemplateQD" class="ColumnA" style="max-width: 150px; min-width: 150px;">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("QuotationDate") %>'></asp:Label>
                                                    </div>
                                                    <div runat="server" id="divTemplateQDE" class="ColumnA ItemHidecss" style="max-width: 130px; min-width: 130px;">
                                                        <asp:Label ID="lblvlExpiredDate" runat="server" Text='<%# Eval("ExpiredDate") %>' CssClass="VerticalAlignSub"></asp:Label>
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lblvlPricesChaneged" runat="server" Text='<%# Eval("PricesChaneged") %>'></asp:Label>
                                                    </div>
                                                    <div class="ColumnA" style="max-width: 30px; min-width: 30px;">
                                                        <asp:ImageButton runat="server" ID="ImgPVA" OnClientClick='<%# "return TransactionExistReadOnly(" + DataBinder.Eval(Container.DataItem, "AS400Number").ToString + ",this.src)"%>' CssClass="imgRver" />
                                                    </div>
                                                    <div class="ColumnA" style="max-width: 120px; min-width: 120px;">
                                                        <asp:Label ID="lbllvLastUpdate" runat="server" Text='<%# Eval("LastUpdate") %>' CssClass="VerticalAlignSub"></asp:Label>
                                                    </div>
                                                    <div class="ColumnA" style="max-width: 120px; min-width: 120px;">
                                                        <asp:ImageButton ID="ImageRepFlag" runat="server" ImageUrl='<%#"../media/flags/" + DataBinder.Eval(Container.DataItem, "ReportBranchCode").ToString + ".svg" %>' CssClass="lvImageFlag" Enabled="false" />
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lbllvExpired" runat="server" Text='<%# Eval("Expired") %>'></asp:Label>
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lbllvOrdered" runat="server" Text='<%# Eval("Ordered") %>'></asp:Label>
                                                    </div>
                                                    <div class="ColumnA" style="width: 125px; display: flex">
                                                        <asp:ImageButton runat="server" ID="lvimgStatus" ImageUrl="../media/Icons/Created.png" Style="height: 28px; width: 28px;"></asp:ImageButton>
                                                        <div style="padding-top: 5px">
                                                            <asp:Label ID="lbllvStatus" runat="server" Text="Created" CssClass="VerticalAlignSub"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lblvlModelNum" runat="server" Text='<%# Eval("ModelNum") %>'></asp:Label>
                                                    </div>
                                                    <div runat="server" id="divBCA" class="ColumnA" style="max-width: 20px; min-width: 20px; display: none">
                                                        <asp:Label ID="lblvlBranchCode" runat="server" Text='<%# Eval("BranchCode") %>'></asp:Label>
                                                    </div>
                                                    <div class="ColumnA DisplayNone">
                                                        <asp:Label ID="lblvlOfferBy_ID" runat="server" Text='<%# Eval("OfferBy_ID") %>'></asp:Label>
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lblvlOpenType" runat="server" Text='<%# Eval("OpenType") %>'></asp:Label>
                                                    </div>

                                                    <div class="ColumnA ItemHidecss" style="width: 110px;">
                                                        <asp:Label ID="lblvlitemNumber" runat="server" Text='<%# Eval("itemNumber") %>' CssClass="VerticalAlignSub"></asp:Label>
                                                    </div>

                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lblvlDuplicate" runat="server" Text='<%# Eval("Duplicate") %>'></asp:Label>
                                                    </div>

                                                    <div class="col-smL ColumnA " style="max-width: 85px; min-width: 85px;">

                                                        <asp:ImageButton ID="imgDuplicate" runat="server" ImageUrl="~\media\Icons\Duplicate.png" CommandName="cmdDuplicate" CssClass="VerticalAlignSub" CommandArgument='<%#DataBinder.Eval(Container, "DataItemIndex")%>' />

                                                    </div>
                                                    <div class="ColumnA" style="max-width: 85px; min-width: 85px;">
                                                        <asp:ImageButton ID="imgRenew" runat="server" ImageUrl="~\media\Icons\Renew.png" CommandName="cmdRenew" CssClass="VerticalAlignSub" CommandArgument='<%#DataBinder.Eval(Container, "DataItemIndex")%>' />
                                                    </div>

                                                    <div class="ColumnA" style="max-width: 100px; min-width: 100px;">
                                                        <asp:Label ID="lblvlloggedEmail" runat="server" Text='<%# Eval("loggedEmail") %>'></asp:Label>
                                                    </div>

                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lblvlTemporarilyQuotation" runat="server" Text='<%# Eval("TemporarilyQuotation") %>' CssClass="DisplayNone"></asp:Label>
                                                    </div>
                                                    <div runat="server" id="divCUSno" class="ColumnA" style="max-width: 100px; min-width: 100px; display: none">
                                                        <asp:Label ID="lblvlCustomerNumber" runat="server" Text='<%# Eval("CustomerNumber") %>'></asp:Label>
                                                    </div>
                                                    <div runat="server" id="divCUSna" class="ColumnA" style="max-width: 100px; min-width: 100px; display: none">
                                                        <asp:Label ID="lblvlCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'></asp:Label>
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lblvlFolderPath" runat="server" Text='<%# Eval("FolderPath") %>'></asp:Label>
                                                    </div>
                                                    <div class="DisplayNone">
                                                        <asp:Label ID="lbllvSubmitted" runat="server" Text='<%# Eval("Submitted") %>'></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="hfGridQutNo" runat="server" Value="" />
                <asp:HiddenField ID="hfGridItemDesc" runat="server" Value="" />
                <asp:HiddenField ID="hfQuotationDate" runat="server" Value="" />
                <asp:HiddenField ID="hfExpiryDate" runat="server" Value="" />
                <asp:HiddenField ID="hfLastUpdate" runat="server" Value="" />
                <asp:HiddenField ID="hfReportLanguage" runat="server" Value="" />
                <asp:HiddenField ID="hfStatus" runat="server" Value="" />
                <%--<asp:HiddenField ID="hfSatatus" runat="server" Value="" />--%>
                <asp:HiddenField ID="hfItemNumber" runat="server" Value="" />
                <asp:HiddenField ID="hfCustomerName" runat="server" Value="" />

                <asp:HiddenField ID="hfduplicateTitle" runat="server" />
                <asp:HiddenField ID="hfduplicateMessage" runat="server" />
                <asp:HiddenField ID="hfduplicateButton" runat="server" />
                <asp:HiddenField ID="hfduplicateButtonCancel" runat="server" />
                <asp:HiddenField ID="hfPrevious" runat="server" />
                <asp:HiddenField ID="hfNext" runat="server" />
                <asp:HiddenField ID="hfFirst" runat="server" />
                <script>                  
                    $(document).ready(function () {
                        try {
                            document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnliQuoteNOAs400').innerHTML = document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnliQuoteNOAs400').innerHTML.replace("Quotation Number", document.getElementById("ContentPlaceHolderMain_hfGridQutNo").value);
                            document.getElementById('pnlItemDesc').innerHTML = document.getElementById('pnlItemDesc').innerHTML.replace("Item Description", document.getElementById("ContentPlaceHolderMain_hfGridItemDesc").value).replace(":", "");
                            document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnlqutDated').innerHTML = document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnlqutDated').innerHTML.replace("Quotation Date", document.getElementById("ContentPlaceHolderMain_hfQuotationDate").value).replace(":", "");
                            document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnlqutDatee').innerHTML = document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnlqutDatee').innerHTML.replace("Expiry Date", document.getElementById("ContentPlaceHolderMain_hfExpiryDate").value).replace(":", "");
                            document.getElementById('pnlqutDatlu').innerHTML = document.getElementById('pnlqutDatlu').innerHTML.replace("Last Update", document.getElementById("ContentPlaceHolderMain_hfLastUpdate").value).replace(":", "");
                            document.getElementById('pnlRepLAng').innerHTML = document.getElementById('pnlRepLAng').innerHTML.replace("Report Language", document.getElementById("ContentPlaceHolderMain_hfReportLanguage").value).replace(":", "");
                            document.getElementById('pnlItemNu').innerHTML = document.getElementById('pnlItemNu').innerHTML.replace("Item Number", document.getElementById("ContentPlaceHolderMain_hfItemNumber").value).replace(":", "");
                            document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnliCusNA').innerHTML = document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnliCusNA').innerHTML.replace("Customer Name", document.getElementById("ContentPlaceHolderMain_hfCustomerName").value).replace(":", "");
                            document.getElementById('pnlColStatus').innerHTML = document.getElementById('pnlColStatus').innerHTML.replace("Status", document.getElementById("ContentPlaceHolderMain_hfStatus").value).replace(":", "");

                            document.getElementById('lblFNext').innerHTML = document.getElementById('ContentPlaceHolderMain_hfNext').value;
                            document.getElementById('lblFPrevious').innerHTML = document.getElementById('ContentPlaceHolderMain_hfPrevious').value;
                            document.getElementById('lblFFirst').innerHTML = document.getElementById('ContentPlaceHolderMain_hfFirst').value;
                            //document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnliQuoteNOAs400').innerHTML = document.getElementById('ContentPlaceHolderMain_lvQuotationListA_pnliQuoteNOAs400').innerHTML.replace("Quotation Number", document.getElementById("ContentPlaceHolderMain_hfGridQutNo").value);
                        }
                        catch (err) {
                            //alert(err)
                        }
                    });


                    function SetDivHeightWithScrollQutLIst() {
                        try {

                            var dFreB = window.innerHeight - 80 - document.getElementById('DivListForScroll').getBoundingClientRect().top + 'px';
                            document.getElementById('DivListForScroll').style.maxHeight = dFreB;

                        }
                        catch (error) {

                        }
                    }
                </script>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

