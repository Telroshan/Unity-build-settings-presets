using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BuildSettingsPreset : ScriptableObject
    {
        public BuildTarget activeBuildTarget;
        public string[] activeScriptCompilationDefines;
        public bool allowDebugging;
        public MobileTextureSubtarget androidBuildSubtarget;
        public AndroidETC2Fallback androidETC2Fallback;
        public bool buildAppBundle;
        public bool buildScriptsOnly;
        public bool compressFilesInPackage;
        public bool compressWithPsArc;
        public bool connectProfiler;
        public bool development;
        public bool enableHeadlessMode;
        public bool explicitArrayBoundsChecks;
        public bool explicitDivideByZeroChecks;
        public bool explicitNullChecks;
        public bool exportAsGoogleAndroidProject;
        public bool forceInstallation;
        public bool installInBuildFolder;
        public iOSBuildType iOSBuildConfigType;
        public bool movePackageToDiscOuterEdge;
        public bool needSubmissionMaterials;
        public PS4BuildSubtarget ps4BuildSubtarget;
        public PS4HardwareTarget ps4HardwareTarget;
        public BuildTargetGroup selectedBuildTargetGroup;
        public BuildTarget selectedStandaloneTarget;
        public int streamingInstallLaunchRange;
        public bool symlinkLibraries;
        public bool waitForPlayerConnection;
        public string windowsDevicePortalAddress;
        public string windowsDevicePortalPassword;
        public string windowsDevicePortalUsername;
        public WSABuildAndRunDeployTarget wsaBuildAndRunDeployTarget;
        public WSASubtarget wsaSubtarget;
        public string wsaUWPSDK;
        public string wsaUWPVisualStudioVersion;
        public XboxBuildSubtarget xboxBuildSubtarget;
        public XboxOneDeployDrive xboxOneDeployDrive;
        public XboxOneDeployMethod xboxOneDeployMethod;
        public bool xboxOneRebootIfDeployFailsAndRetry;

        public static BuildSettingsPreset FromCurrentSettings()
        {
            BuildSettingsPreset preset = CreateInstance<BuildSettingsPreset>();

            preset.activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            preset.activeScriptCompilationDefines = EditorUserBuildSettings.activeScriptCompilationDefines;
            preset.allowDebugging = EditorUserBuildSettings.allowDebugging;
            preset.androidBuildSubtarget = EditorUserBuildSettings.androidBuildSubtarget;
            preset.androidETC2Fallback = EditorUserBuildSettings.androidETC2Fallback;
            preset.buildAppBundle = EditorUserBuildSettings.buildAppBundle;
            preset.buildScriptsOnly = EditorUserBuildSettings.buildScriptsOnly;
            preset.compressFilesInPackage = EditorUserBuildSettings.compressFilesInPackage;
            preset.compressWithPsArc = EditorUserBuildSettings.compressWithPsArc;
            preset.connectProfiler = EditorUserBuildSettings.connectProfiler;
            preset.development = EditorUserBuildSettings.development;
            preset.enableHeadlessMode = EditorUserBuildSettings.enableHeadlessMode;
            preset.explicitArrayBoundsChecks = EditorUserBuildSettings.explicitArrayBoundsChecks;
            preset.explicitDivideByZeroChecks = EditorUserBuildSettings.explicitDivideByZeroChecks;
            preset.explicitNullChecks = EditorUserBuildSettings.explicitNullChecks;
            preset.exportAsGoogleAndroidProject = EditorUserBuildSettings.exportAsGoogleAndroidProject;
            preset.forceInstallation = EditorUserBuildSettings.forceInstallation;
            preset.installInBuildFolder = EditorUserBuildSettings.installInBuildFolder;
            preset.iOSBuildConfigType = EditorUserBuildSettings.iOSBuildConfigType;
            preset.movePackageToDiscOuterEdge = EditorUserBuildSettings.movePackageToDiscOuterEdge;
            preset.needSubmissionMaterials = EditorUserBuildSettings.needSubmissionMaterials;
            preset.ps4BuildSubtarget = EditorUserBuildSettings.ps4BuildSubtarget;
            preset.ps4HardwareTarget = EditorUserBuildSettings.ps4HardwareTarget;
            preset.selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            preset.selectedStandaloneTarget = EditorUserBuildSettings.selectedStandaloneTarget;
            preset.streamingInstallLaunchRange = EditorUserBuildSettings.streamingInstallLaunchRange;
            preset.symlinkLibraries = EditorUserBuildSettings.symlinkLibraries;
            preset.waitForPlayerConnection = EditorUserBuildSettings.waitForPlayerConnection;
            preset.windowsDevicePortalAddress = EditorUserBuildSettings.windowsDevicePortalAddress;
            preset.windowsDevicePortalPassword = EditorUserBuildSettings.windowsDevicePortalPassword;
            preset.windowsDevicePortalUsername = EditorUserBuildSettings.windowsDevicePortalUsername;
            preset.wsaBuildAndRunDeployTarget = EditorUserBuildSettings.wsaBuildAndRunDeployTarget;
            preset.wsaSubtarget = EditorUserBuildSettings.wsaSubtarget;
            preset.wsaUWPSDK = EditorUserBuildSettings.wsaUWPSDK;
            preset.wsaUWPVisualStudioVersion = EditorUserBuildSettings.wsaUWPVisualStudioVersion;
            preset.xboxBuildSubtarget = EditorUserBuildSettings.xboxBuildSubtarget;
            preset.xboxOneDeployDrive = EditorUserBuildSettings.xboxOneDeployDrive;
            preset.xboxOneDeployMethod = EditorUserBuildSettings.xboxOneDeployMethod;
            preset.xboxOneRebootIfDeployFailsAndRetry = EditorUserBuildSettings.xboxOneRebootIfDeployFailsAndRetry;

            return preset;
        }

        public void Import()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(selectedBuildTargetGroup, activeBuildTarget);
            // TODO : defines
//            EditorUserBuildSettings.activeScriptCompilationDefines = activeScriptCompilationDefines;
            EditorUserBuildSettings.allowDebugging = allowDebugging;
            EditorUserBuildSettings.androidBuildSubtarget = androidBuildSubtarget;
            EditorUserBuildSettings.androidETC2Fallback = androidETC2Fallback;
            EditorUserBuildSettings.buildAppBundle = buildAppBundle;
            EditorUserBuildSettings.buildScriptsOnly = buildScriptsOnly;
            EditorUserBuildSettings.compressFilesInPackage = compressFilesInPackage;
            EditorUserBuildSettings.compressWithPsArc = compressWithPsArc;
            EditorUserBuildSettings.connectProfiler = connectProfiler;
            EditorUserBuildSettings.development = development;
            EditorUserBuildSettings.enableHeadlessMode = enableHeadlessMode;
            EditorUserBuildSettings.explicitArrayBoundsChecks = explicitArrayBoundsChecks;
            EditorUserBuildSettings.explicitDivideByZeroChecks = explicitDivideByZeroChecks;
            EditorUserBuildSettings.explicitNullChecks = explicitNullChecks;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = exportAsGoogleAndroidProject;
            EditorUserBuildSettings.forceInstallation = forceInstallation;
            EditorUserBuildSettings.installInBuildFolder = installInBuildFolder;
            EditorUserBuildSettings.iOSBuildConfigType = iOSBuildConfigType;
            EditorUserBuildSettings.movePackageToDiscOuterEdge = movePackageToDiscOuterEdge;
            EditorUserBuildSettings.needSubmissionMaterials = needSubmissionMaterials;
            EditorUserBuildSettings.ps4BuildSubtarget = ps4BuildSubtarget;
            EditorUserBuildSettings.ps4HardwareTarget = ps4HardwareTarget;
            EditorUserBuildSettings.selectedBuildTargetGroup = selectedBuildTargetGroup;
            EditorUserBuildSettings.selectedStandaloneTarget = selectedStandaloneTarget;
            EditorUserBuildSettings.streamingInstallLaunchRange = streamingInstallLaunchRange;
            EditorUserBuildSettings.symlinkLibraries = symlinkLibraries;
            EditorUserBuildSettings.waitForPlayerConnection = waitForPlayerConnection;
            EditorUserBuildSettings.windowsDevicePortalAddress = windowsDevicePortalAddress;
            EditorUserBuildSettings.windowsDevicePortalPassword = windowsDevicePortalPassword;
            EditorUserBuildSettings.windowsDevicePortalUsername = windowsDevicePortalUsername;
            EditorUserBuildSettings.wsaBuildAndRunDeployTarget = wsaBuildAndRunDeployTarget;
            EditorUserBuildSettings.wsaSubtarget = wsaSubtarget;
            EditorUserBuildSettings.wsaUWPSDK = wsaUWPSDK;
            EditorUserBuildSettings.wsaUWPVisualStudioVersion = wsaUWPVisualStudioVersion;
            EditorUserBuildSettings.xboxBuildSubtarget = xboxBuildSubtarget;
            EditorUserBuildSettings.xboxOneDeployDrive = xboxOneDeployDrive;
            EditorUserBuildSettings.xboxOneDeployMethod = xboxOneDeployMethod;
            EditorUserBuildSettings.xboxOneRebootIfDeployFailsAndRetry = xboxOneRebootIfDeployFailsAndRetry;
        }
    }
}