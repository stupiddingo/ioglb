Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Net
Imports System.Collections.Generic

Public Class outfitterarea
  Inherits System.Web.UI.Page

  Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  
    ' Note this all used to be one stored procedure with user defined functions to concatenate. This was not supported when the database was migrated to the State server. The following works, but is far from ideal.
    
    'Try

      ' Read in ID from QueryString           
      Dim ID As Integer = -99
      Dim LicenseAreaN As String
      If Integer.TryParse(Request.QueryString("ID"), ID) Then
        ' Open a new Connection Object
        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("oglbDB").ConnectionString)

        'Create DataReader
        Dim myDR As SqlDataReader
        Dim i As Integer = 0
        Dim LicenseID As Integer
        Dim DBATemp, xDBA As String
        
        Dim MyStringBuilder, JSStringBuilder As New System.Text.StringBuilder
        Dim Q As String = "SELECT View_OGLB_License.*, '' AS DBA, View_OGLB_LicenseArea.LicenseAreaN, View_OGLB_LicenseArea.LicenseAreaID, View_OGLB_License.[LicenseN] + '-' + View_OGLB_LicenseArea.[LicenseAreaN] AS LicenseArea, View_OGLB_LicenseArea.AreaDescID, View_OGLB_LicenseArea.Title, View_OGLB_LicenseArea.Notes AS LANotes, OGLB_AreaDesc.AreaDesc, OGLB_AreaDesc.PermitAdmin FROM View_OGLB_License INNER JOIN View_OGLB_LicenseArea ON View_OGLB_License.LicenseID = View_OGLB_LicenseArea.LicenseID INNER JOIN OGLB_AreaDesc ON View_OGLB_LicenseArea.AreaDescID = OGLB_AreaDesc.AreaDescID WHERE View_OGLB_LicenseArea.LicenseAreaID = " & ID.ToString()

        Dim myCMD As SqlCommand = New SqlCommand(Q, conn)
        myCMD.CommandTimeout = 30

        Dim url As String = ""
        
        conn.Open()
        myDR = myCMD.ExecuteReader()
        If myDR.HasRows Then
          Do While myDR.Read()
            DBATemp = myDR("Name")
            LicenseID = myDR("LicenseID")
            LicenseAreaN = myDR("LicenseAreaN")
            If (IsDBNull(myDR("url")) = False And Len(Trim(myDR("url"))) > 0) Then
              If (Left(Trim(myDR("url")),4) = "http") Then
                url = "<a class=""externallink"" title=""Outfitter Website"" href=""" & myDR("url") & """>" & myDR("url") & "</a>"
              Else
                url = "<a class=""externallink"" title=""Outfitter Website"" href=""http://" & myDR("url") & """>" & myDR("url") & "</a>"
              End If
            End If
            SubTitle.InnerHtml = "<dt>License Number: " & myDR("LicenseN") & "</dt><dd>License Holder: <a href=""/ifwis/ioglb/outfitter/" & myDR("LicenseID") & """>" & myDR("Name") & "</a></dd>"
            Contact.InnerHtml ="<dt>Contact:</dt><dd>" & StrConv(myDR("Address1").ToString(), vbProperCase) & " " & StrConv(myDR("City").ToString(), vbProperCase) & " " & myDR("State") & " " & myDR("Zip") & "<br/><br/>" & _
              myDR("Phone1") & myDR("PhType1") & "<br/>" & myDR("Phone2") & myDR("PhType2") & "<br/>" & myDR("Phone3") & myDR("PhType3") & "<br/>" & myDR("Email") & "<br/>" & url & "</dd>"

            MyStringBuilder.Append("<h3>Operating Area " & myDR("LicenseArea") & "</h3><p>" & myDR("Title") & "</p>")

            If System.IO.File.Exists("C:\Websites\ioglb\resources\images\" & myDR("AreaDescID") & ".png") Then
              MyStringBuilder.Append("<div class=""mapframe"" id=""mapframe" & myDR("AreaDescID") & """><div class=""layerToggle""><div style=""width: 40%; display: inline-block;""><input type=""checkbox"" id=""landLayer" & myDR("AreaDescID") & """ checked> Surface Management Agency</div><div style=""width: 30%; display: inline-block;""><input type=""checkbox"" id=""gmuLayer" & myDR("AreaDescID") & """ checked> Game Units</div><div style=""width: 30%; display: inline-block;""><input type=""checkbox"" id=""wildLayer" & myDR("AreaDescID") & """> Wilderness</div></div><div class=""mapview"" id=""mapview" & myDR("AreaDescID") & """></div><div class=""mapfail""><a title=""Open the Map Center."" href=""https://idfg.idaho.gov/ifwis/huntplanner/mapcenter?lyr=1&lbl=Outfitter+License+Area+" & myDR("LicenseArea") & "&val=" & myDR("AreaDescID") & """><img class=""border"" src='resources/images/" & myDR("AreaDescID") & ".png' alt='Map of Operating Area'></a></div><div class=""btn-group btn-group-justified pad-bottom"" role=""group"" aria-label=""Justified button group""><a class=""btn btn-default"" role=""button"" title=""Open the Map Center."" href=""https://idfg.idaho.gov/ifwis/huntplanner/mapcenter?lyr=1&amp;val=" & myDR("AreaDescID") & "&amp;lbl=""><span class=""glyphicon glyphicon-globe""></span> Interactive Map<br><small class=""hidden-xs"">Change basemap, overlays and print</small></a><a class=""btn btn-default"" role=""button"" title=""Open the Map Center."" href=""https://gis.idfg.idaho.gov/server/rest/services/Outfitters/MapServer/0/query?where=ID+%3D+" & myDR("AreaDescID") & "&amp;text=&amp;outFields=*&amp;returnGeometry=true&amp;outSR=4326&amp;f=kmz""><span class=""glyphicon glyphicon-map-marker""></span> Download KMZ<br><small class=""hidden-xs"">For GPS, Phone or Google Earth</small></a></div></div><h4>Operating Area Description</h4><p>" & myDR("AreaDesc").ToString().Replace(Environment.NewLine, "<br/>") & "</p><p>" & myDR("LANotes").ToString().Replace(Environment.NewLine, "<br/>") & "</p>")
              JSStringBuilder.Append("mapIt('https://gis.idfg.idaho.gov/server/rest/services/Outfitters/MapServer', 0, 'ID', 'int', " & myDR("AreaDescID") & ", 'mapview" & myDR("AreaDescID") & "');")
            Else
              Dim tempImg As String = MapIt(myDR("AreaDescID"))
              If tempImg = "-99" Or tempImg = "0" Then
                MyStringBuilder.Append("<div class=""alert alert-danger""><strong>Sorry, this area has not yet been mapped.</strong></div><h4>Operating Area Description</h4><p>" & myDR("AreaDesc").ToString().Replace(Environment.NewLine, "<br/>") & "</p><p>" & myDR("LANotes").ToString().Replace(Environment.NewLine, "<br/>") & "</p>")
              Else
                MyStringBuilder.Append("<div class=""mapframe""><div class=""layerToggle""><div style=""width: 40%; display: inline-block;""><input type=""checkbox"" id=""landLayer" & myDR("AreaDescID") & """ checked> Surface Management Agency</div><div style=""width: 30%; display: inline-block;""><input type=""checkbox"" id=""gmuLayer" & myDR("AreaDescID") & """ checked> Game Units</div><div style=""width: 30%; display: inline-block;""><input type=""checkbox"" id=""wildLayer" & myDR("AreaDescID") & """> Wilderness</div></div><div class=""mapview"" id=""mapview" & myDR("AreaDescID") & """></div><div class=""mapfail""><a title=""Open the Map Center."" href=""https://idfg.idaho.gov/ifwis/huntplanner/mapcenter?lyr=1&lbl=Outfitter+License+Area+" & myDR("LicenseArea") & "&val=" & myDR("AreaDescID") & """><img class=""border"" src='resources/images/" & myDR("AreaDescID") & ".png' alt='Map of Operating Area'></a></div><div class=""btn-group btn-group-justified pad-bottom"" role=""group"" aria-label=""Justified button group""><a class=""btn btn-default"" role=""button"" title=""Open the Map Center."" href=""https://idfg.idaho.gov/ifwis/huntplanner/mapcenter?lyr=1&amp;val=" & myDR("AreaDescID") & "&amp;lbl=""><span class=""glyphicon glyphicon-globe""></span> Interactive Map<br><small class=""hidden-xs"">Change basemap, overlays and print</small></a><a class=""btn btn-default"" role=""button"" title=""Open the Map Center."" href=""https://gis.idfg.idaho.gov/server/rest/services/Outfitters/MapServer/0/query?where=ID+%3D+" & myDR("AreaDescID") & "&amp;text=&amp;outFields=*&amp;returnGeometry=true&amp;outSR=4326&amp;f=kmz""><span class=""glyphicon glyphicon-map-marker""></span> Download KMZ<br><small class=""hidden-xs"">For GPS, Phone or Google Earth</small></a></div></d<h4>Operating Area Description</h4><p>" & myDR("AreaDesc").ToString().Replace(Environment.NewLine, "<br/>") & "</p><p>" & myDR("LANotes").ToString().Replace(Environment.NewLine, "<br/>") & "</p>")
                JSStringBuilder.Append("mapIt('https://gis.idfg.idaho.gov/server/rest/services/Outfitters/MapServer', 0, 'ID', 'int', " & myDR("AreaDescID") & ", 'mapview" & myDR("AreaDescID") & "');")
              End If
            End If
          Loop
          myDR.Close()
          
          Q  = "SELECT DBA FROM OGLB_DBA WHERE OutfitterLicense = " & LicenseID.ToString()
          myCMD = New SqlCommand(Q, conn)
          myCMD.CommandTimeout = 30
          myDR = myCMD.ExecuteReader()

          If myDR.HasRows Then
            Do While myDR.Read()
              If (IsDBNull(myDR("DBA")) = False AND Len(Trim(myDR("DBA"))) > 0) Then
                If (i > 0) Then
                  xDBA = xDBA & "<li>" & myDR("DBA") & "</li>"
                  DBATemp = DBATemp & ", " & myDR("DBA")
                Else
                  xDBA = "<li>" & myDR("DBA") & "</li>"
                  DBATemp = myDR("DBA")
                End If
              End If
              i = i + 1
            Loop
            DBA.InnerHtml = "Doing Business As:<ul>" & xDBA & "</ul>"
          End If
          myDR.Close()
          
          DBATemp = Replace(Replace(DBATemp, Chr(39), "&#39;"), "& ", "&amp; ") & " AREA " & LicenseAreaN
          Page.Title = DBATemp & " | Idaho Outfitters &amp; Guides Licensing Board"
          Title.InnerHtml = DBATemp
      
          MyStringBuilder.Append("<h4>Licensed Activities</h4><ul>")    
          Q = "SELECT OGLB_LicenseArea_Activity.ActivityID, OGLB_Activity.Activity FROM OGLB_LicenseArea_Activity INNER JOIN OGLB_Activity ON OGLB_LicenseArea_Activity.ActivityID = OGLB_Activity.ActivityID WHERE OGLB_LicenseArea_Activity.LicenseAreaID = " & ID.ToString()
          myCMD = New SqlCommand(Q, conn)
          myCMD.CommandTimeout = 30

          myDR = myCMD.ExecuteReader()

          If myDR.HasRows Then
            Do While myDR.Read()
              MyStringBuilder.Append("<li>" & myDR("Activity") & "</li>")
            Loop
          End If
          myDR.Close()
          MyStringBuilder.Append("</ul>")
          conn.Close()
      Else
          SubTitle.InnerHtml = "<dt>Sorry, Operating Area was not found.</dt>"
      End If
      OutfitterArea.InnerHtml = MyStringBuilder.ToString
      JS.Text = "<script>$(function() { " & JSStringBuilder.ToString & "});</script>"
      End If
  'Catch ex As Exception
  '    OutfitterArea.InnerHtml = "An unknown error has occurred, please try this link again from the search page.  If the problem persists please contact us."
  'End Try
End Sub

Function MapIt(ByVal ID As String)
  Dim tempVar As String = "-99"
  Dim mapminx, mapminy, mapmaxx, mapmaxy, dblExtentMax, dblExtentAdd As Double
  mapminx = 0
  mapminy = 0
  mapmaxx = 0
  mapmaxy = 0

  Dim request As HttpWebRequest
  Dim jsonresponse As HttpWebResponse = Nothing
  Dim reader As StreamReader
  Dim raw As String

  Try
    ' Get Envelope.
    request = DirectCast(WebRequest.Create("https://gis.idfg.idaho.gov/server/rest/services/Apps/HuntPlanner/MapServer/1/query?where=ID+%3D+" & ID & "&returnExtentOnly=true&f=json"), HttpWebRequest)
    jsonresponse = DirectCast(request.GetResponse(), HttpWebResponse)
    reader = New StreamReader(jsonresponse.GetResponseStream())
    raw = Reader.ReadToEnd().Replace("{""extent"":","").Replace(",""spatialReference"":{""wkid"":102100,""latestWkid"":3857}}","")
    Dim jss As New JavaScriptSerializer()
    Dim dict As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(raw)
    If (dict("xmin") <> "NaN") Then
      mapminx = dict("xmin")
      mapminy = dict("ymin")
      mapmaxx = dict("xmax")
      mapmaxy = dict("ymax")
    End If
 
    ' Portrait or landscape?
    If (mapmaxx - mapminx) > (mapmaxy - mapminy) Then
      dblExtentMax = mapmaxx - mapminx
    Else
      dblExtentMax = mapmaxy - mapminy
    End If

    ' Buffer the zoom extent based on scale.
    If (dblExtentMax < 10000) Then
      dblExtentAdd = dblExtentMax * 0.8
    Else
      If (dblExtentMax < 25000) Then
        dblExtentAdd = dblExtentMax * 0.4
      Else
        dblExtentAdd = dblExtentMax * 0.2
      End If
    End If
    mapminx = mapminx - dblExtentAdd
    mapminy = mapminy - dblExtentAdd
    mapmaxx = mapmaxx + dblExtentAdd
    mapmaxy = mapmaxy + dblExtentAdd

    If mapmaxx <> Math.Abs(mapminx) Then
      tempVar = ID
      Dim url As String = "https://gis.idfg.idaho.gov/server/rest/services/Utilities/PrintingTools/GPServer/Export%20Web%20Map%20Task/execute?Web_Map_as_JSON=%7B%22mapOptions%22%3A%7B%22showAttribution%22%3Atrue%2C%22extent%22%3A%7B%22xmin%22%3A-117.3%2C%22ymin%22%3A41.8%2C%22xmax%22%3A-110.9%2C%22ymax%22%3A49.1%2C%22spatialReference%22%3A%7B%22wkid%22%3A4326%7D%7D%2C%22spatialReference%22%3A%7B%22wkid%22%3A102605%7D%7D%2C%22operationalLayers%22%3A%5B%7B%22id%22%3A%22O%22%2C%22title%22%3A%22Outfitters%22%2C%22opacity%22%3A1%2C%22minScale%22%3A0%2C%22maxScale%22%3A0%2C%22visibility%22%3Atrue%2C%22url%22%3A%22https%3A%2F%2Fgis.idfg.idaho.gov%2Fserver%2Frest%2Fservices%2FOutfitters%2FMapServer%2F0%22%2C%22layerDefinition%22%3A%7B%22definitionExpression%22%3A%22ID%3D+" & ID & "%22%7D%7D%2C%7B%22id%22%3A%22S%22%2C%22title%22%3A%22Counties%22%2C%22opacity%22%3A1%2C%22minScale%22%3A0%2C%22maxScale%22%3A0%2C%22visibility%22%3Atrue%2C%22url%22%3A%22https%3A%2F%2Fgis.idfg.idaho.gov%2Fserver%2Frest%2Fservices%2FReference%2FMapServer%2F1%22%7D%5D%2C%22exportOptions%22%3A%7B%22outputSize%22%3A%5B100%2C160%5D%2C%22dpi%22%3A96%7D%2C%22layoutOptions%22%3A%7B%22titleText%22%3A%22%22%2C%22scaleBarOptions%22%3A%7B%7D%2C%22legendOptions%22%3A%7B%7D%7D%7D&Format=PNG32&Layout_Template=&env%3AoutSR=&env%3AprocessSR=&returnZ=false&returnM=false&returnTrueCurves=false&returnFeatureCollection=false&f=pjson"
      request = DirectCast(WebRequest.Create(url), HttpWebRequest)
      jsonresponse = DirectCast(request.GetResponse(), HttpWebResponse)
      reader = New StreamReader(jsonresponse.GetResponseStream())
      raw = reader.ReadToEnd()
      Dim s As Integer = raw.IndexOf("url"": """)
      Dim p As Integer = raw.IndexOf(".png")
      Dim imgUrl As String = raw.Substring(s + 7, p - s - 3)
      Dim webClient As WebClient = New WebClient
      webClient.DownloadFile(imgUrl, "C:\Websites\ioglb\resources\thumbs\" & ID.ToString() & ".png")

      url = "https://gis.idfg.idaho.gov/server/rest/services/Utilities/PrintingTools/GPServer/Export%20Web%20Map%20Task/execute?Web_Map_as_JSON=%7B%22mapOptions%22%3A%7B%22showAttribution%22%3Atrue%2C%22extent%22%3A%7B%22xmin%22%3A" & mapminx & "%2C%22ymin%22%3A" & mapminy & "%2C%22xmax%22%3A" & mapmaxx & "%2C%22ymax%22%3A" & mapmaxy & "%2C%22spatialReference%22%3A%7B%22wkid%22%3A102113%7D%7D%2C%22spatialReference%22%3A%7B%22wkid%22%3A102605%7D%7D%2C%22operationalLayers%22%3A%5B%7B%22id%22%3A%22Outfitters%22%2C%22title%22%3A%22Outfittesr%22%2C%22opacity%22%3A1%2C%22minScale%22%3A0%2C%22maxScale%22%3A0%2C%22visibility%22%3Atrue%2C%22url%22%3A%22https%3A%2F%2Fgis.idfg.idaho.gov%2Fserver%2Frest%2Fservices%2FOutfitters%2FMapServer%2F0%22%2C%22layerDefinition%22%3A%7B%22definitionExpression%22%3A%22ID%3D+" & ID & "%22%7D%7D%2C%7B%22id%22%3A%22Land%22%2C%22title%22%3A%22Land%22%2C%22opacity%22%3A0.4%2C%22minScale%22%3A0%2C%22maxScale%22%3A0%2C%22visibility%22%3Afalse%2C%22url%22%3A%22https%3A%2F%2Ftiles.arcgis.com%2Ftiles%2FFjJI5xHF2dUPVrgK%2Farcgis%2Frest%2Fservices%2FBLM_Surface_Management%2FMapServer%22%7D%5D%2C%22baseMap%22%3A%7B%22title%22%3A%22World+Shaded+Relief%22%2C%22baseMapLayers%22%3A%5B%7B%22url%22%3A%22https%3A%2F%2Fserver.arcgisonline.com%2Farcgis%2Frest%2Fservices%2FNatGeo_World_Map%2FMapServer%22%7D%5D%7D%2C%22exportOptions%22%3A%7B%22outputSize%22%3A%5B785%2C480%5D%2C%22dpi%22%3A96%7D%2C%22layoutOptions%22%3A%7B%22titleText%22%3A%22%22%2C%22scaleBarOptions%22%3A%7B%7D%2C%22legendOptions%22%3A%7B%7D%7D%7D&Format=PNG32&Layout_Template=&env%3AoutSR=&env%3AprocessSR=&returnZ=false&returnM=false&returnTrueCurves=false&returnFeatureCollection=false&f=pjson"
      request = DirectCast(WebRequest.Create(url), HttpWebRequest)
      jsonresponse = DirectCast(request.GetResponse(), HttpWebResponse)
      reader = New StreamReader(jsonresponse.GetResponseStream())
      raw = reader.ReadToEnd()
      s = raw.IndexOf("url"": """)
      p = raw.IndexOf(".png")
      imgUrl = raw.Substring(s + 7, p - s - 3)
      webClient.DownloadFile(imgUrl, "C:\Websites\ioglb\resources\images\" & ID.ToString() & ".png")
    Else
      tempVar = "-99"
    End If

  Catch
    tempVar = "-99"
  End Try
  MapIt = tempVar
End Function

End Class
