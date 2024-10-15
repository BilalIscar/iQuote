<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CatalogTableDetails.aspx.vb" Inherits="semiPrj.CatalogTableDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Label ID="lblItemNoCat" runat="server" ForeColor="#dddddd"></asp:Label><br />
  <asp:GridView runat="server" CssClass="cssGridRulles" ID="gvRulles" AutoGenerateColumns="true" ShowHeader="true">
      </asp:GridView>

        </div>
        
    </form>
</body>
</html>
