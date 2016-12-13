function Create-Nuspec
{
    <#
    .Synopsis
        Creates nuspec-files from projects files.
    #>
    param
    (
        [Parameter(HelpMessage = "Path to the solution directory.")]
        [String] $solutionDir = '.',

        [Parameter(HelpMessage = "Path to the solution output directory.")]
        [String] $outputDir = 'Assemblies',

        [Parameter(HelpMessage = "Path to GlobalAssemblyInfo.cs.")]
        [String] $assemblyInfo = 'Files\Packaging\GlobalAssemblyInfo.cs',

        [Parameter(HelpMessage = "VCS branch name.")]
        [String] $branchName = '',

        [Parameter(HelpMessage = "VCS commit hash.")]
        [String] $commitHash = '',

        [Parameter(HelpMessage = ".NET version.")]
        [String] $framework = 'net452',

        [Parameter(HelpMessage = "Solution extensions.")]
        [Array] $extensions = @(
            'Infinni.Agent',
            'Infinni.Server',
            'InfinniPlatform.Auth.Adfs',
            'InfinniPlatform.Auth.Cookie',
            'InfinniPlatform.Auth.Facebook',
            'InfinniPlatform.Auth.Google',
            'InfinniPlatform.Heartbeat',
            'InfinniPlatform.Auth.Internal',
            'InfinniPlatform.Auth.Vk',
            'InfinniPlatform.Plugins.ViewEngine',
            'InfinniPlatform.PrintView',
            'InfinniPlatform.PushNotification',
            'InfinniPlatform.Scheduler',
            'InfinniPlatform.Watcher'
        )
    )

    process
    {
        $solutionRefs = @()

        # -----------------------------------
        # 1. Gets projects and their versions
        # -----------------------------------

        $projectVersions = Solution-GetProjectVersions `
                                -solutionDir $solutionDir `
                                -assemblyInfo $assemblyInfo `
                                -branchName $branchName `
                                -extensions $extensions

        # ----------------------------------------
        # 2. Creates nuspec-files for all projects
        # ----------------------------------------

        foreach ($projectFile in $projectVersions.Keys)
        {
            $projectRefs = @()
            $projectName = Project-GetName $projectFile
            $projectDirectory = Project-GetDirectory $projectFile
            $projectVersion = $projectVersions[$projectFile]
            $isNotExtension = ($extensions -notcontains $projectName)
            $isPlugin = ($projectName -like 'InfinniPlatform.Plugins.*')

            if ($isPlugin) {
                $targetFolder = "plugin"
            }
            else {
                $targetFolder = "lib"
            }
            
            Write-Host "Create $projectName.$projectVersion.nuspec"

            # Adds nuspec-header

            $projectNuspec = 
                "<?xml version=""1.0"" encoding=""utf-8""?>`r`n" + `
                "<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">`r`n" + `
                "    <metadata>`r`n" + `
                "        <id>$projectName</id>`r`n" + `
                "        <version>$projectVersion</version>`r`n" + `
                "        <authors>Infinnity Solutions</authors>`r`n" + `
                "        <owners>Infinnity Solutions</owners>`r`n" + `
                "        <requireLicenseAcceptance>false</requireLicenseAcceptance>`r`n" + `
                "        <description>Commit $commitHash</description>`r`n" + `
                "        <copyright>Infinnity Solutions $(Get-Date -Format yyyy)</copyright>`r`n" + `
                "        <dependencies>`r`n"

            # Adds internal dependencies from project file

            [xml] $projectXml = Get-Content $projectFile

            $projectReferences = Project-InternalReferences $projectXml

            foreach ($projectReference in $projectReferences)
            {
                $projectReferenceFile = (Get-Item (Join-Path $projectDirectory $projectReference)).FullName
                $projectReferenceName = Project-GetName $projectReferenceFile
                $projectReferenceVersion = $projectVersions[$projectReferenceFile]
                $projectReferenceFramework = Project-GetFrameworkVersion (Get-Content $projectReferenceFile)

                $projectNuspec = $projectNuspec + "            <dependency id=""$projectReferenceName"" version=""[$projectReferenceVersion]"" />`r`n"

                $projectRefs += "$projectReferenceName.$projectReferenceVersion\lib\$projectReferenceFramework\$projectReferenceName.dll"            
            }  

            if (-Not $isPlugin) 
            {
                # Adds external dependencies from packages.config
                
                $projectPackages = Project-GetPackages $projectFile

                foreach ($package in $projectPackages)
                {
                    $projectNuspec = $projectNuspec + "            <dependency id=""$($package.id)"" version=""[$($package.version)]"" />`r`n"
                }

                $projectRefs += Project-GetExternalReferences $projectXml
            }

            # Ends the dependencies part

            $projectNuspec = $projectNuspec + `
                "        </dependencies>`r`n" + `
                "    </metadata>`r`n" + `
                "    <files>`r`n"

            $projectTargetPath = "$projectName.$projectVersion\$targetFolder\$framework";

            if ($isPlugin) 
            {
                # Adds external dependencies from packages.config

                $projectPackagesRefs = Project-GetExternalReferences $projectXml                

                foreach ($package in $projectPackagesRefs)
                {
                    $pluginAssembly = $package.substring($package.LastIndexOf('\') + 1)
                    $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""$pluginAssembly"" />`r`n"
                    $solutionRefs += "$projectTargetPath\$pluginAssembly"
                }                
            }

            # Adds project assembly

            $projectAssemblyName = Project-GetAssemblyName $projectXml
            $projectIsLibrary = Project-IsLibrary $projectXml
            $projectAssembly = $projectAssemblyName + $(if ($projectIsLibrary) { '.dll' } else { '.exe' })
            $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""$projectAssembly"" />`r`n"
            if ($isNotExtension) { $solutionRefs += "$projectTargetPath\$projectAssembly" }

            # Adds resources for ru-RU

            if (Project-HasEmbeddedResource $projectXml 'ru-RU')
            {
                $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework\ru-RU"" src=""ru-RU\$projectAssemblyName.resources.dll"" />`r`n"
                if ($isNotExtension) { $solutionRefs += "$projectTargetPath\ru-RU\$projectAssemblyName.resources.dll" }
            }

            # Adds resources for en-US

            if (Project-HasEmbeddedResource $projectXml 'en-US')
            {
                $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework\en-US"" src=""en-US\$projectAssemblyName.resources.dll"" />`r`n"
                if ($isNotExtension) { $solutionRefs += "$projectTargetPath\en-US\$projectAssemblyName.resources.dll" }
            }

            # Adds symbol file

            $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""$projectAssemblyName.pdb"" />`r`n"
            if ($isNotExtension) { $solutionRefs += "$projectTargetPath\$projectAssemblyName.pdb" }

            # Adds XML-documentation

            if (Project-HasDocumentationFile $projectXml)
            {
                $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""$projectAssemblyName.xml"" />`r`n"
            }

            # Adds app config-file

            if (Project-HasFile $projectXml 'App.config')
            {
                $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""$projectAssembly.config"" />`r`n"
                if ($isNotExtension) { $solutionRefs += "$projectTargetPath\$projectAssembly.config" }
            }

            # Adds log config-file

            if (Project-HasFile $projectXml 'AppLog.config')
            {
                $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""AppLog.config"" />`r`n"
                if ($isNotExtension) { $solutionRefs += "$projectTargetPath\AppLog.config" }
            }

            # Adds platform config-file

            if (Project-HasFile $projectXml 'AppCommon.json')
            {
                $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework"" src=""AppCommon.json"" />`r`n"
                if ($isNotExtension) { $solutionRefs += "$projectTargetPath\AppCommon.json" }
            }

            # Adds extension config-file

            if (Project-HasFile $projectXml 'AppExtension.json')
            {
                if (-Not $isNotExtension) { $projectNuspec = $projectNuspec + "        <file target=""$targetFolder\$framework\AppExtension.json"" src=""$projectName.AppExtension.json"" />`r`n" }                
            }

            # Adds content files

            $projectContentFiles = Project-GetContentFiles $projectDirectory

            foreach ($contentFile in $projectContentFiles)
            {
                $projectNuspec = $projectNuspec +
                    "        <file target=""$contentFile"" src=""$contentFile"" />`r`n"
            }

            # Ends the files part

            $projectNuspec = $projectNuspec + `
                "        <file target=""$targetFolder\$framework\$projectName.references"" src=""$projectName.references"" />`r`n" + `
                "    </files>`r`n" + `
                "</package>"

            if ($isNotExtension) { $solutionRefs += $projectRefs }

            # Creates file with project references
            Set-Content (Join-Path $outputDir "$projectName.references") -Value ($projectRefs | Sort-Object | Get-Unique -AsString)

            # Creates nuspec-file
            Set-Content (Join-Path $outputDir "$projectName.nuspec") -Value $projectNuspec
        }

        # ---------------------------------------
        # 3. Creates nuspec-file for the solution
        # ---------------------------------------

        # Version of the solution is version of InfinniPlatform.Sdk project
        $solutionVersion = $projectVersions[($projectVersions.Keys | Where-Object { $_ -match 'InfinniPlatform.Sdk' } | Select-Object -First 1)]

        Write-Host "Create InfinniPlatform.$solutionVersion.nuspec"

        # Adds nuspec-header

        $solutionNuspec = 
            "<?xml version=""1.0"" encoding=""utf-8""?>`r`n" + `
            "<package xmlns=""http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd"">`r`n" + `
            "    <metadata>`r`n" + `
            "        <id>InfinniPlatform</id>`r`n" + `
            "        <version>$solutionVersion</version>`r`n" + `
            "        <authors>Infinnity Solutions</authors>`r`n" + `
            "        <owners>Infinnity Solutions</owners>`r`n" + `
            "        <requireLicenseAcceptance>false</requireLicenseAcceptance>`r`n" + `
            "        <description>Commit hash: $commitHash</description>`r`n" + `
            "        <copyright>Infinnity Solutions $(Get-Date -Format yyyy)</copyright>`r`n" + `
            "        <dependencies>`r`n"

        # Adds projects of the solution except extensions

        foreach ($projectFile in $projectVersions.Keys)
        {
            $projectName = Project-GetName $projectFile
            $projectVersion = $projectVersions[$projectFile]
            $isNotExtension = ($extensions -notcontains $projectName)

            if ($isNotExtension)
            {
                $solutionNuspec = $solutionNuspec + "            <dependency id=""$projectName"" version=""[$projectVersion]"" />`r`n"
            }
        }

        # Ends nuspec file

        $solutionNuspec = $solutionNuspec + `
            "        </dependencies>`r`n" + `
            "    </metadata>`r`n" + `
            "    <files>`r`n" + `
            "        <file target=""lib\$framework\InfinniPlatform.references"" src=""InfinniPlatform.references"" />`r`n" + `
            "        <file target=""content\monitoring"" src=""monitoring\**"" />`r`n" + `
            "    </files>`r`n" + `
            "</package>"

        # Creates file with project references
        Set-Content (Join-Path $outputDir 'InfinniPlatform.references') -Value ($solutionRefs | Sort-Object | Get-Unique -AsString)

        # Creates nuspec-file
        Set-Content (Join-Path $outputDir 'InfinniPlatform.nuspec') -Value $solutionNuspec
    }
}


function Solution-GetProjectVersions
{
    <#
    .Synopsis
        Returns hash table with projects of the solution and their versions.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the solution directory.")]
        [String] $solutionDir,

        [Parameter(HelpMessage = "Path to GlobalAssemblyInfo.cs.")]
        [String] $assemblyInfo,

        [Parameter(HelpMessage = "VCS branch name.")]
        [String] $branchName,

        [Parameter(HelpMessage = "Extensions.")]
        [Array] $extensions
    )

    process
    {
        #$projectVersions = @{}
        #$projectVersions.Add('C:\Projects\InfinniPlatform\Infinni.Server\Infinni.Server.csproj', '1.11.0.0-test')

        #return $projectVersions

        $isReleaseBranch = $false

        # Gets semantic version of the solution
        $semanticVersion = Get-Content $assemblyInfo `
            | Select-String -Pattern 'AssemblyVersion\s*\(\s*\"(?<version>.*?)\"\s*\)' `
            | ForEach-Object { $_.Matches[0].Groups['version'].Value }

        # Gets suffix for prerelease versions
        $prereleaseSuffix = ($branchName -replace '^(refs/heads/){0,1}(f\-){0,1}', '')

        # Checks if the branch is release
        if ($prereleaseSuffix -match '^release\-[0-9\.]+$')
        {
            $prereleaseSuffix = 'prerelease'
            $isReleaseBranch = $true
        }

        # Finds projects of the solution
        $projects = Get-ChildItem `
                        -Path $solutionDir `
                        -Filter '*.csproj' `
                        -Exclude '*.Tests.csproj' `
                        -Recurse

        $projectVersions = @{}

        if ($isReleaseBranch)
        {
            $isPrereleaseSolution = $false
            $projectPrereleaseList = @{}

            # Checks each project if it is prerelease
            foreach ($project in $projects)
            {
                $projectName = Project-GetName($project.FullName)

                # If the project is prerelease
                if (Project-IsPrerelease $project.FullName $projectPrereleaseList)
                {
                    # And it is not an extension
                    if ($extensions -notcontains $projectName)
                    {
                        # All projects of the solution are prerelease
                        $isPrereleaseSolution = $true
                    }
                }
            }

            foreach ($project in $projects)
            {
                $projectName = Project-GetName($project.FullName)

                # If the solution or an extension is prerelease
                if ($isPrereleaseSolution `
                    -or (($extensions -contains $projectName) `
                            -and $projectPrereleaseList.ContainsKey($project.FullName) `
                            -and $projectPrereleaseList[$project.FullName]))
                {
                    $projectVersions.Add($project.FullName, $semanticVersion + '-' + $prereleaseSuffix)
                }
                else
                {
                    $projectVersions.Add($project.FullName, $semanticVersion)
                }
            }
        }
        else
        {
            # On a prerelease branch all projects are prerelease
            foreach ($project in $projects)
            {
                $projectVersions.Add($project.FullName, $semanticVersion + '-' + $prereleaseSuffix)
            }
        }

        return $projectVersions
    }
}


