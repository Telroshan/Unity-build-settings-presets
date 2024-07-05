using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TelroshanTools.BuildSettingsPresets.Editor
{
    public class BuildSettingsPreset : ScriptableObject
    {
        [Serializable]
        public class BuildScene
        {
            public SceneAsset scene;
            public string guid;
            public string path;
            public bool enabled;
        }

        [SerializeField] private BuildScene[] scenes;

        [SerializeField] private BuildTarget activeBuildTarget;
        [SerializeField] private List<string> activeScriptCompilationDefines = new List<string>();
        [SerializeField] private bool allowDebugging;
        [SerializeField] private MobileTextureSubtarget androidBuildSubtarget;
        [SerializeField] private AndroidETC2Fallback androidEtc2Fallback;
        [SerializeField] private bool buildAppBundle;
        [SerializeField] private bool buildScriptsOnly;
        [SerializeField] private bool compressFilesInPackage;
        [SerializeField] private bool connectProfiler;
        [SerializeField] private bool development;
        [SerializeField] private bool enableHeadlessMode;
        [SerializeField] private bool explicitArrayBoundsChecks;
        [SerializeField] private bool explicitDivideByZeroChecks;
        [SerializeField] private bool explicitNullChecks;
        [SerializeField] private bool exportAsGoogleAndroidProject;
        [SerializeField] private bool forceInstallation;
        [SerializeField] private bool installInBuildFolder;
        [SerializeField] private XcodeBuildConfig iOsBuildConfigType;
        [SerializeField] private bool movePackageToDiscOuterEdge;
        [SerializeField] private bool needSubmissionMaterials;
        [SerializeField] private PS4BuildSubtarget ps4BuildSubtarget;
        [SerializeField] private PS4HardwareTarget ps4HardwareTarget;
        [SerializeField] private BuildTargetGroup selectedBuildTargetGroup;
        [SerializeField] private BuildTarget selectedStandaloneTarget;
        [SerializeField] private int streamingInstallLaunchRange;
        [SerializeField] private bool symlinkLibraries;
        [SerializeField] private bool waitForPlayerConnection;
        [SerializeField] private string windowsDevicePortalAddress;
        [SerializeField] private string windowsDevicePortalPassword;
        [SerializeField] private string windowsDevicePortalUsername;
        [SerializeField] private WSABuildAndRunDeployTarget wsaBuildAndRunDeployTarget;
        [SerializeField] private string wsaUwpsdk;
        [SerializeField] private string wsaUwpVisualStudioVersion;
        [SerializeField] private XboxBuildSubtarget xboxBuildSubtarget;
        [SerializeField] private XboxOneDeployDrive xboxOneDeployDrive;
        [SerializeField] private XboxOneDeployMethod xboxOneDeployMethod;
        [SerializeField] private bool xboxOneRebootIfDeployFailsAndRetry;

        public void OverwriteWithCurrentBuildSettings()
        {
            scenes = EditorBuildSettings.scenes.Select(x => new BuildScene
                {
                    scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(x.path),
                    path = x.path,
                    guid = x.guid.ToString(),
                    enabled = x.enabled,
                })
                .ToArray();

            activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            activeScriptCompilationDefines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                    .Split(';').ToList();
            allowDebugging = EditorUserBuildSettings.allowDebugging;
            androidBuildSubtarget = EditorUserBuildSettings.androidBuildSubtarget;
            androidEtc2Fallback = EditorUserBuildSettings.androidETC2Fallback;
            buildAppBundle = EditorUserBuildSettings.buildAppBundle;
            buildScriptsOnly = EditorUserBuildSettings.buildScriptsOnly;
            compressFilesInPackage = EditorUserBuildSettings.compressFilesInPackage;
            connectProfiler = EditorUserBuildSettings.connectProfiler;
            development = EditorUserBuildSettings.development;
            enableHeadlessMode = EditorUserBuildSettings.standaloneBuildSubtarget == StandaloneBuildSubtarget.Server;
            explicitArrayBoundsChecks = EditorUserBuildSettings.explicitArrayBoundsChecks;
            explicitDivideByZeroChecks = EditorUserBuildSettings.explicitDivideByZeroChecks;
            explicitNullChecks = EditorUserBuildSettings.explicitNullChecks;
            exportAsGoogleAndroidProject = EditorUserBuildSettings.exportAsGoogleAndroidProject;
            forceInstallation = EditorUserBuildSettings.forceInstallation;
            installInBuildFolder = EditorUserBuildSettings.installInBuildFolder;
            iOsBuildConfigType = EditorUserBuildSettings.iOSXcodeBuildConfig;
            movePackageToDiscOuterEdge = EditorUserBuildSettings.movePackageToDiscOuterEdge;
            needSubmissionMaterials = EditorUserBuildSettings.needSubmissionMaterials;
            ps4BuildSubtarget = EditorUserBuildSettings.ps4BuildSubtarget;
            ps4HardwareTarget = EditorUserBuildSettings.ps4HardwareTarget;
            selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            selectedStandaloneTarget = EditorUserBuildSettings.selectedStandaloneTarget;
            streamingInstallLaunchRange = EditorUserBuildSettings.streamingInstallLaunchRange;
            symlinkLibraries = EditorUserBuildSettings.symlinkSources;
            waitForPlayerConnection = EditorUserBuildSettings.waitForPlayerConnection;
            windowsDevicePortalAddress = EditorUserBuildSettings.windowsDevicePortalAddress;
            windowsDevicePortalPassword = EditorUserBuildSettings.windowsDevicePortalPassword;
            windowsDevicePortalUsername = EditorUserBuildSettings.windowsDevicePortalUsername;
            wsaBuildAndRunDeployTarget = EditorUserBuildSettings.wsaBuildAndRunDeployTarget;
            wsaUwpsdk = EditorUserBuildSettings.wsaUWPSDK;
            wsaUwpVisualStudioVersion = EditorUserBuildSettings.wsaUWPVisualStudioVersion;
            xboxBuildSubtarget = EditorUserBuildSettings.xboxBuildSubtarget;
            xboxOneDeployDrive = EditorUserBuildSettings.xboxOneDeployDrive;
            xboxOneDeployMethod = EditorUserBuildSettings.xboxOneDeployMethod;
            xboxOneRebootIfDeployFailsAndRetry = EditorUserBuildSettings.xboxOneRebootIfDeployFailsAndRetry;
        }

        public static BuildSettingsPreset FromCurrentSettings()
        {
            BuildSettingsPreset preset = CreateInstance<BuildSettingsPreset>();

            preset.OverwriteWithCurrentBuildSettings();

            return preset;
        }

        public void Apply()
        {
            EditorBuildSettings.scenes = scenes.Select(x => new EditorBuildSettingsScene()
            {
                guid = new GUID(x.guid),
                path = x.path,
                enabled = x.enabled,
            }).ToArray();

            EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTargetGroup, activeBuildTarget);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(selectedBuildTargetGroup,
                string.Join(";", activeScriptCompilationDefines));
            EditorUserBuildSettings.allowDebugging = allowDebugging;
            EditorUserBuildSettings.androidBuildSubtarget = androidBuildSubtarget;
            EditorUserBuildSettings.androidETC2Fallback = androidEtc2Fallback;
            EditorUserBuildSettings.buildAppBundle = buildAppBundle;
            EditorUserBuildSettings.buildScriptsOnly = buildScriptsOnly;
            EditorUserBuildSettings.compressFilesInPackage = compressFilesInPackage;
            EditorUserBuildSettings.connectProfiler = connectProfiler;
            EditorUserBuildSettings.development = development;
            EditorUserBuildSettings.standaloneBuildSubtarget = enableHeadlessMode
                ? StandaloneBuildSubtarget.Server
                : StandaloneBuildSubtarget.Player;
            EditorUserBuildSettings.explicitArrayBoundsChecks = explicitArrayBoundsChecks;
            EditorUserBuildSettings.explicitDivideByZeroChecks = explicitDivideByZeroChecks;
            EditorUserBuildSettings.explicitNullChecks = explicitNullChecks;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = exportAsGoogleAndroidProject;
            EditorUserBuildSettings.forceInstallation = forceInstallation;
            EditorUserBuildSettings.installInBuildFolder = installInBuildFolder;
            EditorUserBuildSettings.iOSXcodeBuildConfig = iOsBuildConfigType;
            EditorUserBuildSettings.movePackageToDiscOuterEdge = movePackageToDiscOuterEdge;
            EditorUserBuildSettings.needSubmissionMaterials = needSubmissionMaterials;
            EditorUserBuildSettings.ps4BuildSubtarget = ps4BuildSubtarget;
            EditorUserBuildSettings.ps4HardwareTarget = ps4HardwareTarget;
            EditorUserBuildSettings.selectedBuildTargetGroup = selectedBuildTargetGroup;
            EditorUserBuildSettings.selectedStandaloneTarget = selectedStandaloneTarget;
            EditorUserBuildSettings.streamingInstallLaunchRange = streamingInstallLaunchRange;
            EditorUserBuildSettings.symlinkSources = symlinkLibraries;
            EditorUserBuildSettings.waitForPlayerConnection = waitForPlayerConnection;
            EditorUserBuildSettings.windowsDevicePortalAddress = windowsDevicePortalAddress;
            EditorUserBuildSettings.windowsDevicePortalPassword = windowsDevicePortalPassword;
            EditorUserBuildSettings.windowsDevicePortalUsername = windowsDevicePortalUsername;
            EditorUserBuildSettings.wsaBuildAndRunDeployTarget = wsaBuildAndRunDeployTarget;
            EditorUserBuildSettings.wsaUWPSDK = wsaUwpsdk;
            EditorUserBuildSettings.wsaUWPVisualStudioVersion = wsaUwpVisualStudioVersion;
            EditorUserBuildSettings.xboxBuildSubtarget = xboxBuildSubtarget;
            EditorUserBuildSettings.xboxOneDeployDrive = xboxOneDeployDrive;
            EditorUserBuildSettings.xboxOneDeployMethod = xboxOneDeployMethod;
            EditorUserBuildSettings.xboxOneRebootIfDeployFailsAndRetry = xboxOneRebootIfDeployFailsAndRetry;
        }
    }
}