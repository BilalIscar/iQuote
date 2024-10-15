<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masters/SemiSTDMaster.Master" CodeBehind="Prices.aspx.vb" Inherits="semiPrj.Prices" %>

<%@ Register Src="~/uc/wucTab.ascx" TagName="wuc_Tabs" TagPrefix="wuc_Tabs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">


    <style media="screen" type="text/css">
        .rowP {
            /*--bs-gutter-x: 1.5rem;*/
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(-1 * var(--bs-gutter-y));
            margin-right: calc(-.5 * var(--bs-gutter-x));
            margin-left: calc(-.5 * var(--bs-gutter-x))
        }

        .rowP > * {
            flex-shrink: 0;
            width: 100%;
            max-width: 100%;
            padding-right: calc(var(--bs-gutter-x) * .5);
            padding-left: calc(var(--bs-gutter-x) * .5);
            margin-top: var(--bs-gutter-y)
        }


        @media (min-width:576px) {
            .col-smP {
                flex: 1 0 0%
            }
        }

        .col-sm-13D {
            flex: 0 0 auto;
            width: 38%
        }        

        @media (min-width:576px) {
            .col-sm-51P {
                flex: 0 0 auto;
                width: 27.66666667%
            }
        }

        .table {
    --bs-table-bg: transparent;
    --bs-table-accent-bg: transparent;
    --bs-table-striped-color: #212529;
    --bs-table-striped-bg: rgba(0, 0, 0, 0.05);
    --bs-table-active-color: #212529;
    --bs-table-active-bg: rgba(0, 0, 0, 0.1);
    --bs-table-hover-color: #212529;
    --bs-table-hover-bg: rgba(0, 0, 0, 0.075);
    width: 100%;
    margin-bottom: 1rem;
    color: #212529;
    vertical-align: top;
    border-color: #dee2e6
}

        .lblPricealert {
            margin-top: 10px
        }


        .CloseHistorySlow {
            display: none;
            -webkit-transition: all 0.5s ease;
            -moz-transition: all 0.5s ease;
            -ms-transition: all 0.5s ease;
            -o-transition: all 0.5s ease;
            transition: all 0.5s ease;
        }

        .OpenHistorySlow {
            /*transition: all 2s linear !important;*/
            display: block !important;
            transition: width 2s, height 2s, background-color 2s, rotate 2s;
        }

        /*input[type='radio']:checked:after {
            width: 7px;
            height: 7px;
            border-radius: 15px;
            top: -2px;
            left: 3px;
            position: relative;
            background-color: #12498a;
            content: '';
            display: inline-block;*/
            /*visibility: visible;*/
            /*border: 2px solid #12498a;
        }*/

        .divVer {
            width: 100%;
            padding-left: 20px;
            padding-right: 20px;
            padding-top: 20px;
            margin-top: 20px;
        }


        .PaddingCont {
            /*padding-right: 20px;*/
            cursor: pointer;
            width: 32px;
            margin-bottom: -6px;
        }

        .PaddingContDis {
            /*padding-right: 20px;*/
            cursor: default;
            width: 32px;
            margin-bottom: -6px;
        }

        .paddingR2 {
            padding-right: 20px;
            margin-bottom: -6px;
        }

        .iFrameHcss1 {
            height: 520px;
        }

        @media screen and (max-width: 500px) {

            .iFrameHcss1 {
                height: 340px;
            }
        }

        .iFrameHcss2 {
            height: 86%;
        }

        @media screen and (max-width: 500px) {

            .iFrameHcss2 {
                height: 76%;
            }
        }
    </style>

    <script language="javascript" type="text/javascript">

        function HideUpdateProgress() {
            $get('UpdateProgress1').style.display = 'none';
            Sys.Application.remove_load(HideUpdateProgress);
        }

        function DeleteQuotation1() {
            var bte = document.getElementById("ContentPlaceHolderMain_hfBackToEdit").value; 
            var bte22 = document.getElementById("ContentPlaceHolderMain_hfDeleteQ").value; 
            $.confirm({
                useBootstrap: false,
                title: '<asp:label id="lblAlertDeleteQut" runat="server" class="AlertTitleStyle"  text="' + bte22 +'!" />',
                content: '<asp:label id="lblAlertDeleteQut1" runat="server" class="AlertBodyStyle"  text="Are you sure you want to delete the quotation?" />',
                buttons: {
                    somethingElse: {
                        text: '<asp:label id="lblAlertDeleteQutButton" runat="server" class="AlertTitleStyle" text="' + bte22 +'" style="color: white !important; background-color: transparent !important;" />',
                        btnClass: 'AlertOkButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                            cancelonbeforeunload();
                            DeleteQuotation2();
                        }
                    },
                    Cancel: {
                        text: '<asp:label id="lblBacktoEditing" runat="server" class="AlertTitleStyle" text="'+ bte +'" style="color: white !important; background-color: transparent !important;" />',
                        btnClass: 'AlertCancelButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {

                        }
                    }
                }
            });
        };

        function DeleteQuotation2() {
            var bte2 = document.getElementById("ContentPlaceHolderMain_hfBackToEdit").value; 
            var bte22 = document.getElementById("ContentPlaceHolderMain_hfDeleteQ").value; 
            var bte23 = document.getElementById("ContentPlaceHolderMain_hfDeleteQContent").value; 
            alert(bte23);
            useBootstrap: false,
                $.confirm({
                    useBootstrap: false,
                    boxWidth: '420px',
                    title: '<div class="FontFamily FontSizeOswald18" style="padding-top: 10px; text-align: center;position:absolute; width:100%; height:50%; margin-left: auto; margin- right: auto; display: block;"><img src="../media/Icons/Screa.gif" /><div>',
                    //content: '<div style="padding-top: 100px;" class="AlertBodyStyle">Are you sure you want to<br /><b>permanently</b> delete your quotation?<br />Please note that it will no longer be accessible.</div>',
                    content: '<div style="overflow: hidden; padding-top: 110px;" class="AlertBodyStyle"><asp:label id="lblAlertDeleteFirsLine" runat="server" class="AlertTitleStyle" text="' + bte23  + '" style="color: black !important; background-color: transparent !important;" /></div>',
                    buttons: {
                        somethingElse: {
                            text: '<asp:label id="lblAlertDeleteQutButton1" runat="server" class="AlertTitleStyle" text="' + bte22 +'" style="color: white !important; background-color: transparent !important;" />',
                            btnClass: 'AlertOkButtonStyle',
                            keys: ['enter', 'shift'],
                            action: function () {
                                cancelonbeforeunload();
                                $('#ContentPlaceHolderMain_btnDoDeleteQuotation').click();
                            }
                        },
                        Cancel: {
                            text: '<asp:label id="lblBacktoEditing1" runat="server" class="AlertTitleStyle" text="' + bte2  +'" style="color: white !important; background-color: transparent !important;" />',
                            btnClass: 'AlertCancelButtonStyle',
                            keys: ['enter', 'shift'],
                            action: function () {

                            }
                        }


                    }
                });
        };

        function CheckToSend() {
            if (document.getElementById("ContentPlaceHolderMain_txtFlagPricesChanged").value == "TRUE") {
                DoYouWantToSave2();
                return false;
            }
            else {
                SendTechnicalOffer();
                return false;
            }
        }

        function fileUploadAlert() {
            $('#lblrongTypeVal').css("display", "block")
            $('#imgorderFer').css("display", "block")
        }

        function SetDivHistory(divid, MaxId) {
            for (let i = 1; i <= MaxId; i++) {
                $("#" + "divOldPriceRev" + i).removeClass("OpenHistorySlow");
                $("#" + "divOldPriceRev" + i).addClass("CloseHistorySlow");
                $("#" + "imgOldPriceRev" + i).attr("src", "../media/Icons/arrowup.png");
            };
            $("#" + "divOldPriceRev" + divid).addClass("OpenHistorySlow");
            $("#" + "imgOldPriceRev" + divid).attr("src", "../media/Icons/arrowdown.png");
        }

        function VersionHistory() {

            var sdf = document.getElementById("ContentPlaceHolderMain_hfPricesVers").value;

            sdf = sdf.replaceAll("#", "<");
            sdf = sdf.replaceAll("$", ">");
            sdf = sdf.replaceAll("*", "#");
            $.confirm({
                useBootstrap: false,
                boxWidth: '420px',
                title: '',
                content: sdf,
                buttons: {
                    cancel: {
                        text: 'Close',
                        btnClass: 'AlertCancelButtonStyle',
                        action: function () {
                        }
                    },
                }
            });

            $("#divOldPriceRev1").addClass("OpenHistorySlow");
        }

        function VersionOrdered() {

            var sdf = document.getElementById("ContentPlaceHolderMain_hfPriceOrdered").value;
            let hfCloseButCC = document.getElementById('ContentPlaceHolderMain_hfClose').value;

            sdf = sdf.replaceAll("#", "<");
            sdf = sdf.replaceAll("$", ">");
            sdf = sdf.replaceAll("*", "#");
            $.confirm({
                useBootstrap: false,
                boxWidth: '420px',
                title: '',
                content: sdf,
                buttons: {
                    cancel: {
                        text: hfCloseButCC,
                        btnClass: 'AlertCancelButtonStyle',
                        action: function () {
                        }
                    },
                }
            });
        }

        function DoYouWantToSave2() {
            $.confirm({
                useBootstrap: false,
                title: '<div class="AlertTitleStyle">iQuote message<div>',
                content: '<div class="AlertBodyStyle">Kindly note, your changes have not been saved<br>Save changes?</div>',
                buttons: {
                    somethingElse: {
                        text: 'Yes',
                        btnClass: 'AlertOkButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $('#ContentPlaceHolderMain_btnCreateSendMail2').click();
                        }
                    },
                    Cancel: {
                        text: 'Cancel',
                        btnClass: 'AlertCancelButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                            SendTechnicalOffer()
                        }
                    }


                }
            });
        };
        //--------------------


        function ShowReportFaildTechnicalAlert() {
            $.confirm({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">iQuote message.<div>',
                content: '<div style="; height:140px" class="FontFamily FontSizeOswald18 divAlertConfirm">The drawing requests were submitted to request queue.<br>It will be attached to your technical information when it will be available!</div>',
                //content: '<div class="FontFamily FontSizeOswald18 divAlertConfirm">The drawing requests were submitted to request queue.<br>It will be attached to your ' + t + ' when it will be available!</div>',
                buttons: {
                    cancel: {
                        text: 'Close',
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {
                        }
                    }
                }
            });
        }

        function ShowFileToLarge() {
            $.confirm({
                //useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">iQuote message.<div>',
                content: '<div style="; height:100px" class="FontFamily FontSizeOswald18 divAlertConfirm"My documents are too large to upload</br>Max file size 10MB!</div>',
                //content: '<div class="FontFamily FontSizeOswald18 divAlertConfirm">The drawing requests were submitted to request queue.<br>It will be attached to your ' + t + ' when it will be available!</div>',
                buttons: {
                    cancel: {
                        text: 'Close',
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {
                            //location.reload();

                        }
                    }
                }
            });
        }


        function UploadInvalidMIMEtype(sMsg) {
            $.confirm({
                //useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">iQuote message.<div>',
                content: '<div style="; height:100px; width:200px" class="FontFamily FontSizeOswald18 divAlertConfirm">' + sMsg + '</div>',
                buttons: {
                    cancel: {
                        text: 'Close',
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {
                            //location.reload();
                        }
                    }
                }
            });
        }




        function SendAlert5Sec(sname) {
            $.confirm({
                useBootstrap: true,
                boxWidth: '550px',
                title: '<div class="FontFamily FontSizeOswald20 divAlertConfirm" style="color:#12498a"><asp:label id="lblTitleEmailQut" runat="server" text="Email Quotation" /> <div>',
                content: '<div class="AlertBodyStyle" style="height:70px; height:120px; margin-top: 10px; margin-left: 10px;text-align:left"><asp:label id="lblAlertEmailQut" runat="server" text="Email Quotation" /></br><b>' + sname.replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>').replace(';', '</br>') + '</b></div>',
                buttons: {
                    Ok: {
                        text: 'Ok',
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {
                            document.getElementById("lblSendTemporaryIdx").value = sname
                            document.getElementById("ContentPlaceHolderMain_btnSendTemporaryId").click();
                            DisableblockUpdatProg();
                        }
                    }
                }
            });
        }

        function ShowClickShopAlert() {
            $.alert({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">iQuote<div>',
                content: '<div class="FontFamily FontSizeOswald18 divAlertConfirm">This feature is not available yet.</div>',
                buttons:
                {
                    text: 'close',
                    btnClass: 'btn-blueD',
                    ok: function () {

                    }
                }
            });
        }
        //function SubmitErrorMsg(e) {
        //    $.alert({
        //        useBootstrap: true,
        //        title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">iQuote<div>',
        //        content: '<div class="FontFamily FontSizeOswald18 divAlertConfirm">' + e + '</div>',
        //        //buttons:
        //        //{
        //        //    text: 'close',
        //        //    btnClass: 'btn-blueE',
        //        //    ok: function () {

        //        //    }
        //        //}
        //    });
        //}


        function SubmitErrorMsg(e) {
            $.confirm({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">iQuote<div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto16 divAlertConfirm">' + e + '</div><br><br>',
                buttons: {
                    Ok: {
                        text: 'Ok',
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {

                        }
                    }
                }
            });
        }





        function refiFrame() {
            cancelonbeforeunload();
            location.reload();
        }

        function ShowAccountDetailsX() {
            $.confirm({
                title: '<div class="FontFamily FontSizeRoboto14" style="color:#12498a">My Account Details<div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto14 AlignLeft"> dsds </div>',
                buttons: {
                    Cancel: {
                        text: 'Cancel',
                        btnClass: 'btn-blueE ',
                        keys: ['enter', 'shift'],
                        action: function () {

                        }
                    },


                    somethingElse: {
                        text: 'Delete',
                        btnClass: 'btn-blueD',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $('#ContentPlaceHolderMain_btnDoDeleteQuotation').click();
                        }
                    }
                }
            });
        };


        function showBrowseDialog() {
            cancelonbeforeunload();
            var hfFilesCount = document.getElementById('ContentPlaceHolderMain_hfFilesCoun').value;

            if (isNumeric(hfFilesCount)) {
                if (hfFilesCount > 3) {
                    UploadInvalidMIMEtype('Only 4 files can be ulpaoded!');
                    return false;
                } else {
                    var fileuploadctrl = document.getElementById('ContentPlaceHolderMain_fuFile');
                    fileuploadctrl.click()
                }
            } else {
                var fileuploadctrl = document.getElementById('ContentPlaceHolderMain_fuFile');
                fileuploadctrl.click()
            }
        }

        function isNumeric(value) {
            return !isNaN(parseFloat(value)) && isFinite(value);
        }


         <%---------*****SS*****-------------%>

        function ShowPDF() {
            var xhr = new XMLHttpRequest;
            xhr.open('get', '/path/to/pdf', true);
            xhr.responseType = 'arraybuffer';
            xhr.send();

            var blob = new Blob([xhr.response], { type: 'application/pdf' });
            var url = URL.createObjectURL(blob);

            iframe.src = url;
        }

        function disableEnterKey(e) {
            if (window.event.keyCode == 13) {
                return false;
            }
            else {
                return true;
            }
        }
        document.onkeypress = disableEnterKey;



        function SetBson(U) {
            document.getElementById("txtPathBson").text = U;
            var url = document.getElementById("txtPathBson").text;
            var iframe = document.createElement('iframe');
            iframe.frameBorder = 0;
            iframe.width = "300px";
            iframe.height = "250px";
            iframe.id = "randomid";
            iframe.setAttribute("src", U);
            document.getElementById("iframe-wrapper").appendChild(iframe);

        }

        function CantDeleteThisFileAlert() {
            $.dialog({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a">Warning!<div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto18">Cant delete this file.</div>'
            });
        }

        function DeleteQuotationDocument() {
            $.confirm({
                title: '<asp:Label runat="server" CssClass="FontFamily MainSubTitleB" Text="Delete File!"></asp:Label>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto14">Are you sure you want to delete this file?</div>',
                buttons: {
                    Cancel: {
                        text: 'Cancel',
                        btnClass: 'btn-blueE ',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $('#ContentPlaceHolderMain_btnDoDeleteDocumentCancel').click();

                        }
                    },

                    somethingElse: {
                        text: 'Delete',
                        btnClass: 'btn-blueD',
                        keys: ['enter', 'shift'],
                        action: function () {
                            DisableblockUpdatProg();
                            $('#ContentPlaceHolderMain_btnDoDeleteDocument').click();

                        }
                    }
                }
            });
        };


        function EncorrectMAilMessage(sTn) {

            if (sTn == '1') {
                $.dialog({
                    title: '',
                    content: '<br><div class="FontFamilyRoboto FontSizeRoboto18" style="width:100%; ">Wrong email address!</div>'
                });
            }
            else {
                if (sTn == '2') {
                    $.dialog({
                        title: '',
                        content: '<br><div class="FontFamilyRoboto FontSizeRoboto18" style="width:100%; ">There are no reports to send!</div>'
                    });
                }
                else {
                    $.dialog({
                        title: '',
                        content: '<br><div class="FontFamilyRoboto FontSizeRoboto18" style="width:100%; ">Mailbox unavailable!</div>'
                    });
                };
            }

        }

    </script>

    <style>
        .DivParams {
            width: 1750px;
            display: inline-block;
        }

        @media screen and (max-width: 1000px) {

            .DivParams {
                width: 1820px;
                display: inline-block;
            }
        }

        @media screen and (max-width: 1000px) {

            .DivParams {
                width: 1600px;
                display: inline-block;
            }
        }

        @media screen and (max-width: 1400px) {

            .DivParams {
                width: 1055px;
                display: inline-block;
            }
        }

        @media screen and (max-width: 1000px) {

            .DivParams {
                width: 760px;
                display: inline-block;
            }
        }

        @media screen and (max-width: 700px) {

            .DivParams {
                width: 600px;
                display: inline-block;
            }
        }

        @media screen and (max-width: 600px) {

            .DivParams {
                width: 400px;
                display: inline-block;
            }
        }

        .TextDirF {
            direction: ltr;
            text-align: left;
        }


        .cssdivQdetails {
            width: 49%;
            height: 100%
        }

        .divRepD {
            width: 600px;
        }

        @media screen and (max-width: 1600px) {

            .divRepD {
                width: 350px;
                height: 250px;
            }
        }

        @media screen and (max-width: 1600px) {
            .cssdivQdetails {
                width: 100%;
                height: 170px
            }
        }

        .UseBtnC {
            width: 600px;
            height: 400px;
            vertical-align: middle;
            text-align: center;
            justify-content: center
        }

        @media screen and (max-width: 1600px) {

            .UseBtnC {
                width: 350px;
                height: 250px;
                vertical-align: middle;
                text-align: center;
                justify-content: center
            }
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
                width: 100%;
            }
        }

        .myDivRightPrice {
            height: 100%;
            width: 1250px;
            display: inline-block;
            vertical-align: top;
        }

        @media screen and (max-width: 1000px) {

            .myDivRightPrice {
                height: 100%;
                width: 100%;
                display: inline-block;
                vertical-align: top;
            }
        }


        .QuotTextToZero {
            font-size: 1px;
            border-style: none;
            position: absolute;
            top: 100px;
            /*visibility:hidden;*/
        }

        .ImageCopyhidden {
            visibility: hidden;
        }

        #navi, #infoi {
            width: 40px;
            height: 20px;
            position: absolute;
            top: 0;
            left: 0;
        }

        #infoi {
            z-index: 10;
        }

        /*----------***************/
        .cssSelectedPrice {
            margin-top: 10px;
            margin-left: 40px
        }

        .OrderDivPrice {
            background-color: white;
            /*height: 200px;*/
            overflow: auto;
            /*margin-left: 20px;*/
            /*width: 100%*/
        }

        @media screen and (max-width: 420px) {

            .OrderDivPrice {
                max-width: 350px
            }
        }

        .OrderDivPricetable {
            margin: 0 auto;
        }

        .OrderDivPricediv {
            text-align: center;
        }

        .OrderDivPricetd {
            width: 100px !important;
            text-align: center;
            border: solid;
            border-color: lightgray;
            border-width: thin;
        }

        .OrderDivPricetdNet {
            background-color: lightgray;
        }

        .pricesMArgl {
            margin-left: 37px;
        }

        .file-upload {
            cursor: pointer;
            border-style: solid;
            border-color: #12498a;
            border-radius: 4px;
            border-width: thin;
            /*margin-left: 37px;*/
        }

            .file-upload input {
                top: 0;
                left: 0;
                margin: 0;
                /* Loses tab index in webkit if width is set to 0 */
                opacity: 0;
                filter: alpha(opacity=0);
            }


        .FileUploadcss {
            width: 18px;
        }

        .alignleft {
            text-align: left
        }

        .cssdF {
            /*width: 300px !important;*/
        }

        @media screen and (max-width: 550px) {

            .cssdF {
                width: 300px !important;
            }
        }

        .heightResp1 {
            height: 160px;
        }

        @media screen and (max-width: 1400px) {

            .heightResp1 {
                height: auto;
            }
        }

        @media screen and (max-width: 500px) {

            .heightResp1 {
                padding-right: 1px !important;
                border-top: thin solid lightgray;
                width: 100% !important;
            }
        }

        .divQdetailsA {
            width: 60%
        }

        @media screen and (max-width: 1400px) {

            .divQdetailsA {
                width: 100%
            }
        }
    </style>
    <script>

        function continueAlert() {
            $.confirm({
                useBootstrap: false,
                title: ' ',
                boxWidth: '350px',
                content: '<div style="text-align:center; ">' +
                    '<div>' +
                    '<input type="image" src="../media/Images/SearchRep.png" style=width:250px;" />' +
                    '</div>' +
                    '<div>' +
                    '<input style="width: 200px; text-align: center; color:#134a8b; padding-bottom:10px;" type="text" class="FontFamilyRoboto FontSizeRoboto20 BorderNone" value="Oops..."></input>' +
                    '</div>' +
                    '<div>' +
                    '<input style="width: 250px; text-align: center" type="text" class="FontFamilyRoboto FontSizeRoboto14 BorderNone " value="There is no option to show prices."></input><br />' +
                    '<input style="width: 250px; text-align: center" type="text" class="FontFamilyRoboto FontSizeRoboto14 BorderNone " value="Please Contact iQuote Support."></input>' +
                    '<br />' +
                    '</div>' +
                    '</div >',
                buttons: {
                    somethingElse: {
                        text: 'Report',
                        btnClass: 'btn-blueD',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $('#ContentPlaceHolderMain_btnSubmitMessagePrices').click();
                        }
                    }
                }
            }
            );
        }

        //function runaftertimeout(f) {
        //    setTimeout(function () {
        //        SendTechnicalOffer(f);;
        //    }, 5000);
        //}

        //--------------------------

        function DrawingErrorSendTechnicalMessage(FileType) {
            if (document.getElementById("hfAllredySentErrorMessage").value == "YES") {
                var ff = "'";
                var ffff1;
                var ffff2;
                var ffff3;
                var ffff4;
                var srcIm;

                ffff1 = "&nbsp;We've encountered an error while creating the technical drawing/ " /*3D model*/
                ffff1 += FileType
                ffff2 = "&nbsp;and we are working on fixing the problem."
                ffff3 = "&nbsp;The technical drawing/ 3D model will be available within the next 24 hours."
                ffff4 = "<b>&nbsp;Want to follow-up your quotation? Click on 'Email quotation details'.</b>"

                var ddtitle = '<div style="width:100%;padding-top: 10px; text-align: center"><img src="../media/Images/ErrorImgMsg.png" style="" /><br/><div class="AlertTitleStyle FontSizeRoboto25">Something Went Wrong...<div></div>'
                ddtitle = ddtitle + '<div class="FontFamilyRoboto FontSizeRoboto18" style="text-align: center"></div>'


                var cont =
                    $.confirm({
                        useBootstrap: false,
                        boxWidth: '560px',
                        title: ddtitle,
                        content: '' +
                            '<div style="text-align:left; padding-left:10px; padding-top: 0px; padding-right: 10px;">' +
                            '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff1 + '"></asp:Label><br/>' +
                            '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff2 + '"></asp:Label><br/>' +
                            '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff3 + '"></asp:Label><br/>' +
                            '<div>&nbsp;</div>' +
                            '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff4 + '"></asp:Label>' +
                            '</div>',
                        buttons: {
                            formSubmit: {
                                text: 'Email quotation details',
                                btnClass: 'AlertOkButtonStyle',
                                action: function () {
                                    document.getElementById("ContentPlaceHolderMain_btnSendTemporaryIdTech").click();
                                }
                            },
                            cancel: {
                                text: 'Close',
                                btnClass: 'AlertCancelButtonStyle',
                                action: function () {
                                    $("#ContentPlaceHolderMain_btnRefreshN").click();
                                }

                            },
                        },
                    });
            }

        }

        //-----------------------------


        function SendTechnicalOffer() {

            let witEu;
            let witEb;
            const mobileWidthSendTechnicalOffer = 600;
            if (window.innerWidth <= mobileWidthSendTechnicalOffer) {
                witEu = '650px';
                witEb = true;
            } else {
                witEu = '650px';
                witEb = false;
            }

            var txtFirst;
            var ff = "'";
            var ffff = "";
            var srcIm;
            debugger;
            if (document.getElementById("ContentPlaceHolderMain_lblQDtitle").innerHTML != "Quotation Details") {
                txtFirst = "Save your technical offer number!";
                if ($("#ContentPlaceHolderMain_hfTheSerialNo").val() != '') {
                    ffff = document.getElementById("ContentPlaceHolderMain_lblQutNumber").innerHTML;
                    //ffff = "The serial number is
                    ffff = $("#ContentPlaceHolderMain_hfTheSerialNo").val();  /*+ ": " + ffff + " &nbsp;";*/
                    srcIm = "../media/Icons/copyIcon.png"
                }
                else {
                    srcIm = "../media/Images/Pxl.png"
                };
            }
            else {
                txtFirst = "Please fill your mail address and press<br>send button";
                ffff = ""
                srcIm = "../media/Images/Pxl.png"
            };

            var ddt1 = '<div style="text-align: center; padding-top:30px" class="AlertTitleStyle FontSizeRoboto20">' + document.getElementById("ContentPlaceHolderMain_hfSubmetted").value + ' </div></div>';

            let hfSubmitAlertLine_1 = $("#ContentPlaceHolderMain_hfSubmitAlertLine1").val();
            let hfSubmitAlertLine_2 = $("#ContentPlaceHolderMain_hfSubmitAlertLine2").val();
            let hfSubmitAlertLine_3 = $("#ContentPlaceHolderMain_hfSubmitAlertLine3").val();
            let hfSubmitAlertLine_4 = $("#ContentPlaceHolderMain_hfSubmitAlertLine4").val();
            let hfSubmitAlertLine_5 = $("#ContentPlaceHolderMain_hfSubmitAlertLine5").val();
            let hfbSendS = document.getElementById('hfbSend').value
            let hfbCancel = document.getElementById('hfbCancel').value
            var bteIEA = document.getElementById("ContentPlaceHolderMain_hfInsertEmailAdd").value;

            $.confirm({                
                useBootstrap: witEb,
                boxWidth: witEu,

                title: ddt1,
                content: '' +
                    '<form action="" class="formName">' +
                    '<div class="FontFamilyRoboto MainSubTitle FontSizeRoboto17" style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; margin-top: 5px; color:black; padding-bottom: 0px; font-weight:600">' + hfSubmitAlertLine_1 + '</div>' +
                    '<div class="FontFamilyRoboto MainSubTitle FontSizeRoboto16" style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; margin-top: 5px; color:black; padding-bottom: 0px">' + hfSubmitAlertLine_2 + '</div>' +
                    '<div class="FontFamilyRoboto MainSubTitle FontSizeRoboto16" style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; color:black; padding-top: 0px">' + hfSubmitAlertLine_3 + '</div></br>' +
                    '<div class="FontFamilyRoboto MainSubTitle FontSizeRoboto16" style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; color:black; padding-top: 0px; padding-bottom: 0px;">' + hfSubmitAlertLine_4 + '</div>' +
                    '<div class="FontFamilyRoboto MainSubTitle FontSizeRoboto16" style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; color:black; padding-top: 0px">' + hfSubmitAlertLine_5 + '</div>' +
                    '<asp:TextBox id="lblSerToc" runat="server" CssClass="QuotTextToZero" />' +
                    '<div style="text-align:left; padding-left:10px; padding-top: 0px; padding-right: 10px;">' +
                    '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff + '"></asp:Label><input alt="Copy" type="image" src="' + srcIm + '" onclick="CopyToClipboard();" id="btnCopeSer" /></div>' +
                    '<div style="text-align:left; padding-top:5px !important; padding-left:10px; padding-right:10px;">' +
                    '<asp:TextBox style="height:30px; width:100%; " ID="txtFindSNOffer" AutoCompleteType="Enabled" CssClass="AlertBodyStyle" runat="server" ></asp:TextBox>' + bteIEA +'</div>' +
                    '<div style="text-align:left; padding-left:10px;   padding-right:10px;"></div>' +
                    '</form>',
                buttons: {
                    formSubmit: {
                        text: hfbSendS,
                        btnClass: 'AlertOkButtonStyle',
                        action: function () {
                            var name = $('#ContentPlaceHolderMain_txtFindSNOffer').val();
                            if (!name || name.indexOf("@") === -1 || name.length < 5) {
                                $.dialog('<br><div style="margin-top: 50px; height: 80px; " class="FontFamilyRoboto FontSizeRoboto18" style="width:90%; ">provide a valid Email address!</div>');
                                return false;
                            }

                            else {
                                setTimeout('SendAlert5Sec("' + name + '")', 1000);

                            }
                        }
                    },
                    cancel: {
                        text: hfbCancel,
                        btnClass: 'AlertCancelButtonStyle',
                        action: function () {
                        }

                    },
                },
                onContentReady: function () {
                    var jc = this;
                    this.$content.find('form').on('submit', function (e) {
                        e.preventDefault();
                    });
                }
            });
        }

        function HideExport() {
            document.getElementById("divAddOrders").style.visibility = "hidden";
        }
        function ShowExport() {
            document.getElementById("divAddOrders").style.visibility = "visible";
        }

        function urlExists(testUrl) {
            var http = jQuery.ajax({
                type: "HEAD",
                url: testUrl,
                async: false
            })
            return http.status != 404;
        }


        //function CheckRadC() {
        //    alert(22);
        //    drawingShow('PDF');
        //    qbtn = document.getElementById("ContentPlaceHolderMain_Tab2");
        //    qbtn.click();
        //    return false;
        //}

        function urlExistsOnce(testUrlOnce) {
            var httpOnce = jQuery.ajax({
                type: "HEAD",
                url: testUrlOnce,
                async: false
            })
            return httpOnce.status != 404;
        }


        function DisableHideUpdatProg() {
            $get('UpdateProgress1').style.display = 'none';
        }
        function DisableblockUpdatProg() {
            $get('UpdateProgress1').style.display = 'block';
        }


        function QuantityEnterPressed(txtId) {
            var txt;
            txt = document.getElementById(txtId);
            if (window.event.keyCode == 13) {
                $(txt).blur();
                return false;
            }
            return true;
        }

        function QuantityEnterChange(btnId) {
            var btn;
            btn = document.getElementById(btnId);
            btn.click();

            return false;
        }



        function showBrowseDialogOrder() {
            var fileuploadctrlfuOrder = document.getElementById('ContentPlaceHolderMain_fuOrder');
            fileuploadctrlfuOrder.click()
        }

        function sleep(ms) {
            return new Promise(resolve => setTimeout(resolve, ms));
        }

        function ManualydialogHidePricesOnly() {

            $('#dialogPrices').dialog('open');
            try {

                var dataListItems = document.querySelectorAll("#<%= lstvSelectedPrice.ClientID %> td");

                for (var i = 0; i < dataListItems.length; i++) {
                    var itemDiv = dataListItems[i];
                    itemDiv.style.backgroundColor = '';

                    if (itemDiv.innerText == document.getElementById('<%= hfSelectedQuantity.ClientID %>').value && itemDiv.innerText != '' && document.getElementById('<%= hfSelectedQuantity.ClientID %>') != '') {
                        itemDiv.style.backgroundColor = '#bdcbe0';
                        document.getElementById('<%= hfSelectedQuantity.ClientID %>').value = itemDiv.innerText;
                    }
                };

            }
            catch (error) {

            }

            DisableEnablePlaceOrder();
        }

        function hideshlogbuttonPrices() {
            if (document.getElementById("txthideshlogbutton").value != '') {
                SetLogOutUserName(document.getElementById("txthideshlogbutton").value);
            }

            try {
                const queryStringPrice = window.location.search;
                const paramsPrice = new URLSearchParams(queryStringPrice);

                let paramValuePrice = $('#reBC').text();
                if (paramValuePrice == "") {
                    paramValuePrice = "ZZ"
                };

                document.getElementById("currentflagPrice").src = "../media/flags/" + paramValuePrice + ".svg"
            }
            catch (error) {
                document.getElementById("currentflagPrice").src = "../media/flags/EN.svg"
            }
        }

        function ReportDeleteAlertPrice(lng, IMGs) {
            var sARep = "Quotation report is being prepared according";
            var sARep2 = "to the selected language."
            if ($('#ContentPlaceHolderMain_hfTemporaryQuotation').val() == 'True') {
                sARep = "The Technical Offer report is being prepared according";
            }
            let paramValue = $('#ContentPlaceHolderMain_hfRepoprtsNames').val();
            if (!paramValue.includes('_' + IMGs) && paramValue != '') {
                $.confirm({
                    useBootstrap: false,
                    boxWidth: '520px',
                    title: '<div class="AlertTitleStyle">Report Generetion</div>',
                    content: '<div style="width:95%;">' +
                        '<div style="width:100% !important;  text-align: left; margin-left: 10px; margin-top: 5px;"><input type="text" style="width:95% !important; font-weight:bold" disabled="disabled" class="MessageTitleLabel2 MailGeneralBodyRowStyle" value="' + sARep + '" ></div>' +
                        '<div style=" text-align: left; margin-left: 10px; margin-top: 5px;"><input type="text" style="font-weight:bold" disabled="disabled" class="MessageTitleLabel2 MailGeneralBodyRowStyle" value="' + sARep2 + '" ></div>' +
                        '</div><div>&nbsp;</div>',
                    buttons: {
                        somethingElse: {
                            text: 'Ok',
                            btnClass: 'btn-blueD btnDlgDeclineSend',
                            action: function () {
                                doclick(lng)
                            }
                        }
                    }
                }
                )
            }
            else {
                doclick(lng)
            }
        }

        function doclick(lng) {

            var getUrlParameters = function getUrlParameters(sParam) {

                var sPageURL = $('#reBCDcod').text(),

                    sURLVariables = sPageURL.split('&'),
                    sParameterName,
                    i;

                for (i = 0; i < sURLVariables.length; i++) {

                    sParameterName = sURLVariables[i].split('=');

                    if (sParameterName[0] === sParam) {
                        return sParameterName[1] === undefined ? true : sParameterName[1];
                    }
                }
            };

            try {

                var urlLang = getUrlParameters('repLang');
                var newURLwithLang = location.host + location.pathname
                var sttrErepTr = $('#ttrErepTr').text();
                const parameterIndex = sttrErepTr;
                if (urlLang === undefined) {


                    window.location.href = location.protocol + '//' + location.host + location.pathname + "?rErepTr=" + parameterIndex + "&iqlang=uGjFBoxudy4=&repLang=" + lng;

                }
                else {
                    window.location.href = newURLwithLang.replace("repLang=" + $('#reBCDcod').text(), "repLang=" + lng);
                }
            } catch (error) {
            }
        }

        function gridPricesOrderClick(qtyToOrder) {
            document.getElementById('<%= hfSelectedQuantity.ClientID %>').value = qtyToOrder;
            $('#lblrongTypeVal').css("display", "none");
            $('#imgorderFer').css("display", "none");
            ManualydialogHidePricesOnly();
            return false;
        };

        function ManualydialogHidePrices() {

            var reporlLca = '';
            try {
                reporlLca = document.getElementById("ContentPlaceHolderMain_lblSelectRepLang").innerHTML;
                var wordsArray = reporlLca.substring(reporlLca.indexOf(' '))
            } catch (err) {
                reporlLca = "Report Language"
            }




            $('#dialogLangPrice').dialog({
                autoOpen: false,
                show: { effect: 'fade', duration: 700 },
                hide: { effect: 'fade', duration: 500 },
                position: { my: "right top", at: "right bottom", of: "#currentflagPrice" },
                width: "260px",
                title: wordsArray //"Report Language"
            });

            $('#currentflagPrice').click(function () {
                $('#dialogLangPrice').dialog('open');
            });
            let lblgr = $('#ContentPlaceHolderMain_hfplaceanOrderTitle').val();
            $('#dialogPrices').dialog({
                autoOpen: false,
                modal: true,
                show: { effect: 'fade', duration: 700 },
                hide: { effect: 'fade', duration: 500 },
                position: { my: "left top", at: "left top", of: "#ContentPlaceHolderMain_wucTabs_lblMaterial" },
                width: "800px",
                title: lblgr
            });

            $('#ContentPlaceHolderMain_btnCancelSelectQuantity').click(function () {
                ClosePlaceOrderDialog();
            });

        }

        function ClosePlaceOrderDialog() {
            $('#dialogPrices').dialog('close');
        }

        function ChangeBackgroundColor(item) {
            try {
                var dataListItems = document.querySelectorAll("#<%= lstvSelectedPrice.ClientID %> td");
                for (var i = 0; i < dataListItems.length; i++) {
                    var itemDiv = dataListItems[i];
                    RestoreBackgroundColor(itemDiv);
                };

                item.style.backgroundColor = '#bdcbe0'; // Change the background color to your desired color on mouseover
                document.getElementById('<%= hfSelectedQuantity.ClientID %>').value = item.innerText;
            }
            catch (error) {

            }
            DisableEnablePlaceOrder();
        }

        function RestoreBackgroundColor(item) {
            item.style.backgroundColor = ''; // Restore the original background color on mouseout
        }

        function PlaceOrderClick() {
            DisableEnablePlaceOrder();
            DisableblockUpdatProg();
            document.getElementById('<%= btnPlaceOrder.ClientID %>').click();
        }

        function DisableEnablePlaceOrder() {


            $('#btnPlaceOrderInput').val($('#ContentPlaceHolderMain_hfplaceanOrder').val());
            $('#ContentPlaceHolderMain_btnCancelSelectQuantity').val($('#hfbCancel').val());
            var SelectMail = document.getElementById('ContentPlaceHolderMain_txtSelectMail').value;
            var PhoneNo = document.getElementById('ContentPlaceHolderMain_txtSelectPhoneNo').value;
            var fileExInOrd = document.getElementById('ContentPlaceHolderMain_lblApprovalDocs').innerHTML;
            var SelectedQuantit = document.getElementById('ContentPlaceHolderMain_hfSelectedQuantity').value;
            var SelectFile = document.getElementById('ContentPlaceHolderMain_hfSelectFile').value;

            var CustomerOrdernoInOrder = document.getElementById('ContentPlaceHolderMain_txtCustomerOrderN1').value;
            var CustomerItemnoInOrder = document.getElementById('ContentPlaceHolderMain_txtCustomerItemN2').value;
            var CustomerAdditionnoInOrder = document.getElementById('ContentPlaceHolderMain_txtCustomerAdditionalReq').value;

            document.getElementById('ContentPlaceHolderMain_hfSelectedEmail').value = SelectMail;
            document.getElementById('ContentPlaceHolderMain_hfSelectedPhoneNo').value = PhoneNo;

            document.getElementById('ContentPlaceHolderMain_hfCustomerOrderN').value = CustomerOrdernoInOrder;
            document.getElementById('ContentPlaceHolderMain_hfCustomerItemN').value = CustomerItemnoInOrder;
            document.getElementById('ContentPlaceHolderMain_hfCustomerReqN').value = CustomerAdditionnoInOrder;

            var lDoenab = 'False';

            //29.07.24
            //if (CustomerAdditionnoInOrder != '' && CustomerItemnoInOrder != '' && CustomerOrdernoInOrder != '') {
            if (CustomerOrdernoInOrder != '') {
                lDoenab = 'True';
            };

            if ((lDoenab == 'True') && ((SelectMail != '' && SelectedQuantit != '' && SelectFile != '') || (SelectMail != '' && SelectedQuantit != '' && fileExInOrd != ''))) {
                document.getElementById('btnPlaceOrderInput').style.backgroundColor = "#48b649";
                document.getElementById('btnPlaceOrderInput').disabled = false;
                document.getElementById('btnPlaceOrderInput').style.cursor = 'pointer';
                //alert(1);
            } else {
                document.getElementById('btnPlaceOrderInput').style.backgroundColor = "lightgray";
                document.getElementById('btnPlaceOrderInput').disabled = true;
                document.getElementById('btnPlaceOrderInput').style.cursor = 'default';
                //alert(2);
            }
        }




        function SavedToMyQuotation() {
            let vhfMyQuotationsN = document.getElementById("ContentPlaceHolderMain_hfMyQuotations").value
            let vhfQuotationSavedN = document.getElementById("ContentPlaceHolderMain_hfQuotationSaved").value
            var f = "'";

            $.dialog({
                title: '',
                content: '<table width="200px" height="150px" style="margin: 0 auto; border: 0px solid; text-align: center"><tr><td><img src="../media/Images/Succeded.svg" style="height: 50px; float: right" /></td><td><div class="FontFamilyRoboto FontSizeRoboto18">' + vhfQuotationSavedN + '<br>' + vhfMyQuotationsN + '</div></td></tr></table>'
            });
        }

    </script>

    <style>
        .qtyButtonscss {
            width: 100%;
            border-style: none;
            display: flex;
        }


        @media screen and (max-width: 769px) {

            .qtyButtonscss {
                width: 100%;
                border-style: none;
                display: flex;
            }
        }


        @media screen and (max-width: 769px) {

            .qtyButtonscss {
                width: 100%;
                border-style: none;
                display: flex;
            }
        }

        @media screen and (max-width: 540px) {

            .qtyButtonscss {
                width: 100%;
                border-style: none;
                display: block;
            }
        }

        .qtyButtonscssInbut {
            width: 220px;
            margin-left: 18px;
            display: inline-flex;
            justify-content: center;
            align-items: center;
        }

        @media screen and (max-width: 540px) {

            .qtyButtonscssInbut {
                width: 220px;
                display: inline-flex;
                justify-content: center;
                align-items: center;
            }
        }

        .qtyButtonscssInbut2 {
            width: 200px;
            margin-left: 18px;
        }

        @media screen and (max-width: 540px) {

            .qtyButtonscssInbut2 {
                width: 200px;
                margin-left: 0px;
            }
        }

        .delBut {
            width: 120px;
            background-color: white;
            color: #12498a;
            border-style: solid;
            border-color: #12498a;
            border-width: 1px;
            margin-left: 20px
        }

        @media screen and (max-width: 540px) {
            .delBut {
                margin-left: 0px;
                float: left
            }
        }

        .leftwidthparamA {
        }

        @media screen and (max-width: 1024px) {
            .leftwidthparamA {
                min-width: 600px !important
            }
        }

        @media screen and (max-width: 768px) {
            .leftwidthparamA {
                /*min-width: 400px !important*/
                min-width: 100% !important
            }
        }

        @media screen and (max-width: 500px) {
            .leftwidthparamA {
                min-width: 300px !important;
                padding-left: 6px !important;
            }
        }


        .leftwidthparamB {
            /*width: 100%*/
        }

        @media screen and (max-width: 1200px) {
            .leftwidthparamB {
                min-width: 100% !important
            }
        }

        @media screen and (max-width: 769px) {
            .leftwidthparamB {
                min-width: 100% !important
            }
        }

        @media screen and (max-width: 500px) {
            .leftwidthparamB {
                min-width: 100% !important;
                padding-left: 6px !important;
            }
        }


        .TempAlertcs {
        }

        @media screen and (max-width: 769px) {
            .TempAlertcs {
                font-size: 12px
            }
        }

        .divContainerA {
            margin-right: 20px;
        }

        @media screen and (max-width: 500px) {
            .divContainerA {
                margin-right: 6px;
            }
        }


        .divPanelA {
            width: 100%;
            float: left;
            border: thin solid #e9eaec;
            border-style: none
        }

        @media screen and (max-width: 500px) {
            .divPanelA {
                margin-top: 10px !important;
                border-top: thin solid lightgray;
                padding-top: 10px
            }
        }


        .divContainerC {
        }

        @media screen and (max-width: 500px) {
            .divContainerC {
                margin-top: 10px !important;
                border-top: thin solid lightgray;
                padding-top: 10px
            }
        }

        .divPriceAll {
            max-height: 700px;
            overflow: auto;
        }

        @media screen and (max-width: 1030px) {
            .divPriceAll {
                max-height: 500px;
            }
        }

        @media screen and (max-width: 768px) {
            .divPriceAll {
                max-height: 680px;
            }
        }

        @media screen and (max-width: 500px) {
            .divPriceAll {
                max-height: 500px;
            }
        }
    </style>

    <wuc_Tabs:wuc_Tabs ID="wucTabs" runat="server" />

    <asp:UpdatePanel ID="upPnlAll_Prices" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveQuotation" />
            <asp:PostBackTrigger ControlID="gvDocuments" />
            <asp:AsyncPostBackTrigger ControlID="Tab2" />
            <asp:PostBackTrigger ControlID="btnDoDeleteDocument" />
            <asp:PostBackTrigger ControlID="btnDoDeleteDocumentCancel" />
            <asp:PostBackTrigger ControlID="btnSendTemporaryId" />
        </Triggers>
        <ContentTemplate>
            <asp:Button Name="BtnSubmitMessage" ID="btnSubmitMessagePrices" runat="server" CssClass="MyQuotationFind FontFamilyRoboto FontSizeRoboto13 BCbachColor FCbachColor display_non" Width="60px" Text="Report" />
            <asp:FileUpload runat="server" ID="fuFile" onchange="this.form.submit(); DisableblockUpdatProg();" CssClass="DisplayNone" />
            <%---------*****SS*****-------------%>
            <asp:FileUpload runat="server" ID="fuOrder" onchange="DisableblockUpdatProg(); this.form.submit();" EnableViewState="true" CssClass="DisplayNone" />
            <%---------**********-------------%>
            <input type="hidden" value="" id="lblSendTemporaryIdx" runat="server" clientidmode="Static" />
            <input type="hidden" value="" id="hfMinQuantity" runat="server" clientidmode="Static" />
            <asp:Button ID="btnSendTemporaryId" runat="server" Text="btnSendTemporaryId" CssClass="DisplayNone" />
            <asp:Button ID="btnSendTemporaryIdTech" runat="server" Text="btnSendTemporaryIdTech" CssClass="DisplayNone" />
            <asp:Button ID="btnFillGPHided" runat="server" Text="btnFillGPHided" CssClass="DisplayNone" />
            <asp:Button ID="btnDoDeleteQuotation" runat="server" Text="DoDeleteQuotation" CssClass="DisplayNone" />
            <asp:Button runat="server" ID="CuptureiFrame" Text="GetIframeImage" CssClass="display_non" />
            <asp:Button runat="server" Text="Refresh" ID="btnRefreshN" CssClass="display_non" />
            <asp:TextBox ID="BsonTextQutNo" runat="server" CssClass="display_non"></asp:TextBox>
            <asp:TextBox ID="BsonTextQutNoX" runat="server" CssClass="display_non"></asp:TextBox>
            <%--<asp:TextBox ID="BsonText" runat="server" CssClass="display_non"></asp:TextBox>--%>
            <asp:TextBox ID="BsonTextID" runat="server" CssClass="display_non"></asp:TextBox>
            <asp:Label ID="lblDOCSFileId" runat="server" CssClass="display_non"></asp:Label>
            <asp:Label ID="lblDOCSFileName" runat="server" CssClass="display_non"></asp:Label>
            <asp:Label ID="lblDOCSFolderPath" runat="server" CssClass="display_non"></asp:Label>
            <asp:Label ID="AllRedy3DAlertShows" runat="server" CssClass="display_non"></asp:Label>
            <asp:TextBox ID="txtDoTryToFindDrawing" runat="server" CssClass="display_non"></asp:TextBox>
            <asp:TextBox ID="txtDoDraw" runat="server" CssClass="display_non" ReadOnly="true"></asp:TextBox>
            <asp:HiddenField ID="hfSecondsAllCount" runat="server" />
            <asp:TextBox ID="hfSecondsStartCount" runat="server" CssClass="display_non" />
            <asp:Button ID="btnDoDeleteDocumentCancel" runat="server" Text="DoDeleteQuotation" Style="display: none;" />
            <asp:Button ID="btnDoDeleteDocument" runat="server" Text="DoDeleteQuotation" Style="display: none;" />
            <asp:HiddenField ID="hfSubmetted" runat="server" />
            <input type="hidden" id="hfAllredySentErrorMessage" value="" />
            <asp:HiddenField ID="hfPricesVers" runat="server" />
            <asp:HiddenField ID="hfPriceOrdered" runat="server" />
            <asp:HiddenField ID="hfPricesSelectedVers" runat="server" />
            <asp:HiddenField ID="hfTemporaryQuotation" runat="server" />
            <asp:HiddenField ID="hfRepoprtsNames" runat="server" />
            <asp:HiddenField ID="hfSelectFile" runat="server" />
            <asp:HiddenField ID="hfSelectedQuantity" runat="server" />
            <asp:HiddenField ID="hfSelectedEmail" runat="server" />

            <asp:HiddenField ID="hfCustomerOrderN" runat="server" />
            <asp:HiddenField ID="hfCustomerItemN" runat="server" />
            <asp:HiddenField ID="hfCustomerReqN" runat="server" />

            <asp:HiddenField ID="hfSelectedPhoneNo" runat="server" />
            <asp:HiddenField ID="hfFilesCoun" runat="server" />
            <%--<span id="spanS"></span>--%>
            <asp:HiddenField ID="hfcapiQuoteMessage" runat="server" Value="" />
            <asp:HiddenField ID="hfQuotationSaved" runat="server" Value="" />
            <asp:HiddenField ID="hfMyQuotations" runat="server" Value="" />
            <asp:HiddenField ID="hf3DlineSecond" runat="server" Value="" />
            <asp:HiddenField ID="hf3DlineFirst" runat="server" Value="" />

            <asp:HiddenField ID="hfSubmitAlertLine1" runat="server" Value="" />
            <asp:HiddenField ID="hfTheSerialNo" runat="server" Value="" />
            <asp:HiddenField ID="hfSubmitAlertLine2" runat="server" Value="" />
            <asp:HiddenField ID="hfSubmitAlertLine3" runat="server" Value="" />
            <asp:HiddenField ID="hfSubmitAlertLine4" runat="server" Value="" />
            <asp:HiddenField ID="hfSubmitAlertLine5" runat="server" Value="" />

            <asp:HiddenField ID="hfplaceanOrderTitle" runat="server" Value="" />
            <asp:HiddenField ID="hfplaceanOrder" runat="server" Value="" />
            <asp:HiddenField ID="hfadddocument" runat="server" Value="" />
            <asp:HiddenField ID="hfViwe" runat="server" Value="" />
            <asp:HiddenField ID="hf3DlabelView" runat="server" Value="" />
            <asp:HiddenField ID="hf2DlabelView" runat="server" Value="" />
            <asp:HiddenField ID="hfClose" runat="server" Value="" />
            <asp:HiddenField ID="hfBSONfolder" runat="server" Value="" />
            <asp:HiddenField ID="hfExistQ" runat="server" Value="" />
            <asp:HiddenField ID="hfDeleteQ" runat="server" Value="" />
            <asp:HiddenField ID="hfBackToEdit" runat="server" Value="" />
            <asp:HiddenField ID="hfInsertEmailAdd" runat="server" Value="" />
            <asp:HiddenField ID="hfDeleteQContent" runat="server" Value="" />

            <div class="divPriceAll">

                <div class="mainLeftRightPadding GlobDef_MLAll BorderRightLeft" id="GlobDef_MLAllsm">
                    <div class="" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; border-bottom: thin solid #e9eaec;">
                        <div class="rowP divContainerA">
                            <div class="col-smP leftwidthparamA" style="padding-left: 20px; padding-top: 10px; height: 80%" id="divLeftForH">
                                <div style="width: 100%; border-style: none; display: inline-block">
                                    <asp:Label ID="lblParamsDes" runat="server" Text="Parameters Overview" CssClass="MainSubTitleBlack FontFamily FontSizeRoboto18" ForeColor="#212529"></asp:Label>
                                    <div id="DataDiv" class="DataListcssR">
                                        <asp:ListView ID="lvParams" runat="server" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1" Enabled="false">

                                            <LayoutTemplate>
                                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>

                                            </LayoutTemplate>

                                            <GroupTemplate>

                                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>

                                            </GroupTemplate>
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlX" runat="server" CssClass="pnlParamS">
                                                    <asp:Label ID="lblParamArray" runat="server" Text='<%# Eval("ParamArray") %>' CssClass="ListViewLinkButtonNDisPrice FontFamilyRoboto FontSizeRoboto13"></asp:Label>
                                                    <div class="dispalyNonWhnM"></div>
                                                    <asp:Label ID="lblMeasure" runat="server" Text='<%# Eval("Measure") %>' CssClass="ListViewMeasureNDisPrice FontFamilyRoboto FontSizeRoboto13"></asp:Label>
                                                    <asp:Label ID="lblMeasureCaption" runat="server" Text='<%# Eval("Measure") %>' CssClass="ListViewMeasureNDisPrice FontFamilyRoboto FontSizeRoboto13"></asp:Label>


                                                    <div class="DisplayNone">
                                                        <asp:Panel ID="pnlPA0" runat="server" CssClass="cssPanelNormal">
                                                            <asp:ImageButton ID="pImgR" runat="server" ImageUrl='<%# Eval("ParamIcon")%> ' CssClass="ListViewImage" Enabled="false" />
                                                        </asp:Panel>
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

                            <div class="col-sm-13D leftwidthparamA" style="padding-left: 20px; padding-top: 10px;">
                                <div id="DataDivFistx" class="DataListcssL heightResp1 ">


                                    <div style="border-style: none;">
                                        <asp:Label runat="server" ID="lblQDtitle" CssClass="MainSubTitle FontFamily"></asp:Label>
                                        <asp:Label ID="lblQDtitleSM" runat="server" Text="" CssClass="FontSizeRoboto13 FontFamily ReshlTransition" ForeColor="Red" Font-Italic="true"></asp:Label>
                                    </div>
                                    <div id="divQdetails" style="border-style: none">
                                        <div style="border-style: none; float: left; margin-top: 10px;" class="divQdetailsA">
                                            <div>
                                                <asp:Label ID="lblQut" runat="server" Text="Quotation Number:" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay" Width="120px"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblQutNumber" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:Label ID="lblID" runat="server" Text="Item Description:" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay" Width="120px"></asp:Label>&nbsp;&nbsp;
                                                <%--<asp:Label ID="LblItemDesc" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto quotationDataLabel"></asp:Label>--%>
                                                <asp:Label ID="lblItemDescription" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:Label ID="lblDelv" runat="server" Text="Delivery Time:" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay" Width="120px"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblDelivery" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>
                                            <div class="DisplayNone">
                                                <asp:Label ID="lblcurrency" runat="server" Text="Customer Currency" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay" Width="120px"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblCustomerCurrency" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="border-style: none; float: left; margin-top: 10px;">
                                            <div>
                                                <asp:Label ID="lblCreatedDate" runat="server" Text="Created Date:" CssClass="FontFamilyRoboto FontSizeRoboto13 LabelTitle2_Colored margindisplay" Width="100px"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="LblCreateDate" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:Label ID="lblLastUpdate" runat="server" Text="Last Update:" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay " Width="100px"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblLastUpdateDate" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>
                                            <div>
                                                <asp:Label ID="lblExpiryDate" runat="server" Text="Expiry Date:" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay " Width="100px"></asp:Label>&nbsp;&nbsp;
                                                <asp:Label ID="lblExpDate" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            </div>

                                        </div>
                                    </div>

                                    <br />


                                    <div class="QuotationDetailsSquare divBorderSolidColored" style="display: none; vertical-align: top">



                                        <div>
                                            <asp:Label ID="Label4" runat="server" Text="Customer Number:" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay" Width="120px"></asp:Label>
                                            <asp:Label ID="lblCustomerNo" runat="server" CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13 margindisplay"></asp:Label>
                                            <asp:Label ID="lblCustoName" runat="server"></asp:Label>
                                            <asp:Label ID="lblCustomerAddress" runat="server"></asp:Label>
                                            <asp:Label ID="lblSubsidiary" runat="server"></asp:Label>
                                            <asp:Label ID="lblSubsAddre" runat="server"></asp:Label>
                                            <asp:Label ID="lblContactDetails" runat="server"></asp:Label>
                                            <asp:Label ID="lblISCARSubsidiary" runat="server"></asp:Label>
                                            <asp:Label ID="lblSubsidiaryAddress" runat="server"></asp:Label>
                                            <asp:Label ID="lblPaymenttTerms" runat="server"></asp:Label>
                                            <asp:Label ID="lblShippingMethod" runat="server"></asp:Label>

                                        </div>

                                        <div>
                                        </div>
                                    </div>

                                </div>

                                <div>
                                    <asp:UpdatePanel ID="upPrices" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="divPanelA">
                                                <div style="padding-bottom: 6px; display: flex">
                                                    <asp:Label CssClass="FontFamilyRoboto FontSize lblPricealert TempAlertcs" Visible="false" ID="lblPriceTempAlert" runat="server" Text="&nbsp;*Please note that in order to view prices you have to be logged in" ForeColor="Gray"></asp:Label>
                                                </div>
                                                <div>
                                                    <table style="width: 98%; margin-bottom: 10px;">
                                                        <tr>
                                                            <td>
                                                                <div id="" style="padding-right: 6px;">
                                                                    <asp:Panel ID="pnlViewHistory" runat="server">
                                                                        <button class="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton" style="width: 160px;" onclick="VersionHistory()" id="btn" value="">

                                                                            <asp:Label runat="server" ID="lblViHis"></asp:Label>
                                                                            <img src="../media/Icons/VHistory.png" style="height: 25px;" onclick="VersionHistory(); "></button>
                                                                    </asp:Panel>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div id="">

                                                                    <asp:Panel ID="pnlViewOrdered" runat="server">
                                                                        <button class="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton" style="width: 160px;" onclick="VersionOrdered()" id="btnOrdered">Quotation Orders&nbsp;&nbsp;<img src="../media/Icons/orderslogo.svg" style="height: 20px;" onclick="VersionOrdered(); "></button>
                                                                    </asp:Panel>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <asp:ValidationSummary runat="server" ID="ValidationSummary3" DisplayMode="List" CssClass="FontFamily validSumcss" />
                                                                <asp:Label runat="server" ID="ValidationSummaryLabel" CssClass="FontFamily validSumcss" Height="30px" Text="&nbsp;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>

                                                <div style="max-height: 425px; min-height: 230px; overflow-y: auto;" id="DataDivForScrolingA">
                                                    <asp:GridView runat="server" ID="gvPrices" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both">
                                                        <HeaderStyle CssClass="cssGridHeaderStyle FontSizeRoboto FontFamilyRoboto divBorderSolidColored" VerticalAlign="Middle" HorizontalAlign="Center" BorderColor="#e3e3e3" Wrap="true" />
                                                        <RowStyle CssClass="divBorderSolidColored cssGridRowStylePrice FontSizeRoboto FontFamilyRoboto" BorderStyle="Solid" />
                                                        <SortedAscendingCellStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderText="ID">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblPriceID"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:ButtonField ItemStyle-CssClass="ItemBorderS" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" ButtonType="Image" ImageUrl="~/media/Icons/OK_Dis.png" CommandName="btnPrice" ItemStyle-Width="20px"></asp:ButtonField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemBorderSLeft" HeaderStyle-CssClass="cssGridHeaderStyleLeft" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="&nbsp;&nbsp;&nbsp;Quantity" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtQuantity" CssClass="cssGridCell_1 FontFamilyRoboto FontSizeRoboto14" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemBorderSLeft AlignCenter" HeaderStyle-CssClass="cssGridHeaderStyleLeft" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Net Price" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtNetPrice" CssClass="cssGridCell_Color1_1 FontFamilyRoboto FontSizeRoboto14" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemBorderSLeft" HeaderStyle-CssClass="cssGridHeaderStyleLeft" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="&nbsp;&nbsp;Total Price" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtTotal" CssClass="cssGridCell_Color1_1 FontFamilyRoboto FontSizeRoboto14" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Cost" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtCostPrice" CssClass="cssGridCell_Color" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="GP">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtGP" CssClass="cssGridCell_Color" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Delivery Weeks">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtDeliveryWeeks" CssClass="cssGridCell_Color1_1" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="TFR Price" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtTFRPrice" CssClass="cssGridCell_Color1_1" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px" ItemStyle-Height="36px" ItemStyle-CssClass="ItemBorderSM paddingR2" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" HeaderText="" HeaderStyle-ForeColor="" HeaderStyle-CssClass="">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton runat="server" ID="ImgPqo" OnClientClick='<%# "return gridPricesOrderClick(" + DataBinder.Eval(Container.DataItem, "QTY").ToString + ",this.src)"%>' CssClass="PaddingCont paddingR2" CommandArgument="PaddingCont" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:ButtonField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" ButtonType="Image" ImageUrl="../media/Images/Pxl.png" CommandName="btnDelete" HeaderText="" ItemStyle-Width="20px"></asp:ButtonField>
                                                        </Columns>
                                                    </asp:GridView>



                                                    <asp:GridView runat="server" ID="gvPrices_Temp" AutoGenerateColumns="False" CssClass="cssGrid_NPrices" GridLines="Both">
                                                        <HeaderStyle CssClass="cssGridHeaderStyle FontSizeRoboto FontFamilyRoboto divBorderSolidColored " VerticalAlign="Middle" HorizontalAlign="Center" BorderColor="#e3e3e3" Wrap="true" />
                                                        <RowStyle CssClass="divBorderSolidColored cssGridRowStylePrice FontSizeRoboto FontFamilyRoboto" BorderStyle="Solid" />
                                                        <SortedAscendingCellStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-CssClass="ItemBorderSLeft" HeaderStyle-CssClass="cssGridHeaderStyleLeft padL" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="&nbsp;&nbsp;&nbsp;Quantity" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtQuantityTemp" CssClass="cssGridCell_1 FontFamilyRoboto FontSizeRoboto14 margL" Enabled="false" AutoCompleteType="None"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemBorderSLeft AlignCenter" HeaderStyle-CssClass="cssGridHeaderStyleLeft padL" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="Net Price" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtNetPriceTemp" CssClass="cssGridCell_Color1_1 FontFamilyRoboto FontSizeRoboto14" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField ItemStyle-CssClass="ItemBorderSLeft" HeaderStyle-CssClass="cssGridHeaderStyleLeft " HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Center" HeaderText="&nbsp;&nbsp;Total Price" HeaderStyle-Font-Bold="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox runat="server" ID="txtTotalTemp" CssClass="cssGridCell_Color1_1 FontFamilyRoboto FontSizeRoboto14" ReadOnly="true" Enabled="false"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>

                                            <div class="qtyButtonscss">
                                                <div style="width: 100%">
                                                    <asp:Button runat="server" Text="Update Quotation" ID="btnSaveQuotation" Width="200px" CssClass="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton" OnClientClick="DisableblockUpdatProg()" />

                                                </div>

                                                <asp:Panel runat="server" ID="pnlClient" Enabled="false">
                                                    <button id="btnShowExportTemp" type="button" class="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButtonDis qtyButtonscssInbut">
                                                        <img src="../media/Images/DEFAULTRun.gif" class="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButtonDis" style="float: left; height: 20px; width: 20px; margin-bottom: 3px;" />&nbsp;&nbsp;Submit Quotation</button>
                                                </asp:Panel>

                                                <asp:Panel runat="server" ID="pnlServer" Enabled="false">
                                                    <input type="button" class="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton qtyButtonscssInbut2 " onclick="CheckToSend()" id="btnShowExport" value="Submit Quotation" />
                                                </asp:Panel>

                                                <div>
                                                    <asp:Button runat="server" ID="btnDelQut" CssClass="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton floatrightCss delBut" OnClientClick="DeleteQuotation1()" Text="Delete Quotation" />
                                                </div>

                                                <asp:Button runat="server" Text="SR" ID="btnCreateSendMail2" CssClass="DisplayNone" Width="100px" OnClientClick="DisableHideUpdatProg()" />

                                                <asp:TextBox ID="txtFlagPricesChanged" runat="server" CssClass="display_non" ReadOnly="true"></asp:TextBox>
                                            </div>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-sm-13D leftwidthparamB" style="padding-left: 20px; padding-top: 10px;">
                                <div style="float: right">
                                    <div style="border-style: none; float: right">
                                        <div class="container">
                                            <div class="rowP divContainerC">
                                                <div style="width: 100%; border-style: none;">
                                                    <asp:Label runat="server" ID="lblQD" CssClass="MainSubTitleK FontFamily" Text="Quotation Documents"></asp:Label>
                                                    <div class="cssUnitSwitchflags" style="border-left: none; float: right">
                                                        <img id="currentflagPrice" class="MNUiMGCSS" title="" alt="" src="../media/flags/ZZ.svg" height="32" style="padding: 0px !important">
                                                        <div id="dialogLangPrice" title="" class="flagDiv">
                                                            <asp:Repeater ID="rptLanguagesListPrice" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="width: 100%; direction: ltr; display: inline-block; text-align: left">
                                                                        <a href="javascript:ReportDeleteAlertPrice('<%# Eval("LanguageCode")%>','<%# Eval("ISOCode")%>');" style="text-decoration: none;">
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="flagcounty" src="../media/flags/<%# Eval("LanguageImg")%>.svg">&nbsp;</td>
                                                                                    <td>&nbsp;<%# Eval("LanguageName")%></td>
                                                                                </tr>
                                                                            </table>
                                                                        </a>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                    <asp:Label runat="server" ID="lblSelectRepLang" CssClass="MainSubTitleK FontFamily floatrightCss" Text="Select Report Language"></asp:Label>
                                                </div>
                                                <div id="divsmScal" class="" style="float: left; max-height: 170px; overflow-x: auto; width:100% ">
                                                    <div>
                                                        <asp:UpdatePanel ID="upPnlAll_Documents" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <div id="upPnlAll_Documentsdiv" runat="server" style="overflow-y: auto; overflow-x: hidden; border-style: solid; border-color: #f3ebeb; border-width: thin; height: 110px">
                                                                    <asp:GridView runat="server" ID="gvDocuments" AutoGenerateColumns="False" CssClass="cssGrid_NDoc" GridLines="Both" ShowHeaderWhenEmpty="true" ShowFooter="false" ShowHeader="true">
                                                                        <HeaderStyle CssClass="FontFamilyRoboto LabelTitle2_Colored FontSizeRoboto13" BorderColor="" Wrap="true" BackColor="#e9eaec" />
                                                                        <RowStyle CssClass="divBorderSolidColored cssGridRowStyle FontSizeRoboto13 FontFamilyRoboto" />
                                                                        <Columns>
                                                                            <asp:BoundField DataField="FileId" HeaderText="FileId" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                                            <asp:TemplateField ItemStyle-Width="22px" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton Enabled="false" CssClass="css_GridItemImageW" ID="NewImage" runat="server" ImageUrl="~/media/Icons/NEWc.png" AlternateText="NewFile" ToolTip="New" CommandArgument='<%# Container.DisplayIndex %>' CommandName="NewFile" Width="20px" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Desc" HeaderText="File Type" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="ItemBorderSDoc" HeaderStyle-Font-Bold="false" ItemStyle-Font-Bold="false" />
                                                                            <asp:BoundField DataField="FName" HeaderText="File Name" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="ItemBorderSDoc" HeaderStyle-Font-Bold="false" ItemStyle-Font-Bold="false" />
                                                                            <asp:BoundField DataField="FilePath" HeaderText="FilePath" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                                            <asp:BoundField DataField="FolderArr" HeaderText="FolderArr" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                                            <asp:BoundField DataField="Subject" HeaderText="Subject" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                                            <asp:BoundField DataField="FileDate" DataFormatString="{0:G}" HeaderText="File Date" ItemStyle-BorderStyle="None" HeaderStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="ItemBorderSDoc" HeaderStyle-Font-Bold="false" ItemStyle-Font-Bold="false" />
                                                                            <asp:BoundField DataField="Object" HeaderText="Object" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                                            <asp:BoundField DataField="SubjectId" HeaderText="SubjectId" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                                            <asp:BoundField DataField="FileSize" HeaderText="File Size (KB)" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="ItemBorderSDoc" HeaderStyle-Font-Bold="false" ItemStyle-Font-Bold="false" />
                                                                            <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton CssClass="css_GridItemImageW" ID="DeleteButton" runat="server" ImageUrl="~/media/Icons/Delete.png" AlternateText="DeleteFile" ToolTip="Delete File" CommandArgument='<%# Container.DisplayIndex %>' CommandName="DeleteFile" OnClientClick="cancelonbeforeunload();" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None">
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton CssClass="css_GridItemImageW" ID="OpenButton" runat="server" ImageUrl="~/media/Icons/DOWNLOADc.png" AlternateText="btnOpen" ToolTip="Download" CommandArgument='<%# Container.DisplayIndex %>' CommandName="btnOpen" OnClientClick="cancelonbeforeunload();" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                    </asp:GridView>

                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>

                                                        <input id="btn_adddocument" type="button" onclick=" showBrowseDialog();" value="+Add documents (Images\Documents\CAD)" class="FontFamilyRoboto FontSizeRoboto12 " style="font-weight: bold; border-style: none; background-color: white; color: #12498a" />
                                                        <asp:Label runat="server" ID="lblMaxFileSize" class="ItemBorderSDoc" Style="color: gray; float: right; border: none; font-size: 12px">*max file size 10MB

                                                        </asp:Label>

                                                    </div>
                                                </div>
                                                <div class="col-sm-51P" style="border: none; float: right; max-height: 180px; height: 100%;">

                                                    <div style="width: 100%; border-style: none;">
                                                    </div>
                                                    <div class="QuotationDetailsSquare " style="width: 100%">

                                                        <div style="border-style: none; border-color: #fff2f2; width: 210px; float: right; direction: rtl; text-align: right">
                                                            <asp:Button Enabled="false" runat="server" Text="View Similar Standard Items" ID="btmVSI" CssClass="FontFamilyRoboto FontSizeRoboto QuotationDetailsButtonDis" Width="210px" Visible="false" />
                                                        </div>
                                                        <div style="border-style: none; border-color: #fff2f2; width: 210px; display: flex; float: right; direction: rtl">
                                                            <div style="border-style: none; border-color: #fff2f2; width: 50%; float: right; direction: rtl; text-align: right">
                                                            </div>
                                                            <div style="border-style: none; border-color: #fff2f2; width: 50%; display: flex; float: left; direction: ltr">
                                                            </div>
                                                        </div>
                                                        <div style="border-style: none; border-color: #fff2f2; width: 210px; display: flex; float: right; direction: rtl">
                                                            <div style="border-style: none; border-color: #fff2f2; width: 50%; float: right; direction: rtl; text-align: right">
                                                            </div>
                                                            <div style="border-style: none; border-color: #fff2f2; width: 50%; display: flex; float: left; direction: ltr">
                                                            </div>
                                                        </div>

                                                        <table style="width: 100%; direction: rtl; margin: 0px 0px 0px 0px">
                                                            <tr>
                                                                <td colspan="2" style="text-align: right"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="text-align: right"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="text-align: right">

                                                                    <div id="divAddOrders" class="Export_Price" style="visibility: hidden;">
                                                                        <div style="text-align: right">
                                                                            <input type="button" class="ButtonClose" onclick="HideExport()" value="X" />
                                                                        </div>
                                                                        <div style="text-align: left; padding-left: 10px;">
                                                                        </div>
                                                                        <br />
                                                                        <hr class="DifLine" />
                                                                        <div style="width: 100%; margin-right: -10px; text-align: left; vertical-align: top">
                                                                            <div style="margin-bottom: 4px;">
                                                                                <asp:Button runat="server" Text="Create Report" ID="btnCreate_Report2" CssClass="FontFamilyRoboto FontSizeRoboto QuotationDetailsButton" Width="100px" />
                                                                            </div>

                                                                            <asp:Button ID="btnSendMail" runat="server" CssClass="btnButtonsD FontFamily " Text="Send" OnClientClick="CheckIfTestFill(); return false;" />

                                                                            <asp:Button ID="btnSendMailHT" runat="server" CssClass="btnButtonsD FontFamily display_non" Text="Send" />
                                                                            <asp:TextBox ID="txtEmailAdd" CssClass="FontFamilyRoboto FontSizeRoboto13 AlignLeft FloatRLeft TextDirF" BorderStyle="Solid" BorderWidth="1px" BorderColor="#c0c0c0" runat="server" Width="150px" placeholder=" Email address"></asp:TextBox>

                                                                            <asp:RegularExpressionValidator CssClass="FontSizeOswald16 FontFamily" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ID="RegularExpValidEmail" runat="server" ControlToValidate="txtEmailAdd" Text="!Input value is an email address" ForeColor="Red"></asp:RegularExpressionValidator>
                                                                        </div>
                                                                        <br />

                                                                    </div>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div style="width: 100%; overflow-y: hidden !important; overflow-x: hidden;" class="" id="diviFrameHeight">
                                    <div style="width: 100%; float: right;" class="iFrameHcss2">

                                        <div style="width: 100%; float: right; height: 37px; position: relative">
                                            <div style="position: absolute; bottom: 0; left: 0">
                                                <asp:Label runat="server" ID="lblQuotationDr" CssClass="MainSubTitleK FontFamily" Text="Quotation Drawing / 3D Model"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="width: 99%; height: 92%; float: right; border: thin solid #e9eaec; margin-top: 6px;">
                                            <div style="width: 100%; float: right; text-align: right; border-style: none; height: 100%;">
                                                <iframe runat="server" id="ifif" style="width: 100%; float: right; background-color: white; border-style: none;"></iframe>
                                                <iframe runat="server" id="PdfView" style="width: 100%; float: right; background-color: white; border-style: none;" enableviewstate="true"></iframe>
                                            </div>

                                        </div>
                                    </div>
                                    <div style="width: 100%; float: right; text-align: right; border-style: none; padding-top: 10px;">
                                        <div style="display: flex; width: 100%">
                                            <input id="lbl_View" type="text" class="Initial3D_R FontFamilyRoboto FontSizeRoboto13 " value="View: " style="width: 60px; border-style: none; cursor: default; color: black" />
                                            <input name="OptD" type="radio" id="btnA_R" class="Initial3D_RR" onchange="drawingShow('BSON'); " />
                                            <label onclick="drawingShow('BSON');" id="3DlabelView" style="cursor: default; text-align: left; padding-left: 10px; margin-top: 8px" class="Initial3D_R FontFamilyRoboto FontSizeRoboto13 ">3D Model</label>
                                            <input name="OptD" type="radio" id="btnC_R" class="Initial3D_RR" onchange="drawingShow('PDF');" />
                                            <label onclick="drawingShow('PDF');" id="2DlabelView" style="cursor: default; padding-left: 10px; width: 150px; text-align: left; margin-top: 8px" class="Initial3D_R FontFamilyRoboto FontSizeRoboto13" />
                                            Technical Drawing</label>

                                        </div>
                                        <br />

                                        <asp:TextBox ID="txtPDFexist" runat="server" CssClass="display_non"></asp:TextBox>
                                        <asp:TextBox ID="txtPDFexistBSON" runat="server" CssClass="display_non"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtTabToShow" runat="server" CssClass="display_non"></asp:TextBox>--%>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <asp:Button runat="server" Text="." ID="btnPlaceOrder" Width="150px" CssClass="display_non" OnClick="btnPlaceOrder_Click" />

                </div>

                <%---------------*************---------------------%>
                <div style="position: absolute; display: none;">
                    <asp:Panel runat="server" ID="PanelParamList" Height="200px" Width="100%" CssClass="pnlRullescss">


                        <asp:GridView Width="100%" runat="server" ID="dgvParamList" AutoGenerateColumns="False" CssClass="cssGridParams" GridLines="None" HorizontalAlign="Center" Font-Bold="True" ShowHeader="False">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="SelectParam" />
                                <asp:BoundField DataField="TabIndex" HeaderText="TabIndex" />
                                <asp:BoundField DataField="Label" HeaderText="Label" ItemStyle-Width="140px" />
                                <asp:BoundField DataField="CostName" HeaderText="Label" ItemStyle-Width="40px" />

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
                            </Columns>
                            <AlternatingRowStyle BackColor="White" />
                            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" />
                            <RowStyle BackColor="#FFFFE6" ForeColor="#333333" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:ValidationSummary runat="server" ID="vsMessages" Font-Bold="True" />

                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="DisplayNone"></asp:Button>

                    <asp:Button ID="btmDrawint3D" runat="server" Text="Drawint-2D" CssClass="DisplayNone" Visible="false"></asp:Button>

                    <asp:Button ID="btnCatiaDrawing" runat="server" Text="Drawing" CssClass="DisplayNone"></asp:Button>
                    <asp:ValidationSummary runat="server" ID="VldUser" DisplayMode="List" CssClass="validSumcss" />
                    <asp:Panel runat="server" ID="pnlbson">
                    </asp:Panel>
                    <asp:ValidationSummary runat="server" ID="ValidationSummary1" DisplayMode="List" CssClass="FontFamily validSumcss" />
                    <asp:Label ID="label2" runat="server" Text="GP/COST" CssClass="RegLabel" Width="50px" EnableViewState="true"></asp:Label>
                    <asp:CheckBox ID="chkGPCOST" Text="" runat="server" AutoPostBack="true" />
                    <asp:Label ID="labelC" runat="server" Text="Currency" CssClass="RegLabel" Width="50px" EnableViewState="true"></asp:Label>
                    <asp:TextBox ID="txtCurrency" runat="server" Width="60px" CssClass="RegTextBold AlignCenter FontFamily FontSize" ReadOnly="true"></asp:TextBox>
                </div>

                <asp:TextBox ID="txtPdfViewsrc4" runat="server" CssClass="display_non"></asp:TextBox>
                <asp:Button Text="3D Model Preview" BorderStyle="None" ID="TabBSON" CssClass="display_non" runat="server" />
                <asp:Button Text="Drawing Preview" BorderStyle="None" ID="Tab2" CssClass="display_non" runat="server" />
                <asp:Button Text="Drawing Preview" BorderStyle="None" ID="Tab2OnlySesstion" CssClass="display_non" runat="server" />
                <asp:TextBox Text="" ID="txtAllReadyTriedtoBuildDrawing" runat="server" CssClass="display_non" />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <input type="button" id="btnSaveQuotationPriceOrder" value="Place Order" style="margin-left: 18px; width: 120px; background-color: #48b649 !important" class="DisplayNone" />

    <%---------------******SS*******---------------------%>
    <div id="dialogPrices" title="" class="OrderDivPrice">


        <img id="img1Price" class="imgNoPrice" title="" alt="" src="../media/Icons/Orde1.png" style="width: 33px; vertical-align: bottom">
        <asp:Label runat="server" ID="lblSelectQutstitle" class="FontSizeRoboto22 FontFamilyRoboto">Select Quantity </asp:Label>
        <asp:Label runat="server" ID="lblSelectQutstitleS" class="FontSizeRoboto13 FontFamilyRoboto">(Please note: the displayed prices represent the <b style="font-weight: 600">total price)</b></asp:Label>



        <div>
            <asp:DataList runat="server" ID="lstvSelectedPrice" RepeatColumns="3" Width="90%" RepeatDirection="Horizontal" CssClass="cssSelectedPrice" EnableViewState="false">
                <ItemTemplate>
                    <div class="OrderDivPricediv" id="divDivPricetable">
                        <table class="flagDivPricetable">
                            <tr>
                                <td class="OrderDivPricetd" onclick="ChangeBackgroundColor(this);" onmouseover="this.style.cursor='pointer'" id="lblQtySelectPtdPrev">
                                    <asp:Label runat="server" ID="lblQtySelectP" Text='<%# Eval("Qty")%>' EnableViewState="false"></asp:Label>
                            </tr>
                            <tr>
                                <td class="OrderDivPricetd OrderDivPricetdNet" id="tdNetPrice">
                                    <asp:Label runat="server" ID="lblQtySelectN" Text='<%# Eval("NetPrice")%>' EnableViewState="false"></asp:Label>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>


                    </div>

                </ItemTemplate>
            </asp:DataList>

        </div>
        <div style="width: 100%; display: flex">
            <div id="dialogPrices2" title="" class="OrderDivPrice">
                <img id="img2Price" class="imgNoPrice" title="" alt="" src="../media/Icons/Orde2.png" style="width: 33px; vertical-align: bottom">
                <asp:Label runat="server" ID="lblSelectQutstitle2" class="FontSizeRoboto22 FontFamilyRoboto">Attach Signed Drawing</asp:Label>
                <div style="width: 100%;">
                    <div>
                        <label class="file-upload pricesMArgl" style="width: 180px;">
                            <img src="../media/Icons/foldericon.svg" style="margin-left: 10px; " />
                            <input type="button" onclick="showBrowseDialogOrder()" style="height: 3px" /><asp:Label runat="server" Text="Attach File" ID="lblPlaceanOrderAttachFile"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                        </label>
                    </div>
                    <div style="margin-left: 37px">
                        <asp:Label Text="" runat="server" ID="lblApprovalDocs" CssClass="FontFamilyRoboto FontSizeRoboto13"></asp:Label>
                    </div>
                    <table style="margin-left: 14px">
                        <tr>
                            <td style="width: 20px">
                                <img id="imgorderFer" title="" alt="" src="../media/Icons/redwarning.png" style="display: none"></td>
                            <td>
                                <label id="lblrongTypeVal" class="FontFamilyRoboto FontSizeRoboto13 validSumcss " style="display: none; width: 450px; font-weight: bold">Invalid file format. Please upload PDF/PNG/JPEG only.</label></td>
                        </tr>
                    </table>

                </div>

                <div>&nbsp;</div>
                <img id="img3Price" class="imgNoPrice" title="" alt="" src="../media/Icons/Orde3.png" style="width: 33px; vertical-align: bottom">
                <asp:Label runat="server" ID="lblSelect" class="FontSizeRoboto22 FontFamilyRoboto">My Contact Details</asp:Label>
                <div>
                    <div style="width: 180px; display:unset"><label id="lblead" class="FontFamilyRoboto FontSizeRoboto14" style="width: 120px; margin-left: 37px;padding-right: 5px">* Email Address</label></div>
                    <asp:TextBox Width="200px" CssClass="QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14 alignleft" ID="txtSelectMail" runat="server" onkeyup="DisableEnablePlaceOrder()"></asp:TextBox><br />
                    <div style="width: 180px; display:unset"><asp:Label runat="server" ID="lbleadP" class="FontFamilyRoboto FontSizeRoboto14" Style="width: 120px; margin-left: 37px; ">&nbsp&nbsp;Phone Number</asp:Label></div>
                    <asp:TextBox Width="200px" CssClass="QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14 alignleft" ID="txtSelectPhoneNo" runat="server" onkeyup="DisableEnablePlaceOrder()"></asp:TextBox>
                </div>
            </div>
            <div id="dialogPrices3" title="" class="OrderDivPrice" style="padding-left: 10px;">
                <div>
                    <img id="img2Price4" class="imgNoPrice" title="" alt="" src="../media/Icons/Orde4.png" style="width: 33px; vertical-align: bottom">&nbsp;<asp:Label runat="server" ID="lblCustomerOrderN1" class="FontSizeRoboto22 FontFamilyRoboto"> Customer Order Number</asp:Label>
                </div>
                <div>
                    <asp:TextBox placeholder="&nbsp;*" Width="240px" CssClass="QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14 alignleft pricesMArgl" ID="txtCustomerOrderN1" runat="server" onkeyup="DisableEnablePlaceOrder()"></asp:TextBox>
                </div>
                <div>
                    <img id="img2Price5" class="imgNoPrice" title="" alt="" src="../media/Icons/Orde5.png" style="width: 33px; vertical-align: bottom">&nbsp;<asp:Label runat="server" ID="lblCustomerItemN2" class="FontSizeRoboto22 FontFamilyRoboto"> Customer Item Number</asp:Label>
                </div>
                <div>
                    <asp:TextBox Width="240px" CssClass="QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14 alignleft pricesMArgl" ID="txtCustomerItemN2" runat="server" onkeyup="DisableEnablePlaceOrder()"></asp:TextBox>
                </div>
                <div>
                    <img id="img2Price6" class="imgNoPrice" title="" alt="" src="../media/Icons/Orde6.png" style="width: 33px; vertical-align: bottom">&nbsp;<asp:Label runat="server" ID="lblCustomerAdditionalReq" class="FontSizeRoboto22 FontFamilyRoboto"> Customer Additional Req.</asp:Label>
                </div>
                <div>
                    <asp:TextBox Width="240px" CssClass="QtyEnableCssControl FontFamilyRoboto FontSizeRoboto14 alignleft pricesMArgl" ID="txtCustomerAdditionalReq" runat="server" onkeyup="DisableEnablePlaceOrder()"></asp:TextBox>
                </div>
            </div>

        </div>

        <div>&nbsp;</div>
        <div style="text-align: center">
            <asp:Button runat="server" Text="Cancel" ID="btnCancelSelectQuantity" Width="100px" CssClass="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton" />
            &nbsp;&nbsp;&nbsp;&nbsp;
                      
                          <input type="button" value="Place Order" id="btnPlaceOrderInput" style="width: 128px" class="FontFamilyRoboto FontSizeRoboto13 QuotationDetailsButton " onclick="PlaceOrderClick()" />
        </div>


    </div>
    <%---------------*************---------------------%>

    <script>

        function CheckIfTestFill() {

            if (document.getElementById("ContentPlaceHolderMain_txtEmailAdd").value == '') {
                document.getElementById("ContentPlaceHolderMain_RegularExpValidEmail").style.visibility = "visible";
            }
            else {

                btnSS = document.getElementById("ContentPlaceHolderMain_btnSendMailHT");
                btnSS.click();
            }
        }

        function sleepBSONrun(mssleepBSONrun) {
            return new Promise(resolveBSON => setTimeout(resolveBSON, mssleepBSONrun));
        }
        async function CheckIfBSONExist(SHOWALERT) {
            SecForl = document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value;
            var addressddrun = document.getElementById("ContentPlaceHolderMain_BsonTextQutNo").value;
            var ulObj;
            var ulObjn;
            var ulObjL;

            for (let jjrun = 0; jjrun <= SecForl; jjrun++) {

                try {
                    SecForl = document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value;
                    if (urlExistsBSONrun(addressddrun)) {
                        drawingShow('BSON')

                    } else {

                        if (SecForl <= 0) {
                            if (SHOWALERT == 'YES') {
                                if (document.getElementById("hfAllredySentErrorMessage").value == '') {
                                    document.getElementById("hfAllredySentErrorMessage").value = "YES";
                                    DrawingErrorSendTechnicalMessage('3D model');
                                }
                            }
                            return false;
                        }
                    };

                    await sleepBSONrun(1000);
                } catch (error) {
                    return false
                }

            };

            if (SecForl != '1') {
                return;
            };
            return false
        };

        function urlExistsBSONrun(testUrlBSON) {
            var httpF = jQuery.ajax({
                type: "HEAD",
                url: testUrlBSON,
                async: false
            })
            return httpF.status != 404;
        }

        function sleepFindPDFRUN(msPDFRUN) {
            return new Promise(resolveFindPDFRUN => setTimeout(resolveFindPDFRUN, msPDFRUN));
        }

        async function CheckIfBSONExistPDF(SHOWALERT) {

            SecForlBSON = document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value;

            var address4PDFRUN = document.getElementById("ContentPlaceHolderMain_txtPdfViewsrc4").value;
            var ulObjPDF;
            var ulObjnPDF;
            var ulObjLPDF;

            for (let iPDFRUN = 0; iPDFRUN <= SecForlBSON; iPDFRUN++) {
                try {
                    SecForlBSON = document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value;
                    if (urlExistsPDFRUN(address4PDFRUN)) {
                        drawingShow('PDF')
                    }
                    else {

                        if (SecForlBSON <= 0) {
                            if (SHOWALERT == 'YES') {
                                if (document.getElementById("hfAllredySentErrorMessage").value == '') {
                                    document.getElementById("hfAllredySentErrorMessage").value = "YES";
                                    DrawingErrorSendTechnicalMessage('2D model');
                                }
                            }
                            return false;
                        }
                    }
                    await sleepFindPDFRUN(1000);
                } catch (error) {
                    return false
                };

            };
        };

        function urlExistsPDFRUN(testUrlPDFRUN) {
            var httpPdfRr = jQuery.ajax({
                type: "HEAD",
                url: testUrlPDFRUN,
                async: false
            })
            return httpPdfRr.status != 404;
        };
        function urlExistsBSONRUNn(testUrlBSONRUN) {
            var httpbsonRr = jQuery.ajax({
                type: "HEAD",
                url: testUrlBSONRUN,
                async: false
            })
            return httpbsonRr.status != 404;
        };
        function sleepFindPDFRUN3D(msPDFRUN3D) {
            return new Promise(resolveFindPDFRUN3D => setTimeout(resolveFindPDFRUN3D, msPDFRUN3D));
        }

        async function CheckIfBSONExistPDF3D(SHOWALERT) {
            let vhfMyQuotations = document.getElementById("ContentPlaceHolderMain_hf3DlineFirst").value
            let vhfQuotationSaved = document.getElementById("ContentPlaceHolderMain_hf3DlineSecond").value
            var SecForlBSONPDF;
            var SecForlBSONPDFP;
            if (document.getElementById("ContentPlaceHolderMain_BsonTextQutNoX").value != '') {

                var address4PDFRUN3D = document.getElementById("ContentPlaceHolderMain_BsonTextQutNoX").value;
                for (let SecForlBSONPDF = 0; SecForlBSONPDF >= 0; SecForlBSONPDF++) {
                    SecForlBSONPDF = document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value;

                    SecForlBSONPDFP = document.getElementById("ContentPlaceHolderMain_txtPdfViewsrc4").value;

                    if (urlExistsPDFRUN3D(address4PDFRUN3D) && urlExistsPDFRUN3D(SecForlBSONPDFP)) {
                        const mobileWidthThreshold = 768;
                        let witE;
                        if (window.innerWidth <= mobileWidthThreshold) {
                            witE = '320px';
                            iQuoteMessage3Dmodel('3D Model and Drawing Preview<br>Created!', 'These documents are attached to<br>Quotation Documents!', witE, true);
                        } else {
                            witE = '520px';
                            iQuoteMessage3Dmodel(vhfMyQuotations, vhfQuotationSaved, witE, false);
                        };

                        return false
                    }
                    else {
                        if (SecForlBSONPDF <= 0) {
                            if (SHOWALERT == 'YES') {
                                if (document.getElementById("hfAllredySentErrorMessage").value == '') {
                                    document.getElementById("hfAllredySentErrorMessage").value = "YES";
                                    DrawingErrorSendTechnicalMessage('2D/3D model');
                                }
                            }

                            return false;
                        }
                    }
                    await sleepFindPDFRUN3D(1000);
                };

                function urlExistsPDFRUN3D(testUrlPDFRUN3D) {
                    var httpPdfRr3D = jQuery.ajax({
                        type: "HEAD",
                        url: testUrlPDFRUN3D,
                        async: false
                    })
                    return httpPdfRr3D.status != 404;
                };
            };
        }

        function ReportDeleteAlert(sARep, sARep2) {
            $.confirm({
                useBootstrap: false,
                boxWidth: '520px',
                title: '<div class="AlertTitleStyle">Report Generetion</div>',
                content: '<div style="width:95%;">' +
                    '<div style=" text-align: left; margin-left: 10px; margin-top: 5px;"><input type="text" style="font-weight:bold" disabled="disabled" class="MessageTitleLabel2 MailGeneralBodyRowStyle" value="' + sARep + '" ></div>' +
                    '<div style=" text-align: left; margin-left: 10px; margin-top: 5px;"><input type="text" style="font-weight:bold" disabled="disabled" class="MessageTitleLabel2 MailGeneralBodyRowStyle" value="' + sARep2 + '" ></div>' +
                    '</div><div>&nbsp;</div>',
                buttons: {
                    somethingElse: {
                        text: 'Ok',
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {
                            $("#ContentPlaceHolderMain_btnRefreshN").click();
                        }
                    }
                }
            }
            )
        }

        function iQuoteMessage3Dmodel(sA1, sA2, witE, useb) {
            let hfcount = document.getElementById('ContentPlaceHolderMain_hfcapiQuoteMessage').value;
            let hfCloseBut = document.getElementById('ContentPlaceHolderMain_hfClose').value;
            $.confirm({
                useBootstrap: useb,
                boxWidth: witE,
                title: false,
                content: '<div>' +
                    '<div style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; margin-top: 5px; "><input type="text" disabled="disabled" class="FontFamily MainSubTitle" value="' + hfcount + '"></div>' +
                    '<div style="text-align: left; width: 90%;overflow:hidden; margin-left: 10px; margin-top: 5px;">' +
                    '<div style="text-align: left; margin-left: 10px; margin-top: 5px; font-weight:bold" class="MessageTitleLabel2 MailGeneralBodyRowStyle" >' + sA1 + '</div>' +
                    '<div style="width: 100%; text-align: left; margin-left: 10px; margin-top: 5px; width: 100%; font-weight:bold" class="MessageTitleLabel2 MailGeneralBodyRowStyle">' + sA2 + '</div>' +
                    '<div>&nbsp;</div>' +
                    '<div>&nbsp;</div>' +
                    '</div></div>',
                buttons: {
                    somethingElse: {
                        text: hfCloseBut,
                        btnClass: 'btn-blueD btnDlgDeclineSend',
                        action: function () {
                            $("#ContentPlaceHolderMain_btnRefreshN").click();
                        }
                    }

                }
            }
            )
        }

        function pageLoad() {
            //checkBSONTEXTID();  
            drawingShow('PDF');
            drawingShow('BSON');

            //drawingShow('PDF', 'BSON');

            try {
                document.getElementById("btnShowExport").value = document.getElementById("ContentPlaceHolderMain_hfSubmetted").value;
                document.getElementById("btnShowExportTemp").value = document.getElementById("ContentPlaceHolderMain_hfSubmetted").value;
            } catch (pageLoad) {
            };

            if (document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value != '0' && document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value != '') {
                const myTimeout = setTimeout(SetTimerDrawingFieldValue, document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value * 1000);
            }

            var addressddF = document.getElementById("ContentPlaceHolderMain_BsonTextQutNo").value;

            if (addressddF == "EXIST") {
                //  alert('exist');
                //document.getElementById("btnA_R").disabled = false;
                //document.getElementById("btnC_R").disabled = false;
                //document.getElementById("2DlabelView").disabled = false;
                //document.getElementById("btnA_R").className = "Initial3D_RR";
                //document.getElementById("3DlabelView").className = "Initial3D_R";
                //document.getElementById("3DlabelView").disabled = false;
                //document.getElementById("btnC_R").className = "Initial3D_RR";
                //document.getElementById("2DlabelView").className = "Initial3D_R";
            }
            else {

                if (document.getElementById("ContentPlaceHolderMain_txtDoDraw").value == 'YES') {
                    if (document.getElementById("ContentPlaceHolderMain_txtAllReadyTriedtoBuildDrawing").value == 'YES2') {
                        document.getElementById("ContentPlaceHolderMain_txtAllReadyTriedtoBuildDrawing").value = 'YES3'
                        //setTimeout(CheckIfBSONExist("YES"), 2000);
                        //setTimeout(CheckIfBSONExistPDF("YES"), 2000);
                        setTimeout(CheckIfBSONExistPDF3D("YES"), 2000);
                    }
                    else {
                        if (document.getElementById("ContentPlaceHolderMain_txtAllReadyTriedtoBuildDrawing").value == 'YES4') {
                            //setTimeout(CheckIfBSONExistPDF("YES"), 2000);
                            setTimeout(CheckIfBSONExistPDF3D("YES"), 2000);
                        }
                        else {
                            //CheckIfBSONExist("NO")
                            //CheckIfBSONExistPDF("NO")
                        }
                    }
                }
            };

            SetCaptionForLabelsCon();
        }

        function SetTimerDrawingFieldValue() {
            document.getElementById("ContentPlaceHolderMain_hfSecondsStartCount").value = '0';
        }

        function CopyToClipboard() {
            document.getElementById("ContentPlaceHolderMain_lblSerToc").value = document.getElementById("ContentPlaceHolderMain_lblQutNumber").innerHTML;
            var copyTxt = document.getElementById("ContentPlaceHolderMain_lblSerToc");
            copyTxt.select();
            document.execCommand("copy");
            document.getElementById("btnCopeSer").focus;
            $.dialog('<br><div style="margin-top: 50px; height: 80px; " class="FontFamilyRoboto FontSizeRoboto18" style="width:90%; ">Serial number copied to clipboard!</div>');
            return false;
        }

        function UpdateDefaultprices() {
            DisableHideUpdatProg();
            $.confirm({
                useBootstrap: false,
                title: '<div class="AlertTitleStyle">Update Prices!<div>',
                content: '<div class="AlertBodyStyle">Are you sure you want to load default quantities?</div>',
                buttons: {
                    somethingElse: {
                        text: 'Yes',
                        btnClass: 'AlertOkButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                            $('#ContentPlaceHolderMain_btnDefaultQuantityHided').click();
                        }
                    },
                    Cancel: {
                        text: 'Cancel',
                        btnClass: 'AlertCancelButtonStyle',
                        keys: ['enter', 'shift'],
                        action: function () {
                        }
                    }


                }
            });
        };

        function scrollToLastRow() {

            var gridView = document.getElementById('<%= gvPrices.ClientID %>');

            if (gridView) {
                var lastRowIndex = gridView.rows.length - 1;
                var lastRow = gridView.rows[lastRowIndex];

                if (lastRow) {
                    var offsetTop = lastRow.offsetTop;
                    gridView.scrollTop = offsetTop;
                }
            }
        }


        function SetDivHeightWithScrollPrice() {
            try {

                var dFreC = window.innerHeight - 100 - document.getElementById('DataDivForScrolingA').getBoundingClientRect().top + 'px';
                document.getElementById('DataDivForScrolingA').style.maxHeight = dFreC;

                var scaleA = window.devicePixelRatio;
                if (scaleA === 1.5) {

                    document.getElementById('DataDiv').style.height = "500px";
                    document.getElementById('diviFrameHeight').style.height = "60%"
                }
                else {

                    var dFreB = window.innerHeight - 100 - document.getElementById('DataDiv').getBoundingClientRect().top + 'px';
                    document.getElementById('DataDiv').style.maxHeight = dFreB;
                    var dFreD = window.innerHeight - 60 - document.getElementById('diviFrameHeight').getBoundingClientRect().top + 'px';
                    document.getElementById('diviFrameHeight').style.height = dFreD
                }
            }
            catch (error) {

            }

            try {
                var scale = window.devicePixelRatio;
                if (scale === 1.5) {
                    document.getElementById("GlobDef_MLAllsm").style.overflow = "auto";
                    document.getElementById("GlobDef_MLAllsm").style.height = "300px";
                    document.getElementById("upPnlAll_Documentsdiv").style.overflow = "unset";
                    document.getElementById("divsmScal").style.overflow = "unset";
                    document.getElementById("upPnlAll_Documentsdiv").style.height = "100%";
                    document.getElementById('diviFrameHeight').style.height = "100% !important";
                }
            }
            catch (error) {

            }

        }


        function SetCaptionForLabelsCon() {
            try {
                $('#btn_adddocument').val($('#ContentPlaceHolderMain_hfadddocument').val());
                $('#lbl_View').val($('#ContentPlaceHolderMain_hfViwe').val());
                $('#3DlabelView').text($('#ContentPlaceHolderMain_hf3DlabelView').val());
                $('#2DlabelView').text($('#ContentPlaceHolderMain_hf2DlabelView').val());
            } catch (error) {
                // Code to handle the exception
                //alert(error);
            }
        }

        //function checkBSONTEXTID() {

        //    const bsonElement = document.getElementById('ContentPlaceHolderMain_BsonTextID').value;
        //    //alert(bsonElement);
        //    if (bsonElement !== '') {
        //        var path = document.getElementById('ContentPlaceHolderMain_BsonTextID').value;

        //        if (path !== '') {
        //            var iframe = document.getElementById('ContentPlaceHolderMain_ifif');
        //            iframe.src = '../BSON/viewer_index.html';
        //            //iframe.src = '../BSON/viewer_index.html';
        //            iframe.onload = function () {
        //                iframe.contentWindow.findcontrol(path);
        //            };
        //        };

        //    };
        //};

        function drawingShow(wts) {

            document.getElementById("ContentPlaceHolderMain_ifif").style.height = "100%";
            document.getElementById("ContentPlaceHolderMain_PdfView").style.height = "100%";


            //if (document.getElementById("ContentPlaceHolderMain_ifif").src == '') {
            //alert(1);
            //document.getElementById("btnC_R").disabled = true;
            //document.getElementById("btnC_R").className = "Initial3D_RR_Dis";
            //document.getElementById("2DlabelView").className = "Initial3D_R_Dis FontFamilyRoboto FontSizeRoboto13";
            //document.getElementById("2DlabelView").disabled = true;
            //document.getElementById("btnA_R").disabled = false;
            //document.getElementById("btnA_R").className = "Initial3D_RR";
            //document.getElementById("3DlabelView").className = "Initial3D_R FontFamilyRoboto FontSizeRoboto13"
            //document.getElementById("3DlabelView").disabled = false;
            //if (document.getElementById("ContentPlaceHolderMain_ifif").src.includes('ShowPDFReportBSON.aspx')) {

            if (wts == 'BSON') {


                var doRefs = false;
                try {
                    var parentDocument = window.parent.document;
                    var iframeElement = parentDocument.getElementById('ContentPlaceHolderMain_ifif'); // Change 'myIframe' to the id of your iframe

                    if (iframeElement.src.includes('viewer_index.html')) {
                        doRefs = true;
                    };
                }
                catch {
                    doRefs = false
                };


                //if (document.getElementById("ContentPlaceHolderMain_ifif").src == '') {
                //    document.getElementById("ContentPlaceHolderMain_ifif").src = 'ShowPDFReportBSON.aspx'
                //};

                const ExitQutT = document.getElementById("ContentPlaceHolderMain_hfExistQ").value;



                if (doRefs === false) {
                    var path = document.getElementById('ContentPlaceHolderMain_BsonTextID').value;

                    if (ExitQutT == 'YES' && path == '') {
                        document.getElementById("ContentPlaceHolderMain_ifif").src = 'ShowPDFReportBSONError_3.html'
                    }
                    else {
                        //alert(path);
                        if (path != '') {

                            const bsonlink = document.getElementById("ContentPlaceHolderMain_hfBSONfolder").value;

                            const bsonlinkex = urlExistsBSONRUNn(bsonlink + 'BSON' + '/' + path + '/model.bson');
                            //const bsonlinkex1 = urlExistsBSONRUNn(bsonlink + 'BSON' + '/' + path + 'model.bson');
                            //const bsonlinkex2 = urlExistsBSONRUNn(bsonlink + 'BSON' + '/' + path + 'model.bson');
                            ////\r\0

                            if (ExitQutT == 'YES' && bsonlinkex == false) {
                                document.getElementById("ContentPlaceHolderMain_ifif").src = 'ShowPDFReportBSONError_3.html';
                            }
                            else {
                                if (bsonlinkex == true) {
                                    var iframe = document.getElementById('ContentPlaceHolderMain_ifif');

                                    iframe.src = '../BSON/viewer_index.html';

                                    try {
                                        iframe.onload = function () {
                                            iframe.contentWindow.findcontrol(path);
                                        };
                                    }
                                    catch {
                                        document.getElementById("ContentPlaceHolderMain_ifif").src = 'ShowPDFReportBSONError_3.html';
                                    };
                                };
                            }

                        }
                        else {
                            document.getElementById("ContentPlaceHolderMain_ifif").src = 'ShowPDFReportBSON.aspx';
                        };
                    };
                };

                document.getElementById("ContentPlaceHolderMain_PdfView").style.display = "none";
                document.getElementById("ContentPlaceHolderMain_ifif").style.display = "block";
                document.getElementById("btnA_R").checked = true;
                document.getElementById("btnC_R").checked = false;
            }
            else if (wts == 'PDF') {

                var doRefsPDF = false;
                try {
                    var parentDocumentpdf = window.parent.document;
                    var iframeElementpdf = parentDocumentpdf.getElementById('ContentPlaceHolderMain_PdfView'); // Change 'myIframe' to the id of your iframe

                    if (iframeElementpdf.src.includes('.pdf')) {
                        doRefsPDF = true;
                    };
                }
                catch {
                    doRefsPDF = false
                };

                //if (document.getElementById("ContentPlaceHolderMain_ifif").src == '') {
                //    document.getElementById("ContentPlaceHolderMain_ifif").src = 'ShowPDFReportBSON.aspx'
                //};



                const pDFfILEEX = document.getElementById("ContentPlaceHolderMain_txtPdfViewsrc4").value;
                const ExitQut = document.getElementById("ContentPlaceHolderMain_hfExistQ").value;
                const existFi = urlExistsPDFRUN(pDFfILEEX);

                if (existFi == false && ExitQut == 'YES') {
                    document.getElementById("ContentPlaceHolderMain_PdfView").src = 'ShowPDFReportBSONError_2.html';
                    enablepdfcheck();
                } else {

                    if (existFi == false) {
                        document.getElementById("ContentPlaceHolderMain_PdfView").src = 'ShowPDFReport.aspx';
                        disablepdfcheck();
                    }
                    else {
                        if (doRefsPDF == false) {

                            if (pDFfILEEX != '' && ExitQut == 'YES' && urlExistsPDFRUN(pDFfILEEX)) {
                                document.getElementById("ContentPlaceHolderMain_PdfView").src = pDFfILEEX;
                                enablepdfcheck();
                            } else {
                                if (pDFfILEEX != '' && ExitQut == 'YES' && !urlExistsPDFRUN(pDFfILEEX)) {
                                    document.getElementById("ContentPlaceHolderMain_PdfView").src = 'ShowPDFReportBSONError_2.html';
                                    disablepdfcheck();
                                }
                                else {
                                    if (pDFfILEEX != '' && urlExistsPDFRUN(pDFfILEEX)) {
                                        document.getElementById("ContentPlaceHolderMain_PdfView").src = pDFfILEEX;
                                        enablepdfcheck();
                                    } else {
                                        document.getElementById("ContentPlaceHolderMain_PdfView").src = 'ShowPDFReport.aspx';
                                        disablepdfcheck();
                                    }
                                };
                            };
                        };
                    };
                }





                //var pDFfILEEX = document.getElementById("ContentPlaceHolderMain_txtPdfViewsrc4").value;

                //if (urlExistsPDFRUN(pDFfILEEX)) {
                //    document.getElementById("ContentPlaceHolderMain_PdfView").src = pDFfILEEX
                //}
                //else {
                //    if (document.getElementById("ContentPlaceHolderMain_BsonTextQutNo").value == 'EXIST') {
                //        document.getElementById("ContentPlaceHolderMain_PdfView").src = 'ShowPDFReportBSONError_2.html'
                //    }
                //    else {
                //        document.getElementById("ContentPlaceHolderMain_PdfView").src = 'ShowPDFReport.aspx';
                //    }

                //};

                ////if (document.getElementById("ContentPlaceHolderMain_PdfView").src == '') {
                //document.getElementById("btnC_R").disabled = false;
                //document.getElementById("btnC_R").className = "Initial3D_RR";
                //document.getElementById("2DlabelView").className = "Initial3D_R FontFamilyRoboto FontSizeRoboto13"
                //document.getElementById("2DlabelView").disabled = false;
                document.getElementById("ContentPlaceHolderMain_PdfView").style.display = "block";
                document.getElementById("btnC_R").checked = true
                document.getElementById("btnA_R").checked = false
                //document.getElementById("btnA_R").disabled = true;
                //document.getElementById("btnA_R").className = "Initial3D_RR_Dis";
                //document.getElementById("3DlabelView").className = "Initial3D_R_Dis FontFamilyRoboto FontSizeRoboto13";
                //document.getElementById("3DlabelView").disabled = true;
                document.getElementById("ContentPlaceHolderMain_ifif").style.display = "none";
                //}
            }
        }

        //function urlGeneralFileExisty(xFile) {
        //    debugger;
        //    return new Promise((resolve, reject) => {
        //        jQuery.ajax({
        //            type: "HEAD",
        //            url: xFile,
        //            success: () => resolve(true),
        //            error: (jqXHR) => {
        //                if (jqXHR.status === 404) {
        //                    resolve(false);
        //                } else {
        //                    reject(new Error(`Unexpected status code: ${jqXHR.status}`));
        //                }
        //            }
        //        });
        //    });
        //}


        function disablepdfcheck() {
            document.getElementById("btnC_R").disabled = true;
            document.getElementById("btnC_R").className = "Initial3D_RR_Dis";
            document.getElementById("2DlabelView").className = "Initial3D_R_Dis FontFamilyRoboto FontSizeRoboto13"
            document.getElementById("2DlabelView").disabled = true;
        };

        function enablepdfcheck() {
            document.getElementById("btnC_R").disabled = false;
            document.getElementById("btnC_R").className = "Initial3D_RR";
            document.getElementById("2DlabelView").className = "Initial3D_R FontFamilyRoboto FontSizeRoboto13"
            document.getElementById("2DlabelView").disabled = false;
        };

    </script>

</asp:Content>
