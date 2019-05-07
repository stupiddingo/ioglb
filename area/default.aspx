<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false"  CodeFile="default.aspx.vb" Inherits="outfitterarea" %>
<asp:Content ID="Content0" ContentPlaceHolderID="ContentPlaceHolderHeader" Runat="Server">
  <link rel="stylesheet" href="https://js.arcgis.com/4.11/esri/css/main.css">
  <link rel="stylesheet" href="../resources/map.css">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="fullpage">
	<h1 id="Title" runat="server"></h1>
	<dl id="SubTitle" runat="server"></dl>
  <div id="DBA" runat="server"></div>
  <dl id="Contact" runat="server"></dl>
	<div id="OutfitterArea" runat="server"></div>
	<div class="caption">
    <p>DISCLAIMER: These digitized maps are only a representation of the official licensed areas and are not intended to replace the official written description that may be found in the offices of the Idaho Outfitters and Guides Licensing Board (IOGLB).  In the preparation of these digitized maps, extensive efforts have been made to offer the most current, correct, and clearly expressed information possible.  However, inadvertent errors do occur.  Prior to relying on information provided in these digitized maps, users should contact the Idaho Outfitters and Guides for verification.</p>
    <p>In using the information provided by these digitized maps, users should be aware that the information was created for planning and conceptual uses.  These maps are not designed for any detailed purposes.</p>
    <p></p>Use of any of the information provided on this website is at the user's own risk.  Neither the Idaho Outfitters and Guides Licensing Board, nor any of their employees, agents or contractors, assumes any legal responsibility for the use of the information contained on this website, which is provided as the best available, but with no warranties of any kind.  The IOGLB disclaim any and all liability arising out of the misuse of this information and disclaim all express or implied warranties, including warranties of merchantability, fitness for a particular purpose, and non-infringement of proprietary rights.  Neither the IOGLB, the State of Idaho, nor any of their employees, agents, or contractors will be liable for any actions, claims, damages, or judgments of any nature whatsoever arising out of the use or misuse of the information contained on this website.</p>
  </div>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderFooter" Runat="Server">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<script src="https://js.arcgis.com/4.11/"></script>
<asp:Literal id="JS" runat="server"></asp:Literal>
<script src="../resources/map.js"></script>
</asp:Content>