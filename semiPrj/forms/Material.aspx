<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/masters/SemiSTDMaster.Master" CodeBehind="Material.aspx.vb" Inherits="semiPrj.Material" EnableSessionState="True" EnableViewState="true" MaintainScrollPositionOnPostback="true" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <%@ Register Src="~/uc/wucTab.ascx" TagName="wuc_Tabs" TagPrefix="wuc_Tabs" %>

    <style>
        .divgridCss {
            padding-left: 30px;
            border-left: thin solid #e9eaec;
            border-bottom: thin solid #e9eaec;
            border-right: thin solid #e9eaec;
            padding-top: 10px;
        }

        @media screen and (max-width: 500px) {

            .divgridCss {
                padding-left: 10px;
            }
        }

        .row {
            /*--bs-gutter-x: 1.5rem;*/
            --bs-gutter-y: 0;
            display: flex;
            flex-wrap: wrap;
            margin-top: calc(-1 * var(--bs-gutter-y));
            margin-right: calc(-.5 * var(--bs-gutter-x));
            margin-left: calc(-.5 * var(--bs-gutter-x))
        }

        .col-sm-9M {
            flex: 0 0 auto;
            width: 100%
        }

        .one_N {
            width: 90%;
        }

        .two_NSub {
            width: 100%;
            height: 94%;
            vertical-align: top;
            border-style: none;
            overflow: auto;
            padding-right: 20px;
            overflow: auto
        }


        .DivHideForMobile {
            display: block !important;
        }

        @media screen and (max-width: 500px) {

            .DivHideForMobile {
                display: none !important;
            }
        }

        .DivHideForMobile2 {
            display: none !important;
        }

        @media screen and (max-width: 500px) {

            .DivHideForMobile2 {
                display: block !important;
            }
        }

        .DivPaddingForMobile {
            padding-bottom: 0px;
        }

        @media screen and (max-width: 500px) {

            .DivPaddingForMobile {
                padding-bottom: 50px;
            }
        }

        .clsSetMatc{
            width: 74%;
            display: table
        }

                @media screen and (max-width: 500px) {

            .clsSetMatc {
                display: contents;
            }
        }
    </style>

    <script>

        function ModelSelect(btnId) {
            var btn;
            btn = document.getElementById(btnId);
            btn.click();
            return true;
        }

        function SubModelSelect(btnId) {
            var btn;
            btn = document.getElementById(btnId);
            btn.click();
            return true;

        }





    </script>

    <wuc_Tabs:wuc_Tabs ID="wucTabs" runat="server" />
    <asp:UpdatePanel ID="upPnlAll_Material" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="GlobDef_MLAll BorderRightLeft" style="height: 98% !important">



                <div class="" style="height: 94% !important; width: 100%;">
                    <div class="mainLeftRightPadding" style="height: 94% !important;">
                        <div class="row" style="border-left: thin solid #e9eaec; border-bottom: thin solid #e9eaec; border-right: thin solid #e9eaec; padding-top: 0px;">

                            <div style="width: 25%">
                                <div class="DivPaddingForMobile" style="padding-right: 50px; padding-top: 10px;">
                                    <asp:DataList runat="server" ID="dlMaterialMain" RepeatColumns="1" RepeatDirection="Horizontal" Height="100%" AlternatingItemStyle-Wrap="true" SelectedItemStyle-BackColor="#bcc2d8" CssClass="DataListcss DivHideForMobile">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <AlternatingItemStyle Wrap="true" />
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlSelected" runat="server" class="datalistbuttnsDivH">
                                                <asp:Panel runat="server" class="datalistbuttnsDiv" ID="datalistbuttnsDiv">
                                                    <asp:Button runat="server" ID="btnModelIcon" CssClass="datalistbuttns" Font-Bold="true" />
                                                    <asp:Label runat="server" ID="lblMaterialName" CssClass="MaterialNamec FontFamilyRoboto FontSizeRoboto " Height="100%" />
                                                    <asp:Label runat="server" ID="btnModelIconBackColor" CssClass="DisplayNone"></asp:Label>
                                                    <asp:Label runat="server" ID="lblGroupName" CssClass="DisplayNone"></asp:Label>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <asp:DataList runat="server" ID="dlMaterialMain1" RepeatColumns="6" RepeatDirection="Horizontal" AlternatingItemStyle-Wrap="true" SelectedItemStyle-BackColor="#bcc2d8" CssClass="DataListcss DivHideForMobile2">
                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                        <AlternatingItemStyle Wrap="true" />
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlSelected1" runat="server" class="datalistbuttnsDivH">
                                                <asp:Panel runat="server" class="datalistbuttnsDiv" ID="datalistbuttnsDiv">
                                                    <asp:Button runat="server" ID="btnModelIcon1" CssClass="datalistbuttns" Font-Bold="true" />
                                                    <asp:Label runat="server" ID="lblMaterialName1" CssClass="MaterialNamec FontFamilyRoboto FontSizeRoboto DisplayNone" Height="100%" />
                                                    <asp:Label runat="server" ID="btnModelIconBackColor1" CssClass="DisplayNone"></asp:Label>
                                                    <asp:Label runat="server" ID="lblGroupName1" CssClass="DisplayNone"></asp:Label>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
                            </div>
                            <div class="clsSetMatc" >
                                <div id="divgrid" class="col-sm-9M divgridCss">
                                    <div style="width: 50%; padding-bottom: 10px">
                                        <asp:Label ID="lblSetMat" runat="server" Text="Set Material" CssClass="MainSubTitleK FontFamily"></asp:Label>
                                    </div>

                                    <div class="two_NSub">

                                        <asp:GridView Width="100%" runat="server" ID="gvMaterials" AutoGenerateColumns="False" CssClass="cssGridMaterials" GridLines="None" HorizontalAlign="Left" ShowHeader="true">
                                            <HeaderStyle CssClass="FontFamilyRoboto FontSizeRoboto GridMaterialHeader" />
                                            <RowStyle CssClass="FontFamilyRoboto FontSizeRoboto GridMaterialRow" />
                                            <Columns>
                                                <asp:TemplateField ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone">
                                                    <ItemTemplate>
                                                        <asp:Button ID="ButtonSelect" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="SelectMaterial" OnClientClick="cancelonbeforeunload()" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group" ItemStyle-Height="30px" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="AlignCenter">
                                                    <ItemTemplate>
                                                        <asp:Panel ID="pnlSquareColor" runat="server" CssClass="datalistbuttnsM ">
                                                            <asp:Label ID="lblGroup" runat="server" Height="100%" CssClass="FontFamilyRoboto Align_v_bottom"></asp:Label>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="20px" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="400px" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto AlignLeft VerticalAlignMiddle" />

                                                <asp:BoundField DataField="condition" HeaderText="Condition" ItemStyle-Width="150px" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto AlignLeft VerticalAlignMiddle" />
                                                <asp:BoundField DataField="Hardness" HeaderText="Hardness" ItemStyle-Width="60px" ItemStyle-CssClass="FontFamilyRoboto FontSizeRoboto AlignLeft VerticalAlignMiddle" />
                                                <asp:BoundField DataField="TabDescription" HeaderText="DisplayTab" ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone" />
                                            </Columns>

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>





                        </div>
                    </div>
                </div>



                <div style="width: 1px; border-right-style: solid; border-right-color: #e3e3e3; border-right-width: 1px;"></div>


                <asp:Button runat="server" ID="btmOenQbuilder" Text="start build" Visible="false" />
                <asp:Label ID="SelectedColor" runat="server" CssClass="DisplayNone" Text="blue"></asp:Label>
                <asp:Label ID="SelectedForColor" runat="server" CssClass="DisplayNone" Text="white"></asp:Label>
                <asp:HiddenField ID="hfColumnRepeat" runat="server" Value="6" />
                <asp:HiddenField ID="hfRepoprtsNames" runat="server" />


                <script>
                    window.addEventListener("resize", displayWindowSize);
                    displayWindowSize();

                    function displayWindowSize() {
                        var w = document.documentElement.clientWidth;
                        var h = (document.documentElement.clientHeight - 250) + 'px';
                        document.getElementById("divgrid").style.height = h;
                    }
                </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
