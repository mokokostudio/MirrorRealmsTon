using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloudUrlUtil {
#if UNITY_ANDROID
    public static string Url => "https://mirror-realms-app-hotdata-android.oss-ap-southeast-1.aliyuncs.com";
#elif UNITY_IOS
    public static string Url => "https://mirror-realms-app-hotdata-ios.oss-ap-southeast-1.aliyuncs.com";
#else
    public static string Url => "https://mirror-realms-app-hotdata-win.oss-ap-southeast-1.aliyuncs.com";
#endif
}
