function Build-Symbols
{
    <#
    .Synopsis
        Includes into pdb-files information about the source of the code.
    #>
    param
    (
        [Parameter(HelpMessage = "Path to the solution directory.")]
        [String] $solutionDir,

        [Parameter(HelpMessage = "VCS repository URL.")]
        [String] $repositoryUrl,

        [Parameter(HelpMessage = "VCS commit hash.")]
        [String] $commitHash
    )

    process
    {
        $buildToolsDir = Join-Path $env:ProgramData 'BuildTools'

        # Install NuGet package manager

        $nugetDir = Join-Path $buildToolsDir 'NuGet'
        $nugetPath = Join-Path $nugetDir 'nuget.exe'

        if (-not (Test-Path $nugetPath))
        {
            if (-not (Test-Path $nugetDir))
            {
                New-Item $nugetDir -ItemType Directory -ErrorAction SilentlyContinue
            }

            $nugetSourceUri = 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe'
            Invoke-WebRequest -Uri $nugetSourceUri -OutFile $nugetPath
        }

        # Install GitLink package

        $gitLinkDir = Join-Path $buildToolsDir 'GitLink'
        $gitLinkPath = Join-Path $gitLinkDir 'build\GitLink.exe'

        if (-not (Test-Path $gitLinkPath))
        {
            & "$nugetPath" install 'GitLink' -OutputDirectory $buildToolsDir -NonInteractive -Prerelease -ExcludeVersion
        }

        # Build symbol files

        if ($repositoryUrl -like '*.git')
        {
            $repositoryUrl = $repositoryUrl -replace '\.git$', ''
        }

        $pdbFiles = Get-ChildItem -Path $solutionDir -Filter '*.pdb' -Recurse `
            | Where-Object { $_.FullName -notlike '*\obj\*' } `

        foreach ($pdbFile in $pdbFiles)
        {
            & "$gitLinkPath" --baseDir $solutionDir --url $repositoryUrl --commit $commitHash $_.FullName
        }
    }
}