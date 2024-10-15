<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ucBettredirect.aspx.vb" Inherits="semiPrj.ucBettredirect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" action="UserCustomerCnt.aspx" method="post" >
        <div>
            <label for="name">Name:</label>
            <input type="text" id="name" name="name" />
        </div>
        <div>
            <label for="email">Email:</label>
            <input type="email" id="email" name="email" />
        </div>
        <div>
            <input type="submit" value="Submit" />
        </div>
    </form>
</body>
</html>
