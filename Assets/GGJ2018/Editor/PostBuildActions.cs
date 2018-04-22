 using System;
 using System.IO;
 using System.Text.RegularExpressions;
 using UnityEditor;
 using UnityEditor.Callbacks;
 
 public class PostBuildActions {
	 [PostProcessBuild]
	 public static void OnPostProcessBuild(BuildTarget target, string targetPath) {
		var path = Path.Combine(targetPath, "Build/UnityLoader.js");
		var text = File.ReadAllText(path);
		// UnityLoader.SystemInfo.mobile?e.popup("Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway.",[{text:"OK",callback:t}]):
		text = text.Replace("UnityLoader.SystemInfo.mobile?e.popup(\"Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway.\",[{text:\"OK\",callback:t}]):", "");
		File.WriteAllText(path, text);
	 }
 }
 