function Project-IsPrerelease
{
    <#
    .Synopsis
        Checks if the given project has prerelease dependencies.
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
        $isPrerelease = $false;

        # Check if the project is already processed
        if ($projectPrereleaseList.ContainsKey($projectFile))
        {
            $isPrerelease = $projectPrereleaseList[$projectFile];
        }
        else
        {
            $projectDirectory = Project-GetDirectory $projectFile

            # Checks external dependencies

            $projectPackages = Project-GetPackages $projectFile

            # Check if the project has any prerelease packages
            foreach ($package in $projectPackages)
            {
                $isPrerelease = ($isPrerelease -or ($package.version -match '^[0-9\.]+\-.*?$'))

                if ($isPrerelease)
                {
                    break
                }
            }

            # Checks internal dependencies

            if (-not $isPrerelease)
            {
                [xml] $projectXml = Get-Content $projectFile

                $projectReferences = Project-InternalReferences $projectXml

                # Checks recursively if the project has any prerelease reference
                foreach ($projectReference in $projectReferences)
                {
                    $projectReferenceFile = (Get-Item (Join-Path $projectDirectory $projectReference)).FullName

                    $isPrerelease = ($isPrerelease -or (Project-IsPrerelease $projectReferenceFile $projectPrereleaseList))

                    if ($isPrerelease)
                    {
                        break
                    }
                }
            }

            # Marks the project as processed

            $projectPrereleaseList.Add($projectFile, $isPrerelease)
        }

        return $isPrerelease;
    }
}


