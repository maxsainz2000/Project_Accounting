Imports Microsoft.VisualBasic.ApplicationServices

Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_ApplyApplicationDefaults(sender As Object, e As ApplyApplicationDefaultsEventArgs) Handles Me.ApplyApplicationDefaults
            Dim fontName As String = "Segoe UI"
            Try
                Using fam As New FontFamily("Segoe UI Variable Text")
                    fontName = "Segoe UI Variable Text"
                End Using
            Catch
                ' Fallback to universal Segoe UI
            End Try

            e.Font = New Font(fontName, 9.5F, FontStyle.Regular, GraphicsUnit.Point)
            e.HighDpiMode = HighDpiMode.PerMonitorV2
            e.ColorMode = SystemColorMode.System
        End Sub
    End Class
End Namespace
