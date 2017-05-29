$latestInfo = Invoke-WebRequest https://github.com/yamachu/World/releases/latest -Headers @{"Accept"="application/json"}
$json = $latestInfo.Content | ConvertFrom-Json
$latestVersion = $json.tag_name
$baseUrl = "https://github.com/yamachu/World/releases/download/$latestVersion"

Invoke-WebRequest "$baseUrl/libworld.dylib" -OutFile library\resources\osx\libworld.dylib
Invoke-WebRequest "$baseUrl/Linux_libworld.so" -OutFile library\resources\linux\libworld.so
Invoke-WebRequest "$baseUrl/x86_world.dll" -OutFile library\resources\win-x86\world.dll
Invoke-WebRequest "$baseUrl/x64_world.dll" -OutFile library\resources\win-x64\world.dll
Invoke-WebRequest "$baseUrl/Android_arm_libworld.so" -OutFile library\resources\android\arm\libworld.so
Invoke-WebRequest "$baseUrl/Android_arm64_libworld.so" -OutFile library\resources\android\arm64\libworld.so
Invoke-WebRequest "$baseUrl/Android_x86_64_libworld.so" -OutFile library\resources\android\x86_64\libworld.so
Invoke-WebRequest "$baseUrl/Android_x86_libworld.so" -OutFile library\resources\android\x86\libworld.so
Invoke-WebRequest "$baseUrl/ios_libworld.a" -OutFile library\resources\ios\universal\libworld.a