function Project-GetAssemblyName
{
    <#
    .Synopsis
        Returns assembly name of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return $projectXml.Project.PropertyGroup.AssemblyName[0]
    }
}


function Project-IsLibrary
{
    <#
    .Synopsis
        Checks if the given project is a class library.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return ($projectXml.Project.PropertyGroup.OutputType -like '*Library*')
    }
}


function Project-InternalReferences
{
    <#
    .Synopsis
        Returns internal references of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return $projectXml.Project.ItemGroup.ProjectReference.Include
    }
}


function Project-GetExternalReferences
{
    <#
    .Synopsis
        Returns external references of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return $projectXml.Project.ItemGroup.Reference.HintPath | Where { $_ -like '..\packages\*.dll' } | % { $_ -replace '^\.\.\\packages\\', '' }
    }
}


function Project-HasEmbeddedResource
{
    <#
    .Synopsis
        Checks if the given project has an embedded resource.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml,

        [Parameter(HelpMessage = "Culture.")]
        [String] $culture
    )

    process
    {
        return (($projectXml.Project.ItemGroup.EmbeddedResource.Include | Where { $_ -like "*.$culture.*" }).Count -gt 0)
    }
}


function Project-HasDocumentationFile
{
    <#
    .Synopsis
        Checks if of the given project has a documentation file.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        return (($projectXml.Project.PropertyGroup.DocumentationFile | Where { $_ }).Count -gt 0)
    }
}


