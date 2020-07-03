// Fill out your copyright notice in the Description page of Project Settings.

using System.IO;
using UnrealBuildTool;

public class OSSThirdParty : ModuleRules
{
    //������������ﻹ�Ƿ��ڹ�����
    private bool IsEnginePlugin()
    {
        return Path.GetFullPath(ModuleDirectory).EndsWith("Engine\\Plugins\\Runtime\\SimpleOSS\\Source\\SimpleOSS");
    }

    //���·����Ҳ����.Build.cs��ǰĿ¼
    private string ModulePath
    {
        get { return ModuleDirectory; }
    }

    //ThirdPartyĿ¼��һ����UE4��������ĵ������⣬�Ͳ���Ĵ���Ŀ¼ƽ��
    private string ThirdPartyPath
    {
        get
        {
            if (IsEnginePlugin())
            {
                return Path.GetFullPath(Path.Combine(EngineDirectory, "Source/ThirdParty"));
            }
            else
            {
                return Path.GetFullPath(Path.Combine(ModulePath, "../../ThirdParty"));
            }
        }
    }

    //������Ŀ¼�������Ŀ¼�µĶ������ļ�Ŀ¼�����磬Plugins/SimpleOSS/Binaries
    private string BinariesPath
    {
        get
        {
            if (IsEnginePlugin())
            {
                return Path.GetFullPath(Path.Combine(EngineDirectory, "ThirdParty/OSSThirdParty/Win64/Release"));
            }
            else
            {
                return Path.GetFullPath(Path.Combine(ModulePath, "../../../Binaries/Win64"));
            }
        }
    }

    //��������ĵ�������Ŀ¼
    private string LibraryPath
    {
        get
        {
            if (IsEnginePlugin())
            {
                return Path.GetFullPath(Path.Combine(ThirdPartyPath, "OSSThirdParty/third_party/lib"));
            }
            else
            {
                return Path.GetFullPath(Path.Combine(ThirdPartyPath, "OSSThirdParty/third_party/lib"));
            }
        }
    }

    //��������ĵ�����ͷ�ļ�Ŀ¼
    private string IncludePath
    {
        get
        {
            if (IsEnginePlugin())
            {
                return Path.GetFullPath(Path.Combine(ThirdPartyPath, "OSSThirdParty/third_party/include"));
            }
            else
            {
                return Path.GetFullPath(Path.Combine(ThirdPartyPath, "OSSThirdParty/third_party/include"));
            }
        }
    }

    //����Ŀ¼��.uproject����Ŀ¼
    public string GetUProjectPath()
    {
        return Path.Combine(ModuleDirectory, "../../../../..");
    }

    //string��ϣֵ
    private int HashFile(string FilePath)
    {
        string DLLString = File.ReadAllText(FilePath);
        return DLLString.GetHashCode() + DLLString.Length;  //ensure both hash and file lengths match
    }

    //����������ĵ������⣬����������Ŀ¼��Binaries�ж�Ӧ��ƽ̨��
    private void CopyToProjectBinaries(string Filepath, ReadOnlyTargetRules Target)
    {
        System.Console.WriteLine("uprojectpath is: " + Path.GetFullPath(GetUProjectPath()));
        string BinariesDir = Path.Combine(GetUProjectPath(), "Binaries", Target.Platform.ToString());
        string Filename = Path.GetFileName(Filepath);
        
        string FullBinariesDir = Path.GetFullPath(BinariesDir);

        if (!Directory.Exists(FullBinariesDir))
        {
            Directory.CreateDirectory(FullBinariesDir);
        }

        string FullExistingPath = Path.Combine(FullBinariesDir, Filename);
        bool ValidFile = false;

        if (File.Exists(FullExistingPath))
        {
            int ExistingFileHash = HashFile(FullExistingPath);
            int TargetFileHash = HashFile(Filepath);
            ValidFile = ExistingFileHash == TargetFileHash;
            if (!ValidFile)
            {
                System.Console.WriteLine("HLS6AxesPlugin: outdated dll detected.");
            }
        }

        if (!ValidFile)
        {
            System.Console.WriteLine("HLS6AxesPlugin: Copied from " + Filepath + ", to " + Path.Combine(FullBinariesDir, Filename));
            File.Copy(Filepath, Path.Combine(FullBinariesDir, Filename), true);
        }
    }

