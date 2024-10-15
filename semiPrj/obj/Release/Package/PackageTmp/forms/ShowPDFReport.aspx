<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowPDFReport.aspx.vb" Inherits="semiPrj.ShowPDFReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/LTR/cssSemiGlobal-min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="border-style:none; text-align:center; width:100%">
            <input id="PDFinput1" type="text" class="FontFamilyRoboto FontSizeRoboto" style="color: #bdbdbd; text-align: center;margin-top: 25%;  border-style: none; width: 250px" value="2D Model Drawing is in process..." /><br />
            <input id="PDFinput2" type="text" class="FontFamilyRoboto FontSizeRoboto" style="color: #bdbdbd; text-align: center;  border-style: none; width: 350px" value="Use the Drawing Preview button to load it" />
                    <br />
            <br />
                                            <img runat="server" id="ajaxLoader1" alt="loading" src="~/media/Images/Wait.gif" width="100"  /></div>
    </form>
</body>
</html>
