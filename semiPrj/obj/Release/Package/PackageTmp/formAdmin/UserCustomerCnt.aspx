<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserCustomerCnt.aspx.vb" Inherits="semiPrj.UserCustomerCnt" EnableViewState="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="icon" type="image/ico" href="../media/Icons/IQ.png" />
    <script src="../jq/jquery-3.5.0.min.js"></script>
    <link href="../App_Themes/LTR/jquery-confirm.min.css" rel="stylesheet" />
     <script src="../jq/jquery-confirm.min.js"></script>
    <link href="../App_Themes/LTR/cssSemiGlobal-min.css" rel="stylesheet" />
    <style>
        .searchdiv {
            border: solid;
            border-color: lightgray;
            border-radius: 14px;
            width: 250px;
            height: 20px;
            padding-left: 10px;
            height: 32px;
            border-width: thin;
            display: flex !important;
            align-items: center !important;
        }

        .center {
            margin: auto;
            width: 50%;
            border: 3px solid green;
            padding: 10px;
        }

        .labelPad {
            padding-top: 10px
        }

        .PadL {
            padding-left: 50px;
        }

                .btnDlgDeclineSend {
            position: absolute;
            bottom: 2px;
            right: 20px;
        }

                .btn-blueD {
    margin-right:10px !important;
    background-color: #12498a !important;
    color: white;
    text-shadow: none;
    -webkit-transition: background .2s;
    transition: background .2s;
    text-align:center !important;

}
    </style>
    <script>
        function EnterValueForSearch(txtId, btnId) {
            var txt;
            var btn;
            txt = document.getElementById(txtId);
            btn = document.getElementById(btnId);
            if (window.event.keyCode == 13) {
                // alert (11)
                btn.click();
                return false;
            }
            return true;
        }

        function WrongData() {
            $.dialog({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a"><div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto18">Missing data! Fill all required entry fields.</div>'
            });
            return false;
        }

        function SaveNewUser() {
            if (document.getElementById("txtBranchCode").value == "" || document.getElementById("txtDisplayName").value == "" || document.getElementById("txtLoggedEmail").value == "" || document.getElementById("txtSurname").value == "" || document.getElementById("txtGivenName").value == "") {
                // document.getElementById("lblErrorConect").innerHTML = "Missing data! Fill all required entry fields."
                WrongData();
            }
            else {
                var btn;
                btn = document.getElementById("btnActiveSave");
                btn.click();
                return true;
            }
        }
        function ShowAlert(sMs) {
            $.dialog({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a"><div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto18">' + sMs + '</div>'
            });
            return false;
        }

        //--
        function ShowAlertSuccess(sname) {

            //Your request has been submitted!
            $.confirm({
                useBootstrap: false,
                title: '<div class="FontFamily FontSizeOswald18 divAlertConfirm" style="color:#12498a"><div>',
                content: '<div class="FontFamilyRoboto FontSizeRoboto18">' + sname + '</div>',
                buttons: {
                    Ok: {
                        text: 'Ok',
                        btnClass: 'btn-blueD',
                        action: function () {
                            btn = document.getElementById("btnGoHome");
                            btn.click();
                            return true;
                        }
                    }
                }
            });
        }

        function Close() {
            window.location.href = "../Default.aspx";
        }

        function ConnectUser() {
            if (document.getElementById("lblEmailtoConnect").innerHTML == "" || document.getElementById("txtCustomerNumber").value == "" || document.getElementById("txtCustomerNumber").value == "0") {
                WrongData();
            }
            else {
                var btn;
                btn = document.getElementById("btnActiveConnect");
                btn.click();
                return true;
            }
        }
    </script>
    <style>
        .sddbc {
            border-color: lightgray !important;
        }

        .btn {
            background-color: #757677;
            width: 160px;
            cursor: pointer
        }

        .margtop {
            margin-top: 4px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div style="padding-top: 20px; width: 100%" id="MdIv">

        <div class="center" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; padding-top: 20px">
            <asp:Label runat="server" ID="lblError" Font-Size="Medium" ForeColor="Red" Font-Bold="true"></asp:Label>
        </div>

            <div class="center" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; padding-top: 20px">
                <div>&nbsp;</div>

                <div style="width: 100%; text-align: center; padding-bottom:10PX" class="AlertTitleStyle FontSizeRoboto25">
                    <asp:Label ID="lblTitle" runat="server" CssClass="AlertTitleStyle FontSizeRoboto25"></asp:Label>
                    
                </div>

                <div>
                    <asp:DropDownList ID="ddlBranch" Width="262px" runat="server" placeholder="Branch" CssClass="sddbc searchdiv FontFamilyRoboto FontSizeRoboto " Height="29px" EnableViewState="true" AutoPostBack="true"></asp:DropDownList></div>

                <div style="padding-top: 10px; display: flex; width: 100%">

                    <div class="searchdiv divBorderSolid" style="margin-right: auto; height: 29px">

                        <asp:TextBox Width="220px" ID="txtSearchAll" runat="server" placeholder="Search Any." CssClass="BorderNone FontFamilyRoboto FontSizeRoboto FloatRLeft" Height="29px"></asp:TextBox>
                        <asp:ImageButton ID="btnSearchAll" runat="server" ImageUrl="../media/Icons/search29.png" />
                    </div>
                    &nbsp;
                    <asp:Label Width="200px" placeholder="Insert User Email address" runat="server" ID="lblEmailtoConnect" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto FloatRLeft searchdiv " Height="29px"></asp:Label>
                    &nbsp; 
                    <asp:TextBox Width="200px" ID="txtCustomerNumber" runat="server" placeholder="Insert Customer Number" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto FloatRLeft searchdiv " Height="29px"></asp:TextBox>&nbsp;
                    <input type="button" onclick="ConnectUser()" class="MyAc btn FloatRLeft" style="width:120px" value="Save" /><br />
                    <input type="button" onclick="Close()" class="MyAc btn FloatRLeft" style="width:120px" value="Close" />
                </div>
            
                <div>
                    <br />
                    <div style="width: 100%">
                        <img src="../media/Icons/iAdd.png" onclick="HideShowAddDiv()" style="width: 30px; height: 30px; cursor:pointer" class="margtop" /></div>

                    <div id="AddNewUser" style="width: 100%;">
                        <div style="width: 100%; display:flex">
                            <asp:TextBox ReadOnly="true" Width="230px" ID="txtBranchCode" runat="server" placeholder="Branch Code" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto  searchdiv margtop" Height="29px"></asp:TextBox></div>
                        <div style="width: 100%; display:flex">
                            <asp:TextBox Width="230px" ID="txtLoggedEmail" runat="server" placeholder="Insert Email Address" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto  searchdiv margtop" Height="29px"></asp:TextBox>
                            <asp:RegularExpressionValidator CssClass="FontSizeOswald16 FontFamily" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ID="RegularExpValidEmail" runat="server" ControlToValidate="txtLoggedEmail" Text="!Input value is an email address" ForeColor="Red"></asp:RegularExpressionValidator></div>
                        <div style="width: 100%; display:flex">
                            <asp:TextBox Width="230px" ID="txtDisplayName" runat="server" placeholder="Insert Name" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto  searchdiv margtop" Height="29px"></asp:TextBox></div>
                        <div style="width: 100%; display:flex">
                            <asp:TextBox Width="230px" ID="txtSurname" runat="server" placeholder="Insert Surname" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto  searchdiv margtop" Height="29px"></asp:TextBox>
                            </div>

                    
                        <div style="width: 100%">
                            <asp:TextBox Width="230px" ID="txtGivenName" runat="server" placeholder="Insert Given Name" CssClass="BorderNone FontFamilyRoboto FontSizeRoboto  searchdiv margtop" Height="29px"></asp:TextBox></div>
                        <div style="width: 100%"><input type="button" onclick="SaveNewUser()" class="MyAc btn FloatRLeft" style="width:150px" value="Save" />

                        </div>
                        <div style="width: 100%">&nbsp;</div>
    </div>
                    </div>
                    



                <br />
                   
               
                <div class="GridListDiv divBorderSolidColored" style="width: 100%; border-style: none; padding-top:10px;overflow: auto;
    height: 600px;">
                    <asp:GridView RowStyle-VerticalAlign="Middle" AllowSorting="true" runat="server" ShowHeaderWhenEmpty="true" CssClass="FontFamilyRoboto FontSizeRoboto14" ID="gvConnect" AutoGenerateColumns="false" ShowHeader="true" Width="100%" GridLines="None"  AllowPaging="false" PagerStyle-CssClass="FontFamilyRoboto FontSizeRoboto13">

                        <HeaderStyle CssClass="cssGridHeaderStyle FontSizeRoboto15 FontFamilyRoboto" BorderColor="" Wrap="true" Height="40px" />
                        <RowStyle CssClass="divBorderSolidColored cssGridRowStyle" Height="36px" />
                        <SortedAscendingHeaderStyle BackColor="Yellow" />
                        <Columns>

                            <asp:ButtonField ItemStyle-Width="3%" HeaderStyle-Width="3%" ButtonType="Image" CommandName="Select" ImageUrl="~/media/Icons/ListArrow.png" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto14 AlignCenter" HeaderStyle-BackColor="#e9eaec"></asp:ButtonField>
                            <asp:BoundField SortExpression="" HeaderStyle-ForeColor="#354993" DataField="BranchCode" HeaderText="BranchCode" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto14 AlignLeft css_GridItemiMportant" HeaderStyle-CssClass="css_GridHeaderiMportant FontSizeRoboto15 FontFamilyRoboto AlignLeft"></asp:BoundField>
                            <asp:BoundField HeaderStyle-ForeColor="#354993" DataField="DisplayName" HeaderText="Name" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto14 AlignLeft css_GridItemiMportant" HeaderStyle-CssClass="css_GridHeaderiMportant FontSizeRoboto15 FontFamilyRoboto AlignLeft"></asp:BoundField>
                            <asp:BoundField HeaderStyle-ForeColor="#354993" DataField="loggedEmail" HeaderText="Email" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto14 AlignLeft css_GridItemiMportant" HeaderStyle-CssClass="css_GridHeaderiMportant FontSizeRoboto15 FontFamilyRoboto AlignLeft"></asp:BoundField>
                            <asp:BoundField HeaderStyle-ForeColor="#354993" DataField="AS400_Cust" HeaderText="Customer number" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto14 AlignLeft css_GridItemiMportant" HeaderStyle-CssClass="css_GridHeaderiMportant FontSizeRoboto15 FontFamilyRoboto AlignLeft"></asp:BoundField>


                        </Columns>

                    </asp:GridView>
                </div> </div>
            </div>
        


        <input type="text" id="streI" />
        <asp:HiddenField ID="unf" runat="server" />
        <asp:Button ID="btnActiveConnect" runat="server" Text="Save" Width="150px" CssClass="display_non" ></asp:Button>
        <asp:Button ID="btnGoHome" runat="server" Text="Save" Width="150px"  CssClass="display_non"></asp:Button>
        <asp:Button ID="btnActiveSave" runat="server" Text="Save" Width="150px"  CssClass="display_non"></asp:Button>

        <script>


            function ShowAddDiv() {
                $("#AddNewUser").toggle(1000);
            }
            function HideAddDiv() {
                $("#AddNewUser").hide(1000);
            }

            function HideShowAddDiv() {
                if ($("AddNewUser").is(":visible")) {
                    $("#AddNewUser").hide(1000);
                }
                else {
                    $("#AddNewUser").toggle(1000);
                }
            }

            function HideDiv() {
                $("#AddNewUser").hide(0);
            }
            function ShowDivT() {
                $("#AddNewUser").toggle(0);
            }
        </script>



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
