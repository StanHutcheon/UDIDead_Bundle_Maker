Imports Ionic.Zip
Module Module1
    Public build As String = ""
    Public firmwareversion As String = ""
    Public outputdirectory As String = ""
    Public CommandLine As Boolean
    Const quote As String = """"

    Sub Main()
        Console.Title = "UDIDead Bundle Maker rev2"
        Dim a_strArgs() As String

        Dim i As Integer

        a_strArgs = Split(Command$, " ")
        For i = LBound(a_strArgs) To UBound(a_strArgs)
            Select Case LCase(a_strArgs(i))
                Case "-h"
                    Console.WriteLine("usage:" + vbNewLine + "-b <build number>" + vbNewLine + "-f <firmware version>" + vbNewLine + "optional:" + vbNewLine + "-o <output directory>")
                    Exit Sub
                Case "-b"
                    If i = UBound(a_strArgs) Then
                        CommandLine = False
                    Else
                        i = i + 1
                    End If
                    If Left(a_strArgs(i), 1) = "-" Then
                        MsgBox("Invalid string.")
                    Else
                        build = a_strArgs(i)
                        CommandLine = True
                    End If
                Case "-f"
                    If i = UBound(a_strArgs) Then
                        CommandLine = False
                    Else
                        i = i + 1
                    End If
                    If Left(a_strArgs(i), 1) = "-" Then
                        MsgBox("Invalid argument.")
                    Else
                        firmwareversion = a_strArgs(i)
                    End If
                Case "-o"
                    If i = UBound(a_strArgs) Then

                    Else
                        i = i + 1
                    End If
                    If Left(a_strArgs(i), 1) = "-" Then
                        MsgBox("Invalid argument.")
                        Exit Sub
                    Else
                        outputdirectory = a_strArgs(i)
                        CommandLine = True
                    End If
                Case Else

            End Select

        Next
        Everything()
    End Sub
    Public Sub Everything()
        Console.WriteLine("UDIDead bundle maker")
        Console.WriteLine(" ")
        If CommandLine = False Then
            Do
                Console.WriteLine("Enter build number for required iOS version (e.g 9A5313e or 8H7 ect.):")
                build = Console.ReadLine()
            Loop Until Not build = ""
            Do
                Console.WriteLine("Enter firmware version for required iOS version (e.g 5.0 or 4.3.3 ect.):")
                firmwareversion = Console.ReadLine()
            Loop Until Not build = ""
        Else
            Console.WriteLine("Build Number: " + build)
            Console.WriteLine("Firmware Version: " + firmwareversion)
            Console.WriteLine(" ")
        End If
        Console.WriteLine("Creating files...")
        Dim SystemVersionplist As String = "<?xml version=" + quote + "1.0" + quote + " encoding=" + quote + "UTF-8" + quote + "?>" + vbNewLine + "<!DOCTYPE plist PUBLIC " + quote + "-//Apple//DTD PLIST 1.0//EN" + quote + " " + quote + "http://www.apple.com/DTDs/PropertyList-1.0.dtd" + quote + ">" + vbNewLine + "<plist version=" + quote + "1.0" + quote + ">" + vbNewLine + "<dict>" + vbNewLine + "<key>ProductBuildVersion</key>" + vbNewLine + "<string>" + build + "</string>" + vbNewLine + "<key>ProductCopyright</key>" + vbNewLine + "<string>1983-2011 Apple Inc.</string>" + vbNewLine + "<key>ProductName</key>" + vbNewLine + "<string>iPhone OS</string>" + vbNewLine + "<key>ProductVersion</key>" + vbNewLine + "<string>" + firmwareversion + "</string>" + vbNewLine + "</dict>" + vbNewLine + "</plist>"
        System.IO.File.WriteAllText(My.Computer.FileSystem.SpecialDirectories.Temp & "\SystemVersion.plist", SystemVersionplist)
        Console.WriteLine("Zipping it up...")
        Using zip1 As Ionic.Zip.ZipFile = New ZipFile
            zip1.AddDirectoryByName("\System")
            zip1.AddDirectoryByName("\System\Library")
            zip1.AddDirectoryByName("\System\Library\CoreServices")
            zip1.AddFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\SystemVersion.plist", "/System/Library/CoreServices/")
            zip1.Save(My.Computer.FileSystem.SpecialDirectories.Temp + "\UDIDeadBundle.tar")
        End Using
        Using zip2 As Ionic.Zip.ZipFile = New ZipFile
            zip2.AddFile(My.Computer.FileSystem.SpecialDirectories.Temp + "\UDIDeadBundle.tar", "\")
            If outputdirectory = "" Then
                zip2.Save(My.Application.Info.DirectoryPath + "\UDIDeadBundle_" + firmwareversion + "_" + build + ".tar.gz")
            Else
                zip2.Save(outputdirectory + "\UDIDeadBundle_" + firmwareversion + "_" + build + ".tar.gz")
            End If
        End Using
        Console.WriteLine("Your newly created UDIDead bundle is located at: " + outputdirectory + "\UDIDeadBundle_" + firmwareversion + "_" + build + ".tar.gz")
        Console.WriteLine("Press any key to continue...")
        Console.ReadLine()
    End Sub
End Module
