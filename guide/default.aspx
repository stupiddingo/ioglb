<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" maintainscrollpositiononpostback="false"  CodeFile="default.aspx.vb" Inherits="outfitterguide" title="Search for an Outfitter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<div id="fullpage">
						<script type="text/javascript">					
			function selLoc(checkBoxIndex)
			{
				// Get the checkbox to verify.
				var objItemChecked = 
				document.getElementById('ctl00_ContentPlaceHolder1_lsAreas_' + checkBoxIndex);
				// Does the individual checkbox exist?
				if(objItemChecked == null)
				{
					return;
				}
			    
				// Is the checkbox to verify checked?
				var isChecked = objItemChecked.checked;
				if (isChecked == false)
				{
					objItemChecked.checked = true;
				} else {
					objItemChecked.checked = false;
				}
				objItemChecked.focus()
			}
			</script>
					<div id="ResultsDiv" runat="server">
						<div id="OutfitterList" runat="server" EnableViewState="False">
						</div>
					</div>
					<div id="SearchDiv" runat="server">
						<div id="GameGuideContainer">
							<div id="GameGuideContent">
							</div>
						</div>
					</div>
					<p><asp:button id="butSearchAgain" runat="server" Text="Modify my search." Visible="False"></asp:button></p>
					<div id="QueryTable" runat="server">
						<h1 runat="server" id="txtTitle">Find Your Outfitter.</h1>
                        <p>This tool will search for the combination of activities 
							you select. Some outfitters (e.g. Float Boat, Day Hike) offer only one 
							activity. If you are getting too few results, try limiting your search to one 
							activity.</p>
						<h3>Select the Guided Activities that Interest You</h3>
						<asp:CheckBoxList id="chkActivity" runat="server" RepeatColumns="3"></asp:CheckBoxList>
						<h3>Limit Query Extent By</h3>
						<asp:radiobuttonlist id="radArea" runat="server" RepeatColumns="3" AutoPostBack="True"></asp:radiobuttonlist>
						<div id="AreasContainer" runat="server">
							<fieldset>
								<legend id="txtlsAreas" runat="server"></legend>
								<p class="caption">You may select multiple areas.  Leaving all options blank will result in all areas being returned.</p>
								<table id="outfitter-results">
									<tr valign="top">
										<td>
											<div runat="server" id="lsAreasScroll" style="border:#aaa 1px solid; overflow:auto; height:670px; width:140px; float:left;">
												<asp:CheckBoxList id="lsAreas" name="lsAreas" runat="server" EnableViewState="True" CellPadding="0"
													CellSpacing="0"></asp:CheckBoxList>
											</div>
                                        </td>
										<td>
                                            <div id="AreaMap" runat="server"></div>
										</td>
									</tr>
								</table>
							</fieldset>
						</div>
						<h3>Enter All or Part of an Outfitter Name</h3>
							<asp:textbox id="txtName" runat="server"></asp:textbox>
                            <div class="caption">Leave null to search for all outfitters.</div>
					
						<asp:button id="btnSubmit" runat="server" Text="Search" CssClass="button btn btn-default"></asp:button> <asp:button id="btnReset" runat="server" Text="Clear" CssClass="button btn btn-default"></asp:button>
					</div>
    </div>
</asp:Content>
