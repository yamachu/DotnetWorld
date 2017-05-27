$latestInfo = Invoke-WebRequest https://github.com/yamachu/World/releases/latest -Headers @{"Accept"="application/json"}
$json = $latestInfo.Content | ConvertFrom-Json
$latestVersion = $json.tag_name
$baseUrl = "https://github.com/yamachu/World/releases/download/$latestVersion"

Invoke-WebRequest "$baseUrl/libworld.dylib" -OutFile library\resources\osx\libworld.dylib
Invoke-WebRequest "$baseUrl/libworld.so" -OutFile library\resources\linux\libworld.so
Invoke-WebRequest "$baseUrl/x86_world.dll" -OutFile library\resources\win-x86\world.dll
Invoke-WebRequest "$baseUrl/x64_world.dll" -OutFile library\resources\win-x64\world.dll
