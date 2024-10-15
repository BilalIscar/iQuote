<%@ Page Title="ConfiguratorBuilder" Language="vb" AutoEventWireup="false" MasterPageFile="~/masters/SemiSTDMaster.Master" CodeBehind="ConfiguratorBuilder.aspx.vb" Inherits="semiPrj.ConfiguratorBuilder" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <%@ Register Src="~/uc/wucTab.ascx" TagName="wuc_Tabs" TagPrefix="wuc_Tabs" %>

    <style>
        .imgModelIcon {
            border-style: solid;
            border-width: 0;
            border-color: #B2B2B2;
            cursor: pointer;
        }

        .imgModelIconD {
            border-style: solid;
            border-width: 0;
            border-color: #B2B2B2;
        }

        .imgModelIconSub {
            border-style: none;
            height: 140px;
            border-style: solid;
            border-width: 0;
            border-color: #B2B2B2;
        }

        .imgModelIconFilter {
            /*height: 120px;*/
            border-style: none;
            border-width: 0;
            border-color: #B2B2B2;
            filter: opacity(0.5);
            width: 158px;
            height: 123px;
        }

        .ApplicationListItemDivMain {
            border-style: none;
            border-color: brown;
            border-width: 0;
            font-family: Arial;
            color: #58595b;
            text-align: center;
        }

        .imgModelIconFilterF {
            /*height: 120px;*/
            border-style: none;
            border-width: 0;
            border-color: #B2B2B2;
            filter: opacity(0.5);
/*            width: 158px;
            height: 123px;*/
            position: relative;
            z-index: 1;
        }


        .ApplicationListItemDivMainF {
            border-style: none;
            border-color: brown;
            border-width: 0;
            font-family: Arial;
            color: #58595b;
            text-align: center;
/*            width: 158px;
            height: 123px;*/
        }

        @media screen and (max-width: 1000px) {
            .imgModelIconFilterF {
                border-style: none;
                border-width: 0;
                border-color: #B2B2B2;
                filter: opacity(0.1);
                /*                width: 60px;
                height: 46px;*/
            }
        }



        /***********************************/




        @media screen and (min-resolution: 105dpi) {
            .imgModelIconFilterF, .ApplicationListItemDivMainF, .ApplicationListItemDivMainSS {
                width: 128px !important;
            }
        }

        @media screen and (min-resolution: 115dpi) {
            .imgModelIconFilterF, .ApplicationListItemDivMainF, .ApplicationListItemDivMainSS {
                width: 108px !important;
                height: 98px !important;
            }
        }

        @media screen and (min-resolution: 144dpi) {
            .imgModelIconFilterF, .ApplicationListItemDivMainF, .ApplicationListItemDivMainSS {
                width: 78px !important;
                height: 68px !important;
            }
        }
        /***********************************/
        /***********************************/






        @media screen and (min-resolution: 105dpi) {
            .iTemdev {
                width: 260px !important;
            }
        }

        @media screen and (min-resolution: 115dpi) {
            .iTemdev {
                width: 200px !important;
            }
        }

        @media screen and (min-resolution: 144dpi) {
            .iTemdev {
                width: 180px !important;
            }
        }
        /***********************************/

        @media screen and (max-width: 1000px) {
            .ApplicationListItemDivMainF {
                border-style: none;
                border-color: brown;
                border-width: 0;
                font-family: Arial;
                color: #58595b;
                text-align: center;
            }
        }


        .imgModelIconFilterS {
            border-style: none;
            border-width: 0;
            border-color: #B2B2B2;
            filter: opacity(0.5);
            width: 158px;
            height: 123px;
        }



        @media screen and (max-width: 1000px) {
            .imgModelIconFilterS {
                border-style: none;
                border-width: 0;
                border-color: #B2B2B2;
                filter: opacity(0.5);
            }
        }





        .ApplicationListItemDivMainSS {
            border-style: none;
            border-color: brown;
            border-width: 0;
            font-family: Arial;
            color: #58595b;
            text-align: center;
            width: 158px;
            height: 123px;
        }


        @media screen and (max-width: 1000px) {
            .ApplicationListItemDivMainSS {
                border-style: none;
                border-color: brown;
                border-width: 0;
                font-family: Arial;
                /*font-size: 11px;*/
                color: #58595b;
                /*margin: 10px 10px 10px 10px;*/
                /*height: 150px;*/
                text-align: center;
                /*                width: 60px;
                height: 60px;*/
            }
        }

        .iTemdev {
            width: 16%;
            text-align: center
        }

        /*@media screen and (max-width: 1024px) {
            .iTemdev {
                width: 33%;*/
        /*height: 61px;*/
        /*text-align: center
            }
        }*/

        @media screen and (max-width: 1000px) {
            .iTemdev {
                width: 226px;
            }
        }

        @media screen and (max-width: 500px) {
            .iTemdev {
                width: 98%;
                /*height: 61px;*/
                text-align: center;
                text-align: left;
                display: flex;
                margin-left: 20px;
            }
        }

        .btnBackConfig {
            width: 50px;
        }



        .divConfigAll {
            max-height: 700px;
            overflow-x: hidden;
            overflow-y: auto;
        }

        @media screen and (max-width: 1030px) {
            .divConfigAll {
                max-height: 500px;
            }
        }

        @media screen and (max-width: 768px) {
            .divConfigAll {
                max-height: 680px;
            }
        }

        @media screen and (max-width: 500px) {
            .divConfigAll {
                max-height: 600px;
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
    </style>



    <asp:HiddenField ID="hfModelIcon_Groove_Turn" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_ISO_Turning" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_Threading" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_Milling" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_Hole_Making" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_Hole_Reaming" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_Indexable" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_Solid" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_SolidEndmilsBallNose" runat="server" Value="" />
    <asp:HiddenField ID="hfModelIcon_SolidEndmilsSquare" runat="server" Value="" />


    <asp:TextBox runat="server" ID="txt1" Text="10" Visible="false"></asp:TextBox>
    <asp:TextBox runat="server" ID="txt2" Text="11" CssClass="display_non"></asp:TextBox>
    <asp:HiddenField runat="server" ID="txt3" Value="12"></asp:HiddenField>



    <asp:HiddenField ID="hfRepoprtsNames" runat="server" />
    <wuc_Tabs:wuc_Tabs ID="wucTabs" runat="server" />


    <asp:UpdatePanel ID="upPnlAll_Config" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <script>
                function lasthideListView() {
                    document.getElementById("ContentPlaceHolderMain_btnBackBi").style.display = "none";
                    document.getElementById("ContentPlaceHolderMain_btnBackCi").style.display = "none";

                    if (window.innerWidth < 768) {
                        document.getElementById("containerC").style.display = "none";
                        document.getElementById("containerB").style.display = "none";
                        document.getElementById("ContentPlaceHolderMain_btnBackBi").style.display = "none";
                        document.getElementById("ContentPlaceHolderMain_btnBackCi").style.display = "none";
                        document.getElementById("containerC").style.display = "none";
                        document.getElementById("containerB").style.display = "none";

                        if (document.getElementById("ContentPlaceHolderMain_hfListViewLastSelected").value == "A") {
                            document.getElementById("containerA").style.display = "none";
                            document.getElementById("containerB").style.display = "block";
                            document.getElementById("ContentPlaceHolderMain_btnBackBi").style.display = "block";
                            document.getElementById("ContentPlaceHolderMain_lvMainApp_Sub_divModelIcon2_0").style.paddingTop = "40px";

                        }
                        else {
                            if (document.getElementById("ContentPlaceHolderMain_hfListViewLastSelected").value == "B") {
                                document.getElementById("containerA").style.display = "none";
                                document.getElementById("containerB").style.display = "none";
                                document.getElementById("containerC").style.display = "block";
                                document.getElementById("ContentPlaceHolderMain_btnBackCi").style.display = "block";
                                document.getElementById("ContentPlaceHolderMain_lvMainApp_SubSub_divModelIcon2_0").style.paddingTop = "40px";
                            }
                        }
                    }
                }
            </script>

            </h3>
                <div>

                    <asp:HiddenField runat="server" ID="hfListViewLastSelected"></asp:HiddenField>


                </div>
            <div class="GlobDef_MLAll BorderRightLeft divConfigAll">
                <div class="mainLeftRightPadding" id="containerForOverFlow" style="overflow: auto;">
                    <div id="containerA" style="border-left: thin solid #e9eaec; border-bottom: thin solid #e9eaec; border-right: thin solid #e9eaec; display: flex">
                        <div class="row" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; width: 100%">

                            <asp:ListView runat="server" ID="lvMainApp">
                                <ItemTemplate>

                                    <div id="divModelIcon1" runat="server" class="iTemdev">
                                        <div class="divIconWi">
                                            <asp:ImageButton ImageUrl='<%# "../media/Configuration/FirstMain/" + Eval("MANUM").ToString.Trim + ".png" %>' Enabled='<%# Eval("Active") %>' runat="server" ID="imgModelIcon1" CssClass="ModelIconButton" />
                                        </div>
                                        <div style="align-content: center">
                                            <asp:Button runat="server" ID="btnModelname1" CssClass="ModelCaptionButton" BackColor="Transparent" /><asp:Label runat="server" ID="lblFamilyCode" CssClass="display_non" />
                                        </div>
                                    </div>

                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <asp:Label ID="SelectedMainIdIndex" runat="server" CssClass="display_non"></asp:Label>
                        <asp:Label ID="SelectedMainIdFamily" runat="server" CssClass="display_non"></asp:Label>
                    </div>


                    <div class="" id="containerB">
                        <div style="position: absolute" class="displayNoneForMobile_A">
                            <asp:ImageButton runat="server" CssClass="btnBackConfig" ID="btnBackBi" ImageUrl="../media/Icons/icon_arrow.svg" />
                        </div>
                        <div style="border-left: thin solid #e9eaec; border-bottom: thin solid #e9eaec; border-right: thin solid #e9eaec; display: flex">
                            <div class="row" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; width: 100%">
                                <asp:ListView runat="server" ID="lvMainApp_Sub">
                                    <ItemTemplate>
                                        <div id="divModelIcon2" runat="server" class="iTemdev">
                                            <div class="divIconWi">
                                                <asp:ImageButton Enabled="false" runat="server" ID="imgModelIcon2" CssClass="ModelIconButton" />
                                            </div>
                                            <div>
                                                <asp:Button runat="server" ID="btnModelname2" CssClass="ModelCaptionButton2" BackColor="Transparent" />
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <asp:Label ID="SelectedSubIdIndex" runat="server" CssClass="display_non"></asp:Label>
                        </div>
                    </div>

                    <div class="" id="containerC">
                        <div style="position: absolute" class="displayNoneForMobile_A">
                            <asp:ImageButton runat="server" CssClass="btnBackConfig" ID="btnBackCi" ImageUrl="../media/Icons/icon_arrow.svg" />
                        </div>

                        <div style="border-left: thin solid #e9eaec; border-bottom: thin solid #e9eaec; border-right: thin solid #e9eaec; display: flex">
                            <div class="row" style="border-left: thin solid #e9eaec; border-right: thin solid #e9eaec; width: 100%">
                                <asp:ListView runat="server" ID="lvMainApp_SubSub">
                                    <ItemTemplate>
                                        <div id="divModelIcon2" runat="server" class="iTemdev">
                                            <div class="divIconWi">
                                                <asp:ImageButton runat="server" ID="imgModelIcon3" CssClass="img-fluid img-thumbnail imgModelIconFilter" OnClientClick="cancelonbeforeunload()" />
                                            </div>
                                            <div>
                                                <asp:Button runat="server" ID="btnModelname3" CssClass="ModelCaptionButton3" BackColor="Transparent" OnClientClick="cancelonbeforeunload()" />
                                                <asp:Label runat="server" ID="lblFamilyNum" CssClass="display_non" />
                                                <asp:Label runat="server" ID="lblFamilyIcon" CssClass="display_non" />
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <script>
                function SetDivHeightWithScroll() {
                    try {
                        var dFre = window.innerHeight - 50 - document.getElementById('containerForOverFlow').getBoundingClientRect().top + 'px';
                        document.getElementById('containerForOverFlow').style.maxHeight = dFre;
                    }
                    catch (error) {

                    }
                };

                //window.onload = function () {
                //    SetCaptionForLabelsCon();
                //};


                function SetCaptionForLabelsCon() {
                    try {

                        document.getElementById("ContentPlaceHolderMain_lvMainApp_btnModelname1_0").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Groove_Turn").value;
                        document.getElementById("ContentPlaceHolderMain_lvMainApp_btnModelname1_1").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_ISO_Turning").value;
                        document.getElementById("ContentPlaceHolderMain_lvMainApp_btnModelname1_2").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Threading").value;
                        document.getElementById("ContentPlaceHolderMain_lvMainApp_btnModelname1_3").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Milling").value;
                        document.getElementById("ContentPlaceHolderMain_lvMainApp_btnModelname1_4").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Hole_Making").value;
                        document.getElementById("ContentPlaceHolderMain_lvMainApp_btnModelname1_5").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Hole_Reaming").value;
                        if (document.getElementById("ContentPlaceHolderMain_lvMainApp_Sub_btnModelname2_0").value.includes('Solid')) {

                            document.getElementById("ContentPlaceHolderMain_lvMainApp_Sub_btnModelname2_0").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Solid").value;
                            document.getElementById("ContentPlaceHolderMain_lvMainApp_Sub_btnModelname2_1").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Indexable").value;
                        }
                        else {
                            document.getElementById("ContentPlaceHolderMain_lvMainApp_Sub_btnModelname2_0").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Indexable").value;
                            document.getElementById("ContentPlaceHolderMain_lvMainApp_Sub_btnModelname2_1").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_Solid").value;
                        };

                        document.getElementById("ContentPlaceHolderMain_lvMainApp_SubSub_btnModelname3_0").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_SolidEndmilsSquare").value;
                        document.getElementById("ContentPlaceHolderMain_lvMainApp_SubSub_btnModelname3_1").value = document.getElementById("ContentPlaceHolderMain_hfModelIcon_SolidEndmilsBallNose").value;


                    } catch (error) {
                        // Code to handle the exception
                        // ...
                    }
                }

            </script>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