    public OSSThirdParty(ReadOnlyTargetRules Target) : base(Target)
    {
        Type = ModuleType.External;

        string PlatformString = Target.Platform.ToString();
        string OSSThirdParty = Path.GetFullPath(Path.Combine(ModuleDirectory, "../OSSThirdParty"));
        string OssPathInclude = Path.Combine(OSSThirdParty, "oss_c_sdk");
        string Third_partyPath = Path.Combine(OSSThirdParty, "third_party/include");
        PublicIncludePaths.Add(OssPathInclude);
        PublicIncludePaths.Add(Third_partyPath);

        //������SDK�����ĵ��������include
        {
            PublicIncludePaths.Add(Path.Combine(Third_partyPath, "curl"));
            PublicIncludePaths.Add(Path.Combine(Third_partyPath, "mxml"));
            PublicIncludePaths.Add(Path.Combine(Third_partyPath, "aprutil"));
            PublicIncludePaths.Add(Path.Combine(Third_partyPath, "apr"));
        }

        LoadLibAndDll(Target);
        
        bUsePrecompiled = true;
    }

    //����dll��Ϣ
    bool LoadLibAndDll(ReadOnlyTargetRules Target)
    {
        bool IsLibrarySupported = false;
        if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            IsLibrarySupported = true;
            string PlatformString = Target.Platform.ToString();
            
            string OSSThirdParty = Path.GetFullPath(Path.Combine(ModuleDirectory, "../OSSThirdParty"));
            PublicAdditionalLibraries.Add(Path.Combine(OSSThirdParty, "x64/Release", "oss_c_sdk.lib"));

            if (IsEnginePlugin())
            {
                PublicDelayLoadDLLs.Add("libapr-1.dll");
                PublicDelayLoadDLLs.Add("libapriconv-1.dll");
                PublicDelayLoadDLLs.Add("libaprutil-1.dll");
                PublicDelayLoadDLLs.Add("libcurl.dll");
                PublicDelayLoadDLLs.Add("libexpat.dll");
                PublicDelayLoadDLLs.Add("mxml1.dll");
                PublicDelayLoadDLLs.Add("libeay32.dll");
                PublicDelayLoadDLLs.Add("ssleay32.dll");
                PublicDelayLoadDLLs.Add("zlibwapi.dll");

                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "libapr-1.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "libapriconv-1.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "libaprutil-1.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "libcurl.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "libexpat.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "mxml1.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "libeay32.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "ssleay32.dll"));
                RuntimeDependencies.Add(Path.Combine(BinariesPath, PlatformString, "zlibwapi.dll"));
            }
            else
            {
                string PluginDLLPath = Path.Combine(BinariesPath);
                System.Console.WriteLine("Project plugin detected, using dll at " + PluginDLLPath);

                string dll1Path = Path.Combine(PluginDLLPath, "libapr-1.dll");
                string dll2Path = Path.Combine(PluginDLLPath, "libapriconv-1.dll");
                string dll3Path = Path.Combine(PluginDLLPath, "libaprutil-1.dll");
                string dll4Path = Path.Combine(PluginDLLPath, "libcurl.dll");
                string dll5Path = Path.Combine(PluginDLLPath, "libexpat.dll");
                string dll6Path = Path.Combine(PluginDLLPath, "mxml1.dll");
                string dll7Path = Path.Combine(PluginDLLPath, "libeay32.dll");
                string dll8Path = Path.Combine(PluginDLLPath, "ssleay32.dll");
                string dll9Path = Path.Combine(PluginDLLPath, "zlibwapi.dll");
                
                //For project plugins, copy the dll to the project if needed
                CopyToProjectBinaries(dll1Path, Target);
                CopyToProjectBinaries(dll2Path, Target);
                CopyToProjectBinaries(dll3Path, Target);
                CopyToProjectBinaries(dll4Path, Target);
                CopyToProjectBinaries(dll5Path, Target);
                CopyToProjectBinaries(dll6Path, Target);
                CopyToProjectBinaries(dll7Path, Target);
                CopyToProjectBinaries(dll8Path, Target);
                CopyToProjectBinaries(dll9Path, Target);

                //Add dll(Project/Binaries) into RuntimeDependencies
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "libapr-1.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "libapriconv-1.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "libaprutil-1.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "libcurl.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "libexpat.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "mxml1.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "libeay32.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "ssleay32.dll")));
                RuntimeDependencies.Add(Path.GetFullPath(Path.Combine(GetUProjectPath(), "Binaries", PlatformString, "zlibwapi.dll")));
            }
        }
        else if (Target.Platform == UnrealTargetPlatform.Win32)
        {
            PublicAdditionalLibraries.Add(Path.Combine(ThirdPartyPath, "Win32/Release", "oss_c_sdk.lib"));
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {

        }
        return IsLibrarySupported;
    }
}
