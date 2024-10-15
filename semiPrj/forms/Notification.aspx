<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Notification.aspx.vb" Inherits="semiPrj.Notification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/LTR/cssSemiGlobal-min.css" rel="stylesheet" />
    <style>
        .center {
            /*border: 5px none #808080;*/
            text-align: center;
            padding-top: 4%;
            vertical-align: middle;
            /*border-style: solid;*/
            /*border-color: greenyellow*/
        }

        .fontLb_l {
            font-size: 30px !important;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div style=" width: 100%;" class="center">
            <div style="width: 50%;">

                    <a href="../forms/ConfiguratorBuilder.aspx">
                    <input type="text" value="iQuote" class="FontFamlyTitle mainhedellabelcss FontSizeRoboto20 fontLb_l BorderNone" style="cursor:pointer;" /></a>
            </div>
            <div>&nbsp;</div>
            <div>&nbsp;</div>
            <div>&nbsp;</div>
            <div>
                <input type="text" value="iQuote - illegal parameter request" class="FontFamilyRoboto FontSizeRoboto fontLb_l BorderNone" style="width: 100%; text-align: center" /><br />&nbsp;<br />
                <hr style="width:50%; color:lightgray; border-color:lightgray; border-top-style:none;border-left-style:none;border-right-style:none;border-bottom-style:solid" />

            </div>
        </div>
    </form>
</body>
</html>
