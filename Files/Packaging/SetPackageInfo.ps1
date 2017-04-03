function Solution-SetPackageInfo
{
    <#
    .Synopsis
        Sets package information to the solution.
    .Description
        Updates project files (.csproj) of the solution before execute 'dotnet pack'.
    #>
    param
    (
        [Parameter(HelpMessage = "Path to the solution directory.")]
        [String] $solutionDir,

        [Parameter(HelpMessage = "Path to GlobalAssemblyInfo.cs.")]
        [String] $assemblyInfo,

        [Parameter(HelpMessage = "VCS branch name.")]
        [String] $branchName
    )

    process
    {
        $semanticVersion = Solution-GetSemanticVersion $assemblyInfo
        $prereleaseSuffix = Solution-PrereleaseSuffix $branchName
        $isPrereleaseVersion = Solution-IsPrereleaseVersion $branchName

        $projectFiles = Solution-GetProjectFiles $solutionDir

        $projectPrereleaseList = @{}

        foreach ($projectFile in $projectFiles)
        {
            [xml] $projectXml = Get-Content $projectFile

            if (Project-IsDotNetCore $projectXml)
            {
                $projectVersion = $semanticVersion

                if ($isPrereleaseVersion -or (Project-IsPrerelease $projectFile.FullName $projectPrereleaseList))
                {
                    $projectVersion = $projectVersion + '-' + $prereleaseSuffix
                }

                Project-SetPackageInfo $projectXml $projectVersion

                $projectXml.Save($projectFile)
            }
        }
    }
}


function Solution-GetSemanticVersion
{
    <#
    .Synopsis
        Returns semantic version of the solution.
    .Description
        The semantic version is extracted from the AssemblyVersionAttribute which is located in GlobalAssemblyInfo.cs.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to GlobalAssemblyInfo.cs.")]
        [String] $assemblyInfo
    )

    process
    {
        return Get-Content $assemblyInfo `
            | Select-String -Pattern 'AssemblyVersion\s*\(\s*\"(?<version>.*?)\"\s*\)' `
            | ForEach-Object { $_.Matches[0].Groups['version'].Value }
    }
}


function Solution-PrereleaseSuffix
{
    <#
    .Synopsis
        Returns pre-release suffix of the solution (e.g beta).
    .Description
        The pre-release suffix is extracted from VCS branch name of the solution.
    #>

    param
    (
        [Parameter(HelpMessage = "VCS branch name.")]
        [String] $branchName
    )

    process
    {
        if (Solution-IsPrereleaseVersion $branchName)
        {
            return ($branchName -replace '^(refs/heads/){0,1}(f\-){0,1}', '')
        }
        else
        {
            return 'prerelease'
        }
    }
}


function Solution-IsPrereleaseVersion
{
    <#
    .Synopsis
        Determines if the solution version is pre-release.
    .Description
        The solution version is pre-release if VCS branch name of the solution does not start with 'release-'.
    #>

    param
    (
        [Parameter(HelpMessage = "VCS branch name.")]
        [String] $branchName
    )

    process
    {
        return ($branchName -notmatch '^(refs/heads/){0,1}(f\-){0,1}release\-[0-9\.]+$')
    }
}


function Solution-GetProjectFiles
{
    <#
    .Synopsis
        Returns list of project files of the solution.
    .Description
        Finds and returns all project files (.csproj) of the solution directory except test projects (.Tests.csproj).
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the solution directory.")]
        [String] $solutionDir
    )

    process
    {
        return Get-ChildItem -Path $solutionDir -Filter '*.csproj' -Exclude '*.Tests.csproj' -Recurse
    }
}


function Project-IsDotNetCore
{
    <#
    .Synopsis
        Checks if the project is .NET Core.
    .Description
        The project is .NET Core if it has Sdk attribute which starts with 'Microsoft.NET.Sdk'.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return ($projectXml.DocumentElement.Attributes['Sdk'].Value -like 'Microsoft.NET.Sdk*')
    }
}


