<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StartFB.aspx.vb" Inherits="semiPrj.StartFB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iQuote</title>

    <script src="../jq/jquery-3.5.0.min.js"></script>
    <link rel="icon" type="image/ico" href="../media/Icons/IQ.png" />

    <link rel="stylesheet" href="~/App_Themes/LTR/cssSemiGlobal-min.css" />
    <link href="../App_Themes/LTR/jquery-confirm.min.css" rel="stylesheet" />

    <script src="../jq/jquery-confirm.min.js"></script>
    <style>
        .csbtnMyQut{
            display:none
        }
    </style>
    <script>
        function LoggedAlert() {
            var ff = "'";
            var ffff1;
            var ffff2;
            ffff1 = "&nbsp;Don't worry, your quotation is saved to My " + ff + ff + "My Quotations" + ff + ff + "."
            ffff2 = "&nbsp;Please log in again to view it or create a new quotation."

            var buseboo = false;
            var iuseboo = '640px';
            if (window.innerWidth < 1024) {
                buseboo = true;
            }

            var ddtitle = '<div style="width:100%;padding-top: 40px; text-align: center"><img src="../media/Images/timouticon.png"  style="width:300px" /><br/><div class="AlertTitleStyleCenter FontSizeRoboto25">Session Timeout<div></div>'
            ddtitle = ddtitle + '<div class="FontFamilyRoboto FontSizeRoboto18" style="text-align: center"></div>'
            var cont =
                $.confirm({
                    useBootstrap: buseboo,
                    boxWidth: iuseboo,
                    title: ddtitle,
                    content: '' +
                        '<div style="text-align:center; padding-left:10px; padding-top: 0px; padding-right: 10px;">' +
                        '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff1 + '"></asp:Label><br/>' +
                        '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff2 + '"></asp:Label><br/>' +
                        '</div>',
                    buttons: {
                        formSubmit: {
                            text: 'Refresh',
                            btnClass: 'AlertOkButtonStyle',
                            action: function () {
                                window.open("../Default.aspx", "_self");
                            }
                        },
                    },
                });
        }

        function UnLoggedAlert() {
            var ff = "'";
            var ffff1;
            var ffff2;
            //var ffff3;

            //ffff1 = "&nbsp;Don't worry, your quotation is saved and you can still access it by logging in"
            ffff1 = "&nbsp;Please refresh the page."
            ffff2 = "&nbsp;You will be redirected to the iQuote home page."
            //ffff2 = "&nbsp;and clicking on <b>" + ff + ff + "My Quotations" + ff + ff + "</b> on the home page."
            //ffff3 = "If you wish, can also create a new quotation."

            var ddtitle = '<div style="width:100%;padding-top: 40px; text-align: center"><img src="../media/Images/timouticon.png" style="width:300px" /><br/><div class="AlertTitleStyleCenter FontSizeRoboto25">Session Timeout<div></div>'
            ddtitle = ddtitle + '<div class="FontFamilyRoboto FontSizeRoboto18" style="text-align: center"></div>'


            var cont =
                $.confirm({
                    useBootstrap: false,
                    boxWidth: '640px',
                    title: ddtitle,
                    content: '' +
                        '<div style="text-align:center; padding-left:10px; padding-top: 0px; padding-right: 10px;">' +
                        '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff1 + '"></asp:Label><br/>' +
                        '<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff2 + '"></asp:Label><br/>' +
                        //'<asp:Label runat="server" CssClass="AlertBodyStyle" Text="' + ffff3 + '"></asp:Label><br/>' +
                        '</div>',
                    buttons: {
                        formSubmit: {
                            text: 'Refresh',
                            btnClass: 'AlertOkButtonStyle',
                            action: function () {
                                window.open("../Default.aspx", "_self");
                            }
                        },
                    },
                });
        }
    </script>


</head>
<body onload="StartForm()">
    <form id="form1" runat="server">
        <div>
            <script>
                function StartForm() {
                
                    if ($("#txtTimeOutType").val() == "No") {
                        UnLoggedAlert();
                    }
                    else {

                        LoggedAlert();

                    }
                }

            </script>
            <asp:TextBox ID="txtTimeOutType" runat="server" CssClass="csbtnMyQut"></asp:TextBox>

        </div>
    </form>
</body>
</html>