function Project-HasFile
{
    <#
    .Synopsis
        Checks if of the given project has a file with specified name.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml,

        [Parameter(HelpMessage = "File name.")]
        [String] $fileName
    )

    process
    {
        return (($projectXml.Project.ItemGroup.None.Include | Where { $_ -match "(^|\\)$fileName$" }).Count -gt 0)
    }
}


function Project-GetPackages
{
    <#
    .Synopsis
        Returns packages of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the project file.")]
        [String] $projectFile
    )

    process
    {
        $projectPackages = @()
        $projectDirectory = Project-GetDirectory $projectFile
        $packagesConfigPath = Join-Path $projectDirectory 'packages.config'

        if (Test-Path $packagesConfigPath)
        {
            [xml] $packagesConfigXml = Get-Content $packagesConfigPath

            $packages = $packagesConfigXml.packages.package

            if ($packages)
            {
                foreach ($package in $packages)
                {
                    $projectPackages += $package
                }
            }
        }

        return $projectPackages
    }
}


function Project-GetName
{
    <#
    .Synopsis
        Returns name of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the project file.")]
        [String] $projectFile
    )

    process
    {
        $projectItem = Get-ChildItem $projectFile
        $projectName = $projectItem.BaseName
        return $projectName
    }
}


function Project-GetDirectory
{
    <#
    .Synopsis
        Returns path to directory of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the project file.")]
        [String] $projectFile
    )

    process
    {
        $projectItem = Get-ChildItem $projectFile
        $projectDirectory = $projectItem.Directory.FullName
        return $projectDirectory
    }
}


function Project-GetFrameworkVersion
{
    <#
    .Synopsis
        Returns framework version of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Project file.")]
        [xml] $projectXml
    )

    process
    {
        $frameworkVersion = ($projectXml.Project.PropertyGroup.TargetFrameworkVersion | Select-Object -First 1) -replace '[v\.\s]', ''

        return 'net' + $frameworkVersion
    }
}


function Project-GetContentFiles
{
    <#
    .Synopsis
        Returns content files of the given project.
    #>

    param
    (
        [Parameter(HelpMessage = "Path to the project directory.")]
        [String] $projectDirectory
    )

    process
    {
        $contentFiles = @()
        $projectContent = Join-Path $projectDirectory 'content'

        if (Test-Path $projectContent)
        {
            $contentFiles += Get-ChildItem -Path $projectContent -File -Recurse `
                                | Where { $_ -notlike '*.vshost.exe*' } `
                                | ForEach-Object { $_.FullName.Substring($projectDirectory.Length).TrimStart('\') }
        }

        return $contentFiles
    }
}