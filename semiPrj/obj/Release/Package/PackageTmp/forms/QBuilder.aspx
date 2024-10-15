<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masters/SemiSTDMaster.Master" CodeBehind="QBuilder.aspx.vb" Inherits="semiPrj.QBuilder" EnableSessionState="True" EnableViewState="true" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <%@ Register Src="~/uc/wucTab.ascx" TagName="wuc_Tabs" TagPrefix="wuc_Tabs" %>

    <style>
        #containter {
            height: 100%;
            display: flex;
            /* flex-direction: column;*/
            align-items: top;
            /* justify-content: center;*/
        }

        #content {
        }

        .rowD {
            /*--bs-gutter-x: 1.5rem;*/
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(-1 * var(--bs-gutter-y));
            margin-right: calc(-.5 * var(--bs-gutter-x));
            margin-left: calc(-.5 * var(--bs-gutter-x))
        }

        .col-sm-9D {
            flex: 0 0 auto;
            width: 70%
        }

        .col-sm-3Q {
            flex: 0 0 auto;
            width: 24%;
        }

        @media screen and (max-width: 500px) {

            .col-sm-3Q {
                flex: 0 0 auto;
                width: 96% !important
            }


            .col-sm-9D {
                flex: 0 0 auto;
                width: 98%
            }
        }
    </style>


    <style>
        #panel {
            display: none;
        }
    </style>



    <style>
        .ImgMainImageLcss {
            max-height: 220px;
        }

        .ImgMainImageLcssS {
            max-width: 756px;
            max-height: 420px;
        }

        .myDivLeft {
            height: 100%;
            width: 400px;
            display: inline-block;
            vertical-align: top;
            padding-left: 20px;
        }

        @media screen and (max-width: 1000px) {

            .myDivLeft {
                height: 90%;
                width: 300px;
                display: inline-block;
                vertical-align: top;
                padding-left: 20px;
                overflow-x: hidden;
            }
        }


        .myDivRight {
            height: 100%;
            width: 100%;
            display: inline-block;
            vertical-align: top;
        }
    </style>

    <style>
        .paddleft220 {
            padding-left: 220px;
        }

        @media screen and (max-width: 1000px) {

            .paddleft220 {
                padding-left: 0px;
            }
        }

        .parentLast {
            height: 100%;
            width: 100%;
            position: relative;
        }

        .childLast {
            width: 300px;
            height: 300px;
            position: absolute;
            top: 35%;
            left: 35%;
            margin: -35px 0 0 -35px;
            text-align: center;
            padding-top: 14%;
        }

        @media screen and (max-width: 1000px) {
            .childLast {
                width: 300px;
                height: 300px;
                position: absolute;
                /*top: 50%;*/
                left: 4%;
                margin: -35px 0 0 -35px;
                text-align: center;
                padding-top: 14%;
            }
        }


        .myDivRightLast {
            height: 100%;
            width: 80%;
            display: inline-block;
            vertical-align: top;
            margin-left: 40px;
        }

        @media screen and (max-width: 1000px) {

            .myDivRightLast {
                height: 400px;
                width: 300px;
                display: inline-block;
                vertical-align: top;
                margin-left: 40px;
            }
        }

        .zIndexAuto {
            z-index: auto
        }

        .parentDisable {
            z-index: 999;
            width: 100%;
            height: 100%;
            display: none;
            position: absolute;
            top: 0;
            left: 0;
            background-color: #ccc;
            color: #aaa;
            opacity: .2;
            filter: alpha(opacity=50);
        }

        .parentDisable1 {
            z-index: 999;
            width: 80%;
            height: 100%;
            display: none;
            position: absolute;
            top: 0;
            left: 0;
            background-color: #ddd;
            color: #aaa;
            /*opacity: .4;*/
            filter: alpha(opacity=50);
        }


        .divLVparamscss {
        }

        @media screen and (max-width: 500px) {

            .divLVparamscss {
                display: inline-flex;
            }
        }

        .ImageLeftBoder {
            border-style: solid;
            border-color: #d9d9d9;
            border-width: 1px;
            width: 99%
        }

        @media screen and (max-width: 500px) {

            .ImageLeftBoder {
                border-style: solid;
                border-color: #d9d9d9;
                border-width: 1px;
                height: 100% !important;
                width: 99% !important;
                /*padding: 10px*/
            }
        }

        .DivRulls {
            overflow-y: auto;
            border: 1px solid #d9d9d9;
            vertical-align: top;
            height: 170px
        }

        /***********************/

        @media screen and (min-resolution: 115dpi) {
            .ImgMainImageLcssS {
                width: 700px !important;
            }
        }

        @media screen and (min-resolution: 144dpi) {
            .ImgMainImageLcssS {
                width: 500px !important;
            }
        }

        @media screen and (min-resolution: 144dpi) {
            .DivRulls {
                height: 120px !important;
            }
        }
        /***********************/

        .divleftwidthr {
        }

        @media screen and (max-width: 1024px) {

            .divleftwidthr {
                width: 58%
            }
        }

        @media screen and (max-width: 500px) {

            .divleftwidthr {
                width: 100%;
                border-top: solid;
                border-color: gray;
                border-width: thin;
            }
        }

        .divleftwidthl {
        }

        @media screen and (max-width: 1024px) {

            .divleftwidthl {
                width: 38%
            }
        }

        @media screen and (max-width: 500px) {

            .divleftwidthl {
                width: 100%
            }
        }



        .divBuildAll {
            height: 100%;
            overflow-x: hidden;
            overflow-y: auto;
        }


        .divBuildAllx {
            max-height: 700px;
            overflow-x: hidden;
            overflow-y: auto;
        }

        @media screen and (max-width: 1030px) {
            .divBuildAllx {
                max-height: 500px;
            }
        }

        @media screen and (max-width: 768px) {
            .divBuildAllx {
                max-height: 680px;
            }
        }

        @media screen and (max-width: 500px) {
            .divBuildAllx {
                max-height: 530px;
            }
        }


        .scrolH {
            overflow: unset;
            /*max-height: 600px*/
        }

        @media screen and (max-width: 1600px) {
            .scrolH {
                overflow: auto;
                max-height: 600px
            }
        }

        .divImgCont {
            width: 100%;
            overflow: unset;
        }

        @media screen and (max-width: 1600px) {
            .divImgCont {
                width: 90%;
                overflow: auto;
            }
        }


        .clsPnlDife {
            text-align: center;
            width: 100%;
            margin-left: 70px;
            margin-right: 70px;
            padding-top: 10px;
            padding-bottom: 10px;
        }


        /*********************************/

        .DivHeightResolution {
            max-height: 630px;
        }

        @media screen and (min-resolution: 105dpi) {
            .DivHeightResolution {
                height: 500px !important;
            }
        }

        @media screen and (min-resolution: 115dpi) {
            .DivHeightResolution {
                height: 400px !important;
            }
        }

        @media screen and (min-resolution: 144dpi) {
            .DivHeightResolution {
                height: 270px !important;
            }
        }






        /*********************************/
    </style>


    <script language="javascript">
        function disableEnterKey(e) {
            if (window.event.keyCode == 13) {
                return false;
            }
            else {
                return true;
            }
        }
        document.onkeypress = disableEnterKey;


        function SavedSuccessfully() {
            $.alert({
                useBootstrap: false,
                //columnClass: 'small',
                title: 'iQuote',
                content: 'New quotation has been saved successfully.'
            });
        }

        function reternAlert() {
            alert(1);
        }

        function EnterParamValue(txtId, btnId) {
            var txt;
            var btn;
            txt = document.getElementById(txtId);
            btn = document.getElementById(btnId);
            if (window.event.keyCode == 13) {
                var xx = document.getElementById("ContentPlaceHolderMain_txtValue").value;
                xx = xx.replace(",", ".")
                if (isNaN(xx)) { document.getElementById("ContentPlaceHolderMain_txtValue0").value = document.getElementById("ContentPlaceHolderMain_txtValue").value }
                else {
                    document.getElementById("ContentPlaceHolderMain_txtValue0").value = xx;
                }
                btn.click();
                return false;
            }
            return true;
        }

        function SavedSuccessfully() {
            $.alert({
                useBootstrap: false,
                title: 'iQuote',
                content: 'New quotation has been saved successfully.'
            });
        }


        function EnterParamValueForSearch(txtId, btnId) {
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

        function ParamValueTextChanged() {
            var xx = document.getElementById("ContentPlaceHolderMain_txtValue").value;
            if (isNaN(xx)) { document.getElementById("ContentPlaceHolderMain_txtValue0").value = document.getElementById("ContentPlaceHolderMain_txtValue").value }
            else {
                document.getElementById("ContentPlaceHolderMain_txtValue0").value = xx;
            }
        }
        function ParamValueTextChanged2(txt) {
            var xx = document.getElementById("ContentPlaceHolderMain_txtValue").value;
            if (isNaN(xx)) { document.getElementById("ContentPlaceHolderMain_txtValue0").value = document.getElementById("ContentPlaceHolderMain_txtValue").value }
            else {
                document.getElementById("ContentPlaceHolderMain_txtValue0").value = xx;
            }
        }

        function startConfigorationConfirm() {
            var InnerTetIn = document.getElementById("ContentPlaceHolderMain_lblCurrnetParameterName").innerHTML;

            InnerTetIn = "Please confirm external range value for " + InnerTetIn + " parameter by press OK button "
            InnerTetIn = InnerTetIn.replace("Set ", "");

            $.confirm({

                useBootstrap: false,
                title: InnerTetIn,
                content: '' +
                    '<form action="" class="formName">' +
                    '<div class="form-group" style="display:none">' +
                    '<label></label>' +
                    '<input type="text" placeholder="Your name" class="name form-control" required value="OK"/>' +
                    '</div>' +
                    '</form>',
                buttons: {
                    formSubmit: {
                        text: 'Ok',
                        btnClass: 'btn-blue',
                        action: function () {
                            document.getElementById("ContentPlaceHolderMain_btnOther").click();
                        }
                    },
                    cancel: function () {
                        //close
                        //return false;

                    },
                },
                onContentReady: function () {
                    var jc = this;
                    this.$content.find('form').on('submit', function (e) {
                        e.preventDefault();
                        jc.$$formSubmit.trigger('click');
                    });
                }
            });
        }

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function startRequest(sender, e) {
            var buttons = document.getElementsByTagName("input");
            for (i = 0; i < buttons.length; i++) {
                if (buttons[i].type == "submit") {
                    buttons[i].disabled = true;
                }
            }
        }
        function endRequest(sender, e) {
            var buttons = document.getElementsByTagName("input");
            for (i = 0; i < buttons.length; i++) {
                if (buttons[i].type == "submit") {
                    buttons[i].disabled = false;
                }
            }
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            HTMLInputElement.prototype.scrollIntoView = function (a) { this.scrollIntoViewIfNeeded(); }
            HTMLSelectElement.prototype.scrollIntoView = function (a) { this.scrollIntoViewIfNeeded(); }
            HTMLAreaElement.prototype.scrollIntoView = function (a) { this.scrollIntoViewIfNeeded(); }
        });

        //Maintain scroll position in given element or control
        var xInputPanel, yInputPanel;
        var xProductPanel, yProductPanel;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            yProductPanel = $get('DataDiv').scrollTop;
        }
        function EndRequestHandler(sender, args) {
            $get('DataDiv').scrollTop = yProductPanel;
            var tt = $("#ContentPlaceHolderMain_txthdn").val();

            if (tt >= 8) { $get('DataDiv').scrollTop = 300; }
            if (tt >= 19) { $get('DataDiv').scrollTop = 500; }
        }

        function ShowhideClick() {
            $("#panel").slideToggle(10);
        }

        function HideSupport() {
            document.getElementById("ContentPlaceHolderMain_lblMailName").style.visibility = "hidden";
            document.getElementById("ContentPlaceHolderMain_lblMailEmail").style.visibility = "hidden";
            document.getElementById("ContentPlaceHolderMain_ph_Export").style.visibility = "hidden";
        }

        function GetSupport() {
            document.getElementById("ContentPlaceHolderMain_ph_Export").style.visibility = "visible";
            document.getElementById("ContentPlaceHolderMain_lblMailName").style.visibility = "hidden";
            document.getElementById("ContentPlaceHolderMain_lblMailEmail").style.visibility = "hidden";
        }



        function CheckFieldMessage() {
            if (document.getElementById("ContentPlaceHolderMain_txtMailName").value == '' && document.getElementById("ContentPlaceHolderMain_txtMailEmail").value == '') {
                document.getElementById("ContentPlaceHolderMain_lblMailName").style.visibility = "visible";
                document.getElementById("ContentPlaceHolderMain_lblMailEmail").style.visibility = "visible";
                return true
            }
            else {
                document.getElementById("ContentPlaceHolderMain_lblMailName").style.visibility = "hidden";
                document.getElementById("ContentPlaceHolderMain_lblMailEmail").style.visibility = "hidden";
                if (document.getElementById("ContentPlaceHolderMain_txtMailName").value == '') {
                    document.getElementById("ContentPlaceHolderMain_lblMailName").style.visibility = "visible";
                    return true
                }
                else {
                    document.getElementById("ContentPlaceHolderMain_lblMailName").style.visibility = "hidden";
                    document.getElementById("ContentPlaceHolderMain_lblMailEmail").style.visibility = "hidden";
                    if (document.getElementById("ContentPlaceHolderMain_txtMailEmail").value == '') {
                        document.getElementById("ContentPlaceHolderMain_lblMailEmail").style.visibility = "visible";
                        return true
                    }
                    else {
                        document.getElementById("ContentPlaceHolderMain_ph_Export").style.visibility = "hidden";
                        document.getElementById("ContentPlaceHolderMain_btnSubmitMessage").click();
                    }

                }
            }
        }


        function MessageEnterPressed() {
            if (event.keyCode == 13) {
                window.event.preventDefault();
                window.event.stopImmediatePropagation();
                var _val = window.event.target.value;
                window.event.target.value = _val + '\n';

            }
        }


        function sliders(startvalue1, minVal, maxVal, Step) {
            $("#sliderJQ").slider({
                orientation: "horizontal",
                range: "min",
                min: minVal,
                max: maxVal,
                step: Step,
                value: startvalue1,
                slide: function (event, ui) {
                    $("#ContentPlaceHolderMain_txtValue").val(ui.value);
                    $("#ContentPlaceHolderMain_txtValue0").val(ui.value);
                    if (String(ui.value).includes('000000')) {
                        $("#ContentPlaceHolderMain_txtValue").val(maxVal);
                        $("#ContentPlaceHolderMain_txtValue0").val(maxVal);
                    }
                    $("#ContentPlaceHolderMain_txtValue").focus();
                    if (document.getElementById("ContentPlaceHolderMain_btnMinValueRange").value.includes(",")) {
                        document.getElementById("ContentPlaceHolderMain_txtValue").value = document.getElementById("ContentPlaceHolderMain_txtValue").value.replace(".", ",")
                    }
                }
            });
        }

        function slidersStartVal(startvalue1, minVal, maxVal, Step) {
            $("#sliderJQ").slider({
                orientation: "horizontal",
                range: "min",
                min: minVal,
                max: maxVal,
                step: Step,
                value: startvalue1,
            });
        }

        function LostFocusTxtVal() {
            let sLostFocusTxtVal = '1';
            try {
                if (document.documentElement.clientWidth < 1000) {
                    document.getElementById("ContentPlaceHolderMain_ImgMainImageR").focus();
                }
                else {
                    document.getElementById("ContentPlaceHolderMain_txtValue").focus();
                }
            } catch (error) {
                return false
                // expected output: ReferenceError: nonExistentFunction is not defined
                // Note - error messages will vary depending on browser
            }
        }

        //------MM INCH------
        function setVerr(ed) {
            if (ed == 'I') {
                $('#chkUnit').prop('checked', false);
                $('.slider.round').css({ 'padding-left': '30px' });
                $('#txtUnit').text('inch');
            }
            else
                if (ed == 'M') {
                    $('#chkUnit').prop('checked', true);
                    $('#txtUnit').text('mm');
                }
        }

        function HidedivLstV() {
            document.getElementById("ContentPlaceHolderMain_divLstV").style.display = 'none';
        }





    </script>

    <wuc_Tabs:wuc_Tabs ID="wucTabs" runat="server" />

    <asp:Label ID="lblSliderId" runat="server"></asp:Label>

    <asp:Label ID="RullsCount" runat="server" Text="" CssClass="DisplayNone"></asp:Label>
    <%--<asp:HiddenField ID="hfLogandP" runat="server" Value="" />--%>

    <div class="mainLeftRightPadding scrolH" id="divInParams">
        <div class="rowD" style="border-left: thin solid #e9eaec; border-bottom: thin solid #e9eaec; border-right: thin solid #e9eaec; padding-top: 0px; max-height: 700px">
            <div class="col-sm-3Q divleftwidthl" style="padding-right: 20px; padding-top: 10px;">
                <div style="padding-left: 20px;">

                    <div style="width: 100%; border-style: none; display: inline-block">
                        <asp:Label ID="lblParamsDes" runat="server" Text="Parameters Overview1" CssClass="MainSubTitleK FontFamily" Width="50%"></asp:Label>

                        <div style="float: right">
                            <asp:Button Text="clear all" runat="server" ID="btnClearAll" CssClass="cursorpointer MainSubTitleK FontFamily" BackColor="Transparent" BorderStyle="None" ForeColor="#5ea2f4" />
                        </div>
                    </div>

                    <div id="DataDiv" class="DataListcssR DivHeightResolution">
                        <asp:ListView ID="lvParams" runat="server" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1">

                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
                            </LayoutTemplate>

                            <GroupTemplate>

                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                            </GroupTemplate>

                            <ItemTemplate>
                                <asp:Panel ID="pnlX" runat="server" CssClass="pnlParamS">
                                    <div id="divLVparams" class="divLVparamscss">
                                        <div style="height: 20px;">
                                            <asp:Button ID="btnParamArray" CommandArgument="btnParamArray" runat="server" Text='<%# Eval("ParamArray") %>' CssClass="ListViewLinkButtonNDis FontFamilyRoboto FontSizeRoboto13" />&nbsp;
                                        </div>
                                        <div>
                                            <asp:Label ID="lblMeasure" runat="server" Text='<%# Eval("Measure") %>' CssClass="ListViewMeasureNDis FontFamilyRoboto FontSizeRoboto13"></asp:Label>
                                            <asp:Label ID="lblMeasureCaption" runat="server" Text='<%# Eval("Measure") %>' CssClass="ListViewMeasureNDis FontFamilyRoboto FontSizeRoboto13"></asp:Label>

                                        </div>
                                    </div>
                                    <div class="DisplayNone">
                                        <asp:Label ID="lblTabIndex" runat="server" Text='<%# Eval("TabIndex") %>' CssClass="ListViewLabel"></asp:Label>
                                        <asp:Label ID="lblLabel" runat="server" Text='<%# Eval("Label") %>' CssClass="ListViewLabel"></asp:Label>
                                        <asp:Label ID="lblPrevParam" runat="server" Text='<%# Eval("PrevParam") %>' CssClass="ListViewLabel"></asp:Label>
                                        <asp:Label ID="lblVisibleTable" runat="server" Text='<%# Eval("VisibleTable") %>' CssClass="ListViewLabel"></asp:Label>

                                    </div>

                                </asp:Panel>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>

            <div class="col-sm-9D divleftwidthr " style="padding: 10px;">
                <div class="myDivRightLast " id="divmyDivRightLastF" runat="server">
                    <div class="FontFamilyRoboto FontSizeRoboto20">
                        <div id="DataDivFistLastF" class="DataListcssLL">
                            <div class="parentLast">
                                <asp:Label runat="server" ID="Label2" CssClass="FontFamilyRoboto FontSizeRoboto20" Text="<br>Press report button to send a notify message to IMC support."></asp:Label><%--There is no reference found!<br>--%>
                                <br />
                                <asp:Button ID="btnSubmitMessagePricesBuild" runat="server" CssClass="FontFamilyRoboto FontSizeRoboto QuotationDetailsButton" Text="Report" BackColor="#12498a" Width="100px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="myDivRightLast" id="divmyDivRightLast" runat="server">
                    <div id="DataDivFistLast" class="DataListcssLb">
                        <div class="parentLast">
                            <div id="divLstV" class="childLast FontFamilyRoboto FontSizeRoboto18" runat="server">
                                <br />
                                <div class="FontFamilyRoboto FontSizeRoboto18">
                                    <asp:Label runat="server" ID="lblT1" CssClass="AlertBodyStyle" Text="Your account request is being processed"></asp:Label>
                                </div>
                                <div class="FontFamilyRoboto FontSizeRoboto18">
                                    <asp:Image ID="imgGalNote" ImageUrl="../media/Icons/AR_l.svg" runat="server" Width="30" Height="60" onclick="return false;" Style="cursor: default;" />
                                </div>
                                <div class="FontFamilyRoboto FontSizeRoboto18">
                                    <asp:Label runat="server" ID="lblLGI" CssClass="AlertBodyStyle" Text="In the meantime, you can receive a temporary offer."></asp:Label>
                                </div>
                                <br />
                                <div style="display: inline-block">
                                    <div style="display: flex;">
                                        <asp:Button runat="server" Text="Login & Get Price" ID="btnSaveQuotation" CssClass="AlertOkButtonStyle zIndexAuto" Width="250px" Height="30px" OnClientClick="cancelonbeforeunload(); HidedivLstV();" />&nbsp;
                                        <asp:Button runat="server" Text="Discard Changes" ID="btnDiscardChanges" CssClass="AlertCancelButtonStyle" Width="250px" Height="30px" OnClientClick="cancelonbeforeunload()" />
                                    </div>
                                </div>
                                <asp:Panel ID="pnlDife" runat="server" CssClass="clsPnlDife">
                                    <hr style="width: 100%; width: 150px; vertical-align: middle; text-align: center;" />
                                </asp:Panel>
                                <asp:Button runat="server" Text="Get Temporary Technical Offer" ID="btnLogOnNORegistration" CssClass="AlertCancelButtonStyle" Height="30px" Width="250px" OnClientClick="cancelonbeforeunload()" />
                                <div>
                                    <br />
                                    <asp:Label runat="server" ID="lblValidation" DisplayMode="List" CssClass="FontFamily validSumcss" />
                                </div>
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
                <div class="myDivRight" id="divmyDivRight" runat="server">


                    <div style="width: 100%; border-style: none; padding-bottom: 12px;">

                        <asp:Label runat="server" ID="lblCurrnetParameterName" CssClass="MainSubTitleK FontFamily"></asp:Label>

                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:PlaceHolder runat="server" ID="PlaceHolder1">
                                    <div id="ph_Export" class="MessageErrordiv" runat="server">
                                        <div style="text-align: right; width: 50px; float: right">
                                            <input type="button" id="btnCloseExportDiv" class="FontFamilyRoboto FontSizeRoboto18 ButtonClose" value="x" onclick="HideSupport()" />
                                        </div>
                                        <div style="text-align: left; width: 50%; padding-left: 10px; padding-top: 5px;">
                                            <asp:Label ID="lblContactUs" runat="server" CssClass="FontFamily MainSubTitle" Text="Contact Us"></asp:Label>
                                        </div>
                                        <div style="text-align: left; padding-left: 10px; padding-top: 10px;">
                                            <asp:Label runat="server" CssClass="FontFamilyRoboto FontSizeRoboto13 MyQuotationPlease" Text="Name *"></asp:Label>
                                        </div>
                                        <div style="text-align: left; padding-left: 10px; padding-top: 0px;">
                                            <asp:TextBox ID="txtMailName" CssClass="FontFamilyRoboto FontSizeRoboto13 MessageTxtN" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblMailName" runat="server" Text="Empty Name!" CssClass="FontFamily validSumcss FontFamilyRoboto FontSizeRoboto13"></asp:Label>
                                        </div>

                                        <div style="text-align: left; padding-left: 10px; padding-top: 10px;">
                                            <asp:Label runat="server" CssClass="FontFamilyRoboto FontSizeRoboto13 MyQuotationPlease" Text="Email *"></asp:Label>
                                        </div>

                                        <div style="text-align: left; padding-left: 10px; padding-top: 0px;">
                                            <asp:TextBox ID="txtMailEmail" CssClass="FontFamilyRoboto FontSizeRoboto13 MessageTxtN" runat="server"></asp:TextBox>
                                            <asp:Label ID="lblMailEmail" runat="server" Text="Empty Email Address!" CssClass="FontFamily validSumcss FontFamilyRoboto FontSizeRoboto13"></asp:Label>
                                        </div>
                                        <div style="text-align: left; padding-left: 10px; padding-top: 10px;">
                                            <asp:Label runat="server" CssClass="FontFamilyRoboto FontSizeRoboto13 MyQuotationPlease" Text="Country"></asp:Label>
                                        </div>

                                        <div style="text-align: left; padding-left: 10px; padding-top: 0px;">
                                            <asp:TextBox ID="txtMailCountry" CssClass="FontFamilyRoboto FontSizeRoboto13 MessageTxtN" runat="server"></asp:TextBox>
                                        </div>
                                        <div style="text-align: left; padding-left: 10px; padding-top: 10px;">
                                            <asp:Label runat="server" CssClass="FontFamilyRoboto FontSizeRoboto13 MyQuotationPlease" Text="Company Name"></asp:Label>
                                        </div>

                                        <div style="text-align: left; padding-left: 10px; padding-top: 0px;">
                                            <asp:TextBox ID="txtMailCompanyName" CssClass="FontFamilyRoboto FontSizeRoboto13 MessageTxtN" runat="server"></asp:TextBox>
                                        </div>
                                        <div style="text-align: left; padding-left: 10px; padding-top: 10px;">
                                            <asp:Label runat="server" CssClass="FontFamilyRoboto FontSizeRoboto13 MyQuotationPlease" Text="Message"></asp:Label>
                                        </div>
                                        <div id="divexternal" style="height: 92px; text-align: left; padding-left: 10px;" onkeydown="MessageEnterPressed()">
                                            <asp:TextBox TextMode="MultiLine" ID="txtMailMessage" CssClass="FontFamilyRoboto FontSizeRoboto13 MessageTxtN" Height="110px" runat="server"></asp:TextBox>
                                        </div>
                                        <div style="text-align: left; padding-left: 10px; padding-top: 28px; text-align: center">
                                            <asp:Button Name="BtnSubmitMessage" ID="btnSubmitMessage" runat="server" CssClass="MyQuotationFind FontFamilyRoboto FontSizeRoboto13 BCbachColor FCbachColor display_non" Width="60px" Text="Find" />
                                            <input type="button" id="btnSubmitMessageActive" name="btnSubmitMessageActive" value="Submit" class="SetButton FontFamilyRoboto FontSizeRoboto" style="vertical-align: middle" onclick="CheckFieldMessage()" />
                                        </div>
                                    </div>



                                </asp:PlaceHolder>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                    <div id="DataDivFistx" class="DataListcssL">
                        <div style="width: 99%">
                            <input runat="server" type="hidden" id="hdnScrollTop" value="" />
                            <input runat="server" type="hidden" id="txthdn" value="" />




                            <div id="divRulls" class="DivRulls" runat="server">
                                <asp:Label runat="server" ID="lblLastCurrentparameterIndex" Visible="False" Text="0" Font-Size="10px"></asp:Label>
                                <asp:Label runat="server" ID="lblisEditMode" Visible="False" Text="False" Font-Size="10px"></asp:Label>
                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" Visible="False" Font-Size="10px" />
                                <asp:Label runat="server" ID="lblCurrentParameterIndex" Visible="False" Width="10px" Font-Size="6px"></asp:Label>
                                <asp:Panel runat="server" ID="pnlNote" Height="20px">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%">
                                        <tr>
                                            <td style="width: 1px; vertical-align: top; padding-bottom: 2px;">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/media/Images/Default/Remark.gif" />
                                            </td>
                                            <td style="vertical-align: bottom; padding-top: 8px">
                                                <asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" CssClass=" FontFamily FontSizeOswald18" ReadOnly="true" Width="100%" BorderStyle="None"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                                <asp:Panel runat="server" ID="pnlRulles" CssClass="pnlRullescss">
                                    <div id="divslider" runat="server">
                                        <table style="width: 100%; padding-top: 40px; border-collapse: unset !important">
                                            <tr>
                                                <td style="width: 100px; text-align: center;">
                                                    <asp:Button runat="server" ID="btnMinValueRange" CssClass="ButtonMin" />
                                                </td>
                                                <td style="width: 40%">
                                                    <div class="" id="sliderJQ" style="margin: 10px; height: 4px; cursor: pointer; background-color: lightgray !important;"></div>
                                                </td>
                                                <td style="width: 100px; text-align: center">
                                                    <asp:Button runat="server" ID="btnMaxValueRange" CssClass="ButtonMin" />
                                                </td>
                                                <td>
                                                    <asp:Panel runat="server" ID="pnlSetParam" HorizontalAlign="Left" CssClass="display_non">
                                                        <asp:TextBox runat="server" ID="txtValue" CssClass="SetParamValue FontFamilyRoboto FontSizeRoboto" onchange="ParamValueTextChanged()" TabIndex="0"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnSelectParam" CssClass="SetButton FontFamilyRoboto FontSizeRoboto" runat="server" ClientIDMode="Static" Text="Set" BorderStyle="Solid" />
                                                    </asp:Panel>
                                                </td>
                                                <asp:TextBox runat="server" ID="txtValue0" CssClass="display_non"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtSliderStepVal" CssClass="display_non"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtSliderMaxVal" CssClass="display_non"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtSliderStartVal" CssClass="display_non"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtSliderMinVal" CssClass="display_non"></asp:TextBox>
                                            </tr>

                                            <tr>
                                                <td></td>
                                                <td colspan="3"></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div id="divGridRulls" runat="server" style="height: 1px;">
                                        <div id="flip" onclick="ShowhideClick()" class="RullButtonDefault FontFamilyRoboto FontSizeRoboto RullButtonOpen" style="cursor: pointer">
                                            <div id="xx" style="border-style: solid; border-width: 1px; border-color: #f3f3f3; padding-top: 4px; color: gray; cursor: pointer">
                                                Click here to select value<div style="float: right">
                                                    <img src="../media/Icons/SmallDownIcon.png" />
                                                </div>
                                            </div>
                                        </div>

                                        <div id="panel">
                                            <%--style="overflow-x: auto;"--%>
                                            <asp:GridView RowStyle-Height="34PX" runat="server" CssClass="cssGridRulles" ID="dgvRulles" AutoGenerateColumns="False" ShowHeader="False" GridLines="None">

                                                <Columns>
                                                    <asp:BoundField DataField="Order" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="OperationH" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HeightLimitH" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table border="0">
                                                                <tr>
                                                                    <td style="width: 0px; vertical-align: top; font-size: 1px"></td>
                                                                    <td style="width: 170px; vertical-align: top; background-image: url('../media/Images/Default/btn_fill.png'); border-style: none; background-repeat: repeat-x">
                                                                        <asp:Button runat="server" ID="btnMinValue" CommandArgument="<%# Container.DataItemIndex %>" CommandName="MinBtn" BackColor="Transparent" Width="170px" BorderStyle="None" />
                                                                        <asp:RadioButton runat="server" ID="rdMinBtn" />
                                                                    </td>
                                                                    <td style="width: 0px; vertical-align: top; font-size: 1px"></td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <ItemStyle BorderStyle="None" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblMakf" Text="-"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnRullSelectValue" CssClass="RullButton FontFamilyRoboto FontSizeRoboto" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="DetailsBtn" />
                                                        </ItemTemplate>
                                                        <ItemStyle BorderStyle="None" VerticalAlign="Middle" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LowLimitH" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remarks" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>

                                                    <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone">
                                                        <ItemTemplate>
                                                            <table style="padding: 0px 0px 0px 0px; border-spacing: 0px;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRemarks" runat="server" CssClass="RullRemark FontFamilyRoboto FontSizeRoboto" Height="20px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <ItemStyle BorderStyle="None" CssClass="cssBoundFieldDisplayNone" VerticalAlign="Middle" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="RullNotation" HeaderText="." NullDisplayText=" ">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Operation" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LowLimit" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HeightLimit" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PictSelect" HeaderText="." NullDisplayText=" ">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:ImageField DataImageUrlField="PictHelp" HeaderText="." NullDisplayText=" " DataImageUrlFormatString="~\media\Images\ModelRullPicture\{0}.jpg">
                                                        <ItemStyle CssClass="cssRullImageStyle" />
                                                    </asp:ImageField>

                                                    <asp:BoundField DataField="StringValue" HeaderText=".">
                                                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <img runat="server" src="" id="imgRullNotation" width="120" style="border-style: none; padding-left: 5px;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle BorderStyle="None" />
                                                <EditRowStyle BorderStyle="None" />
                                                <EmptyDataRowStyle BorderStyle="None" />
                                                <PagerStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                                <SelectedRowStyle BorderStyle="None" />
                                            </asp:GridView>
                                        </div>

                                    </div>
                                    <div style="padding-top: 20px; padding-left: 22px;">
                                        <asp:ValidationSummary runat="server" ID="VldUser" DisplayMode="List" CssClass="FontFamilyRoboto FontSizeRoboto18" />
                                    </div>
                                </asp:Panel>

                            </div>

                        </div>


                        <div style="width: 100%; border-style: none; padding-top: 14px;">

                            <asp:Label runat="server" ID="Label1" CssClass="MainSubTitleK FontFamily" Text="Related Images"></asp:Label>
                        </div>
                        <div class="divImgCont" id="divImagesbuilder">
                            <div>
                                <div class="ImageLeftBoder" runat="server" id="ImageLeftBoderID">
                                    <asp:Image runat="server" ID="ImgMainImageL" BorderStyle="Solid" CssClass="ImgMainImageLcss" />
                                </div>
                            </div>
                            <div style="padding-top: 2px;">
                                <div class="ImageLeftBoder" runat="server" id="ImgMainImageRID">
                                    <asp:Image runat="server" ID="ImgMainImageR" BorderStyle="Solid" CssClass="paddleft220 ImgMainImageLcssS" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="display: none;">
                        <asp:Label runat="server" ID="lblDC" Text="" CssClass="TempLabels"></asp:Label>
                        <asp:Image runat="server" ID="ImgLogoImage" />
                        <asp:Label runat="server" ID="lblCookie"></asp:Label>
                        <asp:Label ID="lblForCheck" runat="server">                    
                        </asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <asp:GridView runat="server" ID="dgvParamList" AutoGenerateColumns="False" ShowHeader="False" CssClass="DisplayNone">
                <Columns>
                    <asp:ButtonField ButtonType="Image" CommandName="SelectParam" />
                    <asp:BoundField DataField="TabIndex" HeaderText="TabIndex" />
                    <asp:BoundField DataField="Label" HeaderText="Label" />
                    <asp:BoundField DataField="CostName" HeaderText="Label" />

                    <asp:BoundField DataField="Measure" HeaderText="Measure" />
                    <asp:BoundField DataField="Order" HeaderText="Order">
                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Formula" HeaderText="Formula">
                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Visible" HeaderText="Visible">
                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PrevParam" HeaderText="PrevParam">
                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MainParameters" HeaderText="PrevParam">
                        <ItemStyle CssClass="cssBoundFieldDisplayNone" />
                    </asp:BoundField>

                </Columns>
                <AlternatingRowStyle BackColor="White" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" />
                <RowStyle BackColor="#FFFFE6" ForeColor="#333333" />
            </asp:GridView>
        </div>
    </div>
    <asp:HiddenField ID="hfRepoprtsNames" runat="server" />



    <script>

        function CallF() {
            if (document.getElementById("ContentPlaceHolderMain_RullsCount").innerHTML == 'OPEN') {
                document.getElementById("flip").style.visibility = "hidden";
                if (document.getElementById("ContentPlaceHolderMain_divslider").style.display != "none") {
                    document.getElementById("flip").style.display = "none";
                }

                ShowhideClick();
                return false;
            }

            else {
                document.getElementById("flip").style.visibility = "visible";
                return false;
            }
        }
        function setXXXX() {
            try {
                document.getElementById("ContentPlaceHolderMain_se1_handleImage").style.visibility = "hidden";
            }
            catch (exception_var) {
                return false
            }
        }

        function DisableHideUpdatProg() {
            $get('UpdateProgress1').style.display = 'none';
        }

        function SendMailRef() {
            document.getElementById('UpdateProgress1').style.display = "none"
        }

        function pageLoad() {
            if (document.getElementById("ContentPlaceHolderMain_RullsCount").innerHTML != 'END') {
                CallF();
                setXXXX();
            };
        }

        function SetDivHeightWithScrollBuilder() {
            try {
                //var dFre = window.innerHeight - document.getElementById('divImagesbuilder').getBoundingClientRect().top + 'px';
                //document.getElementById('divImagesbuilder').style.maxHeight = dFre;

                var dFreD = window.innerHeight - 50 - document.getElementById('divInParams').getBoundingClientRect().top + 'px';
                document.getElementById('divInParams').style.maxHeight = dFreD;

                var dFreS = window.innerHeight - 50 - document.getElementById('divInParams').getBoundingClientRect().top + 'px';
                document.getElementById('divInParams').style.maxHeight = dFreS;

                var imgNamess = document.getElementById("ContentPlaceHolderMain_ImgMainImageR").src
                if (!imgNamess.includes('Finishing_Ae') && !imgNamess.includes('Roughing_Ae') && !imgNamess.includes('Finishing_Ae') && !imgNamess.includes('Roughing_Ae')) {
                    document.getElementById("ContentPlaceHolderMain_ImgMainImageR").style.maxWidth = "100%";
                };
            }
            catch (error) {

            }

        }


    </script>

</asp:Content>