function Project-IsPrerelease
{
    <#
    .Synopsis
        Checks if the project has pre-release dependencies.
    .Description
        The project is pre-release if it has any pre-release package or project dependencies.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the project file.")]
        [String] $projectFile,

        [Parameter(HelpMessage = "List of processed projects.")]
        [Hashtable] $projectPrereleaseList
    )

    process
    {

        # Check if the project is already processed
        if ($projectPrereleaseList.ContainsKey($projectFile))
        {
            return $projectPrereleaseList[$projectFile]
        }

        $isPrerelease = $false;

        [xml] $projectXml = Get-Content $projectFile

        $projectDirectory = (Get-ChildItem $projectFile).Directory.FullName

        # Check if the project has any pre-release package dependencies

        $packageReferences= Project-GetPackageReferences $projectXml

        foreach ($packageReference in $packageReferences)
        {
            if (Package-IsPrereleaseVersion $packageReference.Version)
            {
                $isPrerelease = $true
                break
            }
        }

        # Checks recursively if the project has any pre-release project dependencies

        if (-not $isPrerelease)
        {
            $projectReferences = Project-GetProjectReferences $projectXml

            foreach ($projectReference in $projectReferences)
            {
                $projectReferenceFile = (Get-Item (Join-Path $projectDirectory $projectReference)).FullName

                if (Project-IsPrerelease $projectReferenceFile $projectPrereleaseList)
                {
                    $isPrerelease = $true
                    break
                }
            }
        }

        # Marks the project as processed

        $projectPrereleaseList.Add($projectFile, $isPrerelease)

        return $isPrerelease;
    }
}


function Project-GetProjectReferences
{
    <#
    .Synopsis
        Returns project references of the project.
    .Descriptions
        Returns non empty values of ProjectReference elements.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return $projectXml.Project.ItemGroup.ProjectReference.Include | Where { $_ }
    }
}


function Project-GetPackageReferences
{
    <#
    .Synopsis
        Returns package references of the project.
    .Descriptions
        Returns non empty PackageReference elements.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return $projectXml.Project.ItemGroup.PackageReference | Where { $_ }
    }
}


function Package-IsPrereleaseVersion
{
    <#
    .Synopsis
        Determines if the package version is pre-release.
    .Description
        The package version is pre-release if it has a pre-release suffix.
    #>

    param
    (
        [Parameter(HelpMessage = "Package version.")]
        [String] $packageVersion
    )

    process
    {
        return ($packageVersion -match '^[0-9\.]+\-.*?$')
    }
}


function Project-SetPackageInfo
{
    <#
    .Synopsis
        Sets package information to the project.
    .Description
        Updates the project file (.csproj) adding necessary attributes to execute 'dotnet pack'.
    #>
    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml,

        [Parameter(HelpMessage = "Project version.")]
        [String] $projectVersion
    )

    process
    {
        $propertyGroup = $projectXml.Project.PropertyGroup

        Project-SetChildNode $projectXml $propertyGroup 'PackageVersion' $projectVersion
        Project-SetChildNode $projectXml $propertyGroup 'Authors' 'Infinnity Solutions Ltd'
        Project-SetChildNode $projectXml $propertyGroup 'Company' 'Infinnity Solutions Ltd'
        Project-SetChildNode $projectXml $propertyGroup 'Copyright' "Infinnity Solutions Ltd 2006-$(Get-Date -Format yyyy)"
        Project-SetChildNode $projectXml $propertyGroup 'PackageLicenseUrl' 'https://github.com/InfinniPlatform/InfinniPlatform/blob/master/LICENSE'
        Project-SetChildNode $projectXml $propertyGroup 'PackageProjectUrl' 'http://infinniplatform.readthedocs.io/'
        Project-SetChildNode $projectXml $propertyGroup 'PackageReleaseNotes' 'http://infinniplatform.readthedocs.io/release-notes/index.html'
        Project-SetChildNode $projectXml $propertyGroup 'RepositoryUrl' 'https://github.com/InfinniPlatform/InfinniPlatform.git'
        Project-SetChildNode $projectXml $propertyGroup 'RepositoryType' 'git'
    }
}


function Project-SetChildNode
{
    <#
    .Synopsis
        Sets text of the child node.
    .Description
        Adds or updates text of the child node.
    #>

    param
    (
        [xml] $projectXml,
        [object] $parentNode,
        [string] $childNodeName,
        [string] $childNodeText
    )

    process
    {
        $childNode = $parentNode[$childNodeName]

        if (-not $childNode)
        {
            $childNode = $projectXml.CreateElement($childNodeName)
            $childNode.InnerText = $childNodeText
            $parentNode.AppendChild($childNode)
        }
        else
        {
            $childNode.InnerText = $childNodeText
        }
    }
}