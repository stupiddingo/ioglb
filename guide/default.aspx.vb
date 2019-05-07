Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Collections
Public Class outfitterguide
    Inherits System.Web.UI.Page


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Page.IsPostBack Then

            Try

                ' Open Connection Object
                'Assign a connection string from the Web.config file
                Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("oglbDB").ConnectionString)
                Dim myDR As SqlDataReader

                '---Make the PIC_Activity Listbox---
                'Create the SqlCommand
                Dim myCmd As New SqlCommand()
                Dim Q As String

                Dim ID As String
                ID = StrConv(Server.UrlDecode(Request.QueryString("t")), vbProperCase)

                If (ID = "Hunting" OR ID = "Fishing" OR ID = "Recreation" OR ID = "Winter" OR ID = "Boating") Then
                    Q = "SELECT OGLB_Activity.ActivityID, OGLB_Activity.Activity FROM OGLB_ActivityType " & _
        "INNER JOIN (OGLB_Activity INNER JOIN OGLB_Activity_ActivityType ON OGLB_Activity.ActivityID = OGLB_Activity_ActivityType.ActivityID) " & _
        "ON OGLB_ActivityType.ActivityTypeID = OGLB_Activity_ActivityType.ActivityTypeID WHERE (OGLB_ActivityType.ActivityType = '" & ID & "') " & _
        "AND OGLB_Activity.ActivityID IN (SELECT OGLB_LicenseArea_Activity.ActivityID FROM View_OGLB_LicenseArea " & _
        "INNER JOIN OGLB_LicenseArea_Activity ON View_OGLB_LicenseArea.LicenseAreaID = OGLB_LicenseArea_Activity.LicenseAreaID) " & _
        "ORDER BY OGLB_Activity.SortBy"
                  txtTitle.InnerHtml = "Search for " & StrConv(ID, vbProperCase) & " Outfitters"
                Else
                    txtTitle.InnerHtml = "Search All Outfitters"
                    Q = "SELECT ActivityID, Activity FROM OGLB_Activity " & _
        "WHERE ActivityID IN (SELECT OGLB_LicenseArea_Activity.ActivityID FROM View_OGLB_LicenseArea " & _
        "INNER JOIN OGLB_LicenseArea_Activity ON View_OGLB_LicenseArea.LicenseAreaID = OGLB_LicenseArea_Activity.LicenseAreaID) " & _
        "ORDER BY SortBy"
                End If
                myCmd = New SqlCommand(Q, conn)
                myCmd.CommandTimeout = 30

                'Execute Reader
                conn.Open()
                myDR = myCmd.ExecuteReader()
                chkActivity.DataSource = myDR
                chkActivity.DataTextField = "Activity"
                chkActivity.DataValueField = "ActivityID"
                chkActivity.DataBind()
                myDR.Close()
                conn.Close()

                Q = "SELECT GIS_Type.TypeID, GIS_Type.Type FROM (GIS INNER JOIN GIS_OGLB_AreaDesc ON GIS.GISID = GIS_OGLB_AreaDesc.GISID) INNER JOIN GIS_Type ON GIS.GISTypeID = GIS_Type.TypeID WHERE(((GIS_OGLB_AreaDesc.AreaDescGISID) Is Not Null)) GROUP BY GIS_Type.TypeID, GIS_Type.Type, GIS_Type.Weight ORDER BY GIS_Type.Weight"
                myCmd = New SqlCommand(Q, conn)
                myCmd.CommandTimeout = 30

                'Execute Reader
                conn.Open()
                myDR = myCmd.ExecuteReader()
                radArea.DataSource = myDR
                radArea.DataTextField = "Type"
                radArea.DataValueField = "Type"
                radArea.DataBind()
                myDR.Close()
                conn.Close()
                'Add the "All" item to the HUNT_Area Query drop down list and select the [All] option
                radArea.Items.Add("All Available")
                radArea.SelectedIndex = radArea.Items.Count - 1
                AreasContainer.Visible = False
                If Len(Request.QueryString("Search")) > 0 Then
                    txtName.Text = HttpUtility.UrlDecode(Request.QueryString("Search"))
                    Fill()
                End If
            Catch ex As Exception
                'Do nothing
            End Try

        End If
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Fill()
    End Sub

    Sub Fill()
        Try

 
            Dim MyStringBuilder As New System.Text.StringBuilder
            MyStringBuilder.Append("<h1>Matching Outfitter Areas</h1><p>Your search criteria:<br/><div style=""padding-left:20px;"">")

            Dim Q As String = "SELECT DISTINCT View_OGLB_License.LicenseID, View_OGLB_License.LicenseN, Name, SUBSTRING(( SELECT ', '+DBA AS [text()] FROM OGLB_DBA WHERE OGLB_DBA.OutfitterLicense = View_OGLB_License.LicenseID FOR XML PATH ('')),3, 1000) [DBA], View_OGLB_LicenseArea.LicenseAreaID, View_OGLB_LicenseArea.Title, LicenseAreaN As AreaN, View_OGLB_LicenseArea.AreaDescID FROM View_OGLB_License INNER JOIN View_OGLB_LicenseArea ON View_OGLB_License.LicenseID = View_OGLB_LicenseArea.LicenseID WHERE View_OGLB_License.LicenseID Is Not Null "

            Dim li As ListItem
            Dim temp As String = ""
            For Each li In chkActivity.Items
                If li.Selected = True Then
                    Q &= " AND LicenseAreaID IN (SELECT LicenseAreaID FROM OGLB_LicenseArea_Activity WHERE ActivityID = " & li.Value & ")"
                    temp = temp & li.Text & ", "
                End If
            Next

            If temp <> "" Then
                MyStringBuilder.Append("Activity(s): <em>" & Left(temp, Len(temp) - 2) & "</em><br/>")
                temp = ""
            End If

            If radArea.SelectedItem.Value <> "All Available" And lsAreas.SelectedIndex > -1 Then
                AreasContainer.Visible = True
                Dim templist As String = ""
                For i As Integer = 0 To lsAreas.Items.Count - 1
                    If lsAreas.Items.Item(i).Selected = True Then
                        templist = templist & lsAreas.Items.Item(i).Value & ","
                    End If
                Next
                If Len(templist) > 0 Then
                    templist = Left(templist, Len(templist) - 1)
                End If

                Q &= " AND AreaDescID IN (SELECT AreaDescID FROM GIS_OGLB_AreaDesc WHERE GISID IN (" & templist & "))"

                For Each li In lsAreas.Items
                    If li.Selected = True Then
                        temp = temp & li.Text & ", "
                    End If
                Next

            Else
                AreasContainer.Visible = False
                lsAreas.ClearSelection()
            End If


            If temp <> "" Then
                MyStringBuilder.Append(radArea.SelectedItem.Value & "(s): <em>" & Left(temp, Len(temp) - 2) & "</em><br/>")
            End If
            
            If Len(txtName.Text.ToString()) > 0 Then
              Dim searchText As String = Regex.Replace(Server.UrlDecode(txtName.Text.ToString()), "[^0-9A-Za-z ]", " ").ToLower()
              Dim searches() As String = searchText.Split(New Char() {" "c})
              Dim tempint As Int32
              Dim tempstr As String = ""
                For x As Integer = 0 To searches.Length - 1
                    If Len(Trim(searches(x))) > 0 Then
                        tempstr &= " NAME LIKE '%" & searches(x) & "%' OR View_OGLB_License.LicenseID IN (SELECT OutfitterLicense FROM OGLB_DBA WHERE DBA LIKE '%" & searches(x) & "%') OR "
                        If Len(searches(x)) < 6 Then
                            If (Int32.TryParse(Trim(searches(x)), tempint)) Then
                                tempstr &= " View_OGLB_License.LicenseN LIKE '" & Trim(searches(x)) & "%' OR "
                            End If
                        End If
                    End If
                Next
                If tempstr <> "" Then
                    Q &= " AND (" & Left(tempstr, Len(tempstr) - 4) & ")"
                    MyStringBuilder.Append("Keywords: <em>" & searchText & "</em><br/>")
                End If
            End If

            ' Open Connection Object
            'Assign a connection string from the Web.config file
            Q = Q & " ORDER BY Name, AreaN"
            Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("oglbDB").ConnectionString)

            'Create the SqlCommand
            Dim myCMD As SqlCommand = New SqlCommand(Q, conn)
            myCMD.CommandTimeout = 30

            MyStringBuilder.Append("</div></p>")


            Dim DBATemp As String = ""

            Dim Cnt As String
            Cnt = "t1"

            'Execute Reader
            conn.Open()
            Dim myDR As SqlDataReader = myCMD.ExecuteReader()
            If myDR.HasRows Then
                Do While myDR.Read()

                    MyStringBuilder.Append("<div class=""row " & Cnt & """>")
                    'If System.IO.File.Exists("C:\Websites\ioglb\resources\thumbs\" & myDR("AreaDescID") & ".png") Then
                    '    MyStringBuilder.Append("<tr class=""" & Cnt & """><td><a title=""View License Area Details."" href=""outfitterarea.aspx?ID=" & myDR("LicenseAreaID") & """><img src=""resources/thumbs/" & myDR("AreaDescID") & ".png"" alt=""Map of License Operating Area""></a></td>")
                    'Else
                    '   MyStringBuilder.Append("<tr class=""" & Cnt & """><td><a title=""View License Area Details."" href=""outfitterarea.aspx?ID=" & myDR("LicenseAreaID") & """><img src=""resources/thumbs/-99.png"" alt=""Map of License Operating Area""></a></td>")
                    'End If

                    If (IsDBNull(myDR("DBA"))) Then
                        DBATemp = Replace(Replace(myDR("Name"), Chr(39), "&#39;"), "& ", "&amp;")
                    Else
                        DBATemp = Replace(Replace(myDR("DBA"), Chr(39), "&#39;"), "& ", "&amp;")
                    End If
                    MyStringBuilder.Append("<h4><a title=""View License Operating Area Details."" href=""/ifwis/ioglb/area/" & myDR("LicenseAreaID") & """>" & DBATemp & "</a> <small>License Area " & myDR("LicenseN") & "-" & myDR("AreaN") & "</small></h4>")

                    MyStringBuilder.Append(myDR("Title") & "<br/>View All Operating Areas Licensed by <a href=""/ifwis/ioglb/outfitter/" & myDR("LicenseID") & """ title=""View details for this outfitter"">" & DBATemp & "</a></div>")

                    If Cnt = "t1" Then
                        Cnt = "t2"
                    Else
                        Cnt = "t1"
                    End If
                Loop
                OutfitterList.InnerHtml = MyStringBuilder.ToString
                QueryTable.Visible = False
                ResultsDiv.Visible = True
                butSearchAgain.Visible = True
                SearchDiv.InnerHtml = "<hr class='hide'><h3>Don't see what you're looking for?</h3><p>Try selecting less activities or broading your search area.</p>"
                    Else
                SearchDiv.InnerHtml = "<div class='alert alert-danger'><h3>No outfitters found.</h3><p>Try limiting the number of activities or broading the area of interest.</p></div>"
            End If
            myDR.Close()
            conn.Close()
        Catch ex As Exception
            SearchDiv.InnerHtml = "<div class='alert alert-danger'><h3>No outfitters found.</h3><p>Try limiting the number of activities or broading the area of interest.</p></div>"
        End Try
    End Sub

   Private Sub radArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radArea.SelectedIndexChanged
        If radArea.SelectedItem.Value = "All Available" Then
            AreasContainer.Visible = False
        Else
            ' Open Connection Object
            'Assign a connection string from the Web.config file
            Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("oglbDB").ConnectionString)

            'Construct the HUNT_PIC_XXX- Area - XXX Drop-Down List
            'Create the SqlCommand
            Dim cmdAreas As New SqlCommand _
            ("SELECT * FROM GIS_Type INNER JOIN GIS ON GIS_Type.TypeID = GIS.GISTypeID WHERE GIS_Type.Type = '" & radArea.SelectedItem.Value & "'", conn)
            Dim imagemaplist As New System.Text.StringBuilder("<map name='imagemap'>")

            'Create DataReader
            Dim drAreas As SqlDataReader
            conn.Open()
            drAreas = cmdAreas.ExecuteReader()
            If drAreas.HasRows Then
                Dim i As Integer = 0
                Do While drAreas.Read()
                    imagemaplist.Append("<AREA SHAPE=""POLY"" COORDS=""" & drAreas("Coords") & """ OnClick=""selLoc('" & i & "')"" alt=""" & drAreas("GISLabel") & """>")
                    i = i + 1
                Loop
            End If
            drAreas.Close()

            'Create DataReader
            drAreas = cmdAreas.ExecuteReader()
            lsAreas.DataSource = drAreas
            lsAreas.DataTextField = "GISLabel"
            lsAreas.DataValueField = "GISID"
            lsAreas.DataBind()
            drAreas.Close()
            conn.Close()

            imagemaplist.Append("</map>")


            AreaMap.InnerHtml = "<img class=""fl"" src=""/ifwis/ioglb/resources/maps/" & radArea.SelectedItem.Value & ".jpg"" width=""467"" height=""667"" alt=""Imagemap"" ISMAP USEMAP=""#imagemap"" style=""cursor:pointer; cursor:hand; border:0;""/>" & imagemaplist.ToString()
            AreasContainer.Visible = True
            txtlsAreas.InnerText = radArea.SelectedItem.Text.ToString
        End If
    End Sub

   Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
      chkActivity.ClearSelection()
      radArea.SelectedIndex = radArea.Items.Count - 1
        AreasContainer.Visible = False
        txtName.Text = ""
   End Sub

   Private Sub butSearchAgain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles butSearchAgain.Click
      QueryTable.Visible = True
      ResultsDiv.Visible = False
      butSearchAgain.Visible = False
        SearchDiv.InnerHtml = "<h2>Don't see what you're looking for?</h2><p>Try selecting less activities or broadening your area of interest.</p>"
   End Sub


End Class
