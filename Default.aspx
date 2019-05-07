<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" CodeFile="default.aspx.vb" Inherits="_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="outfittersearchhome" style="width:60%">
					<h1>Outfitter Search</h1>
					<fieldset id="activity-search">
            <legend><h3>Search by Activity</h3></legend>
            <p><a href="guide/?t=hunting"><img alt="Search Hunting Outfitters" class="fl" src="resources/hunting.gif"/></a> 
            <strong><a href="guide/?t=hunting">Hunting</a></strong><br/>
            With the most square miles of wilderness in the lower 48 states, Idaho offers 
            the best in big game hunting and experienced outfitters to escort you.</p>

            <p><a href="guide/?t=fishing"><img alt="Search Fishing Outfitters" class="fl" src="resources/fishing.gif"/></a> 
            <strong><a href="guide/?t=fishing">Fishing</a></strong><br/>
            Idaho’s Silver Creek, the Henrys Fork, and the South Fork of the Snake are 
            renowned all over the world for their fly-fishing opportunities. Whether you 
            hope to catch dinner or just catch and release, Idaho is the perfect place to 
            drop a line.</p>

            <p><a href="guide/?t=boating"><img alt="Search Boating Outfitters" class="fl" src="resources/boating.gif"/></a>
            <strong><a href="guide/?t=boating">Boating</a></strong><br/>
            If it weren't for potatoes, Idaho would be known as the whitewater state. Idaho 
            is home to more miles of whitewater than any other state in the Lower 48.  
            Experience our rivers by jet boat, power boat, raft, canoe or kayak.</p>

            <p><a href="guide/?t=recreation"><img alt="Search Recreation Outfitters" class="fl" src="resources/recreation.gif"/></a>
            <strong><a href="guide/?t=recreation">Recreation</a></strong><br/>
            Idaho has nearly 9 million acres of roadless, undeveloped National Forest land, more 
            than any other state in the lower 48.  This presents a myriad of outdoor recreation 
            possibilities from peak bagging to wildlife photography.</p>

            <p><a href="guide"><img alt="Search All Outfitters" class="fl" src="resources/search.gif"/></a>
             <strong><a href="guide">Search All Guided Activities</a></strong><br/>
            Mix it up. From blast and cast excursions to heli-skiing and whitewater rafting, 
            find outfitters who offer a variety of activities the entire family will enjoy.</p>
          </fieldset>
          <fieldset id="name-search"><legend><h3>Search by Name</h3></legend>
            <asp:TextBox id="TextBox1" runat="server" TabIndex="1"></asp:TextBox> 
            <asp:Button id="Button1" runat="server" Text="Search" TabIndex="0" CssClass="button btn btn-default"></asp:Button>
            <div class="caption">Enter all or part of an outfitter name</div>
          </fieldset>
						<br/><br/><br/><br/><br/><br/>
					<div id="outfitter-partners">
						<img id="partner-imagemap" src="//idfg.idaho.gov/ifwis/ioglb/resources/logos.png" usemap="#partner-imagemap" alt="Cooperator Logos" />
						<map name="partner-imagemap">
							<area shape="rect" coords="0,0,108,108" href="https://idfg.idaho.gov" alt="Idaho Department of Fish and Game" title="Idaho Department of Fish and Game" />
							<area shape="rect" coords="105,0,209,108" href="http://www.fs.fed.us" alt="United States Forest Service" title="United States Forest Service" />
							<area shape="rect" coords="208,0,327,108" href="http://www.blm.gov" alt="United States Bureau of Land Management" title="United States Bureau of Land Management" />
							<area shape="rect" coords="325,0,436,108" href="http://parksandrecreation.idaho.gov/" alt="Idaho State Parks and Recreation" title="Idaho State Parks and Recreation" />
						</map>
					</div>
					<div class="caption">
						Produced in Partnership and Cooperation with 
						<a href="https://idfg.idaho.gov/" title="Idaho Department of Fish and Game">Idaho Fish &amp; Game</a>,
						<a class="externallink" href="http://www.fs.fed.us/" title="United States Forest Service">U.S. Forest Service</a>,
						<a class="externallink" href="http://www.blm.gov/" title="United States Bureau of Land Management">U.S. Bureau of Land Management</a> and
						<a href="http://parksandrecreation.idaho.gov/" title="Idaho Department of State Parks and Recreation">Idaho Parks &amp; Recreation</a>.  
						<br/>Powered by <a href="https://fishandgame.idaho.gov/ifwis/portal" title="Idaho Fish and Wildlife Information System @IDFG">Idaho Fish and Wildlife Information System</a>.
					</div>	

</div>
</asp:Content>
