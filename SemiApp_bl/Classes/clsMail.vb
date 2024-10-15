Imports System.Configuration
Imports System.IO
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Threading
Imports IscarDal


Public Class clsMail

    Dim mailMsg As MailMessage

    Public Shared Sub SendDrawing(Starttext As String, MailAddress As String, Qutno As String, BranchCode As String, sFileToSend As String, sFilesToSend As List(Of String), ServerMapPath As String)
        Try
            'Dim langID As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
            Dim dtMessage As DataTable = clsLanguage.Get_LanguageMessagesCaption(clsLanguage.e_Form.QuotationInformation)

            Dim mailTest0 As String = dtMessage.Rows(0).Item("MESSAGE").ToString
            Dim mailTest1 As String = dtMessage.Rows(1).Item("MESSAGE").ToString
            Dim mailTest2 As String = dtMessage.Rows(2).Item("MESSAGE").ToString
            Dim mailTest3 As String = dtMessage.Rows(3).Item("MESSAGE").ToString
            Dim mailTest4 As String = dtMessage.Rows(4).Item("MESSAGE").ToString


            '' Dim mailTest0 As String = "iQuote Quotation Information. Quotation Number"

            'Dim mailTest1 As String = "Dear Customer"
            'Dim mailTest2 As String = "Please save serial number:"
            'Dim mailTest3 As String = "A new iQuote offer has been created - Quotation Number"
            'Dim mailTest4 As String = "To save and view the quotation, please"

            'If BranchCode = "IG" Then
            '    mailTest0 = "iQuote-Angebotsinformationen. Angebotsnummer"
            '    mailTest1 = "Sehr geehrter Kunde"
            '    'mailTest2 = ""
            '    mailTest3 = "Ein neues iQuote-Angebot wurde erstellt – Angebotsnummer"
            '    mailTest4 = "Um das Angebot anzuzeigen oder zu ändern, melden Sie sich bitte an"
            'End If


            If Starttext = "" Then Starttext = mailTest1 & ","

            If MailAddress <> "" AndAlso MailAddress.Contains("@") Then
                Dim fName As String = ""
                Dim fPath As String = ""
                If CatiaDrawing.GetFilePath_CatiaDrawing(Qutno, BranchCode, fName, fPath) = True Then

                    Dim body As String = "" '= "Test body For iQuote"
                    Dim Subject As String = ""

                    If clsQuatation.IsTemporary_Quotatiom Then
                        Dim ow As String = ConfigurationManager.AppSettings("TemporaryExpiryDateText")
                        Dim sMsg As String = ""
                        Dim sT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.TemporarilyQuotationID, False).ToString

                        sMsg &= "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & Starttext & "</div>"
                        sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>A new temporary technical offer has been created.<br>"
                        sMsg &= mailTest2 & "<b>" & sT & "</b><br>"
                        sMsg &= "This offer is Valid for " & ow & ".<br>"
                        sMsg &= mailTest4 & " <a href='https://iquote.ssl.imc-companies.com'>login.</a><br></div>"

                        Subject = "iQuote technical Quotation Information. quotation serial number: " & sT
                        body = sMsg
                    Else
                        Dim sT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False).ToString
                        Dim sMsg As String = ""
                        sMsg &= "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & Starttext & "</div>"
                        sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & mailTest3 & ": <b>" & Format(CInt(sT), "0000000").ToString & "</b>"
                        sMsg &= "<br>" & mailTest4 & " <a href='https://iquote.ssl.imc-companies.com'>login.</a><br></div>"
                        body = sMsg
                        Subject = mailTest0 & ": " & Format(CInt(sT), "0000000").ToString

                    End If

                    clsMail.SendEmailWithoutAttachment(MailAddress.ToString.Trim, Subject, body, True, True, ServerMapPath, True, sFilesToSend, BranchCode, Qutno, "")

                End If
            End If

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SendDrawing", "Function-SendDrawing : ex_Message=" & ex.Message, BranchCode, Qutno, "", "")

            Throw

        End Try

    End Sub

    Public Shared Sub SendOrder(MailAddress As String, Qutno As String, BranchCode As String, sFileToSend As String, sFilesToSend As List(Of String), ServerMapPath As String, phoneNo As String, OrderQty As String, sNetPrice As String)
        Try

            Dim sMsg As String = ""

            If MailAddress <> "" AndAlso MailAddress.Contains("@") Then

                Dim body As String = ""
                Dim Subject As String = ""

                '--------------For Customer-------------
                ' Dim langID As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.qut_LanguageId, False)
                Dim dtMessage As DataTable = clsLanguage.Get_LanguageMessagesCaption(clsLanguage.e_Form.Order)

                Dim txt0 As String = dtMessage.Rows(0).Item("MESSAGE").ToString
                Dim txt1 As String = dtMessage.Rows(1).Item("MESSAGE").ToString
                Dim txt2 As String = dtMessage.Rows(2).Item("MESSAGE").ToString
                Dim txt3 As String = dtMessage.Rows(3).Item("MESSAGE").ToString

                'Dim txt0 As String = "iQuote Order Submitted - Quotation Number:" & " "
                'Dim txt1 As String = "The local subsidiary has received your request, and your order is being processed."
                'Dim txt2 As String = "You will be notified when your order has been confirmed."
                'Dim txt3 As String = "Dear Customer,"
                'If BranchCode = "IG" Then
                '    txt0 = "Ihre iQuote-Bestellung – Angebotsnummer:" & " "
                '    txt1 = "wir bestätigen den Erhalt Ihrer Bestellung auf Basis der o.g. Angebotsnummer. Unmittelbar nach Einsteuerung in den Produktionsprozess, erhalten."
                '    txt2 = "Sie eine weitere Bestätigung mit zusätzlichen Angaben."
                '    txt3 = "Sehr geehrter Kunde,"
                'End If

                Dim sT As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.AS400Number, False).ToString

                sMsg &= "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & txt3 & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt1 & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txt2 & "</div>"
                body = sMsg
                Subject = txt0 & Format(CInt(sT), "0000000").ToString
                clsMail.SendEmailWithoutAttachment(MailAddress.ToString.Trim, Subject, body, True, True, ServerMapPath, True, sFilesToSend, BranchCode, Qutno, "ORDER")

                '--------------For Team-------------

                Dim dtMessage2 As DataTable = clsLanguage.Get_LanguageMessagesCaption(clsLanguage.e_Form.Order2)

                Dim txtN0 As String = dtMessage2.Rows(0).Item("MESSAGE").ToString
                Dim txtN1 As String = dtMessage2.Rows(1).Item("MESSAGE").ToString
                Dim txtN2 As String = dtMessage2.Rows(2).Item("MESSAGE").ToString
                Dim txtN3 As String = dtMessage2.Rows(3).Item("MESSAGE").ToString
                Dim txtN4 As String = dtMessage2.Rows(4).Item("MESSAGE").ToString
                Dim txtN5 As String = dtMessage2.Rows(5).Item("MESSAGE").ToString
                Dim txtN6 As String = dtMessage2.Rows(6).Item("MESSAGE").ToString
                Dim txtN7 As String = dtMessage2.Rows(7).Item("MESSAGE").ToString
                Dim txtN8 As String = dtMessage2.Rows(8).Item("MESSAGE").ToString
                'Dim txtN9 As String = dtMessage2.Rows(9).Item("MESSAGE").ToString
                'Dim txtN10 As String = dtMessage2.Rows(10).Item("MESSAGE").ToString
                'Dim txtN11 As String = dtMessage2.Rows(11).Item("MESSAGE").ToString
                Dim txtN12 As String = dtMessage2.Rows(11).Item("MESSAGE").ToString
                Dim txtN13 As String = dtMessage2.Rows(12).Item("MESSAGE").ToString
                Dim txtN14 As String = dtMessage2.Rows(13).Item("MESSAGE").ToString
                Dim txtN15 As String = dtMessage2.Rows(14).Item("MESSAGE").ToString

                Dim cn As String = StateManager.GetValue(StateManager.Keys.s_CustomerName, False)
                Dim ce As String = clsQuatation.ACTIVE_UseLoggedEmail
                Dim cg As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False)
                Dim qn As String = Format(CInt(sT), "0000000").ToString
                Dim itd As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)
                ' Dim oqe As String = ""
                Dim qdt As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Delivery_Weeks, False)

                Dim cor1 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, False)
                Dim cor2 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerItemNumber, False)
                Dim cor3 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, False)

                sMsg = "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & txtN0 & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN1 & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>&nbsp;</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN2 & "(" & cn & ", " & ce & ", " & phoneNo & ")</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN3 & " : " & cg & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN4 & " : " & qn & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN5 & " : " & itd & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN6 & " : " & OrderQty & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN7 & " : " & sNetPrice & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN8 & " : " & qdt & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN13 & " : " & cor1 & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN14 & " : " & cor2 & "</div>"
                sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN15 & " : " & cor3 & "</div>"

                body = sMsg
                Subject = txtN12 & " : " & Format(CInt(sT), "0000000").ToString

                clsMail.SendEmailWithoutAttachment(MailAddress.ToString.Trim, Subject, body, True, True, ServerMapPath, True, sFilesToSend, BranchCode, qn, "ORDER_CS")
            End If

        Catch ex As Exception

            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SendDrawing", "Function-SendDrawing : ex_Message=" & ex.Message, BranchCode, Qutno, "", "")

            Throw

        End Try

        'Dim txtN0 As String = "Dear Iscar Customer Service Team,"
        'Dim txtN1 As String = "We have got a new order through Iscar iQuote Web App, which needs to be manually filled-in in your system and thereafter issue order confirmation."
        'Dim txtN2 As String = "customer details"
        'Dim txtN3 As String = "customer Gal Accountr No"
        'Dim txtN4 As String = "Quotation Number"
        'Dim txtN5 As String = "Item desc."
        'Dim txtN6 As String = "Order Qty."
        'Dim txtN7 As String = "Order Qty.end user price"
        'Dim txtN8 As String = "Quotation delivery time"
        'Dim txtN9 As String = "Customer Order Number"
        'Dim txtN11 As String = "Remarks"
        'Dim txtN12 As String = "iQuote Order Submitted - Quotation Number"
        'Dim txtN13 As String = "Customer Purchase Number"
        'Dim txtN14 As String = "Customer Material / Item"
        'Dim txtN15 As String = "Remarks"
        'If BranchCode = "IG" Then
        '    txtN0 = "Sehr geehrtes ISCAR Customer Service Team,"
        '    txtN1 = "wir haben eine neue Bestellung über die ISCAR iQuote-Web-App erhalten. Bitte pfelgen Sie die Bestellung manuell in Ihr GAL System ein, anbei die Bestelldetails."
        '    txtN2 = "Kundendetails"
        '    txtN3 = "Kunde Gal Kontonummer"
        '    txtN4 = "Nummer des Angebots"
        '    txtN5 = "Artikelbeschreibung"
        '    txtN6 = "Bestellmenge"
        '    txtN7 = "Bestellmenge, Endbenutzerpreis"
        '    txtN8 = "Angebotslieferzeit"
        '    txtN9 = "Angebotslieferzeit"
        '    txtN11 = "Bemerkungen"
        '    txtN12 = "Neue iQuote-Bestellung – Angebotsnummer"
        '    txtN13 = "Kundenbestellnummer"
        '    txtN14 = "Kundenmaterial/Artikelnummer"
        '    txtN15 = "Bemerkungen"
        'End If
        'Dim cn As String = StateManager.GetValue(StateManager.Keys.s_CustomerName, False)
        'Dim ce As String = clsQuatation.ACTIVE_UseLoggedEmail
        'Dim cg As String = StateManager.GetValue(StateManager.Keys.s_CustomerNumber, False)
        'Dim qn As String = Format(CInt(sT), "0000000").ToString
        'Dim itd As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.SemiToolDescription, False)
        '' Dim oqe As String = ""
        'Dim qdt As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.Delivery_Weeks, False)

        'Dim cor1 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerOrderNumber, False)
        'Dim cor2 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerItemNumber, False)
        'Dim cor3 As String = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.PricesCustomerAdditionalReq, False)

        'sMsg = "<div style='font-family: Oswald,Calibri,Arial,Script;font-size: 16px'>" & txtN0 & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN1 & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>&nbsp;</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN2 & "(" & cn & ", " & ce & ", " & phoneNo & ")</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN3 & " : " & cg & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN4 & " : " & qn & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN5 & " : " & itd & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN6 & " : " & OrderQty & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN7 & " : " & sNetPrice & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN8 & " : " & qdt & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN13 & " : " & cor1 & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN14 & " : " & cor2 & "</div>"
        'sMsg &= "<div style='font-family: Roboto-Regular !important,Calibri,Arial,Script;font-size: 16px'>" & txtN15 & " : " & cor3 & "</div>"

    End Sub
    Private Sub sendmail_Thread()
        Try
            Dim tMail As MailMessage = mailMsg
            Dim cl As New SmtpClient(ConfigurationManager.AppSettings("MailHost"))
            cl.Send(tMail)
            tMail = Nothing
            cl = Nothing
        Catch ex As Exception
            'GeneralException.WriteEventErrors("Function-sendmail_Thread : ex_Message=" & ex.Message, GeneralException.e_LogTitle.EMAIL)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "sendmail_Thread", "ex_Message=" & ex.Message, "", "", "", "")
        End Try



    End Sub


    Public Shared Sub SendEmailWithoutAttachment(MailTo As String, Subject As String, body As String, DoTheCheckIfCanSendMail As Boolean, bAlternateView As Boolean, SMServer As String, bWithDocs As Boolean, sFilesToSend As List(Of String), BranchCode As String, QutNo As String, MailType As String)

        Try
            Dim sSendMail As String = ConfigurationManager.AppSettings("SendMail_ACTIVE")
            'If DoTheCheckIfCanSendMail = False Then sSendMail = "YES"
            If sSendMail = "YES" Then
                Dim mail As New MailMessage
                mail.Subject = Subject
                mail.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("MailFrom"))



                If MailTo = "" Then
                    MailTo = ConfigurationManager.AppSettings("MailTo")
                End If

                If MailTo <> "" Then
                    If MailTo.Contains(";") Then
                        Dim s() As String = MailTo.Split(";")
                        For i As Integer = 0 To s.Length - 1
                            If s(i).ToString.Contains("@") Then
                                mail.To.Add(s(i).ToString.Trim)
                            End If
                        Next
                    Else
                        mail.To.Add(MailTo)
                    End If
                End If

                If (MailType = "CONTACTUS" Or MailType = "FEEDBACK") AndAlso BranchCode = "IG" Then
                    mail.To.Add(ConfigurationManager.AppSettings("MailTo1IG"))
                    mail.To.Add(ConfigurationManager.AppSettings("MailTo2IG"))
                End If

                Dim CcAddress As String = ConfigurationManager.AppSettings("MailCC")
                If MailType.ToString.ToUpper.Contains("ORDER") Then
                    If BranchCode = "IG" AndAlso MailType.ToString.ToUpper = "ORDER_CS" Then
                        mail.To.Clear()
                        MailTo = ""
                        Dim dtsm As DataTable = clsBranch.GetBranchDetails(BranchCode)
                        If Not dtsm Is Nothing AndAlso dtsm.Rows.Count > 0 Then
                            If dtsm.Rows(0).Item("CustomerServiceMail").ToString <> "" Then
                                MailTo = dtsm.Rows(0).Item("CustomerServiceMail").ToString
                                If MailTo.Contains(";") Then
                                    Dim s() As String = MailTo.Split(";")
                                    For i As Integer = 0 To s.Length - 1
                                        If s(i).ToString.Contains("@") Then
                                            mail.To.Add(s(i).ToString.Trim)
                                        End If
                                    Next
                                Else
                                    mail.To.Add(MailTo)
                                End If
                            End If
                        End If
                    End If
                    If MailTo = "" Then
                        mail.To.Add(ConfigurationManager.AppSettings("MailTO"))
                    End If
                    Dim ssalesP As String = StateManager.GetValue(StateManager.Keys.s_salespersonEmail, True).ToString
                    Dim sDeskP As String = StateManager.GetValue(StateManager.Keys.s_deskUserEmail, True).ToString
                    Dim aTec As String = StateManager.GetValue(StateManager.Keys.s_technicalPersonEmail, True).ToString
                    If ConfigurationManager.AppSettings("SendOrderMailForTestUsers") = "YES" Then
                        'ssalesP = "amitl@iscar.co.il"
                        'sDeskP = "reginat@iscar.co.il"
                        'aTec = "guyz@iscar.co.il"
                    End If
                    If Not ssalesP Is Nothing AndAlso ssalesP <> "" AndAlso ssalesP.Contains("@") Then
                        CcAddress = ssalesP & ";" & CcAddress
                    End If
                    If Not sDeskP Is Nothing AndAlso sDeskP <> "" AndAlso sDeskP.Contains("@") Then
                        CcAddress = sDeskP & ";" & CcAddress
                    End If
                    If Not aTec Is Nothing AndAlso aTec <> "" AndAlso aTec.Contains("@") Then
                        CcAddress = aTec & ";" & CcAddress
                    End If
                End If
                If CcAddress <> "" Then
                    If CcAddress.Contains(";") Then
                        Dim s() As String = CcAddress.Split(";")
                        For i As Integer = 0 To s.Length - 1
                            If s(i).ToString.Contains("@") Then
                                mail.CC.Add(s(i).ToString.Trim)
                            End If
                        Next
                    Else
                        mail.CC.Add(CcAddress)
                    End If
                End If

                Dim BBcAddress As String = ConfigurationManager.AppSettings("MailBCC")
                If BBcAddress <> "" Then
                    If BBcAddress.Contains(";") Then
                        Dim s1() As String = BBcAddress.Split(";")
                        For i1 As Integer = 0 To s1.Length - 1
                            If s1(i1).ToString.Contains("@") Then
                                mail.Bcc.Add(s1(i1).ToString.Trim)
                            End If
                        Next
                    Else
                        mail.Bcc.Add(BBcAddress)
                    End If
                End If

                If bWithDocs = True Then
                    Dim TempFolderFiles As String = ConfigurationManager.AppSettings("TempFilesFolderForMail")
                    Dim ss As Date = DateTime.Now
                    Dim ssss As String = ""

                    Dim folderPath As String = TempFolderFiles & BranchCode & QutNo & "\" & ssss & "\"

                    CreateFolderIfNotExists(folderPath)

                    For i As Integer = 0 To sFilesToSend.Count - 1
                        Try
                            Dim sFileName As String = System.IO.Path.GetFileName(sFilesToSend(i))
                            Dim sFileFolderPath As String = sFilesToSend(i).Replace("\" & sFileName, "")
                            Dim b() As Byte = Nothing
                            Documents.getDocument(sFileFolderPath, sFileName, BranchCode, b, clsBranch.ReturnActiveStorageFolderForDouuments)
                            Try
                                System.IO.File.WriteAllBytes(folderPath & sFileName, b)
                            Catch ex As Exception

                            End Try

                            mail.Attachments.Add(New Attachment(folderPath & sFileName))
                        Catch ex As Exception
                            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SendEmailWithoutAttachment", "ex_Message=" & ex.Message, "", "", "", "")

                        End Try
                    Next
                End If

                mail.IsBodyHtml = True
                If bAlternateView Then
                    body = "<p style='font-family:Oswald !important; font-size:15px !important;'>" & body & "</p>"
                    mail.AlternateViews.Add(Mail_Body(SMServer, body))
                Else
                    mail.Body = "<p style='font-family:Oswald !important; font-size:15px !important;'>" & body & "</p>"
                End If

                Dim sc As New clsMail
                sc.StartThreadSendMAil(mail, MailType, BranchCode)
                mail = Nothing
                sc = Nothing
            End If


        Catch ex As GeneralException
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.EMAIL)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SendEmailWithoutAttachment", "ex_Message=" & ex.Message, "", "", "", "")

            Throw
        Catch ex As Exception
            'GeneralException.WriteEventErrors(ex.Message, GeneralException.e_LogTitle.EMAIL)
            GeneralException.LogWriteEventSql(GeneralException.e_LogTitle.ERROR.ToString, "SendEmailWithoutAttachment", "ex_Message=" & ex.Message, "", "", "", "")

            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "Sending Mail failed")

        End Try

    End Sub

    Public Shared Sub CreateFolderIfNotExists(folderPath As String)
        If Not Directory.Exists(folderPath) Then
            Try
                Directory.CreateDirectory(folderPath)
                Console.WriteLine("Folder created successfully!")
            Catch ex As Exception
                ' Handle any exceptions that might occur during folder creation.
                Console.WriteLine("Error occurred while creating the folder: " & ex.Message)
            End Try
        Else
            Console.WriteLine("Folder already exists!")
        End If
    End Sub


    Public Sub StartThreadSendMAil(mailMsgs As MailMessage, MailType As String, BranchCode As String)

        mailMsg = mailMsgs
        ' sendmail_Thread
        AddMailToSql(MailType, mailMsgs.Subject, mailMsgs.Body, BranchCode, mailMsgs.To.ToString, mailMsgs.CC.ToString, mailMsgs.Bcc.ToString)
        Dim ThreadMail As Thread = New Thread(AddressOf sendmail_Thread)
        Try
            ThreadMail.IsBackground = True
            ThreadMail.Start()
        Catch ex As Threading.ThreadAbortException
            ThreadMail.Abort()
        Catch ex As Exception
            ThreadMail.Abort()
        End Try
    End Sub



    Public Shared Sub SendEmailError(Subject As String, body As String, CatalogError As Boolean, MailType As String, BranchCode As String)

        Try
            Dim sSendMail As String = ConfigurationManager.AppSettings("SendMailError_ACTIVE")
            If sSendMail = "YES" Then

                Dim bc As String = ""
                Dim le As String = ""
                Try
                    bc = SessionManager.GetQuotationValues(SessionManager.ActiveQuotation.BranchCode, False)
                    le = StateManager.GetValue(StateManager.Keys.s_loggedEmail, True)
                Catch ex As Exception
                End Try

                Dim CcAddress As String = ConfigurationManager.AppSettings("Mail_ErrorCC")
                Dim ToAddress As String = ConfigurationManager.AppSettings("Mail_ErrorTo")
                Dim mail As New MailMessage

                mail.Subject = Subject
                mail.Body = "Branch Code: " & bc & "<br>" & "Logged Email: " & le & "<br><br><br>" & body
                mail.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("MailFrom"))

                If ToAddress <> "" AndAlso ToAddress.Contains(";") Then
                    Dim s() As String = ToAddress.Split(";")
                    For i As Integer = 0 To s.Length - 1
                        If s(i).ToString.Contains("@") Then
                            mail.To.Add(s(i).ToString.Trim)
                        End If
                    Next
                ElseIf ToAddress <> "" Then
                    mail.To.Add(ToAddress)
                End If

                If CcAddress <> "" AndAlso CcAddress.Contains(";") Then
                    Dim s() As String = CcAddress.Split(";")
                    For i As Integer = 0 To s.Length - 1
                        If s(i).ToString.Contains("@") Then
                            mail.CC.Add(s(i).ToString.Trim)
                        End If
                    Next
                ElseIf CcAddress <> "" Then
                    mail.To.Add(CcAddress)
                End If

                If CatalogError Then
                    mail.CC.Add("monae@iscar.co.il")
                End If

                mail.IsBodyHtml = True
                Dim cl As New SmtpClient(ConfigurationManager.AppSettings("MailHost"))
                AddMailToSql(MailType, Subject.ToString, mail.Body.ToString, BranchCode, mail.To.ToString, mail.CC.ToString, mail.Bcc.ToString)
                cl.Send(mail)
                mail = Nothing
            End If

        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "Sending Error Mail failed")

        End Try

    End Sub

    Public Shared Sub Send_iQuoteError(Subject As String, body As String, BranchCode As String, MailType As String)


        Try
            Dim sSendMail As String = ConfigurationManager.AppSettings("Send_iQuoteError")
            If sSendMail = "YES" Then

                Dim ToAddress As String = ConfigurationManager.AppSettings("iQuoteError_TO")
                Dim mail As New MailMessage

                mail.Subject = Subject
                mail.Body = body
                mail.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("MailFrom"))

                If ToAddress <> "" AndAlso ToAddress.Contains(";") Then
                    Dim s() As String = ToAddress.Split(";")
                    For i As Integer = 0 To s.Length - 1
                        If s(i).ToString.Contains("@") Then
                            mail.To.Add(s(i).ToString.Trim)
                        End If
                    Next
                ElseIf ToAddress <> "" Then
                    mail.To.Add(ToAddress)
                End If

                If ConfigurationManager.AppSettings("IsDebugMode").ToString.ToUpper = "FALSE" Then

                    mail.IsBodyHtml = True
                    AddMailToSql(MailType, Subject.ToString, mail.Body.ToString, BranchCode, mail.To.ToString, mail.CC.ToString, mail.Bcc.ToString)

                    Dim cl As New SmtpClient(ConfigurationManager.AppSettings("MailHost"))

                    cl.Send(mail)
                    mail = Nothing

                    'mail.IsBodyHtml = True
                    'Dim sc As New clsMail
                    'sc.StartThreadSendMAil(mail)
                    'mail = Nothing
                    'sc = Nothing

                End If
            End If


        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "Sending Error Mail failed")

        End Try

    End Sub

    Public Shared Sub SendEmailErrorCatiaDrawing(Subject As String, body As String, params As String, BranchCode As String, LoggedEmail As String)
        Try
            Dim sSendMail As String = ConfigurationManager.AppSettings("SendMail_ACTIVE")
            If sSendMail = "YES" Then


                Dim sBody As String = "<table><tr><td>Branch Code: " & BranchCode & "<br>Logged Email: " & LoggedEmail & "<br></td></tr><tr><td>" & body & "</td></tr></table><br>"
                params = params.Replace("<Param Name=", "Param Name=").Replace("><Param Value=", "-Param Value=").Replace(">", "<br>")

                Dim mail As New MailMessage

                mail.Subject = Subject
                mail.Body = sBody & "<br>" & params
                mail.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("MailFrom"))

                Get_To_Mail_Error(mail)
                Get_CC_Mail(mail)

                mail.IsBodyHtml = True

                Dim sc As New clsMail
                sc.StartThreadSendMAil(mail, GeneralException.e_LogTitle.MAIL_ERROR, BranchCode)
                mail = Nothing
                sc = Nothing
            End If

        Catch ex As GeneralException
            Throw
        Catch ex As Exception
            Throw New GeneralException(GeneralException.ErrorMessages.General_ErrMsg.ToString, ex, "Sending Error Mail failed")

        End Try

    End Sub
    Public Shared Sub Get_To_Mail_Error(ByRef N_Mail As MailMessage)
        Try
            Dim ToAddress As String = ConfigurationManager.AppSettings("Mail_DrawingErrorTO")
            If ToAddress <> "" AndAlso ToAddress.Contains(";") Then
                Dim s() As String = ToAddress.Split(";")
                For i As Integer = 0 To s.Length - 1
                    If s(i).ToString.Contains("@") Then
                        N_Mail.To.Add(s(i).ToString.Trim)
                    End If
                Next
            Else
                N_Mail.To.Add(ToAddress)
            End If
        Catch ex As GeneralException
            Throw
        End Try
    End Sub
    Public Shared Sub Get_CC_Mail(ByRef N_Mail As MailMessage)
        Try
            Dim CcAddress As String = ConfigurationManager.AppSettings("Mail_ErrorCC")
            If CcAddress <> "" AndAlso CcAddress.Contains(";") Then
                Dim s() As String = CcAddress.Split(";")
                For i As Integer = 0 To s.Length - 1
                    If s(i).ToString.Contains("@") Then
                        N_Mail.CC.Add(s(i).ToString.Trim)
                    End If
                Next
            Else
                N_Mail.To.Add(CcAddress)
            End If
        Catch ex As GeneralException
            Throw
        End Try
    End Sub

    Public Shared Function Mail_Body(SMapPath As String, sbody As String) As AlternateView
        Try
            Dim path As String = SMapPath & "\media\Images\MailLogoD.jpg"
            Dim Img As LinkedResource = New LinkedResource(path, MediaTypeNames.Image.Jpeg)
            Img.ContentId = "MyImage"
            Dim str As String = "  
            <table>  
                <tr>  
                    <td>" & sbody & "
                    </td>  
                </tr>  
                <tr>  
                    <td>  
                      <img src=cid:MyImage  id='img' alt='' />   
                    </td>  
                </tr></table>
            "
            Dim AV As AlternateView = AlternateView.CreateAlternateViewFromString(str, Nothing, MediaTypeNames.Text.Html)
            AV.LinkedResources.Add(Img)
            Return AV
        Catch ex As GeneralException
            Throw
        End Try

    End Function

    Public Shared Sub AddMailToSql(Mail_Type As String, Mail_Subject As String, Mail_Body As String, BranchCode As String, Mail_To As String, CC As String, BCC As String)
        Try

            Dim oTxSql As SqlDal
            oTxSql = New SqlDal(clsBranch.GetSiteConnectionString)

            Dim oParamsD As New SqlParams()
            oParamsD.Add(New SqlParam("@Mail_Type", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Mail_Type.ToString.Substring(0, Math.Min(149, Mail_Type.ToString.Length))))
            oParamsD.Add(New SqlParam("@Mail_Subject", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Mail_Subject.ToString.Substring(0, Math.Min(249, Mail_Subject.ToString.Length))))
            oParamsD.Add(New SqlParam("@Mail_Body", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Mail_Body.ToString.Substring(0, Math.Min(499, Mail_Body.ToString.Length))))
            oParamsD.Add(New SqlParam("@BranchCode", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BranchCode.ToString.Substring(0, Math.Min(2, BranchCode.ToString.Length))))
            oParamsD.Add(New SqlParam("@Mail_To", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, Mail_To.ToString.Substring(0, Math.Min(249, Mail_To.ToString.Length))))
            oParamsD.Add(New SqlParam("@CC", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, CC.ToString.Substring(0, Math.Min(49, CC.ToString.Length))))
            oParamsD.Add(New SqlParam("@BCC", SqlParam.ParamType.ptVarchar, SqlParam.ParamDirection.pdInput, BCC.ToString.Substring(0, Math.Min(49, BCC.ToString.Length))))

            oTxSql.ExecuteSP("USP_EmailData", oParamsD)

            oParamsD = Nothing
            oTxSql = Nothing

        Catch ex As Exception
            GeneralException.whriteSqlExiption(GeneralException.e_LogTitle.MAIL_ERROR, "AddMailToSql", ex.Message, False)
        End Try
    End Sub
End Class
