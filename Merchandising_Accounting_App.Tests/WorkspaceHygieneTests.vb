Option Strict On
Option Explicit On
Option Compare Binary

Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace Merchandising_Accounting_App.Tests

    <TestClass>
    Public Class WorkspaceHygieneTests

        Private Function GetRepositoryRoot() As String
            Dim currentDir As New DirectoryInfo(AppContext.BaseDirectory)
            While currentDir IsNot Nothing
                If File.Exists(Path.Combine(currentDir.FullName, "Merchandising_Accounting_App.slnx")) Then
                    Return currentDir.FullName
                End If
                currentDir = currentDir.Parent
            End While
            Throw New DirectoryNotFoundException("Could not locate repository root containing Merchandising_Accounting_App.slnx")
        End Function

        <TestMethod>
        Public Sub RootMarkdownInvariant()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim mdFiles As String() = Directory.GetFiles(repoRoot, "*.md", SearchOption.TopDirectoryOnly)

            Dim unexpectedFiles As New List(Of String)()
            For Each filePath As String In mdFiles
                Dim fileName As String = Path.GetFileName(filePath)
                If Not String.Equals(fileName, "GEMINI.md", StringComparison.Ordinal) Then
                    unexpectedFiles.Add(fileName)
                End If
            Next

            Assert.AreEqual(0, unexpectedFiles.Count,
                $"Unauthorized markdown file(s) found in repository root: {String.Join(", ", unexpectedFiles)}. " &
                "GEMINI.md must be the only markdown file in repository root per GEMINI.md Section 1.2.")
            Assert.IsTrue(File.Exists(Path.Combine(repoRoot, "GEMINI.md")), "GEMINI.md must exist in repository root.")
        End Sub

        <TestMethod>
        Public Sub RuleFileSizeInvariant()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim geminiPath As String = Path.Combine(repoRoot, "GEMINI.md")
            Assert.IsTrue(File.Exists(geminiPath), "GEMINI.md must exist in repository root.")

            Dim fileInfo As New FileInfo(geminiPath)
            Const MaxAllowedBytes As Long = 24000L

            Assert.IsTrue(fileInfo.Length <= MaxAllowedBytes,
                $"GEMINI.md size ({fileInfo.Length} bytes) exceeds the hard limit of {MaxAllowedBytes} bytes " &
                "(Antigravity agy-customizations limit before line truncation).")
        End Sub

        <TestMethod>
        Public Sub ActiveDocumentationLinksInvariant()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim docsToScan As String() = {
                Path.Combine(repoRoot, "GEMINI.md"),
                Path.Combine(repoRoot, "docs", "README.md")
            }

            Dim allMissingTargets As New List(Of String)()

            For Each docPath As String In docsToScan
                Assert.IsTrue(File.Exists(docPath), $"Documentation file to scan does not exist: {docPath}")
                Dim missing As List(Of String) = ScanDocumentationLinks(docPath, repoRoot)
                allMissingTargets.AddRange(missing)
            Next

            Assert.AreEqual(0, allMissingTargets.Count,
                $"Dead documentation links detected:{Environment.NewLine}{String.Join(Environment.NewLine, allMissingTargets)}")
        End Sub

        <TestMethod>
        Public Sub ActivePlansInvariant()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim plansDir As String = Path.Combine(repoRoot, "docs", "plans")

            Dim unarchivedCompletedPlans As List(Of String) = ScanPlansForTerminalStatus(plansDir, repoRoot)

            Assert.AreEqual(0, unarchivedCompletedPlans.Count,
                $"Completed or merged plan(s) found in active plans directory 'docs/plans/':{Environment.NewLine}" &
                $"{String.Join(Environment.NewLine, unarchivedCompletedPlans)}{Environment.NewLine}" &
                "Per GEMINI.md Section 1.2, completed or merged plans must be moved to 'docs/archive/plans/YYYY-MM-DD_<name>/'.")
        End Sub

        Friend Shared Function ScanPlansForTerminalStatus(plansDirectory As String, repoRoot As String) As List(Of String)
            Dim results As New List(Of String)()

            If Not Directory.Exists(plansDirectory) Then
                Return results
            End If

            Dim planFiles As String() = Directory.GetFiles(plansDirectory, "*.md", SearchOption.AllDirectories)
            If planFiles.Length = 0 Then
                Return results
            End If

            Dim completedStatusRegex As New Regex("(?i)\bStatus\b[\s\*_]*[:|=][\s\*" & """" & "'`\[]*(Completed|Merged)\b", RegexOptions.Compiled)

            For Each planFilePath As String In planFiles
                Dim content As String = File.ReadAllText(planFilePath)
                Dim match As Match = completedStatusRegex.Match(content)
                If match.Success Then
                    Dim relativePath As String = Path.GetRelativePath(repoRoot, planFilePath)
                    results.Add($"{relativePath} (matched: '{match.Value.Trim()}')")
                End If
            Next

            Return results
        End Function

        Friend Shared Function ScanDocumentationLinks(docPath As String, repoRoot As String) As List(Of String)
            Dim missingTargets As New List(Of String)()
            Dim content As String = File.ReadAllText(docPath)
            Dim baseDir As String = Path.GetDirectoryName(docPath)
            If String.IsNullOrEmpty(baseDir) Then
                baseDir = repoRoot
            End If

            Dim linkRegex As New Regex("!?\[(?<text>[^\]]*)\]\((?<target>[^)]+)\)", RegexOptions.Compiled)
            Dim matches As MatchCollection = linkRegex.Matches(content)

            For Each match As Match In matches
                Dim rawTarget As String = match.Groups("target").Value.Trim()
                Dim linkText As String = match.Groups("text").Value

                ' Handle angle bracket destinations: [text](<path with spaces>)
                Dim targetUrl As String
                If rawTarget.StartsWith("<"c) AndAlso rawTarget.Contains(">"c) Then
                    Dim closingAngle As Integer = rawTarget.IndexOf(">"c)
                    targetUrl = rawTarget.Substring(1, closingAngle - 1).Trim()
                Else
                    ' Strip optional markdown title: [text](path "optional title")
                    Dim targetTokens As String() = rawTarget.Split(New Char() {" "c, ControlChars.Tab}, StringSplitOptions.RemoveEmptyEntries)
                    If targetTokens.Length = 0 Then
                        Continue For
                    End If
                    targetUrl = targetTokens(0).Trim()
                End If

                If String.IsNullOrWhiteSpace(targetUrl) Then
                    Continue For
                End If

                ' Skip in-document anchors
                If targetUrl.StartsWith("#", StringComparison.Ordinal) Then
                    Continue For
                End If

                ' Check for absolute URIs
                Dim parsedUri As Uri = Nothing
                If Uri.TryCreate(targetUrl, UriKind.Absolute, parsedUri) Then
                    If parsedUri.IsFile Then
                        Dim localFilePath As String = parsedUri.LocalPath
                        If Not (File.Exists(localFilePath) OrElse Directory.Exists(localFilePath)) Then
                            missingTargets.Add($"In '{Path.GetFileName(docPath)}': link '[{linkText}]({targetUrl})' -> unresolved target: '{localFilePath}'")
                        End If
                    End If
                    ' Skip non-file absolute URIs (http, https, mailto, etc.)
                    Continue For
                End If

                ' Relative path processing
                Dim filePathPart As String = targetUrl

                ' Strip intra-file anchor (#...) if present
                Dim hashIdx As Integer = filePathPart.IndexOf("#"c)
                If hashIdx >= 0 Then
                    filePathPart = filePathPart.Substring(0, hashIdx)
                End If

                ' Strip query parameters (?...) if present
                Dim queryIdx As Integer = filePathPart.IndexOf("?"c)
                If queryIdx >= 0 Then
                    filePathPart = filePathPart.Substring(0, queryIdx)
                End If

                If String.IsNullOrWhiteSpace(filePathPart) Then
                    Continue For
                End If

                ' Unescape URL encoding if any (e.g. %20 -> space)
                Dim cleanRelativePath As String = Uri.UnescapeDataString(filePathPart)

                ' Resolve path against the document's directory
                Dim combinedPath As String = Path.Combine(baseDir, cleanRelativePath)
                Dim resolvedFullPath As String = Path.GetFullPath(combinedPath)

                Dim exists As Boolean = File.Exists(resolvedFullPath) OrElse Directory.Exists(resolvedFullPath)
                If Not exists Then
                    missingTargets.Add($"In '{Path.GetFileName(docPath)}': link '[{linkText}]({targetUrl})' -> unresolved target: '{resolvedFullPath}'")
                End If
            Next

            Return missingTargets
        End Function

        <TestMethod>
        Public Sub Test_ActivePlansStatusRegex_MatchesAllTerminalStatusStyles()
            Dim completedStatusRegex As New Regex("(?i)\bStatus\b[\s\*_]*[:|=][\s\*" & """" & "'`\[]*(Completed|Merged)\b", RegexOptions.Compiled)

            ' Positive cases (must match)
            Assert.IsTrue(completedStatusRegex.IsMatch("Status: Completed"))
            Assert.IsTrue(completedStatusRegex.IsMatch("Status: Merged"))
            Assert.IsTrue(completedStatusRegex.IsMatch("status: completed"))
            Assert.IsTrue(completedStatusRegex.IsMatch("STATUS: MERGED"))
            Assert.IsTrue(completedStatusRegex.IsMatch("**Status**: Completed"))
            Assert.IsTrue(completedStatusRegex.IsMatch("**Status:** Completed"))
            Assert.IsTrue(completedStatusRegex.IsMatch("Status: ""Completed"""))
            Assert.IsTrue(completedStatusRegex.IsMatch("Status: 'Merged'"))
            Assert.IsTrue(completedStatusRegex.IsMatch("Status: `Completed`"))
            Assert.IsTrue(completedStatusRegex.IsMatch("Status: [Completed]"))
            Assert.IsTrue(completedStatusRegex.IsMatch("| Status | Completed |"))
            Assert.IsTrue(completedStatusRegex.IsMatch("| **Status** | Merged |"))
            Assert.IsTrue(completedStatusRegex.IsMatch("Status = Completed"))
            Assert.IsTrue(completedStatusRegex.IsMatch("## Metadata" & Environment.NewLine & "Status: Completed"))

            ' Negative cases (must NOT match)
            Assert.IsFalse(completedStatusRegex.IsMatch("Status: In Progress"))
            Assert.IsFalse(completedStatusRegex.IsMatch("Status: Draft"))
            Assert.IsFalse(completedStatusRegex.IsMatch("Status: Active"))
            Assert.IsFalse(completedStatusRegex.IsMatch("Status: Under Review"))
            Assert.IsFalse(completedStatusRegex.IsMatch("Status: Blocked"))
            Assert.IsFalse(completedStatusRegex.IsMatch("Status: Cancelled"))
            Assert.IsFalse(completedStatusRegex.IsMatch("| Status | In Progress |"))
            Assert.IsFalse(completedStatusRegex.IsMatch("The status of the project is unknown."))
        End Sub

        <TestMethod>
        Public Sub ActivePlansInvariant_DetectsCompletedStatusInIsolatedTempDirectory()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim tempDir As String = Path.Combine(Path.GetTempPath(), "HygieneTest_" & Guid.NewGuid().ToString("N"))
            Directory.CreateDirectory(tempDir)

            Try
                Dim testPlanPath As String = Path.Combine(tempDir, "test_completed_plan.md")
                File.WriteAllText(testPlanPath, "# Synthetic Plan" & Environment.NewLine & "Status: Completed" & Environment.NewLine)

                Dim violations As List(Of String) = ScanPlansForTerminalStatus(tempDir, repoRoot)
                Assert.AreEqual(1, violations.Count, "ScanPlansForTerminalStatus must detect completed status.")
                Assert.IsTrue(violations(0).Contains("test_completed_plan.md"), "Violation must name the offending file.")
            Finally
                If Directory.Exists(tempDir) Then
                    Directory.Delete(tempDir, True)
                End If
            End Try
        End Sub

        <TestMethod>
        Public Sub ActivePlansInvariant_AllowsInProgressStatusInIsolatedTempDirectory()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim tempDir As String = Path.Combine(Path.GetTempPath(), "HygieneTest_" & Guid.NewGuid().ToString("N"))
            Directory.CreateDirectory(tempDir)

            Try
                Dim testPlanPath As String = Path.Combine(tempDir, "test_inprogress_plan.md")
                File.WriteAllText(testPlanPath, "# Synthetic Plan" & Environment.NewLine & "Status: In Progress" & Environment.NewLine)

                Dim violations As List(Of String) = ScanPlansForTerminalStatus(tempDir, repoRoot)
                Assert.AreEqual(0, violations.Count, "Active plan with 'In Progress' status must not produce violations.")
            Finally
                If Directory.Exists(tempDir) Then
                    Directory.Delete(tempDir, True)
                End If
            End Try
        End Sub

        <TestMethod>
        Public Sub DocumentationLinks_LinkTargetResolution_HandlesAllLinkVarieties()
            Dim repoRoot As String = GetRepositoryRoot()
            Dim tempDir As String = Path.Combine(Path.GetTempPath(), "HygieneTestDoc_" & Guid.NewGuid().ToString("N"))
            Directory.CreateDirectory(tempDir)

            Try
                Dim targetFile As String = Path.Combine(tempDir, "existing_target.md")
                File.WriteAllText(targetFile, "# Target Document")

                Dim targetFileWithSpaces As String = Path.Combine(tempDir, "target with spaces.md")
                File.WriteAllText(targetFileWithSpaces, "# Target With Spaces")

                Dim targetUri As New Uri(targetFile)
                Dim testDocPath As String = Path.Combine(tempDir, "source_doc.md")

                Dim sampleDoc As String =
                    $"[Existing Relative](existing_target.md)" & Environment.NewLine &
                    $"[With Anchor](existing_target.md#section-1)" & Environment.NewLine &
                    $"[With Query](existing_target.md?v=2)" & Environment.NewLine &
                    $"[With Title](existing_target.md ""Title text"")" & Environment.NewLine &
                    $"[Angle Brackets](<target with spaces.md>)" & Environment.NewLine &
                    $"[Angle Brackets With Title](<target with spaces.md> ""Title"")" & Environment.NewLine &
                    $"[File URI]({targetUri.AbsoluteUri})" & Environment.NewLine &
                    $"[External Web](https://example.com/test)" & Environment.NewLine &
                    $"[External Http](http://example.com/test)" & Environment.NewLine &
                    $"[Mailto](mailto:support@example.com)" & Environment.NewLine &
                    $"[Intra Document Anchor](#heading-1)" & Environment.NewLine &
                    $"[Dead Relative](nonexistent_file_xyz.md)" & Environment.NewLine &
                    $"[Dead File URI](file:///C:/nonexistent_file_xyz_987654.md)"

                File.WriteAllText(testDocPath, sampleDoc)

                Dim missing As List(Of String) = ScanDocumentationLinks(testDocPath, repoRoot)

                ' Exactly 2 links must fail: [Dead Relative] and [Dead File URI]
                Assert.AreEqual(2, missing.Count, $"Expected exactly 2 missing links, but got: {String.Join(", ", missing)}")
                Assert.IsTrue(missing.Exists(Function(s As String) s.Contains("nonexistent_file_xyz.md")), "Must detect dead relative link.")
                Assert.IsTrue(missing.Exists(Function(s As String) s.Contains("nonexistent_file_xyz_987654.md")), "Must detect dead file URI.")
            Finally
                If Directory.Exists(tempDir) Then
                    Directory.Delete(tempDir, True)
                End If
            End Try
        End Sub

    End Class

End Namespace
